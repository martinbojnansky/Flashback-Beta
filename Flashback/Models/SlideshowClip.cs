using Flashback.ViewModels;
using Flashback.Views;
using Helpers.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Flashback.Models
{
    [DataContract]
    [KnownType(typeof(SlideshowClip))]
    public class SlideshowClip : Clip
    {
        public SlideshowClip()
        {
            SlideshowImages = new ObservableCollection<SlideshowImage>();
            SlideshowImages.CollectionChanged += SlideshowImages_CollectionChanged;
        }

        [IgnoreDataMember]
        private ObservableCollection<SlideshowImage> _slideshowImages;
        /// <summary>
        /// Collection of clip's image files.
        /// </summary>
        [DataMember]
        public ObservableCollection<SlideshowImage> SlideshowImages
        {
            get
            {
                return _slideshowImages;
            }
            set
            {
                _slideshowImages = value;
                RaisePropertyChanged(nameof(SlideshowImages));
            }
        }

        [IgnoreDataMember]
        private double _imageDuration = 1.4;
        /// <summary>
        /// Total seconds of single slideshow image.
        /// </summary>
        [DataMember]
        public double ImageDuration
        {
            get
            {
                return _imageDuration;
            }
            set
            {
                _imageDuration = value;
                RaisePropertyChanged(nameof(ImageDuration));
                ImagesDurationOrOrderChanged = true;
            }
        }

        [IgnoreDataMember]
        public bool ImagesDurationOrOrderChanged = false;

        /// <summary>
        /// Gets preview source for media element.
        /// </summary>
        public async override Task<MediaStreamSource> GetPreviewVideo()
        {
            MediaComposition composition = new MediaComposition();
                
            // Update MediaClips if duration changed
            if (ImagesDurationOrOrderChanged)
            {
                await UpdateSlideshowImagesAsync();
            }

            foreach (var slideshowImage in SlideshowImages)
            {
                var mediaClip = slideshowImage.MediaClip.Clone();
                composition.Clips.Add(mediaClip);
            }

            // BUG FIX - preview element needs any source to regenerate preview after adding first item
            if(composition.Clips.Count == 0)
            {
                composition.Clips.Add(MediaClip.CreateFromColor(Colors.Black, TimeSpan.FromMilliseconds(1)));
            }

            ImagesDurationOrOrderChanged = false;

            return composition.GeneratePreviewMediaStreamSource(427, 240);
        }

        /// <summary>
        /// Creates view for clip management.
        /// </summary>
        [IgnoreDataMember]
        public override Control View => new SlideshowClipView() { SlideshowClip = this };

        [IgnoreDataMember]
        public override Symbol Symbol { get { return Symbol.Pictures; } }

        /// <summary>
        /// Adds new image to slideshow
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task AddSlideshowImageAsync()
        {
            try
            {
                ProjectViewModel.Instance.ProgressObject.Show("Waiting");

                FileOpenPicker picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".jpg");
                var files = await picker.PickMultipleFilesAsync();

                if (files == null)
                    throw new TaskCanceledException();

                ProjectViewModel.Instance.ProgressObject.Show("Initializing image files");

                foreach (var file in files)
                {
                    try
                    {
                        SlideshowImage image = new SlideshowImage();
                        // Create MediaFile object to store info about StorageFile
                        image.MediaFile = new MediaFile(file);
                        // Prepare MediaClip
                        image.MediaClip = await MediaClip.CreateFromImageFileAsync(file, TimeSpan.FromSeconds(ImageDuration));
                        // Update thumbnails
                        await image.UpdateThumbnailAsync(file);
                        // Add image to collection
                        SlideshowImages.Add(image);
                    }
                    catch(Exception ex) { Error.Show(ex.Message); }
                }

                // Update preview image
                await UpdatePreviewImageAsync();
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
            finally
            {
                ProjectViewModel.Instance.ProgressObject.Hide();
            }
        }
        public async void AddSlideshowImage() => await AddSlideshowImageAsync();

        /// <summary>
        /// Restores object.
        /// </summary>
        /// <returns></returns>
        public async override Task RestoreAsync()
        {
            try
            {
                // Restore every image
                foreach (var image in SlideshowImages)
                {
                    try
                    {
                        // Restore file and create media clip
                        var file = await StorageFile.GetFileFromPathAsync(image.MediaFile.Path);
                        image.MediaClip = await MediaClip.CreateFromImageFileAsync(file, TimeSpan.FromSeconds(ImageDuration));
                        // Update thumbnails
                        await image.UpdateThumbnailAsync(file);
                    }
                    catch(Exception ex) { Error.Show(ex.Message); }
                }

                // Detect reorder events
                SlideshowImages.CollectionChanged += SlideshowImages_CollectionChanged;
                // Update preview image
                await UpdatePreviewImageAsync();
            }
            catch(Exception ex) { Error.Show(ex.Message); }
        }

        /// <summary>
        /// Updates slideshow images when duration settings changed. We need to create new MediaClip.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateSlideshowImagesAsync()
        {
            try
            {
                // Recreate every image
                foreach (var image in SlideshowImages)
                {
                    try
                    {
                        // Restore file and create media clip
                        var file = await StorageFile.GetFileFromPathAsync(image.MediaFile.Path);
                        image.MediaClip = await MediaClip.CreateFromImageFileAsync(file, TimeSpan.FromSeconds(ImageDuration));
                        ImagesDurationOrOrderChanged = false;
                    }
                    catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
                }
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Updates preview image.
        /// </summary>
        /// <returns></returns>
        public override Task UpdatePreviewImageAsync()
        {
            if (SlideshowImages.Count > 0)
                PreviewImage = SlideshowImages[0].Thumbnail;
            else
                PreviewImage = null;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes image from slideshow.
        /// </summary>
        /// <param name="clip"></param>
        public void DeleteImage(SlideshowImage image)
        {
            SlideshowImages.Remove(image);
        }
        public void DeleteImageMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = (FrameworkElement)sender;
                SlideshowImage image = (SlideshowImage)element.DataContext;
                DeleteImage(image);
            }
            catch(Exception ex) { Error.Show(ex.Message); }
        }

        /// <summary>
        /// Detect reorder. It is combination of remove and add action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlideshowImages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ImagesDurationOrOrderChanged = true;
            UpdatePreviewImageAsync();
        }
    }
}
