using Flashback.Views;
using Helpers.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Flashback.Models
{
    [DataContract]
    [KnownType(typeof(VideoClip))]
    public class VideoClip : Clip
    {
        [IgnoreDataMember]
        private MediaFile _mediaFile;
        /// <summary>
        /// Has information about video file.
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

        [IgnoreDataMember]
        private TimeSpan _originalDuration;
        /// <summary>
        /// Original duration of video file.
        /// </summary>
        [DataMember]
        public TimeSpan OriginalDuration
        {
            get
            {
                return _originalDuration;
            }
            set
            {
                _originalDuration = value;
                RaisePropertyChanged(nameof(OriginalDuration));
            }
        }

        [IgnoreDataMember]
        private double _startTime = 0;
        /// <summary>
        /// Total seconds of trim from start.
        /// </summary>
        [DataMember]
        public double StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
                RaisePropertyChanged(nameof(StartTime));
                RaisePropertyChanged(nameof(Duration));
            }
        }

        [IgnoreDataMember]
        private double _endTime;
        /// <summary>
        /// Total seconds of trim from end.
        /// </summary>
        [DataMember]
        public double EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
                RaisePropertyChanged(nameof(EndTime));
                RaisePropertyChanged(nameof(Duration));
            }
        }

        [IgnoreDataMember]
        public double Duration => EndTime - StartTime;

        /// <summary>
        /// Creates view for clip management.
        /// </summary>
        [IgnoreDataMember]
        public override Control View => new VideoClipView() { VideoClip = this };

        [IgnoreDataMember]
        public override Symbol Symbol { get { return Symbol.Video; } }

        /// <summary>
        /// Object for MediaComposition.
        /// Member is not saved and has to be restored.
        /// </summary>
        [IgnoreDataMember]
        public MediaClip MediaClip { get; set; }

        /// <summary>
        /// Gets preview source for media element.
        /// </summary>
        public async override Task<MediaStreamSource> GetPreviewVideo()
        {
            return await Task<MediaStreamSource>.Run(() => 
            { 
                MediaComposition composition = new MediaComposition();
                composition.Clips.Add(MediaClip.Clone());
                return composition.GeneratePreviewMediaStreamSource(427, 240);     
            });
        }

        /// <summary>
        /// Initializes object.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task InitializeAsync(StorageFile file)
        {
            // Create MediaFile object to store info about StorageFile
            MediaFile = new MediaFile(file);

            // Get video properties
            var properties = await file.Properties.GetVideoPropertiesAsync();
            OriginalDuration = properties.Duration;
            EndTime = OriginalDuration.TotalSeconds;

            // Prepare MediaClip
            MediaClip = await MediaClip.CreateFromFileAsync(file);
            // Update preview image
            await UpdatePreviewImageAsync();
        }

        /// <summary>
        /// Restores object.
        /// </summary>
        /// <returns></returns>
        public override async Task RestoreAsync()
        {
            try
            {
                // Restore file and create media clip
                var file = await StorageFile.GetFileFromPathAsync(MediaFile.Path);
                MediaClip = await MediaClip.CreateFromFileAsync(file);
                // Update preview image
                await UpdatePreviewImageAsync();
            }
            catch (Exception ex) { Error.Show(ex.Message); }
        }

        /// <summary>
        /// Updates preview image.
        /// </summary>
        /// <returns></returns>
        public override async Task UpdatePreviewImageAsync()
        {
            try
            {
                // Create composition to generate preview
                MediaClip mediaClip = MediaClip.Clone();
                MediaComposition mediaComposition = new MediaComposition();
                mediaComposition.Clips.Add(mediaClip);

                // Generate preview image from media composition
                ImageStream imageStream = await mediaComposition.GetThumbnailAsync(TimeSpan.FromSeconds(StartTime), 178, 100, VideoFramePrecision.NearestKeyFrame);
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(imageStream);
                PreviewImage = bitmapImage;
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }     
    }
}
