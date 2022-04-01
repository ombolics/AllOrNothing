using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizLauncher.Models;
using QuizLauncher.Services;

namespace QuizLauncher.ViewModels
{
    public class MainViewModel : ObservableRecipient
    {
        public MainViewModel(GameImportService gameImportService)
        {
            ButtonCommand = new RelayCommand(On_buttonPressed);
            _gameImportService = gameImportService;

            Games = new List<GamePreviewModel> { new GamePreviewModel() };//_gameImportService.ListAllGame();
        }
        public void On_buttonPressed()
        {
            Process.Start(@"C:\Users\Csabi\Desktop\Godot.exe");
        }
        private List<GamePreviewModel> _games;
        public List<GamePreviewModel> Games
        {
            get => _games;
            set => SetProperty(ref _games, value);
        }

        public ICommand ButtonCommand { get; set; }
        private GameImportService _gameImportService;
    }
}
