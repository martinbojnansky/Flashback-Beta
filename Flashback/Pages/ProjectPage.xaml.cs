using Flashback.ViewModels;
using Flashback.Views;
using Helpers.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Flashback.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProjectPage : Page
    {
        public static ProjectPage Current;
        public ProjectViewModel ProjectViewModel = ProjectViewModel.Instance;

        public NavigationLink[] NavigationLinks =
        {
            new NavigationLink("Music", Symbol.Audio, new MusicView()),
            new NavigationLink("Videos & Photos", Symbol.SlideShow, new ClipsView()),
            new NavigationLink("Effects", Symbol.Edit, new EffectsView()),
            new NavigationLink("Titles", Symbol.Font, new TitlesView()),
            new NavigationLink("Preview", Symbol.Play, new PreviewView()),
            new NavigationLink("Save", Symbol.Save, new SaveView())
        };

        public ProjectPage()
        {
            this.InitializeComponent();

            Current = this;
            App.Current.Suspending += Current_Suspending;
        }

        /// <summary>
        /// Save project before suspending in case of app is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // Save project
            //await ProjectViewModel.SaveProjectAsync();
            deferral.Complete();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Update titlebar backbutton visibility
            NavigationService.UpdateAppViewBackButtonVisibility();
            // Subscribe navigation handler
            SystemNavigationManager.GetForCurrentView().BackRequested += ProjectPage_BackRequested;

            // Load project
            await ProjectViewModel.LoadProjectAsync();

            base.OnNavigatedTo(e);
        }

        private async void ProjectPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (await ProjectViewModel.DiscardProject())
            {
                var frame = (Frame)Window.Current.Content;
                if (frame.CanGoBack)
                    frame.GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Unsubscribe navigation handler
            SystemNavigationManager.GetForCurrentView().BackRequested -= ProjectPage_BackRequested;

            // Save project
            //await ProjectViewModel.SaveProjectAsync();              

            base.OnNavigatedFrom(e);
        }

        private async void FeedbackAppBarButton_Click(object sender, RoutedEventArgs e) { await FeedbackContentDialog.Show(); }

        private void HelpAppBarButton_Click(object sender, RoutedEventArgs e) { NavigationService.Navigate(typeof(HelpPage));  }
    }
}
