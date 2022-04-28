using AllOrNothing.Contracts.Services;
using AllOrNothing.Controls;
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
using System.Windows.Input;

namespace AllOrNothing.ViewModels
{
    public class PlayerAddingViewModel : ViewModelBase
    {
        #region Fields
        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        private ObservableCollection<PlayerDto> _allPlayers;
        private PlayerDto _selectedPlayer;
        private PlayerDto _originalPlayer;
        private PlayerDto _editingPlayer;

        private ICommand _exitCommand;
        private ICommand _newPlayerCommand;
        private ICommand _saveCommand;
        private ICommand _deleteCommand;

        private bool _formEnabled;
        private bool _isNewPlayerSelected;
        private bool _isPlayerUnderEdit;
        #endregion

        #region Constructors
        public PlayerAddingViewModel(INavigationViewService navigationViewService, IUnitOfWork unitOfWork, IMapper mapper)
            : base(navigationViewService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            AllPlayers = new ObservableCollection<PlayerDto>(_mapper.Map<ICollection<PlayerDto>>(_unitOfWork.Players.GetAllAvaible()));
        }
        #endregion

        #region Events
        public event EventHandler<NavigateToEventargs> NavigateTo;
        #endregion

        #region Properties
        public XamlRoot PageXamlRoot { get; set; }
        public PlayerDto SelectedPlayer
        {
            get => _selectedPlayer;
            set
            {
                SetProperty(ref _selectedPlayer, value);
                if (value != null)
                    EditingPlayer = value;
            }
        }

        public ObservableCollection<PlayerDto> AllPlayers
        {
            get => _allPlayers;
            set => SetProperty(ref _allPlayers, value);
        }
        public bool IsNewPlayerSelected
        {
            get => _isNewPlayerSelected;
            set
            {
                SetProperty(ref _isNewPlayerSelected, value);
                IsPlayerUnderEdit = true;
            }
        }
        public bool IsPlayerUnderEdit
        {
            get => _isPlayerUnderEdit;
            set => SetProperty(ref _isPlayerUnderEdit, value);
        }
        public bool FormEnabled
        {
            get => _formEnabled;
            set => SetProperty(ref _formEnabled, value);
        }
        public PlayerDto EditingPlayer
        {
            get => _editingPlayer;
            set
            {
                SetProperty(ref _editingPlayer, value);

                _originalPlayer = value;
                FormEnabled = value != null;
                IsNewPlayerSelected = value?.Id == -1;
            }
        }

        public ICommand ExitCommand => _exitCommand ??= new RelayCommand(Exit);
        public ICommand NewPlayerCommand => _newPlayerCommand ??= new RelayCommand(AddNewPlayer);
        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(Save);
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);
        #endregion

        #region Methods
        private async void Exit()
        {
            if (await PopupManager.ShowDialog(PageXamlRoot, "Biztosan kilép?", "Ha kilép, minden nem mentett módosítás elveszik.", ContentDialogButton.Primary, "Igen", "Mégse") == ContentDialogResult.Primary)
            {
                IsMenuButtonVisible = false;
                NavigateTo?.Invoke(this, new NavigateToEventargs { PageName = "Főmenü", PageVM = typeof(MainMenuViewModel) });
            }
        }

        private async void Delete()
        {
            if (await PopupManager.ShowDialog(PageXamlRoot, "Biztosan törli?", "Biztosan törli a játékost?", ContentDialogButton.Primary, "Igen", "Mégse") == ContentDialogResult.Primary)
            {
                if (!IsNewPlayerSelected)
                {
                    var playerData = _unitOfWork.Players.Get(EditingPlayer.Id);
                    playerData.IsDeleted = true;
                    _unitOfWork.Complete();
                    AllPlayers = new ObservableCollection<PlayerDto>(_mapper.Map<ICollection<PlayerDto>>(_unitOfWork.Players.GetAllAvaible()));
                }
                EditingPlayer = null;
                IsNewPlayerSelected = false;
            }
        }

        private async void Save()
        {
            if (IsNewPlayerSelected)
            {
                EditingPlayer.Id = 0;
                _unitOfWork.Players.Add(_mapper.Map<Player>(EditingPlayer));
            }
            else
            {
                var playerData = _unitOfWork.Players.Get(EditingPlayer.Id);
                playerData.Name = EditingPlayer.Name;
                playerData.NickName = EditingPlayer.NickName;
                playerData.Institute = EditingPlayer.Institute;
            }


            var dialogTitle = "Sikertelen mentés";
            var dialogContent = "Sikertelen mentés!";

            if (_unitOfWork.Complete() > 0)
            {
                AllPlayers = new ObservableCollection<PlayerDto>(_mapper.Map<ICollection<PlayerDto>>(_unitOfWork.Players.GetAllAvaible()));
                dialogTitle = "Sikeres mentés";
                dialogContent = "A játékos sikeresen mentve!";
                IsNewPlayerSelected = false;
                EditingPlayer = null;
            }
            await PopupManager.ShowDialog(PageXamlRoot, dialogTitle, dialogContent, ContentDialogButton.Close, "", "Ok");
        }

        private void AddNewPlayer()
        {
            SelectedPlayer = null;
            EditingPlayer = new PlayerDto
            {
                Id = -1,
                Name = "Új játékos",
            };
        }
        #endregion
    }
}
