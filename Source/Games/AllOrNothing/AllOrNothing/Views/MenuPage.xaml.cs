using AllOrNothing.ViewModels;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Views
{
    public sealed partial class MenuPage : Page
    {
        public MainMenuViewModel ViewModel { get; set; } = Ioc.Default.GetService<MainMenuViewModel>();

        public MenuPage()
        {
            ViewModel = Ioc.Default.GetService<MainMenuViewModel>();
            InitializeComponent();
        }

        private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.PageXamlRoot = XamlRoot;
        }
    }
}
