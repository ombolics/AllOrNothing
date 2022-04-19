using AllOrNothing.Contracts.Services;
using AllOrNothing.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AllOrNothing.ViewModels
{
    public class ViewModelBase : ObservableRecipient, INavigationAware
    {
        #region Private Fields
        private INavigationViewService _navigationViewService;
        private bool _isMenuButtonVisible;
        private ICollection<Type> _reachablePages;
        #endregion

        #region Properties
        public bool IsMenuButtonVisible
        {
            get => _isMenuButtonVisible;
            set => _isMenuButtonVisible = value;
        }
        public ICollection<Type> ReachablePages
        {
            get => _reachablePages;
            set => SetProperty(ref _reachablePages, value);
        }
        #endregion

        #region Constructors
        public ViewModelBase(INavigationViewService navigationViewService)
        {
            _navigationViewService = navigationViewService;
        }
        #endregion

        #region Public methods
        public bool IsReachable() => IsMenuButtonVisible;
        #endregion

        #region Virtual methods
        public virtual void OnNavigatedFrom()
        {
        }

        public virtual void OnNavigatedTo()
        {
            IsMenuButtonVisible = true;
            _navigationViewService.UpdatenavigationMenu(ReachablePages, this.GetType());
        }
        #endregion
    }
}
