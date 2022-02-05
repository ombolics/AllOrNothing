﻿using System;
using Microsoft.UI.Xaml.Controls;
using AllOrNothing.Contracts.Services;
using AllOrNothing.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Navigation;

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

   

        public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
        {
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigated;
            NavigationViewService = navigationViewService;

            AllOrNothingViewModel = Ioc.Default.GetService<AllOrNothingViewModel>();
            AllOrNothingViewModel.NavigateTo += On_NavigateTo;

            AllOrNothingSettingsViewModel = Ioc.Default.GetService<AllOrNothingSettingsViewModel>();
            AllOrNothingSettingsViewModel.NavigateTo += On_NavigateTo;
        }

        private void On_NavigateTo(object sender, NavigateToEventargs e)
        {
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
