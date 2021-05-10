using Flashback.Models;
using Flashback.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ClipsView : UserControl
    {
        public ProjectViewModel ProjectViewModel = ProjectViewModel.Instance;
        public static ClipsView Current;

        public ClipsView()
        {
            this.InitializeComponent();
            Current = this;
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
        /// Select active clip in gridview.
        /// </summary>
        /// <param name="clip"></param>
        public void SelectClip(Clip clip)
        {
            try { ClipsGridView.SelectedItem = clip; } catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }
    }
}
