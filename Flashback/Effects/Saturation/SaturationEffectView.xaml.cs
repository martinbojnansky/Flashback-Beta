using Helpers.ViewModel;
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

namespace Flashback.Effects.Saturation
{
    public sealed partial class SaturationEffectView : UserControl
    {
        public SaturationEffectViewModel ViewModel;

        public SaturationEffectView()
        {
            this.InitializeComponent();
            this.DataContextChanged += SaturationEffectView_DataContextChanged;
        }

        private void SaturationEffectView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var effect = this.DataContext as EffectReference;
            if (effect != null)
            {
                ViewModel = new SaturationEffectViewModel(effect);
            }
        }
    }
}
