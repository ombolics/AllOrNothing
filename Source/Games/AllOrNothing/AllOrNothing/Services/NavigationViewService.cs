﻿using System;
using System.Collections.Generic;
using System.Linq;

using AllOrNothing.Contracts.Services;
using AllOrNothing.Helpers;
using AllOrNothing.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Services
{
    public class NavigationViewService : INavigationViewService
    {
        private readonly INavigationService _navigationService;
        private readonly IPageService _pageService;
        private NavigationView _navigationView;

        public IList<object> MenuItems
            => _navigationView.MenuItems;

        public object SettingsItem
            => _navigationView.SettingsItem;

        public NavigationViewService(INavigationService navigationService, IPageService pageService)
        {
            _navigationService = navigationService;
            _pageService = pageService;
        }

        public void Initialize(NavigationView navigationView)
        {
            _navigationView = navigationView;
            _navigationView.BackRequested += OnBackRequested;
            _navigationView.ItemInvoked += OnItemInvoked;
        }

        public void ShowAllPage()
        {
            foreach (NavigationViewItem navItem in _navigationView.MenuItems)
            {
                navItem.Visibility = Visibility.Visible;
            }
        }

        public void HideAllPageExcept(List<string> itemContents)
        {
            foreach (NavigationViewItem navItem in _navigationView.MenuItems)
            {
                if (!itemContents.Contains(navItem.Content.ToString())) 
                {
                    navItem.Visibility = Visibility.Collapsed;
                }
            }
        }

        public void SetNavItemVisibility(string itemContent, bool value)
        {
            var item = _navigationView.MenuItems
                .Where(i => (i as NavigationViewItem).Content.ToString() == itemContent)
                .First();
            if (item != null)
            {
                NavigationViewItem navItem = item as NavigationViewItem;
                navItem.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Adds a new nav item to the navbar.
        /// </summary>
        /// <param name="item"> </param>
        /// <param name="pageKey"></param>
        /// <param name="navigateToItem"></param>
        public void AddNavItem(NavigationViewItem item, Type vmType)
        {
            string pageKey = vmType.FullName;
            var result = _navigationView.MenuItems.Count(i => (i as NavigationViewItem).Content == item.Content);
            //dont want to add an element multiple times
            if (result == 0 && _pageService.IsPageKey(pageKey))
            {
                NavHelper.SetNavigateTo(item, pageKey);
                _navigationView.MenuItems.Add(item);
            }
        }


        public void UnregisterEvents()
        {
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
        }

        public NavigationViewItem GetSelectedItem(Type pageType)
            => GetSelectedItem(_navigationView.MenuItems, pageType);

        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
            => _navigationService.GoBack();

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                _navigationService.NavigateTo(typeof(SettingsViewModel).FullName);
            }
            else
            {
                var selectedItem = args.InvokedItemContainer as NavigationViewItem;
                var pageKey = selectedItem.GetValue(NavHelper.NavigateToProperty) as string;

                if (pageKey != null)
                {
                    _navigationService.NavigateTo(pageKey);
                }
            }
        }

        private NavigationViewItem GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (var item in menuItems.OfType<NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    return item;
                }

                var selectedChild = GetSelectedItem(item.MenuItems, pageType);
                if (selectedChild != null)
                {
                    return selectedChild;
                }
            }

            return null;
        }

        private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
            if (pageKey != null)
            {
                return _pageService.GetPageType(pageKey) == sourcePageType;
            }

            return false;
        }
    }
}