using AudioEffects;
using Flashback.Effects.Adjustments;
using Flashback.Effects.Titles;
using Flashback.Models;
using Helpers.Dialogs;
using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Flashback.ViewModels
{
    public partial class ProjectViewModel : ViewModelBase
    {
        private VideoEncodingProperties _baseVideoEncodingProperties;

        /// <summary>
        /// Generates MediaComposition synchronously.
        /// </summary>
        /// <returns></returns>
        private async Task<MediaComposition> GetMediaCompositionAsync()
        {
            MediaComposition mediaComposition = new MediaComposition();

            try
            {
                #region Clips

                _baseVideoEncodingProperties = null;

                foreach (var clip in Project.Clips)
                {
                    try
                    {
                        var type = clip.GetType();
                        // VideoClip
                        if (type == typeof(VideoClip))
                        {
                            var videoClip = clip as VideoClip;                       
                            var mediaClip = videoClip.MediaClip.Clone();

                            // Trim media clip
                            mediaClip.TrimTimeFromStart = TimeSpan.FromSeconds(videoClip.StartTime);
                            mediaClip.TrimTimeFromEnd = TimeSpan.FromSeconds(videoClip.OriginalDuration.TotalSeconds - videoClip.EndTime);
                            // Set volume
                            mediaClip.Volume = Project.Track.VideoVolume / 100;

                            // Add effects
                            AddEffectsToMediaClip(mediaClip, mediaComposition);

                            // Add clip to composition
                            mediaComposition.Clips.Add(mediaClip);

                            // Update base video encoding properties
                            if (_baseVideoEncodingProperties == null)
                            {
                                _baseVideoEncodingProperties = mediaClip.GetVideoEncodingProperties();
                            }
                        }
                        // SlideshowClip
                        else if(type == typeof(SlideshowClip))
                        {
                            var slideshowClip = clip as SlideshowClip;

                            // Update MediaClips if duration changed
                            if(slideshowClip.ImagesDurationOrOrderChanged)
                            {
                                await slideshowClip.UpdateSlideshowImagesAsync();
                            }

                            foreach(var slideshowImage in slideshowClip.SlideshowImages)
                            {
                                var mediaClip = slideshowImage.MediaClip.Clone();
                                // Add effects
                                AddEffectsToMediaClip(mediaClip, mediaComposition);
                                mediaComposition.Clips.Add(mediaClip);
                            }
                        }                    
                    }
                    catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
                }

                #endregion

                #region Titles

                // Add fades
                if (mediaComposition.Clips.Count > 0)
                {
                    Project.FadeInEffect.ApplyEffect(mediaComposition.Clips.First(), mediaComposition);
                    Project.FadeOutEffect.ApplyEffect(mediaComposition.Clips.Last(), mediaComposition);
                }
                // Add opening and closing titles
                Project.OpeningTitlesEffect.ApplyEffect(null, mediaComposition);
                Project.ClosingTitlesEffect.ApplyEffect(null, mediaComposition);
                // Add end watermark 
                new EndWatermarkEffect().ApplyEffect(null, mediaComposition);

                #endregion

                #region Music

                // Add background audio track and it's effects
                AddBackgroundAudioTrackToMediaComposition(mediaComposition);

                #endregion
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

            return mediaComposition;
        }

        /// <summary>
        /// Adds effects to single MediaClip
        /// </summary>
        /// <param name="mediaClip"></param>
        /// <param name="mediaComposition"></param>
        private void AddEffectsToMediaClip(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            try
            {
                // Crop image to aspect ratio 16:9
                //mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(CropVideoEffect).FullName));

                foreach (var category in EffectReferenceCategories)
                {
                    foreach (var effectReference in category.EffectReferences)
                    {
                        var effect = effectReference.Effect;
                        if (effect.IsActive)
                        {
                            effect.ApplyEffect(mediaClip, mediaComposition);
                        }
                    }
                }
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Adds background audio track and it's effects to media composition.
        /// </summary>
        /// <param name="mediaComposition"></param>
        public void AddBackgroundAudioTrackToMediaComposition(MediaComposition mediaComposition, bool preview = false)
        {
            try
            {
                var backgroundAudioTrack = Project.Track.BackgroundAudioTrack.Clone();
                backgroundAudioTrack.TrimTimeFromStart = TimeSpan.FromSeconds(Project.Track.StartTime);
                backgroundAudioTrack.Volume = Project.Track.MusicVolume / 100;

                // Fade effects
                var fadeAudioProperties = FadeAudioEffect.GetDefaultProperties();
                fadeAudioProperties["IsFadeInEnabled"] = Project.Track.FadeIn;
                fadeAudioProperties["EndTime"] = mediaComposition.Duration.TotalSeconds;
                var duration = mediaComposition.Duration.TotalSeconds;
                if (duration < 8 && !preview)
                {
                    fadeAudioProperties["FadeInDuration"] = duration / 2;
                    fadeAudioProperties["FadeOutDuration"] = duration / 2;
                }
                else
                {
                    fadeAudioProperties["FadeInDuration"] = 3.0;
                    fadeAudioProperties["FadeOutDuration"] = 5.0;
                }

                backgroundAudioTrack.AudioEffectDefinitions.Add(new AudioEffectDefinition(typeof(FadeAudioEffect).FullName, fadeAudioProperties));

                mediaComposition.BackgroundAudioTracks.Add(backgroundAudioTrack);
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Gets movie preview source.
        /// </summary>
        /// <returns></returns>
        public async Task<MediaStreamSource> GetPreviewAsync()
        {
            ProgressObject.Show("Preparing preview");
            try
            {
                var mediaComposition = await GetMediaCompositionAsync();
                
                // In order to decrease video and audio quality to prevent lagging in preview is created MediaStreamSource instead of PreviewMediaStream source.
                var videoEncodingProperties = VideoEncodingProperties.CreateH264();
                videoEncodingProperties.Width = 480;
                videoEncodingProperties.Height = 270;
                videoEncodingProperties.Bitrate = 400000;
                var frameRate = videoEncodingProperties.FrameRate;
                frameRate.Numerator = 6;
                frameRate.Denominator = 1;

                var mediaStreamSource = mediaComposition.GenerateMediaStreamSource(new MediaEncodingProfile() { Audio = AudioEncodingProperties.CreatePcm(4400, 1, 1), Video = videoEncodingProperties });
                //var mediaStreamSource = mediaComposition.GenerateMediaStreamSource(MediaEncodingProfile.CreateWmv(VideoEncodingQuality.Wvga));

                return mediaStreamSource;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                ProgressObject.Hide();
            }
        }

        /// <summary>
        /// Renders movie to video file
        /// </summary>
        /// <returns></returns>
        public async void RenderMovie() => await RenderMovieAsync();
        private async Task RenderMovieAsync()
        {
            try
            {
                //await SaveProjectAsync();
                ProgressObject.Show("Waiting");

                // Pick file to save movie
                FileSavePicker picker = new FileSavePicker();
                picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
                picker.SuggestedFileName = "";
                picker.FileTypeChoices.Add("MP4", new List<string>() { ".mp4" });
                StorageFile file = await picker.PickSaveFileAsync();

                if (file == null)
                    throw new TaskCanceledException();

                CancellationTokenSource cts = new CancellationTokenSource();
                ProgressObject.Show("Rendering movie", cts);

                // Render movie
                MediaComposition mediaComposition = await GetMediaCompositionAsync();
                // Auto quality is not working, so if auto is selected no encoding profile is created
                if (Project.VideoQuality != 0)
                {
                    var videoQuality = (VideoEncodingQuality)Enum.GetValues(typeof(VideoEncodingQuality)).GetValue(Project.VideoQuality);
                    await mediaComposition.RenderToFileAsync(file, MediaTrimmingPreference.Precise, MediaEncodingProfile.CreateMp4(videoQuality)).AsTask(cts.Token);
                }
                else
                {
                    await mediaComposition.RenderToFileAsync(file, MediaTrimmingPreference.Precise).AsTask(cts.Token);
                }
            }
            catch(Exception ex) { if(ex.GetType() != typeof(TaskCanceledException)) Error.Show(ex.Message); }
            finally
            {
                ProgressObject.Hide();
            }
        }
    }
}
