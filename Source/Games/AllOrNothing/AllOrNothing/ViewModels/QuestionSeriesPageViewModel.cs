using AllOrNothing.Mapping;
using AllOrNothing.Controls;
using AllOrNothing.Data;
using AllOrNothing.Repository;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AllOrNothing.Data.DataExtensions;

namespace AllOrNothing.ViewModels
{
    public class QuestionSeriesPageViewModel : ObservableRecipient
    {
        public QuestionSeriesPageViewModel(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            AllSerie = GetSeriesAsDto();
        }

        private ObservableCollection<QuestionSerieDto> GetSeriesAsDto()
        {
            var tmp = _mapper.Map<IEnumerable<QuestionSerieDto>>(_unitOfWork.QuestionSeries.GetAllAvaible());
            return new ObservableCollection<QuestionSerieDto>(tmp);
        }

        public XamlRoot PageXamlRoot { get; set; }
        private ObservableCollection<QuestionSerieDto> _allSerie;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
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

        private async void Delete()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = PageXamlRoot;
            dialog.Title = "Biztosan törli?";
            dialog.PrimaryButtonText = "Igen";
            dialog.CloseButtonText = "Mégse";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new CustomDialog("Biztosan törli a kérdéssort?");

            if (await dialog.ShowAsync(ContentDialogPlacement.Popup) == ContentDialogResult.Primary)
            {
                if (!IsNewSerieSelected)
                {
                    EditingSerie.IsDeleted = true;
                    _unitOfWork.Complete();
                    AllSerie = GetSeriesAsDto();
                }
                EditingSerie = null;
            }
            IsNewSerieSelected = false;
        }

        private async void Save()
        {
            if (IsNewSerieSelected)
            {
                EditingSerie.Id = 0;
                _unitOfWork.QuestionSeries.Add(_mapper.Map<QuestionSerie>(EditingSerie));
            }
            else if(!EditingSerie.HasTheSameValue(_originalSerie))
            {
                var serie = _unitOfWork.QuestionSeries.Get(EditingSerie.Id);
                //serie.SetValue(_mapper.Map<QuestionSerie>(EditingSerie));

                serie.Name = EditingSerie.Name;
                foreach (var item in EditingSerie.Topics)
                {
                    var data = _unitOfWork.Topics.Get(item.Id);
                    data.Name = item.Name;
                    data.Description = item.Description;
                    //data.Author = //_mapper.Map<Player>(item.Author);
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

                //_unitOfWork.Complete();
                //_unitOfWork.QuestionSeries.Add( _mapper.Map<QuestionSerie>(EditingSerie));
                //serie.IsDeleted = EditingSerie.IsDeleted;
                //serie.Name = EditingSerie.Name;
                //serie.Topics = _mapper.Map<ICollection<Topic>>(EditingSerie.Topics);
            }
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = PageXamlRoot;
            dialog.CloseButtonText = "Ok";
            dialog.DefaultButton = ContentDialogButton.Close;

            var result = _unitOfWork.Complete() > 0;
            if (result)
            {
                AllSerie = GetSeriesAsDto();
                dialog.Title = "Sikeres mentés";
                dialog.Content = new CustomDialog("Sikeres mentés!");
                IsNewSerieSelected = false;
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

                FormEnabled = value != null;
                IsNewSerieSelected = value?.Id == -1;
            }
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
                serie.Topics.Add(new TopicDto());
            }

            EditingSerie = serie;
            _originalSerie = new QuestionSerieDto(serie);
        }
    }
}
