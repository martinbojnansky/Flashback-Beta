using Flashback.Models;
using Helpers.Dialogs;
using Helpers.Navigation;
using Helpers.Storage;
using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Flashback.ViewModels
{
    public partial class ProjectViewModel : ViewModelBase
    {
        private Project _project;
        public Project Project
        {
            get { return _project; }
            set { _project = value; RaisePropertyChanged(nameof(Project)); }
        }

        /// <summary>
        /// Load project
        /// </summary>
        /// <returns></returns>
        public async Task LoadProjectAsync()
        {
            try
            {
                ProgressObject.Show("Loading project"); 

                // Restore project
                await Project.RestoreAsync();

                // Initialize effects for use
                ProgressObject.Show("Preparing video effects");
                await CreateEffectReferenceCategories();
            }
            catch (Exception ex) { Error.Show(ex.Message); NavigationService.NavigateBack(); }
            finally { ProgressObject.Hide(); }
        }

        public async Task<bool> DiscardProject()
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Discard movie?",
                Content = "If you go back now, you won't be able to edit this movie later.",
                PrimaryButtonText = "Discard",
                SecondaryButtonText = "Keep"
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
                return true;
            else
                return false;
        }

        ///// <summary>
        ///// Saves project to local storage.
        ///// </summary>
        //public async Task SaveProjectAsync()
        //{
        //    try
        //    {
        //        ProgressObject.Show("Saving project");

        //        await Task.Run(() =>
        //        {
        //            LocalSettingsHelper.SetValue(nameof(Project), JsonHelper.ToJson(Project));
        //        });
        //    }
        //    catch (Exception ex) { Error.Show(ex.Message); }
        //    finally { ProgressObject.Hide(); }
        //}

        ///// <summary>
        ///// Deletes saved project.
        ///// </summary>
        //public static async void DeleteProject()
        //{
        //    await Task.Run(() =>
        //    {
        //        try { LocalSettingsHelper.RemoveElement(nameof(Project)); } catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        //    });
        //}
    }
}
