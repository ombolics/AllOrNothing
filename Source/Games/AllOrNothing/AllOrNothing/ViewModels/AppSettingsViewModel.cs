using AllOrNothing.Contracts.Services;
using AllOrNothing.Converters;
using AllOrNothing.Helpers;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI;

namespace AllOrNothing.ViewModels
{
    public class AppSettingsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IThemeSelectorService _themeSelectorService;
        private ElementTheme _elementTheme;
        private string _versionDescription;
        private ICommand _switchThemeCommand;
        private ICommand _theme1Command;
        private List<UITheme> _defaultThemes;
        private HexToColorConverter _colorConverter;
        #endregion

        #region Contructors
        public AppSettingsViewModel(INavigationViewService navigationViewService, IThemeSelectorService themeSelectorService)
            : base(navigationViewService)
        {
            _themeSelectorService = themeSelectorService;
            _elementTheme = default(ElementTheme);
            _colorConverter = new HexToColorConverter();
            VersionDescription = GetVersionDescription();
            _defaultThemes = new List<UITheme>
            {
                new UITheme
                {
                    MainColor1 = (Color)_colorConverter.Convert("#578791", null, null, null),
                    MainColor2 = (Color)_colorConverter.Convert("#91c4b1", null, null, null),
                    MainColor3 = (Color)_colorConverter.Convert("#cedfcc", null, null, null),
                    MainColor4 = (Color)_colorConverter.Convert("#e0ebde", null, null, null),
                },
                new UITheme
                {
                    MainColor1 = (Color)_colorConverter.Convert("#6E85B7", null, null, null),
                    MainColor2 = (Color)_colorConverter.Convert("#B2C8DF", null, null, null),
                    MainColor3 = (Color)_colorConverter.Convert("#C4D7E0", null, null, null),
                    MainColor4 = (Color)_colorConverter.Convert("#F8F9D7", null, null, null),
                }
            };
        }
        #endregion

        #region Properties
        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { SetProperty(ref _elementTheme, value); }
        }
        public string VersionDescription
        {
            get { return _versionDescription; }

            set { SetProperty(ref _versionDescription, value); }
        }
        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            if (ElementTheme != param)
                            {
                                ElementTheme = param;
                                //await _themeSelectorService.SetThemeAsync(param);
                            }
                        });
                }

                return _switchThemeCommand;
            }
        }

        public ICommand Theme1Command => _theme1Command ?? new RelayCommand(Theme1);

        bool asd = false;
        private void Theme1()
        {
            _themeSelectorService.SetThemeAsync(asd ? _defaultThemes[0] : _defaultThemes[1]);
        }
        #endregion

        #region Methods
        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
        #endregion
    }
}
