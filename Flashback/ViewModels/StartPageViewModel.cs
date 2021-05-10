using Flashback.Models;
using Flashback.Pages;
using Helpers.Navigation;
using Helpers.Storage;
using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashback.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    { 
        private bool _isContinueEnabled;
        public bool IsContinueEnabled
        {
            get
            {
                return _isContinueEnabled;
            }
            set
            {
                _isContinueEnabled = value;
                RaisePropertyChanged(nameof(IsContinueEnabled));
            }
        }

        /// <summary>
        /// Creates new project
        /// </summary>
        public void CreateProject()
        {
            // Delete saved project
            //ProjectViewModel.DeleteProject();
            // Clears future access list
            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Clear();
            // Creates new project and navigate to project page
            ProjectViewModel.Instance.Project = new Project();
            NavigationService.Navigate(typeof(ProjectPage));
        }

        // Navigates to project page
        public void Continue()
        {
            NavigationService.Navigate(typeof(ProjectPage));
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public async Task LoadSavedProject()
        //{
        //    if (ProjectViewModel.Instance.Project != null)
        //    {
        //        IsContinueEnabled = true;
        //    }
        //    else
        //    {
        //        IsContinueEnabled = await Task.Run(() =>
        //        {
        //            try
        //            {
        //                ProjectViewModel.Instance.Project = JsonHelper.FromJson<Project>((string)LocalSettingsHelper.GetValue(typeof(Project).Name));
        //                return true;
        //            }
        //            catch { return false; }
        //        });
        //    }
        //}
    }
}
