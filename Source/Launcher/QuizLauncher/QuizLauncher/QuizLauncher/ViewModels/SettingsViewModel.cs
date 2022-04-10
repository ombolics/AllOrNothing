using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using QuizLauncher.Contracts.Services;
using QuizLauncher.Contracts.ViewModels;
using QuizLauncher.Helpers;
using QuizLauncher.Models;
using QuizLauncher.Services;
using Windows.ApplicationModel;

namespace QuizLauncher.ViewModels
{
    public class SettingsViewModel : ObservableRecipient, INavigationAware
    {
        public SettingsViewModel(IThemeSelectorService themeSelectorService, GameIOService gameImportService)
        {
            _themeSelectorService = themeSelectorService;
            _elementTheme = _themeSelectorService.Theme;
            VersionDescription = GetVersionDescription();

            _gameImportService = gameImportService;         
        }


        private ObservableCollection<GamePreviewModel> _games;
        public ObservableCollection<GamePreviewModel> Games
        {
            get => _games;
            set => SetProperty(ref _games, value);
        }

        private GameIOService _gameImportService;
        private readonly IThemeSelectorService _themeSelectorService;
        private ElementTheme _elementTheme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { SetProperty(ref _elementTheme, value); }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(Save);

        private ICommand _newGameCommad;
        public ICommand NewGameCommad => _newGameCommad ??= new RelayCommand(NewGame);

        private void NewGame()
        {
            Games.Add(new GamePreviewModel());
        }
        private bool HasContent(GamePreviewModel m)
        {
            return m != null &&
                !string.IsNullOrWhiteSpace(m.Name) &&
                m.Name != "" &&
                !string.IsNullOrWhiteSpace(m.EntryPointLocation) &&
                m.EntryPointLocation != "";
        }
        private void Save()
        {
            _gameImportService.SaveGames(Games.Where(g => HasContent(g)).ToList());
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { SetProperty(ref _versionDescription, value); }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            if (ElementTheme != param)
                            {
                                ElementTheme = param;
                                await _themeSelectorService.SetThemeAsync(param);
                            }
                        });
                }

                return _switchThemeCommand;
            }
        }

       

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        public void OnNavigatedTo(object parameter)
        {
            Games = new ObservableCollection<GamePreviewModel>(_gameImportService.ListAllGame());
        }

        public void OnNavigatedFrom()
        {

        }
    }
}
