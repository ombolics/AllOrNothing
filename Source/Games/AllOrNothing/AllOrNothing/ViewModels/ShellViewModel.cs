using AllOrNothing.Contracts.Services;
using AllOrNothing.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace AllOrNothing.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        #region Fields
        private bool _isBackEnabled;
        private object _selected;

        //page viewModels
        private GameViewModel _gameViewModel;
        private GameSettingsViewModel _allOrNothingSettingsViewModel;
        private MainMenuViewModel _allOrNothingViewModel;
        private PlayerAddingViewModel _playerAddingViewModel;
        private QuestionSerieEditorViewModel _questionSeriesPageViewModel;
        #endregion

        #region Constructors
        public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
        {
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigated;
            NavigationViewService = navigationViewService;

            AllOrNothingViewModel = Ioc.Default.GetService<MainMenuViewModel>();
            AllOrNothingViewModel.NavigateTo += On_NavigateTo;

            PlayerAddingViewModel = Ioc.Default.GetService<PlayerAddingViewModel>();
            PlayerAddingViewModel.NavigateTo += On_NavigateTo;

            QuestionSeriesPageViewModel = Ioc.Default.GetService<QuestionSerieEditorViewModel>();
            QuestionSeriesPageViewModel.NavigateTo += On_NavigateTo;

            AllOrNothingSettingsViewModel = Ioc.Default.GetService<GameSettingsViewModel>();
            AllOrNothingSettingsViewModel.NavigateTo += On_NavigateTo;

            GameViewModel = Ioc.Default.GetRequiredService<GameViewModel>();

        }
        #endregion

        #region Properties
        //frame wrappers
        public object Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }
        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { SetProperty(ref _isBackEnabled, value); }
        }

        public GameSettingsViewModel AllOrNothingSettingsViewModel
        {
            get => _allOrNothingSettingsViewModel;
            set => SetProperty(ref _allOrNothingSettingsViewModel, value);
        }

        public MainMenuViewModel AllOrNothingViewModel
        {
            get => _allOrNothingViewModel;
            set => SetProperty(ref _allOrNothingViewModel, value);
        }

        public PlayerAddingViewModel PlayerAddingViewModel
        {
            get => _playerAddingViewModel;
            set => SetProperty(ref _playerAddingViewModel, value);
        }

        public QuestionSerieEditorViewModel QuestionSeriesPageViewModel
        {
            get => _questionSeriesPageViewModel;
            set => SetProperty(ref _questionSeriesPageViewModel, value);
        }

        public GameViewModel GameViewModel
        {
            get => _gameViewModel;
            set => SetProperty(ref _gameViewModel, value);
        }

        public INavigationService NavigationService { get; }

        public INavigationViewService NavigationViewService { get; }
        #endregion

        #region EventHandler
        private void On_NavigateTo(object sender, NavigateToEventargs e)
        {
            if (!NavigationViewService.MenuPointExists(e.PageName))
                NavigationViewService.AddNavItem(new NavigationViewItem { Content = e.PageName }, e.PageVM);

            NavigationService.NavigateTo(e.PageVM.FullName);
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            if (e.SourcePageType == typeof(AppSettingsPage))
            {
                Selected = NavigationViewService.SettingsItem;
                return;
            }

            var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
            if (selectedItem != null)
            {
                Selected = selectedItem;
            }
        }
        #endregion
    }
}
