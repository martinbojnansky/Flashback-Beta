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
    public sealed partial class Caption : UserControl
    {
        public Caption()
        {
            this.InitializeComponent();
        }

        #region CaptionText

        public string CaptionText
        {
            get { return GetValue(CaptionTextProperty) as string; }
            set { SetValue(CaptionTextProperty, value); }
        }

        public static readonly DependencyProperty CaptionTextProperty =
              DependencyProperty.Register(
                  "CaptionText", typeof(string), typeof(Caption), new PropertyMetadata("")
                  );

        #endregion

        #region CaptionForeground

        public SolidColorBrush CaptionForeground
        {
            get { return GetValue(CaptionForegroundProperty) as SolidColorBrush; }
            set { SetValue(CaptionForegroundProperty, value); }
        }

        public static readonly DependencyProperty CaptionForegroundProperty =
              DependencyProperty.Register(
                  "CaptionForeground", typeof(SolidColorBrush), typeof(Caption), new PropertyMetadata(new SolidColorBrush(new Color() { R = 136, G = 136, B = 136, A = 255 }))
                  );

        #endregion

        #region CaptionMargin
        public Thickness CaptionMargin
        {
            get { return (Thickness)GetValue(CaptionMarginProperty); }
            set { SetValue(CaptionMarginProperty, value); }
        }

        public static readonly DependencyProperty CaptionMarginProperty =
              DependencyProperty.Register(
                  "CaptionMargin", typeof(Thickness), typeof(HamburgerMenu), new PropertyMetadata(new Thickness(0,24,0,0))
                  );

        #endregion
    }
}
