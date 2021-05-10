using Flashback.Effects;
using Flashback.ViewModels;
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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Flashback.Views
{
    public sealed partial class EffectsView : UserControl
    {
        public ProjectViewModel ProjectViewModel = ProjectViewModel.Instance;
        public static EffectsView Current;

        public EffectsView()
        {
            this.InitializeComponent();
            Current = this;
        }

        #region EffectCategories

        /// <summary>
        /// Select first item as active. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EffectCategoriesListView_Loaded(object sender, RoutedEventArgs e)
        {
            try { EffectCategoriesListView.SelectedIndex = 0; } catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Provide UI for current effect preferences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EffectsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedEffectControl.Children.Clear();

            var effectReference = EffectsGridView.SelectedItem as EffectReference;
            if(effectReference != null)
            {
                SelectedEffectControl.Children.Add(effectReference.Effect.GetControl());
                if(EffectCategoriesListView.SelectedIndex == 0)
                {
                    try
                    {
                        ProjectViewModel.Instance.EffectReferenceCategories[0].EffectReferences.Where(er => er.Effect.IsActive == true).FirstOrDefault().Effect.IsActive = false;
                    }
                    catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

                    effectReference.Effect.IsActive = true;
                }
            }
        }

        /// <summary>
        /// Clears effect properties view
        /// </summary>
        public void ResetEffectPropertiesUserControl()
        {
            var index = EffectsGridView.SelectedIndex;
            EffectsGridView.SelectedItem = null;
            EffectsGridView.SelectedIndex = index;
        }

        #endregion

        #region Preview

        public ProgressObject PreviewMediaElementProgressObject = new ProgressObject();
        
        /// <summary>
        /// Generates new preview
        /// </summary>
        public async Task UpdatePreview()
        {
            PreviewMediaElementProgressObject.Show("Loading");
            try
            {
                PreviewMediaElement.SetMediaStreamSource(await ProjectViewModel.GetPreviewAsync());
            }
            catch { PreviewMediaElementProgressObject.Hide(); }
        }
     
        /// <summary>
        /// Set media source when preview element is created.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PreviewMediaElement_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdatePreview();
        }

        /// <summary>
        /// Hide progress bar on complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            PreviewMediaElementProgressObject.Hide();
        }

        #endregion
    }
}
