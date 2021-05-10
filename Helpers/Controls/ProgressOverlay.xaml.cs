using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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

namespace Helpers.Controls
{
    public sealed partial class ProgressOverlay : UserControl
    {
        public ProgressOverlay()
        {
            this.InitializeComponent();
        }

        #region ProgressObjectProperty

        public ProgressObject ProgressObject
        {
            get { return GetValue(ProgressObjectProperty) as ProgressObject; }
            set { SetValue(ProgressObjectProperty, value); }
        }

        public static readonly DependencyProperty ProgressObjectProperty =
              DependencyProperty.Register(
                  "ProgressObject", typeof(ProgressObject), typeof(HamburgerMenu), new PropertyMetadata(new ProgressObject())
                  );

        #endregion

        #region OpacitytProperty

        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty BackgroundOpacityProperty =
              DependencyProperty.Register(
                  "BackgroundOpacity", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(0.25)
                  );

        #endregion
    }

    public class ProgressObject : ViewModelBase
    {
        private bool _isActive = false;
        public bool IsActive { get { return _isActive; } set { _isActive = value; RaisePropertyChanged("IsActive"); } }

        private string _text = "";
        public string Text { get { return _text; } set { _text = value; RaisePropertyChanged("Text"); } }

        private CancellationTokenSource _cancellationToken;
        public CancellationTokenSource CancellationToken { get { return _cancellationToken; } set { _cancellationToken = value; RaisePropertyChanged("CancellationToken"); RaisePropertyChanged("IsCancellable"); } }

        public bool IsCancellable { get { if (_cancellationToken == null) return false; else return true; } }

        /// <summary>
        /// Shows progress overlay.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="cts"></param>
        public void Show(string text = "", CancellationTokenSource cts = null)
        {
            IsActive = true;
            Text = text;
            CancellationToken = cts;
        }

        /// <summary>
        /// Hides progress overlay.
        /// </summary>
        public void Hide()
        {
            IsActive = false;
        }

        /// <summary>
        /// Cancels progress overlay.
        /// </summary>
        public void Cancel()
        {
            try
            {
                CancellationToken.Cancel();
                Hide();
            }
            catch { }
        }
    }
}
