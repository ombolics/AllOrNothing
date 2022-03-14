using CommunityToolkit.Mvvm.ComponentModel;
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

        private void OpenQuestionSeries()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(QuestionSeriesViewModel), PageName = "Kérdéssorok" });
        }

        private void OpenStatsPage()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(StatisticsViewModel), PageName = "Statisztikák" });
        }
        private void NewGameClicked()
        {
            NavigateTo?.Invoke(this, new NavigateToEventargs { PageVM = typeof(AllOrNothingSettingsViewModel), PageName = "Beállítások" });
        }
    }
}
