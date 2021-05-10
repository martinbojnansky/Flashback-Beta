using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Flashback.Views
{
    public sealed partial class FeedbackContentDialog : ContentDialog
    {
        public FeedbackContentDialog()
        {
            this.InitializeComponent();
        }

        public static async Task Show()
        {   
            var dialog = new FeedbackContentDialog();
            var result = await dialog.ShowAsync();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Do not cancel dialog
            args.Cancel = true;
            this.IsEnabled = false;

            try
            {
                // Get category id
                var categoryId = 2;
                if ((bool)ProblemRadioButton.IsChecked)
                    categoryId = 1;

                var client = new HttpClient();
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Title", TitleTextBox.Text),
                    new KeyValuePair<string, string>("Description", DescriptionTextBox.Text),
                    new KeyValuePair<string, string>("DateCreated", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                    new KeyValuePair<string, string>("CategoryId", categoryId.ToString()),
                    new KeyValuePair<string, string>("StatusId", "1")
                });
                var response = await client.PostAsync("https://flashbackfeedbackapi.azurewebsites.net/api/feedbacks", content);
                response.EnsureSuccessStatusCode();
                this.Hide();
            }
            catch { ErrorTextBlock.Visibility = Visibility.Visible; }
            finally { this.IsEnabled = true; }
        }

        // Enables send button
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsPrimaryButtonEnabled = IsInputValid();
        }

        // Validates input
        private bool IsInputValid()
        {
            
            if (!string.IsNullOrEmpty(TitleTextBox.Text))
                return true;
            else
                return false;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }
    }
}
