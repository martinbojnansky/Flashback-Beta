using Helpers.Navigation;
using Helpers.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Flashback.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpPage : Page
    {
        public HelpPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Update titlebar backbutton visibility
            NavigationService.UpdateAppViewBackButtonVisibility();
            // Subscribe navigation handler
            SystemNavigationManager.GetForCurrentView().BackRequested += NavigationService.NavigationService_BackRequested;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Unsubscribe navigation handler
            SystemNavigationManager.GetForCurrentView().BackRequested -= NavigationService.NavigationService_BackRequested;           

            base.OnNavigatedFrom(e);
        }
       
        private void PreviewMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        public static void FirstHelp()
        {
            double current = 1.0;
            double version = 0.0;
            try { version = (double)LocalSettingsHelper.GetValue("HelpVersion"); } catch { }
            if(version < current)
            {
                LocalSettingsHelper.SetValue("HelpVersion", current);
                NavigationService.Navigate(typeof(HelpPage));
            }
        }
    }
}
