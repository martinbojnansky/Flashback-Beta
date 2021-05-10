using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace Helpers.ViewManagement
{
    public static class StatusBarHelper
    {
        #region Desktop

        /// <summary>
        /// 
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="inactiveBackgroundColor"></param>
        /// <param name="inactiveForegroundColor"></param>
        public static void CustomizeTitleBar(
            Color backgroundColor, Color foregroundColor,
            Color inactiveBackgroundColor, Color inactiveForegroundColor)
        {
            //PC customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;

                if (titleBar != null)
                {
                    titleBar.BackgroundColor = backgroundColor;
                    titleBar.ForegroundColor = foregroundColor;
                    titleBar.InactiveBackgroundColor = inactiveBackgroundColor;
                    titleBar.InactiveForegroundColor = inactiveForegroundColor;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="inactiveBackgroundColor"></param>
        /// <param name="inactiveForegroundColor"></param>
        /// <param name="buttonBackgroundColor"></param>
        /// <param name="buttonForegroundColor"></param>
        /// <param name="buttonHoverBackgroundColor"></param>
        /// <param name="buttonHoverForegroundColor"></param>
        /// <param name="buttonPressedBackgroundColor"></param>
        /// <param name="buttonPressedForegroundColor"></param>
        /// <param name="buttonInactiveBackgroundColor"></param>
        /// <param name="buttonInactiveForegroundColor"></param>
        public static void CustomizeTitleBar(
            Color backgroundColor, Color foregroundColor,
            Color inactiveBackgroundColor, Color inactiveForegroundColor,
            Color buttonBackgroundColor, Color buttonForegroundColor,
            Color buttonHoverBackgroundColor, Color buttonHoverForegroundColor,
            Color buttonPressedBackgroundColor, Color buttonPressedForegroundColor,
            Color buttonInactiveBackgroundColor, Color buttonInactiveForegroundColor)
        {
            //PC customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;

                if (titleBar != null)
                {
                    titleBar.BackgroundColor = backgroundColor;
                    titleBar.ForegroundColor = foregroundColor;
                    titleBar.InactiveBackgroundColor = inactiveBackgroundColor;
                    titleBar.InactiveForegroundColor = inactiveForegroundColor;
                    titleBar.ButtonBackgroundColor = buttonBackgroundColor;
                    titleBar.ButtonForegroundColor = buttonForegroundColor;
                    titleBar.ButtonHoverBackgroundColor = buttonHoverBackgroundColor;
                    titleBar.ButtonHoverForegroundColor = buttonHoverForegroundColor;
                    titleBar.ButtonPressedBackgroundColor = buttonPressedBackgroundColor;
                    titleBar.ButtonPressedForegroundColor = buttonPressedForegroundColor;
                    titleBar.ButtonInactiveBackgroundColor = buttonInactiveBackgroundColor;
                    titleBar.ButtonInactiveForegroundColor = buttonInactiveForegroundColor;
                }
            }
        }

        #endregion

        #region Mobile

        /// <summary>
        /// 
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="isVisible"></param>
        /// <param name="backgroundOpacity"></param>
        public static void CustomizeStatusBar(
            Color backgroundColor, Color foregroundColor,
            bool isVisible = true, double backgroundOpacity = 1)
        {
            //Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();

                if (statusBar != null)
                {
                    statusBar.BackgroundColor = backgroundColor;
                    statusBar.ForegroundColor = foregroundColor;
                    statusBar.BackgroundOpacity = backgroundOpacity;

                    if (isVisible)
                        ShowStatusBar();
                    else
                        HideStatusBar();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async static void ShowStatusBar()
        {
            //Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();

                if (statusBar != null)
                {
                    await statusBar.ShowAsync();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async static void HideStatusBar()
        {
            //Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();

                if (statusBar != null)
                {
                    await statusBar.HideAsync();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public async static void ShowStatusBarProgressIndicator(string text = "")
        {
            //Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();

                if (statusBar != null)
                {
                    StatusBarProgressIndicator progressIndicator = statusBar.ProgressIndicator;

                    if (!String.IsNullOrEmpty(text))
                        progressIndicator.Text = text;

                    await progressIndicator.ShowAsync();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async static void HideStatusBarProgressIndicator()
        {
            //Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();

                if (statusBar != null)
                {
                    await statusBar.ProgressIndicator.HideAsync();
                }
            }
        }

        #endregion
    }
}
