using AllOrNothing.Contracts.Services;
using AllOrNothing.Controls;
using AllOrNothing.Data;
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

        private ICommand _newPlayerCommand;
        public ICommand NewPlayerCommand => _newPlayerCommand ??= new RelayCommand(AddNewPlayer);

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
            dialog.Content = new CustomDialog("Biztosan törli a játékost?");

            if (await dialog.ShowAsync(ContentDialogPlacement.Popup) == ContentDialogResult.Primary)
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

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = PageXamlRoot;
            dialog.CloseButtonText = "Ok";
            dialog.DefaultButton = ContentDialogButton.Close;


            if (_unitOfWork.Complete() > 0)
            {
                AllPlayers = new ObservableCollection<PlayerDto>(_mapper.Map<ICollection<PlayerDto>>(_unitOfWork.Players.GetAllAvaible()));
                dialog.Title = "Sikeres mentés";
                dialog.Content = new CustomDialog("Sikeres mentés!");
                IsNewPlayerSelected = false;
                EditingPlayer = null;
            }
            else
            {
                dialog.Title = "Sikertelen mentés";
                dialog.Content = new CustomDialog("Sikertelen mentés!");
            }
            await dialog.ShowAsync(ContentDialogPlacement.Popup);


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
