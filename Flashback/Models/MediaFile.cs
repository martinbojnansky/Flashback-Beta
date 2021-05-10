using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace Flashback.Models
{
    [DataContract]
    public class MediaFile : ViewModelBase
    {
        [IgnoreDataMember]
        private string _path;
        /// <summary>
        /// System path of StorageFile.
        /// </summary>
        [DataMember]
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                RaisePropertyChanged(nameof(Path));
            }
        }

        /// <summary>
        /// Token of StorageFile in FutureAccessList.
        /// </summary>
        [DataMember]
        public string FutureAccessListToken { get; set; }

        /// <summary>
        /// Creation date of file.
        /// </summary>
        [DataMember]
        public DateTimeOffset DateCreated { get; set; }

        public MediaFile(StorageFile file)
        {
            Path = file.Path;
            DateCreated = file.DateCreated;

            // To open file again without file picker we have to add it to FutureAccessList.
            // The list is cleared if the app is uninstalled or if we call the clear method on it. 
            // Maximum number of items is 1000.
            FutureAccessListToken = StorageApplicationPermissions.FutureAccessList.Add(file);
        }
    }
}
