using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Helpers.Controls
{
    public sealed partial class Header : UserControl
    {
        public Header()
        {
            this.InitializeComponent();
        }

        #region HeaderText

        public string HeaderText
        {
            get { return GetValue(HeaderTextProperty) as string; }
            set { SetValue(HeaderTextProperty, value); }
        }

        public static readonly DependencyProperty HeaderTextProperty =
              DependencyProperty.Register(
                  "HeaderText", typeof(string), typeof(Header), new PropertyMetadata(null)
                  );

        #endregion

        #region HeaderForeground

        public SolidColorBrush HeaderForeground
        {
            get { return GetValue(HeaderForegroundProperty) as SolidColorBrush; }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        public static readonly DependencyProperty HeaderForegroundProperty =
              DependencyProperty.Register(
                  "HeaderForeground", typeof(SolidColorBrush), typeof(Header), new PropertyMetadata(new SolidColorBrush(new Color() { R = 136, G = 136, B = 136, A = 255 }))
                  );

        #endregion

        #region HeaderMargin
        public Thickness HeaderMargin
        {
            get { return (Thickness)GetValue(HeaderMarginProperty); }
            set { SetValue(HeaderMarginProperty, value); }
        }

        public static readonly DependencyProperty HeaderMarginProperty =
              DependencyProperty.Register(
                  "HeaderMargin", typeof(Thickness), typeof(HamburgerMenu), new PropertyMetadata(new Thickness(0, 24, 0, 0))
                  );

        #endregion
    }
}
