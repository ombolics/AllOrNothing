using AllOrNothing.Contracts.Services;
using AllOrNothing.Contracts.ViewModels;
using AllOrNothing.Data;
using AllOrNothing.Helpers;
using AllOrNothing.Mapping;
using AllOrNothing.Repository;
using AutoMapper;
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
    public class QuestionSerieEditorViewModel : ViewModelBase, IExitable
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        private List<Competence> _allCompetences;
        private ObservableCollection<QuestionSerieDto> _allSerie;

        private QuestionSerieDto _selectedSerie;
        private QuestionSerieDto _originalSerie;
        private QuestionSerieDto _editingSerie;
        private bool _unselectedTextVisible;
        private bool _isNewSerieSelected;
        private bool _isSerieUnderEdit;
        private bool _formEnabled;

        private ICommand _newSerieCommand;
        private ICommand _saveCommand;
        private ICommand _deleteCommand;
        private ICommand _exitCommand;
        #endregion

        #region Constructors
        public QuestionSerieEditorViewModel(INavigationViewService navigationViewService, IUnitOfWork unitOfWork, IMapper mapper)
            : base(navigationViewService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            AllSerie = GetSeriesAsDto();
            AllCompetences = _unitOfWork.Competences.GetAll().ToList();
            UnselectedTextVisible = true;
        }
        #endregion

        #region Properties
        public IMapper Mapper
        {
            get => _mapper;
            set => SetProperty(ref _mapper, value);
        }
        public XamlRoot PageXamlRoot { get; set; }
        public List<Competence> AllCompetences
        {
            get => _allCompetences;
            set => SetProperty(ref _allCompetences, value);
        }
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

        public ICommand NewSerieCommand => _newSerieCommand ??= new RelayCommand(AddNewSerie);
        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(Save);
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);
        public ICommand ExitCommand => _exitCommand ??= new RelayCommand(Exit);

        public bool UnselectedTextVisible
        {
            get => _unselectedTextVisible;
            set => SetProperty(ref _unselectedTextVisible, value);
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
        public bool FormEnabled
        {
            get => _formEnabled;
            set => SetProperty(ref _formEnabled, value);
        }
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
        #endregion

        #region Methods
        private ObservableCollection<QuestionSerieDto> GetSeriesAsDto()
        {
            var tmp = Mapper.Map<IEnumerable<QuestionSerieDto>>(_unitOfWork.QuestionSeries.GetAllAvaible());
            return new ObservableCollection<QuestionSerieDto>(tmp);
        }

        private async void Delete()
        {
            if (await PopupManager.ShowDialog(PageXamlRoot, "Biztosan törli?", "Biztosan törli a kérdéssort?", ContentDialogButton.Primary, "Igen", "Mégse") == ContentDialogResult.Primary)
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

        public async void Exit()
        {
            if (await PopupManager.ShowDialog(PageXamlRoot, "Biztosan kilép?", "Ha kilép, minden nem mentett módosítás elveszik.", ContentDialogButton.Primary, "Igen", "Mégse") == ContentDialogResult.Primary)
            {
                IsMenuButtonVisible = false;
                RaiseNavigateTo(new NavigateToEventArgs { PageName = "Főmenü", PageVM = typeof(MainMenuViewModel) });
            }
        }
        private async void Save()
        {
            var dialogTitle = "";
            var dialogContent = "";
            try
            {
                if (EditingSerie.Topics.Any(t => t.Questions.Any(q => string.IsNullOrWhiteSpace(q.Text) || string.IsNullOrWhiteSpace(q.Answer))) ||
                    EditingSerie.Topics.Any(t => string.IsNullOrWhiteSpace(t.Name)))
                {
                    throw new InvalidOperationException("Fields must be filled!");
                }
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

                if (_unitOfWork.Complete() > 0 || !serieChanged)
                {
                    AllSerie = GetSeriesAsDto();
                    IsNewSerieSelected = false;
                    EditingSerie = null;
                    dialogTitle = "Sikeres mentés";
                    dialogContent = "Sikeres mentés!";
                }
            }
            catch (Exception e)
            {
                dialogTitle = "Sikertelen mentés";
                dialogContent = "Sikertelen mentés! Töltse ki az összes kötelező mezőt! (Cím, téma címek, kérdések, válaszok, kérdések értékei)";

            }
            PopupManager.ShowDialog(PageXamlRoot, dialogTitle, dialogContent, ContentDialogButton.Close, "", "Ok");
        }

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
        #endregion
    }
}
