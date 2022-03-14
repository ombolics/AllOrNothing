using System;
using Microsoft.UI.Xaml.Controls;
using AllOrNothing.Contracts.Services;
using AllOrNothing.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media;

namespace AllOrNothing.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;

        private object _selected;
        public object Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        private AllOrNothingGameViewModel _gameViewModel;

        private AllOrNothingSettingsViewModel _allOrNothingSettingsViewModel;
        public AllOrNothingSettingsViewModel AllOrNothingSettingsViewModel 
        { 
            get => _allOrNothingSettingsViewModel; 
            set => SetProperty(ref _allOrNothingSettingsViewModel, value); 
        }

        private AllOrNothingViewModel _allOrNothingViewModel;
        public AllOrNothingViewModel AllOrNothingViewModel
        {
            get => _allOrNothingViewModel;
            set => SetProperty(ref _allOrNothingViewModel, value);
        }

        public INavigationService NavigationService { get; }

        public INavigationViewService NavigationViewService { get; }

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { SetProperty(ref _isBackEnabled, value); }
        }

        public AllOrNothingGameViewModel GameViewModel 
        {
            get => _gameViewModel;
            set => SetProperty(ref _gameViewModel, value);
        }

        public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
        {
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigated;
            NavigationViewService = navigationViewService;

            AllOrNothingViewModel = Ioc.Default.GetService<AllOrNothingViewModel>();
            AllOrNothingViewModel.NavigateTo += On_NavigateTo;

            AllOrNothingSettingsViewModel = Ioc.Default.GetService<AllOrNothingSettingsViewModel>();
            AllOrNothingSettingsViewModel.NavigateTo += On_NavigateTo;
            AllOrNothingSettingsViewModel.HidePages += On_HidePages;

            GameViewModel = Ioc.Default.GetRequiredService<AllOrNothingGameViewModel>();
            GameViewModel.HidePages += On_HidePages;
            GameViewModel.HidePage += On_HidePage;
        }

        private void On_HidePage(object sender, string e)
        {
            if(!string.IsNullOrEmpty(e))
            {
                NavigationViewService.SetNavItemVisibility(e, false);
            }
        }

        private void On_HidePages(object sender, System.Collections.Generic.List<string> e)
        {
            if(e is null || e.Count == 0)
            {
                NavigationViewService.ShowAllPage();
            }
            else
            {
                NavigationViewService.HideAllPageExcept(e);
            }
        }

        private void On_NavigateTo(object sender, NavigateToEventargs e)
        {
            //TODO close button
            //StackPanel p = new StackPanel();
            //p.Children.Add(new TextBlock { Text = e.PageName });
            //Button b = new Button();
            //Image i = new Image();
            //var uriSource = new Uri();
            //i.Source = new BitmapImage();
            //i.Width = 10;
            //i.Height = 10;
            //i.Stretch = Stretch.UniformToFill;
            //b.Content = i;

            //p.Children.Add(b);


            NavigationViewService.AddNavItem(new NavigationViewItem { Content = e.PageName }, e.PageVM);
            NavigationService.NavigateTo(e.PageVM.FullName);
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsPage))
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
    }
}
