using Flashback.Models;
using Helpers.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Flashback.Views
{
    public sealed partial class VideoClipView : UserControl
    {
        public VideoClip VideoClip
        {
            get { return GetValue(VideoClipProperty) as VideoClip; }
            set { SetValue(VideoClipProperty, value);  }
        }

        public static readonly DependencyProperty VideoClipProperty =
              DependencyProperty.Register(
                  nameof(VideoClip), typeof(VideoClip), typeof(VideoClipView), new PropertyMetadata(null)
                  );

        public VideoClipView()
        {
            this.InitializeComponent();
            this.Loaded += VideoClipView_Loaded;
        }

        private void VideoClipView_Loaded(object sender, RoutedEventArgs e)
        {
            VideoClip.PropertyChanged += VideoClip_PropertyChanged;
        }

        #region Preview Video

        /// <summary>
        /// Changes preview position when triming slider is moving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoClip_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "EndTime":
                    PreviewMediaElement.Position = TimeSpan.FromSeconds(VideoClip.EndTime);
                    break;
                case "StartTime":
                    PreviewMediaElement.Position = TimeSpan.FromSeconds(VideoClip.StartTime);
                    break;
            }
        }

        /// <summary>
        /// Go back to the start position when triming has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TrimRangeSlider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            await Task.Delay(1500);
            SeekTo(VideoClip.StartTime);
            await VideoClip.UpdatePreviewImageAsync();
        }

        public ProgressObject PreviewMediaElementProgressObject = new ProgressObject();

        /// <summary>
        /// Set media source when preview element is created.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PreviewMediaElement_Loaded(object sender, RoutedEventArgs e)
        {
            PreviewMediaElementProgressObject.Show("Loading");

            try
            {
                PreviewMediaElement.SetMediaStreamSource(await VideoClip.GetPreviewVideo());
            }
            catch
            {
                PreviewMediaElementProgressObject.Hide();
            }
        }

        /// <summary>
        /// Set start position when media source is opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            SeekTo(VideoClip.StartTime);
        }

        // Animate first seek jump
        private void PreviewMediaElement_SeekCompleted(object sender, RoutedEventArgs e)
        {
            PreviewMediaElementProgressObject.Hide();
        }

        /// <summary>
        /// Seeks to specified position of preview source.
        /// </summary>
        /// <param name="seconds"></param>
        private void SeekTo(double seconds)
        {
            try
            {
                if (PreviewMediaElement.CanSeek)
                {
                    PreviewMediaElement.Position = TimeSpan.FromSeconds(seconds);
                }
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Allow bounded preview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PreviewMediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            MediaElement mediaElement = sender as MediaElement;
            switch (mediaElement.CurrentState)
            {
                case MediaElementState.Playing:
                    mediaElement.Position = TimeSpan.FromSeconds(VideoClip.StartTime);
                    while (mediaElement.Position <= TimeSpan.FromSeconds(VideoClip.EndTime)) { await Task.Delay(16); };
                    mediaElement.Pause();
                    break;
                case MediaElementState.Paused:
                    await Task.Delay(1000);
                    PreviewMediaElement.Position = TimeSpan.FromSeconds(VideoClip.StartTime);
                    break;
            }
        }

        #endregion
    }
}
