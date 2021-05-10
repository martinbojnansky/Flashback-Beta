using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Helpers.Navigation
{
    public static class NavigationService
    {
        public static Frame RootFrame
        {
            get { return Window.Current.Content as Frame; }
        }

        public static SystemNavigationManager SystemNavigationManager
        {
            get { return SystemNavigationManager.GetForCurrentView(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void NavigateBack()
        {
            if (RootFrame.Content != null && RootFrame.CanGoBack)
            {
                RootFrame.GoBack();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void NavigateForward()
        {
            if (RootFrame.Content != null && RootFrame.CanGoForward)
            {
                RootFrame.GoForward();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        public static void Navigate(Type type, object args = null)
        {
            if(RootFrame.Content != null)
            {
                RootFrame.Navigate(type, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RegisterBackRequestedEvent()
        {
            SystemNavigationManager.BackRequested += NavigationService_BackRequested;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void NavigationService_BackRequested(object sender, BackRequestedEventArgs e)
        {
            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (RootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                RootFrame.GoBack();
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        public static void UpdateAppViewBackButtonVisibility()
        {
            if (RootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }
    }
}
