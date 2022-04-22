using AllOrNothing.Contracts.Services;
using AllOrNothing.Contracts.ViewModels;
using AllOrNothing.Helpers;
using AllOrNothing.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Text;

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
            var result = _navigationView.MenuItems.SingleOrDefault(i => (i as NavigationViewItem).Content == item.Content) as NavigationViewItem;
            var dbg = _navigationView.MenuItems.ToList();

            if (result != null)
                (result as NavigationViewItem).Visibility = Visibility.Visible;

            item.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            item.FontSize = (double)App.Current.Resources["LargeFontSize"];
            item.FontWeight = new FontWeight(700);



            //dont want to add an element multiple times
            if (result == null && _pageService.IsPageKey(pageKey))
            {
                NavHelper.SetNavigateTo(item, pageKey);
                item.Visibility = Visibility.Visible;
                _navigationView.MenuItems.Add(item);
            }
        }

        public bool MenuPointExists(object content)
        {
            return _navigationView.MenuItems.Count(i => (i as NavigationViewItem).Content.ToString() == content.ToString()) > 0;
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

        private void SetAllNavigationItemVisibility(bool value)
        {
            foreach (var item in MenuItems.OfType<NavigationViewItem>())
            {
                item.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        private Type GetItemKeyType(NavigationViewItem item)
        {
            return Type.GetType(item.GetValue(NavHelper.NavigateToProperty).ToString());
        }
        private void ShowNavigationViewItem(NavigationViewItem item)
        {
            INavigationAware vm = GetItemKeyType(item).Name switch
            {
                nameof(AllOrNothingGameViewModel) => Ioc.Default.GetService<AllOrNothingGameViewModel>(),
                nameof(AllOrNothingSettingsViewModel) => Ioc.Default.GetService<AllOrNothingSettingsViewModel>(),
                nameof(AllOrNothingViewModel) => Ioc.Default.GetService<AllOrNothingViewModel>(),
                nameof(PlayerAddingViewModel) => Ioc.Default.GetService<PlayerAddingViewModel>(),
                nameof(QuestionSeriesPageViewModel) => Ioc.Default.GetService<QuestionSeriesPageViewModel>(),
                nameof(ScoreBoardPageViewModel) => Ioc.Default.GetService<ScoreBoardPageViewModel>(),
                nameof(SettingsViewModel) => Ioc.Default.GetService<SettingsViewModel>(),

                _ => throw new Exception()
            };

            item.Visibility = vm.IsReachable() ? Visibility.Visible : Visibility.Collapsed;
        }
        public void UpdatenavigationMenu(ICollection<Type> enabledPages, Type sender)
        {
            //show them if they agree
            if (enabledPages == null)
            {
                foreach (var item in MenuItems.OfType<NavigationViewItem>())
                {
                    ShowNavigationViewItem(item);
                }
                return;
            }

            //wanish all
            if (enabledPages.Count == 0)
            {
                SetAllNavigationItemVisibility(false);
                var sendersItem = MenuItems.OfType<NavigationViewItem>().Single(item => sender == Type.GetType(item.GetValue(NavHelper.NavigateToProperty).ToString()));
                sendersItem.Visibility = Visibility.Visible;
                return;
            }

            //only show those, who both agree, and wanted to be shown
            foreach (var item in MenuItems.OfType<NavigationViewItem>())
            {
                var type = Type.GetType(item.GetValue(NavHelper.NavigateToProperty).ToString());
                if (enabledPages.Contains(type))
                {
                    ShowNavigationViewItem(item);
                }
                else
                {
                    item.Visibility = type == sender ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
    }
}
