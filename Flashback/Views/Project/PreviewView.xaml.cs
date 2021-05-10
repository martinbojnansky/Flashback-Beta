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
    public sealed partial class PreviewView : UserControl
    {
        public ProjectViewModel ProjectViewModel = ProjectViewModel.Instance;

        public PreviewView()
        {
            this.InitializeComponent();
            this.Loaded += PreviewView_Loaded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PreviewView_Loaded(object sender, RoutedEventArgs e)
        {
            var mediaStreamSource = await ProjectViewModel.Instance.GetPreviewAsync();
            if(mediaStreamSource != null)
                PreviewMediaElement.SetMediaStreamSource(mediaStreamSource);
        }
    }
}
