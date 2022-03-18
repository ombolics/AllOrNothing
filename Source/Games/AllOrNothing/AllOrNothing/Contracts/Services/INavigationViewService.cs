using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace AllOrNothing.Contracts.Services
{
    public interface INavigationViewService
    {
        IList<object> MenuItems { get; }

        object SettingsItem { get; }

        void Initialize(NavigationView navigationView);

        void UnregisterEvents();

        NavigationViewItem GetSelectedItem(Type pageType);
        void SetNavItemVisibility(string itemContent, bool value);
        void AddNavItem(NavigationViewItem item, Type vmType);
        bool MenuPointExists(object content);
        void HideAllPageExcept(List<string> itemContents);
        void ShowAllPage();
    }
}
