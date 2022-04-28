using AllOrNothing.Contracts.Services;
using AllOrNothing.Helpers;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System.Windows.Input;
using Windows.ApplicationModel;

namespace AllOrNothing.ViewModels
{
    public class AppSettingsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IThemeSelectorService _themeSelectorService;
        private ElementTheme _elementTheme;
        private string _versionDescription;
        private ICommand _switchThemeCommand;
        #endregion

        #region Contructors
        public AppSettingsViewModel(INavigationViewService navigationViewService, IThemeSelectorService themeSelectorService)
            : base(navigationViewService)
        {
            _themeSelectorService = themeSelectorService;
            _elementTheme = _themeSelectorService.Theme;
            VersionDescription = GetVersionDescription();
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
                                await _themeSelectorService.SetThemeAsync(param);
                            }
                        });
                }

                return _switchThemeCommand;
            }
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
