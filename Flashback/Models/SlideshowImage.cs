using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace Flashback.Models
{
    [DataContract]
    public class SlideshowImage : ViewModelBase
    {
        [IgnoreDataMember]
        private MediaFile _mediaFile;
        /// <summary>
        /// Has information about image file.
        /// </summary>
        [DataMember]
        public MediaFile MediaFile
        {
            get
            {
                return _mediaFile;
            }
            set
            {
                _mediaFile = value;
                RaisePropertyChanged(nameof(MediaFile));
            }
        }

        /// <summary>
        /// Object for MediaComposition.
        /// Member is not saved and has to be restored.
        /// </summary>
        [IgnoreDataMember]
        public MediaClip MediaClip { get; set; }

        [IgnoreDataMember]
        private BitmapImage _thumbnail;
        /// <summary>
        /// Preview image of image file.
        /// </summary>
        [IgnoreDataMember]
        public BitmapImage Thumbnail
        {
            get
            {
                return _thumbnail;
            }
            set
            {
                _thumbnail = value;
                RaisePropertyChanged(nameof(Thumbnail));
            }
        }

        /// <summary>
        /// Updates thumbnail.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateThumbnailAsync(StorageFile file = null)
        {
            try
            {
                // Open file if it's not provided
                if (file == null)
                    file = await StorageFile.GetFileFromPathAsync(MediaFile.Path);

                // Get thumbnail and create new image
                var storageItemThumbnail = await file.GetThumbnailAsync(ThumbnailMode.PicturesView, 100, ThumbnailOptions.UseCurrentScale);
                var image = new BitmapImage();
                image.SetSource(storageItemThumbnail);
                Thumbnail = image;
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }
    }
}
