using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using QuizLauncher.Contracts.ViewModels;
using QuizLauncher.Models;
using QuizLauncher.Services;

namespace QuizLauncher.ViewModels
{
    public class MainViewModel : ObservableRecipient, INavigationAware
    {
        public MainViewModel(GameIOService gameImportService)
        {
            _gameImportService = gameImportService;

            
        }
        public void ButtonClick(object param)
        {
            Process.Start(param.ToString());
        }

        public void OnNavigatedTo(object parameter)
        {
            Games = _gameImportService.ListAllGame();
            foreach (var item in Games)
            {
                item.ErrorWhileOpening += Item_ErrorWhileOpening;
            }
        }
        public XamlRoot PageXamlRoot { get; set; }

        private async void Item_ErrorWhileOpening()
        {
            ContentDialog dialog = new ContentDialog
            { 
                XamlRoot = PageXamlRoot,
                CloseButtonText = "Ok",
                DefaultButton = ContentDialogButton.Close,
                Content = "Hiba történt a megnyitás során! Lehet, hogy nem rendelkezik megfelelő jogosultságokkal a fájl megnyitásához.",
                Title = "Hiba!"
            };

            await dialog.ShowAsync(ContentDialogPlacement.Popup);
        }

        public void OnNavigatedFrom()
        {

        }

        private List<GamePreviewModel> _games;
        public List<GamePreviewModel> Games
        {
            get => _games;
            set => SetProperty(ref _games, value);
        }

        private ICommand _buttonCommand;
        public ICommand ButtonCommand => _buttonCommand ??= new RelayCommand<object>(ButtonClick);
        private GameIOService _gameImportService;
    }
}
