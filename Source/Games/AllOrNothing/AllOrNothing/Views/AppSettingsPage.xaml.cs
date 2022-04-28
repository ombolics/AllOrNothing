using AllOrNothing.ViewModels;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Views
{
    // TODO WTS: Change the URL for your privacy policy, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class AppSettingsPage : Page
    {
        public AppSettingsViewModel ViewModel { get; }

        public AppSettingsPage()
        {
            ViewModel = Ioc.Default.GetService<AppSettingsViewModel>();
            InitializeComponent();
        }
    }
}
