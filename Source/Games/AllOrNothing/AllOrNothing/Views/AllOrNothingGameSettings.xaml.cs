using AllOrNothing.Data;
using AllOrNothing.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AllOrNothing.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllOrNothingGameSettings : Page
    {
        public AllOrNothingSettingsViewModel ViewModel { get; } = Ioc.Default.GetService<AllOrNothingSettingsViewModel>();
        public AllOrNothingGameSettings()
        {
            this.InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PageXamlRoot = this.Content.XamlRoot;
        }

        public void teamPanel_PlayerDropped(object sender, Player p)
        {

        }

        public async void LoadFromFileClicked()
        {


        }

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{

        //    FileOpenPicker picker = new FileOpenPicker();

        //    //var hwnd = this.As<IWindowNative>().WindowHandle;
        //    var hwnd = WindowNative.GetWindowHandle(App.MainWindow);


        //    WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        //    var file = await picker.PickSingleFileAsync();
        //}
    }
}
