using AllOrNothing.Contracts.Services;
using AllOrNothing.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AllOrNothing.ViewModels
{
    public class ViewModelBase : ObservableRecipient, INavigationAware
    {
        private INavigationViewService _navigationViewService;
        private bool _isMenuButtonVisible;
        public bool IsMenuButtonVisible
        {
            get => _isMenuButtonVisible;
            set => _isMenuButtonVisible = value;
        }

        public ViewModelBase(INavigationViewService navigationViewService)
        {
            _navigationViewService = navigationViewService;
        }
        private ICollection<Type> _reachablePages;
        public ICollection<Type> ReachablePages
        {
            get => _reachablePages;
            set => SetProperty(ref _reachablePages, value);
        }

        public void OnNavigatedFrom()
        {

        }

        public virtual void OnNavigatedTo(object parameter)
        {
            IsMenuButtonVisible = true;
            _navigationViewService.UpdatenavigationMenu(ReachablePages, this.GetType());
        }

        public bool IsReachable() => IsMenuButtonVisible;
    }
}
