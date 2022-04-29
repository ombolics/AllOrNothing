using AllOrNothing.Contracts.Services;
using AllOrNothing.Contracts.ViewModels;
using AllOrNothing.Helpers;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Windows.Input;

namespace AllOrNothing.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        #region Fields
        private ICommand _newGameCommand;
        private ICommand _openStatsCommand;
        private ICommand _openQuestionSeriesCommand;
        private ICommand _openPlayeraddingPageCommand;
        #endregion

        #region Constructors
        public MainMenuViewModel(INavigationViewService navigationViewService)
            : base(navigationViewService)
        {
        }
        #endregion

        #region Properties
        public XamlRoot PageXamlRoot { get; set; }
        public ICommand NewGameCommand => _newGameCommand ?? (_newGameCommand = new RelayCommand(NewGameClicked));
        public ICommand OpenStatsCommand => _openStatsCommand ?? (_openStatsCommand = new RelayCommand(OpenStatsPage));
        public ICommand OpenQuestionSeriesCommand => _openQuestionSeriesCommand ?? (_openQuestionSeriesCommand = new RelayCommand(OpenQuestionSeries));
        public ICommand OpenPlayeraddingPageCommand => _openPlayeraddingPageCommand ?? (_openPlayeraddingPageCommand = new RelayCommand(OpenPlayeraddingPage));
        #endregion

        #region Methods
        private void OpenPlayeraddingPage()
        {
            RaiseNavigateTo(new NavigateToEventArgs { PageVM = typeof(PlayerAddingViewModel), PageName = "Játékosok" });
        }

        private void OpenQuestionSeries()
        {
            RaiseNavigateTo(new NavigateToEventArgs { PageVM = typeof(QuestionSerieEditorViewModel), PageName = "Kérdéssorok" });
        }

        private void OpenStatsPage()
        {
            RaiseNavigateTo(new NavigateToEventArgs { PageVM = typeof(StatisticsViewModel), PageName = "Statisztikák" });
        }

        private async void NewGameClicked()
        {
            var vm = Ioc.Default.GetService<GameSettingsViewModel>();
            if (vm.GameInProgress &&
                await PopupManager.ShowDialog(PageXamlRoot, "Új játékor kezd?", "Egy játék jelenleg is aktív! Ha új játékot kezd, a jelenlegi játék összes eredménye elveszik!",
                ContentDialogButton.Primary, "Igen", "Mégse") != ContentDialogResult.Primary)
            {
                return;
            }
            Ioc.Default.GetService<ScoreBoardPageViewModel>().IsMenuButtonVisible = false;
            vm.ResetSettings();
            RaiseNavigateTo(new NavigateToEventArgs { PageVM = typeof(GameSettingsViewModel), PageName = "Beállítások" });
        }
        #endregion
    }
}
