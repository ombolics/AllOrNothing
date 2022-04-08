using AllOrNothing.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AllOrNothing.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerAddingPage : Page
    {
        public PlayerAddingViewModel ViewModel { get; } = Ioc.Default.GetService<PlayerAddingViewModel>();
        public PlayerAddingPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PageXamlRoot = this.Content.XamlRoot;
        }

        private void OnNavigatedFrom(NavigatingCancelEventArgs e)
        {

        }
    }
}
