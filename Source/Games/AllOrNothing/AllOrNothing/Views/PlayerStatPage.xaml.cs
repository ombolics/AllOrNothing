using AllOrNothing.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AllOrNothing.Views
{

    public sealed partial class PlayerStatPage : Page
    {
        public PlayerStatViewModel ViewModel { get; set; } = Ioc.Default.GetService<PlayerStatViewModel>();
        public PlayerStatPage()
        {
            this.InitializeComponent();
        }
    }
}
