using Helpers.Controls;
using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashback.ViewModels
{
    public partial class ProjectViewModel : ViewModelBase
    {
        #region Singleton

        private static readonly ProjectViewModel instance = new ProjectViewModel();
        private ProjectViewModel() { }
        public static ProjectViewModel Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region ProgressOverlay

        public ProgressObject ProgressObject { get; } = new ProgressObject();

        #endregion
    }
}
