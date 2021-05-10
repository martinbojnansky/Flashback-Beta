using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace Helpers.Storage
{
    public static class FutureAccessListHelper
    {
        // To open file again without file picker we have to add it to FutureAccessList.
        // The list is cleared if the app is uninstalled or if we call the clear method on it. 
        // Maximum number of items is 1000.

        //public static string AddFile(StorageFile file)
        //{
        //    var list = StorageApplicationPermissions.FutureAccessList;
        //}
    }
}
