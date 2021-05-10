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
    public sealed partial class SlideshowClipView : UserControl
    {
        public SlideshowClip SlideshowClip
        {
            get { return GetValue(SlideshowClipProperty) as SlideshowClip; }
            set { SetValue(SlideshowClipProperty, value); }
        }

        public static readonly DependencyProperty SlideshowClipProperty =
              DependencyProperty.Register(
                  nameof(SlideshowClip), typeof(SlideshowClip), typeof(SlideshowClipView), new PropertyMetadata(null)
                  );

        public SlideshowClipView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event for opening flyout menu of sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFlyoutMenu(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        /// <summary>
        /// Event for opening flyout menu of sender.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarFlyoutButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarButton;
            button.Flyout.ShowAt(button);
        }

        #region Preview Video

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
                _firstLoad = true;
                PreviewMediaElement.SetMediaStreamSource(await SlideshowClip.GetPreviewVideo());
            }
            catch
            {
                PreviewMediaElementProgressObject.Hide();
            }
        }

        private bool _firstLoad;

        /// <summary>
        /// Hide progress bar on complete and play slideshow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (!_firstLoad)
                PreviewMediaElement.Play();
            else
                _firstLoad = false;

            PreviewMediaElementProgressObject.Hide();
        }

        /// <summary>
        /// Update preview when play is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PreviewMediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch(PreviewMediaElement.CurrentState)
            {
                case MediaElementState.Playing:
                    if(SlideshowClip.ImagesDurationOrOrderChanged)
                    {
                        PreviewMediaElementProgressObject.Show("Applying changes");
                        PreviewMediaElement.SetMediaStreamSource(await SlideshowClip.GetPreviewVideo());
                    }
                    break;
                case MediaElementState.Paused:
                    await Task.Delay(1000);
                    PreviewMediaElement.Position = TimeSpan.FromSeconds(0);
                    break;          
            }
        }

        #endregion
    }
}
