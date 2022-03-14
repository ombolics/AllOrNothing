using AllOrNothing.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AllOrNothing.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuestionSeriesPage : Page
    {
        public QuestionSeriesViewModel ViewModel { get; set; } = Ioc.Default.GetService<QuestionSeriesViewModel>();
        public QuestionSeriesPage()
        {
            this.InitializeComponent();
        }
    }
}
