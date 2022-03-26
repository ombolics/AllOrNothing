using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;

namespace AllOrNothing.ViewModels
{
    public class AllOrNothingViewModel : ObservableRecipient
    {
        public AllOrNothingViewModel()
        {
        }

        public event EventHandler<NavigateToEventargs> NavigateTo;

        private ICommand _newGameCommand;
        public ICommand NewGameCommand => _newGameCommand ?? (_newGameCommand = new RelayCommand(NewGameClicked));

        private ICommand _openStatsCommand;
        public ICommand OpenStatsCommand => _openStatsCommand ?? (_openStatsCommand = new RelayCommand(OpenStatsPage));


        private ICommand _openQuestionSeriesCommand;
        public ICommand OpenQuestionSeriesCommand => _openQuestionSeriesCommand ?? (_openQuestionSeriesCommand = new RelayCommand(OpenQuestionSeries));

        private ICommand _openPlayeraddingPageCommand;
        public ICommand OpenPlayeraddingPageCommand => _openPlayeraddingPageCommand ?? (_openQuestionSeriesCommand = new RelayCommand(OpenPlayeraddingPage));

        private void OpenPlayeraddingPage()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(PlayerAddingViewModel), PageName = "Új játákos" });
        }

        private void OpenQuestionSeries()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(QuestionSeriesPageViewModel), PageName = "Kérdéssorok" });
        }

        private void OpenStatsPage()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(StatisticsViewModel), PageName = "Statisztikák" });
        }
        private void NewGameClicked()
        {
            var vm = Ioc.Default.GetService<AllOrNothingSettingsViewModel>();
            vm.ResetSettings();
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(AllOrNothingSettingsViewModel), PageName = "Beállítások" });
        }
    }
}
