using Helpers.Navigation;
using Helpers.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Helpers.Controls
{
    public sealed partial class HamburgerMenu : UserControl
    {
        public HamburgerMenu()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public bool IsPaneOpen
        {
            get { try { return (bool)LocalSettingsHelper.GetValue(nameof(IsPaneOpen)); } catch { return false; } }
            set { try { LocalSettingsHelper.SetValue(nameof(IsPaneOpen), value); } catch { } }
        }

        #region NavigationLinksProperty

        public NavigationLink[] NavigationLinks
        {
            get { return GetValue(NavigationLinksProperty) as NavigationLink[]; }
            set { SetValue(NavigationLinksProperty, value); }
        }

        public static readonly DependencyProperty NavigationLinksProperty =
              DependencyProperty.Register(
                  "NavigationLinks", typeof(NavigationLink[]), typeof(HamburgerMenu), new PropertyMetadata(new NavigationLink[0])
                  );

        #endregion

        #region HeaderBackgroundProperty

        public SolidColorBrush HeaderBackground
        {
            get { return GetValue(HeaderBackgroundProperty) as SolidColorBrush; }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HeaderBackgroundProperty =
              DependencyProperty.Register(
                  "HeaderBackground", typeof(SolidColorBrush), typeof(HamburgerMenu), new PropertyMetadata(null)
                  );

        #endregion

        #region HeaderForegroundProperty

        public SolidColorBrush HeaderForeground
        {
            get { return GetValue(HeaderForegroundProperty) as SolidColorBrush; }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        public static readonly DependencyProperty HeaderForegroundProperty =
              DependencyProperty.Register(
                  "HeaderForeground", typeof(SolidColorBrush), typeof(HamburgerMenu), new PropertyMetadata(null)
                  );

        #endregion

        #region PaneBackgroundProperty

        public SolidColorBrush PaneBackground
        {
            get { return GetValue(PaneBackgroundProperty) as SolidColorBrush; }
            set { SetValue(PaneBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PaneBackgroundProperty =
              DependencyProperty.Register(
                  "PaneBackground", typeof(SolidColorBrush), typeof(HamburgerMenu), new PropertyMetadata(null)
                  );

        #endregion

        #region PaneForegroundProperty

        public SolidColorBrush PaneForeground
        {
            get { return GetValue(PaneForegroundProperty) as SolidColorBrush; }
            set { SetValue(PaneForegroundProperty, value); }
        }

        public static readonly DependencyProperty PaneForegroundProperty =
              DependencyProperty.Register(
                  "PaneForeground", typeof(SolidColorBrush), typeof(HamburgerMenu), new PropertyMetadata(null)
                  );

        #endregion

        #region PaneBottomContentControlProperty

        public FrameworkElement PaneBottomContent
        {
            get { return GetValue(PaneBottomContentProperty) as FrameworkElement; }
            set { SetValue(PaneBottomContentProperty, value); }
        }

        public static readonly DependencyProperty PaneBottomContentProperty =
              DependencyProperty.Register(
                  "PaneBottomContent", typeof(FrameworkElement), typeof(HamburgerMenu), new PropertyMetadata(null)
                  );

        #endregion

        #region HeaderRightContentControlProperty

        public FrameworkElement HeaderRightContent
        {
            get { return GetValue(HeaderRightContentProperty) as FrameworkElement; }
            set { SetValue(HeaderRightContentProperty, value); }
        }

        public static readonly DependencyProperty HeaderRightContentProperty =
              DependencyProperty.Register(
                  "HeaderRightContent", typeof(FrameworkElement), typeof(HamburgerMenu), new PropertyMetadata(null)
                  );

        #endregion

        //#region MobileDisplayModeProperty
  
        //public Type MobileDisplayMode
        //{
        //    get { return GetValue(MobileDisplayModeProperty) as Type; }
        //    set { SetValue(MobileDisplayModeProperty, value); }
        //}

        //public static readonly DependencyProperty MobileDisplayModeProperty =
        //      DependencyProperty.Register(
        //          "MobileDisplayMode", typeof(Type), typeof(HamburgerMenu), new PropertyMetadata(SplitViewDisplayMode.Overlay)
        //          );

        //#endregion

        //#region TabletDisplayModeProperty

        //public Type TabletDisplayMode
        //{
        //    get { return GetValue(TabletDisplayModeProperty) as Type; }
        //    set { SetValue(TabletDisplayModeProperty, value); }
        //}

        //public static readonly DependencyProperty TabletDisplayModeProperty =
        //      DependencyProperty.Register(
        //          "TabletDisplayMode", typeof(Type), typeof(HamburgerMenu), new PropertyMetadata(SplitViewDisplayMode.CompactOverlay)
        //          );

        //#endregion

        //#region DesktopDisplayModeProperty

        //public Type DesktopDisplayMode
        //{
        //    get { return GetValue(DesktopDisplayModeProperty) as Type; }
        //    set { SetValue(DesktopDisplayModeProperty, value); }
        //}

        //public static readonly DependencyProperty DesktopDisplayModeProperty =
        //      DependencyProperty.Register(
        //          "DesktopDisplayMode", typeof(Type), typeof(HamburgerMenu), new PropertyMetadata(SplitViewDisplayMode.CompactInline)
        //          );

        //#endregion

        #region Control selection

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HamburgerMenuNavigationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationLink navLink = HamburgerMenuNavigationListView.SelectedItem as NavigationLink;
            HamburgerMenuSplitView.Content = navLink.Control;

            if (HamburgerMenuSplitView.DisplayMode == SplitViewDisplayMode.Overlay || HamburgerMenuSplitView.DisplayMode == SplitViewDisplayMode.CompactOverlay)
                HamburgerMenuToggleButton.IsChecked = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HamburgerMenuNavigationListView_Loaded(object sender, RoutedEventArgs e)
        {
            HamburgerMenuNavigationListView.SelectedIndex = 0;
        }

        #endregion

        //#region PropertyChanged

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void RaisePropertyChanged(string propertyName)
        //{
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        //#endregion

        //#region NavigationLinksProperty

        //public NavigationLink[] NavigationLinks
        //{
        //    get { return GetValue(NavigationLinksProperty) as NavigationLink[]; }
        //    set { SetValue(NavigationLinksProperty, value); }
        //}

        //public static readonly DependencyProperty NavigationLinksProperty =
        //      DependencyProperty.Register(
        //          "NavigationLinks", typeof(NavigationLink[]), typeof(HamburgerMenu), new PropertyMetadata(new NavigationLink[0], OnNavigationLinksPropertyChanged)
        //          );

        //private static void OnNavigationLinksPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    HamburgerMenu hamburgerMenu = d as HamburgerMenu;
        //    hamburgerMenu.RaisePropertyChanged("NavigationLinks");
        //    hamburgerMenu.OnNavigationLinksPropertyChanged(e);
        //}
        //private void OnNavigationLinksPropertyChanged(DependencyPropertyChangedEventArgs e)
        //{

        //}

        //#endregion
    }
}
