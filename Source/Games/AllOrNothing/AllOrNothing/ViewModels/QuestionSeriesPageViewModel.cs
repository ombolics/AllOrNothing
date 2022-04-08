using AllOrNothing.Controls;
using AllOrNothing.Data;
using AllOrNothing.Mapping;
using AllOrNothing.Repository;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AllOrNothing.ViewModels
{
    public class QuestionSeriesPageViewModel : ObservableRecipient
    {
        public QuestionSeriesPageViewModel(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            AllSerie = GetSeriesAsDto();
            AllCompetences = _unitOfWork.Competences.GetAll().ToList();
            UnselectedTextVisible = true;
        }
        private List<Competence> _allCompetences;
        public List<Competence> AllCompetences
        {
            get => _allCompetences;
            set => SetProperty(ref _allCompetences, value);
        }
        private ObservableCollection<QuestionSerieDto> GetSeriesAsDto()
        {
            var tmp = Mapper.Map<IEnumerable<QuestionSerieDto>>(_unitOfWork.QuestionSeries.GetAllAvaible());
            return new ObservableCollection<QuestionSerieDto>(tmp);
        }

        public XamlRoot PageXamlRoot { get; set; }
        private ObservableCollection<QuestionSerieDto> _allSerie;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private QuestionSerieDto _selectedSerie;
        public QuestionSerieDto SelectedSerie
        {
            get => _selectedSerie;
            set
            {
                SetProperty(ref _selectedSerie, value);
                if (value != null)
                {
                    EditingSerie = value;
                    _originalSerie = new QuestionSerieDto(value);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if the original and the editing serie contains the same data</returns>     

        private int _itemsUnderEdit;
        private QuestionSerieDto _originalSerie;
        public ObservableCollection<QuestionSerieDto> AllSerie
        {
            get => _allSerie;
            set
            {
                SetProperty(ref _allSerie, value);
                //foreach (var serie in _allSerie)
                //{
                //    serie.UnderEdit += Item_UnderEdit;
                //    foreach (var topic in serie.Topics)
                //    {
                //        topic.UnderEdit += Item_UnderEdit;
                //    }
                //}
            }
        }

        //private void Item_UnderEdit(object sender, EventArgs e)
        //{
        //    _itemsUnderEdit++;
        //    _isSerieUnderEdit = true;
        //}

        private ICommand _newSerieCommand;
        public ICommand NewSerieCommand => _newSerieCommand ??= new RelayCommand(AddNewSerie);

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(Save);

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);

        private bool _unselectedTextVisible;
        public bool UnselectedTextVisible
        {
            get => _unselectedTextVisible;
            set => SetProperty(ref _unselectedTextVisible, value);
        }
        private async void Delete()
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = PageXamlRoot,
                Title = "Biztosan törli?",
                PrimaryButtonText = "Igen",
                CloseButtonText = "Mégse",
                DefaultButton = ContentDialogButton.Primary,
                Content = new CustomDialog("Biztosan törli a kérdéssort?"),
            };

            if (await dialog.ShowAsync(ContentDialogPlacement.Popup) == ContentDialogResult.Primary)
            {
                if (!IsNewSerieSelected)
                {
                    var serie = _unitOfWork.QuestionSeries.Get(EditingSerie.Id);
                    serie.IsDeleted = true;
                    var result = _unitOfWork.Complete();
                    AllSerie = GetSeriesAsDto();
                }
                EditingSerie = null;
                IsNewSerieSelected = false;
            }

        }

        //TODO mentés előtt ne lehessen másikat kiválasztani

        private async void Save()
        {
            bool serieChanged = !EditingSerie.HasTheSameValue(_originalSerie);
            if (IsNewSerieSelected)
            {
                EditingSerie.Id = 0;
                EditingSerie.CreatedOn = DateTime.Now;
                var mapped = Mapper.Map<QuestionSerie>(EditingSerie);
                for (int i = 0; i < EditingSerie.Topics.Count; i++)
                {
                    for (int j = 0; j < EditingSerie.Topics[i].Competences.Count; j++)
                    {
                        mapped.Topics[i].Competences[j] = _unitOfWork.Competences.Get(EditingSerie.Topics[i].Competences[j].Id);
                    }
                }
                _unitOfWork.QuestionSeries.Add(mapped);
            }
            else if (serieChanged)
            {
                var serie = _unitOfWork.QuestionSeries.Get(EditingSerie.Id);
                serie.Name = EditingSerie.Name;

                foreach (var item in EditingSerie.Topics)
                {
                    var data = _unitOfWork.Topics.Get(item.Id);
                    data.Name = item.Name;
                    List<Competence> tmp = new List<Competence>();
                    foreach (var comp in item.Competences)
                    {
                        tmp.Add(_unitOfWork.Competences.Get(comp.Id));
                    }
                    data.Competences = tmp;
                    data.Description = item.Description;

                    foreach (var question in item.Questions)
                    {
                        var dbQuestion = _unitOfWork.Questions.Get(question.Id);
                        dbQuestion.Resource = question.Resource;
                        dbQuestion.ResourceType = question.ResourceType;
                        dbQuestion.Text = question.Text;
                        dbQuestion.Type = question.Type;
                        dbQuestion.Value = question.Value;
                    }
                }
            }
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = PageXamlRoot,
                CloseButtonText = "Ok",
                DefaultButton = ContentDialogButton.Close,
            };

            if (_unitOfWork.Complete() > 0 || !serieChanged)
            {
                AllSerie = GetSeriesAsDto();
                dialog.Title = "Sikeres mentés";
                dialog.Content = new CustomDialog("Sikeres mentés!");
                IsNewSerieSelected = false;
                EditingSerie = null;
            }
            else
            {
                dialog.Title = "Sikertelen mentés";
                dialog.Content = new CustomDialog("Sikertelen mentés!");
            }
            await dialog.ShowAsync(ContentDialogPlacement.Popup);
        }

        public bool IsNewSerieSelected
        {
            get => _isNewSerieSelected;
            set
            {
                SetProperty(ref _isNewSerieSelected, value);
                IsSerieUnderEdit = true;
            }
        }
        public bool IsSerieUnderEdit
        {
            get => _isSerieUnderEdit;
            set => SetProperty(ref _isSerieUnderEdit, value);
        }
        private bool _formEnabled;
        public bool FormEnabled
        {
            get => _formEnabled;
            set => SetProperty(ref _formEnabled, value);
        }
        public void TextBoxChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox) || EditingSerie == null)
                return;

            //switch (textBox.Name)
            //{
            //    case "nameTextBox":
            //        //TODO validate
            //        IsSerieUnderEdit = _originalSerie.Name != textBox.Text.Trim();
            //        EditingSerie.Name = textBox.Text;
            //        break;
            //    case "nickNameTextBox":
            //        IsSerieUnderEdit = _originalSerie.NickName != textBox.Text.Trim();
            //        EditingSerie.NickName = textBox.Text;
            //        break;
            //    case "instituteTextBox":
            //        IsSerieUnderEdit = _originalSerie.Institute != textBox.Text.Trim();
            //        EditingSerie.Institute = textBox.Text;
            //        break;
            //    default:
            //        break;
            //}
        }


        private QuestionSerieDto _editingSerie;
        public QuestionSerieDto EditingSerie
        {
            get => _editingSerie;
            set
            {
                SetProperty(ref _editingSerie, value);
                bool isValueNull = value == null;
                UnselectedTextVisible = isValueNull;
                FormEnabled = !isValueNull;
                IsNewSerieSelected = value?.Id == -1;
            }
        }

        public IMapper Mapper
        {
            get => _mapper;
            set => SetProperty(ref _mapper, value);
        }

        private bool _isNewSerieSelected;

        private bool _isSerieUnderEdit;

        private void AddNewSerie()
        {
            var serie = new QuestionSerieDto
            {
                Id = -1,
                Name = "Új kérdéssor",
            };
            serie.Topics = new List<TopicDto>();
            for (int i = 0; i < 5; i++)
            {
                serie.Topics.Add(new TopicDto(6, _unitOfWork, Mapper));
            }

            SelectedSerie = null;
            EditingSerie = serie;
            _originalSerie = new QuestionSerieDto(serie);
        }
    }
}
