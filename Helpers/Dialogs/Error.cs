using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Helpers.Dialogs
{
    public static class Error
    {
        public static async void Show(string content)
        {
            ContentDialog deleteFileDialog = new ContentDialog()
            {
                Title = "Error",
                Content = content,
                PrimaryButtonText = "Ok"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();
        }
    }
}
