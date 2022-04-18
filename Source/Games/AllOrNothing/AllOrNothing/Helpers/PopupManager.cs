using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace AllOrNothing.Helpers
{
    public static class PopupManager
    {
        public static async Task<ContentDialogResult> ShowDialog(XamlRoot xamlRoot, string title, string content, ContentDialogButton defaultButton, string primaryButtonText, string closeButtonText)
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = xamlRoot,
                Title = title,
                Content = content,
                DefaultButton = defaultButton,
                PrimaryButtonText = primaryButtonText,
                CloseButtonText = closeButtonText,
            };
            dialog.Background = new SolidColorBrush((Color)App.Current.Resources["NeutralColor1"]);
            dialog.Foreground = new SolidColorBrush((Color)App.Current.Resources["MainColor1"]);
            dialog.Resources["ButtonBackground"] = new SolidColorBrush((Color)App.Current.Resources["MainColor4"]);

            return await dialog.ShowAsync(ContentDialogPlacement.Popup);
        }
    }
}
