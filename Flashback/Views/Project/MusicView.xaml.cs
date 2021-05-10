using Flashback.Pages;
using Flashback.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Flashback.Views
{
    public sealed partial class MusicView : UserControl
    {
        public ProjectViewModel ProjectViewModel = ProjectViewModel.Instance;

        public MusicView()
        {
            this.InitializeComponent();
        }

        public void PlayStop()
        {
            try
            {
                // If playing then stop
                if (PreviewMediaElement.CurrentState == MediaElementState.Playing)
                {
                    PreviewMediaElement.Pause();
                    return;
                }
                // Otherwise, create sample and play it
                if (ProjectViewModel.Project.Track.MediaFile != null)
                {
                    var mediaComposition = new MediaComposition();
                    mediaComposition.Clips.Add(MediaClip.CreateFromColor(Windows.UI.Colors.Black, TimeSpan.FromSeconds(3)));
                    ProjectViewModel.AddBackgroundAudioTrackToMediaComposition(mediaComposition, true);
                    PreviewMediaElement.SetMediaStreamSource(mediaComposition.GeneratePreviewMediaStreamSource(1, 1));
                }
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        private void PreviewMediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (PreviewMediaElement.CurrentState)
            {
                case MediaElementState.Playing:
                    PlayStopButton.Content = new SymbolIcon(Symbol.Stop);
                    break;
                case MediaElementState.Paused:
                case MediaElementState.Closed:
                    PlayStopButton.Content = new SymbolIcon(Symbol.Play);
                    break;
            }
        }
    }
}
