using AllOrNothing.Contracts.Services;
using AllOrNothing.Helpers;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Windows.Input;

namespace AllOrNothing.ViewModels
{
    public class AllOrNothingViewModel : ViewModelBase
    {
        public AllOrNothingViewModel(INavigationViewService navigationViewService)
            : base(navigationViewService)
        {
        }

        //public override void OnNavigatedTo(object param)
        //{

        //}

        public event EventHandler<NavigateToEventargs> NavigateTo;

        private ICommand _newGameCommand;
        public ICommand NewGameCommand => _newGameCommand ?? (_newGameCommand = new RelayCommand(NewGameClicked));

        private ICommand _openStatsCommand;
        public ICommand OpenStatsCommand => _openStatsCommand ?? (_openStatsCommand = new RelayCommand(OpenStatsPage));


        private ICommand _openQuestionSeriesCommand;
        public ICommand OpenQuestionSeriesCommand => _openQuestionSeriesCommand ?? (_openQuestionSeriesCommand = new RelayCommand(OpenQuestionSeries));

        private ICommand _openPlayeraddingPageCommand;
        public ICommand OpenPlayeraddingPageCommand => _openPlayeraddingPageCommand ?? (_openPlayeraddingPageCommand = new RelayCommand(OpenPlayeraddingPage));

        private void OpenPlayeraddingPage()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(PlayerAddingViewModel), PageName = "Játékosok" });
        }

        private void OpenQuestionSeries()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(QuestionSeriesPageViewModel), PageName = "Kérdéssorok" });
        }

        private void OpenStatsPage()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(StatisticsViewModel), PageName = "Statisztikák" });
        }

        public XamlRoot PageXamlRoot;

        private async void NewGameClicked()
        {
            var vm = Ioc.Default.GetService<AllOrNothingSettingsViewModel>();
            if(vm.GameInProgress &&
                await PopupManager.ShowDialog(PageXamlRoot, "Új játékor kezd?", "Egy játék jelenleg is aktív! Ha új játékot kezd, a jelenlegi játék összes eredménye elveszik!",
                ContentDialogButton.Primary, "Igen", "Mégse") != ContentDialogResult.Primary)
            {
                return;
            }
            Ioc.Default.GetService<ScoreBoardPageViewModel>().IsMenuButtonVisible = false;
            vm.ResetSettings();
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(AllOrNothingSettingsViewModel), PageName = "Beállítások" });
        }
    }
}
