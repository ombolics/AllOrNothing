using AllOrNothing.Contracts.Services;
using AllOrNothing.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AllOrNothing.ViewModels
{
    /// <summary>
    /// Base class for page viewmodels with virtual methods that can be overridden 
    /// </summary>
    public class ViewModelBase : ObservableRecipient, INavigationAware
    {
        #region Private Fields
        private INavigationViewService _navigationViewService;
        private bool _isMenuButtonVisible;
        private ICollection<Type> _reachablePages;
        #endregion

        #region Constructors
        public ViewModelBase(INavigationViewService navigationViewService)
        {
            _navigationViewService = navigationViewService;
        }
        #endregion

        #region INavigationAware implementation (virtual)
        /// <summary>
        /// Method to be executed after navigating from the page
        /// </summary>
        public virtual void OnNavigatedFrom()
        {
        }

        /// <summary>
        /// Method to be executed after navigating to the page
        /// </summary>
        public virtual void OnNavigatedTo()
        {
            IsMenuButtonVisible = true;
            _navigationViewService.UpdatenavigationMenu(ReachablePages, this.GetType());
        }
        #endregion

        #region Properties
        /// <summary>
        /// Indicates whether the page appears in the navigation bar
        /// </summary>
        public bool IsMenuButtonVisible
        {
            get => _isMenuButtonVisible;
            set => _isMenuButtonVisible = value;
        }

        /// <summary>
        /// Collection of each page that is reachable from this page through the navigation bar.
        /// All page is reachable = null
        /// No page is reachable = empty collection
        /// </summary>
        public ICollection<Type> ReachablePages
        {
            get => _reachablePages;
            set => SetProperty(ref _reachablePages, value);
        }
        #endregion

        #region Public methods
        public bool IsReachable() => IsMenuButtonVisible;
        #endregion
    }
}
