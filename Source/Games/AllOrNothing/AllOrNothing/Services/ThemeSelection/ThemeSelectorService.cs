using AllOrNothing.Contracts.Services;
using AllOrNothing.Helpers;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace AllOrNothing.Services
{

    public class ThemeSelectorService : IThemeSelectorService
    {
        #region Fields
        private const string SettingsKey = "UITheme";

        #endregion

        #region Properties
        public UITheme Theme { get; set; }
        #endregion

        #region Methods
        public async Task InitializeAsync()
        {
            //Theme = await LoadThemeFromSettingsAsync();
            await Task.CompletedTask;
        }

        public async Task SetThemeAsync(UITheme theme)
        {
            Theme = theme;

            await SetRequestedThemeAsync();
            //await SaveThemeInSettingsAsync(Theme);
        }

        public async Task SetRequestedThemeAsync()
        {
            //if (App.MainWindow.Content is FrameworkElement rootElement)
            //{
            //    rootElement.RequestedTheme = Theme;
            //}

            App.Current.Resources["MainColor1"] = Theme.MainColor1;
            App.Current.Resources["MainColor2"] = Theme.MainColor2;
            App.Current.Resources["MainColor3"] = Theme.MainColor3;
            App.Current.Resources["MainColor4"] = Theme.MainColor4;

            App.Current.Resources["NavigationViewTopPaneBackground"] = Theme.MainColor1;

            var theme = App.Current.RequestedTheme == ApplicationTheme.Light ? ApplicationTheme.Dark : ApplicationTheme.Light;
            App.Current.RequestedTheme = theme;

            await Task.CompletedTask;
        }

        private async Task<UITheme> LoadThemeFromSettingsAsync()
        {
            UITheme cacheTheme = new UITheme();
            string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);
            //var themeName = await ApplicationData.Current.LocalSettings.

            //if (!string.IsNullOrEmpty(themeName))
            //{
            //    Enum.TryParse(themeName, out cacheTheme);
            //}

            return cacheTheme;
        }

        private async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString());
        }
        #endregion
    }
}
