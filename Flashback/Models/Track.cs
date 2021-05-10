using Helpers.Dialogs;
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

namespace Flashback.Models
{
    [DataContract]
    public class Track : ViewModelBase
    {
        [IgnoreDataMember]
        private MediaFile _mediaFile;
        /// <summary>
        /// Has information about audio file.
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
        public string Path
        {
            get
            {
                if (MediaFile == null)
                    return "...";
                else
                    return MediaFile.Path;
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
            }
        }

        [IgnoreDataMember]
        private TimeSpan _duration = TimeSpan.FromDays(365); // fixes bug on restoring project, when start time is modified
        /// <summary>
        /// Duration of audio file.
        /// </summary>
        [DataMember]
        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                RaisePropertyChanged(nameof(Duration));
            }
        }

        [IgnoreDataMember]
        private bool _fadeIn;
        /// <summary>
        /// Determines whether fade in start of audio file.
        /// </summary>
        [DataMember]
        public bool FadeIn
        {
            get
            {
                return _fadeIn;
            }
            set
            {
                _fadeIn = value;
                RaisePropertyChanged(nameof(FadeIn));
            }
        }

        [IgnoreDataMember]
        private double _musicVolume = 100;
        /// <summary>
        /// Determines volume of audio file in MediaComposition.
        /// </summary>
        [DataMember]
        public double MusicVolume
        {
            get
            {
                return _musicVolume;
            }
            set
            { _musicVolume = value;
                RaisePropertyChanged(nameof(MusicVolume));
            }
        }

        [IgnoreDataMember]
        private double _videoVolume = 0;
        /// <summary>
        /// Determines volume of MediaClips in MediaComposition.
        /// </summary>
        [DataMember]
        public double VideoVolume
        {
            get
            {
                return _videoVolume;
            }
            set
            {
                _videoVolume = value;
                RaisePropertyChanged(nameof(VideoVolume));
            }
        }

        /// <summary>
        /// Object for MediaComposition.
        /// Member is not saved and has to be restored.
        /// </summary>
        [IgnoreDataMember]
        public BackgroundAudioTrack BackgroundAudioTrack { get; set; }

        public async Task InitializeAsync(StorageFile file)
        {
            // Create MediaFile object to store info about StorageFile
            MediaFile = new MediaFile(file);

            // Get audio properties
            var properties = await file.Properties.GetMusicPropertiesAsync();
            RaisePropertyChanged(nameof(Path));
            Duration = properties.Duration;
            StartTime = 0;

            // Prepare BackgroundAudioTrack
            BackgroundAudioTrack = await BackgroundAudioTrack.CreateFromFileAsync(file);
        }

        public async Task RestoreAsync()
        {
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(MediaFile.Path);
                BackgroundAudioTrack = await BackgroundAudioTrack.CreateFromFileAsync(file);
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }
    }
}
