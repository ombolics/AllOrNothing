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
        public PlayerAddingViewModel(INavigationViewService navigationViewService, IUnitOfWork unitOfWork, IMapper mapper)
            : base(navigationViewService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            AllPlayers = new ObservableCollection<PlayerDto>(_mapper.Map<ICollection<PlayerDto>>(_unitOfWork.Players.GetAllAvaible()));
        }

        private IMapper _mapper;
        public XamlRoot PageXamlRoot { get; set; }

        private ObservableCollection<PlayerDto> _allPlayers;
        private readonly IUnitOfWork _unitOfWork;

        private PlayerDto _selectedPlayer;
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

        private PlayerDto _originalPlayer;
        public ObservableCollection<PlayerDto> AllPlayers
        {
            get => _allPlayers;
            set => SetProperty(ref _allPlayers, value);
        }

        private ICommand _exitCommand;
        public ICommand ExitCommand => _exitCommand ??= new RelayCommand(Exit);

        public event EventHandler<NavigateToEventargs> NavigateTo;

        private async void Exit()
        {
            if(await PopupManager.ShowDialog(PageXamlRoot, "Biztosan kilép?", "Ha kilép, minden nem mentett módosítás elveszik.", ContentDialogButton.Primary, "Igen", "Mégse") == ContentDialogResult.Primary)
            {
                IsMenuButtonVisible = false;
                NavigateTo?.Invoke(this, new NavigateToEventargs { PageName = "Főmenü", PageVM = typeof(AllOrNothingViewModel) });               
            }
        }

        private ICommand _newPlayerCommand;
        public ICommand NewPlayerCommand => _newPlayerCommand ??= new RelayCommand(AddNewPlayer);

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(Save);

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);

        private async void Delete()
        {
            if (await PopupManager.ShowDialog(PageXamlRoot, "Biztosan törli?", "Biztosan törli a játékost?",ContentDialogButton.Primary,"Igen", "Mégse") == ContentDialogResult.Primary)
            {
                if (!IsNewPlayerSelected)
                {
                    EditingPlayer.IsDeleted = true;
                    _unitOfWork.Complete();
                    AllPlayers = new ObservableCollection<PlayerDto>(_mapper.Map<ICollection<PlayerDto>>(_unitOfWork.Players.GetAllAvaible()));
                }
                EditingPlayer = null;
            }
            IsNewPlayerSelected = false;
        }

        private async void Save()
        {
            if (IsNewPlayerSelected)
            {
                EditingPlayer.Id = 0;
                _unitOfWork.Players.Add(_mapper.Map<Player>(EditingPlayer));
            }

            var dialogTitle = "Sikertelen mentés";
            var dialogContent = "Sikertelen mentés!";
          
            if (_unitOfWork.Complete() > 0)
            {
                AllPlayers = new ObservableCollection<PlayerDto>(_mapper.Map<ICollection<PlayerDto>>(_unitOfWork.Players.GetAllAvaible()));
                dialogTitle = "Sikeres mentés";
                dialogContent = "Sikeres mentés!";
                IsNewPlayerSelected = false;
                EditingPlayer = null;
            }     
            await PopupManager.ShowDialog(PageXamlRoot, dialogTitle, dialogContent, ContentDialogButton.Close, "", "Ok");
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
        private bool _formEnabled;
        public bool FormEnabled
        {
            get => _formEnabled;
            set => SetProperty(ref _formEnabled, value);
        }
        public void TextBoxChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox) || EditingPlayer == null)
                return;

            switch (textBox.Name)
            {
                case "nameTextBox":
                    //TODO validate
                    IsPlayerUnderEdit = _originalPlayer.Name != textBox.Text.Trim();
                    EditingPlayer.Name = textBox.Text;
                    break;
                case "nickNameTextBox":
                    IsPlayerUnderEdit = _originalPlayer.NickName != textBox.Text.Trim();
                    EditingPlayer.NickName = textBox.Text;
                    break;
                case "instituteTextBox":
                    IsPlayerUnderEdit = _originalPlayer.Institute != textBox.Text.Trim();
                    EditingPlayer.Institute = textBox.Text;
                    break;
                default:
                    break;
            }
        }


        private PlayerDto _editingPlayer;
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
        private bool _isNewPlayerSelected;

        private bool _isPlayerUnderEdit;

        private void AddNewPlayer()
        {
            SelectedPlayer = null;
            EditingPlayer = new PlayerDto
            {
                Id = -1,
                Name = "Új játékos",
            };
        }
    }
}
