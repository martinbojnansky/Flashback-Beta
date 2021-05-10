using Helpers.Storage;
using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Flashback.Models
{
    [DataContract]
    [KnownType(typeof(VideoClip))]
    [KnownType(typeof(SlideshowClip))]
    public abstract class Clip : ViewModelBase
    {
        [IgnoreDataMember]
        private BitmapImage _previewImage;
        /// <summary>
        /// Preview image of clip.
        /// </summary>
        [IgnoreDataMember]
        public BitmapImage PreviewImage
        {
            get
            {
                return _previewImage;
            }
            set
            {
                _previewImage = value;
                RaisePropertyChanged(nameof(PreviewImage));
            }
        }

        [IgnoreDataMember]
        public abstract Control View { get; }

        [IgnoreDataMember]
        public abstract Symbol Symbol { get; }

        /// <summary>
        /// Abstract task to restore object which inherits from this class.
        /// </summary>
        /// <returns></returns>
        public abstract Task RestoreAsync();

        /// <summary>
        /// Gets deep copy.
        /// </summary>
        /// <returns></returns>
        public virtual Clip GetCopy()
        {
            try
            {
                // To make a deep copy is object serialized and deserialized
                var json = JsonHelper.ToJson(this);
                return JsonHelper.FromJson<Clip>(json); 
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); return null; }
        }

        /// <summary>
        /// Updates preview image.
        /// </summary>
        /// <returns></returns>
        public abstract Task UpdatePreviewImageAsync();

        /// <summary>
        /// Gets preview source for media element.
        /// </summary>
        public abstract Task<MediaStreamSource> GetPreviewVideo();
    }
}
