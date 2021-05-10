using Flashback.Models;
using Flashback.Views;
using Helpers.Dialogs;
using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace Flashback.ViewModels
{
    public partial class ProjectViewModel : ViewModelBase
    {
        /// <summary>
        /// Pick video file task.
        /// </summary>
        /// <returns></returns>
        public async Task PickVideoClipsAsync()
        {
            try
            {
                ProgressObject.Show("Waiting");

                FileOpenPicker picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".mp4");
                var files = await picker.PickMultipleFilesAsync();

                if (files == null)
                    return;

                ProgressObject.Show("Initializing video files");

                foreach (var file in files)
                {
                    try
                    {
                        var clip = new VideoClip();
                        await clip.InitializeAsync(file);
                        Project.Clips.Add(clip);
                    }
                    catch(Exception ex) { Error.Show(ex.Message); } 
                }

                ClipsView.Current.SelectClip(Project.Clips.LastOrDefault());
            }
            catch(Exception ex) { Error.Show(ex.Message); } 
            finally
            {
                ProgressObject.Hide();
            }
        }
        public async void AddVideoClips() => await PickVideoClipsAsync();

        /// <summary>
        /// Deletes clip from project.
        /// </summary>
        /// <param name="clip"></param>
        public void DeleteClip(Clip clip)
        {
            Project.Clips.Remove(clip);
        }
        public void DeleteClipMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = (FrameworkElement)sender;
                Clip clip = (Clip)element.DataContext;
                DeleteClip(clip);
            }
            catch(Exception ex) { Error.Show(ex.Message); }
        }

        /// <summary>
        /// Duplicates clip in project.
        /// </summary>
        /// <param name="clip"></param>
        public async void DuplicateClip(Clip clip)
        {
            ProgressObject.Show("Duplicating clip");

            try
            {
                var copy = clip.GetCopy();
                await copy.RestoreAsync();

                var index = Project.Clips.IndexOf(clip) + 1;
                Project.Clips.Insert(index, copy);
            }
            catch (Exception ex) { Error.Show("Clip could not be duplicated."); System.Diagnostics.Debug.WriteLine(ex.ToString()); }
            finally { ProgressObject.Hide(); }
        }
        public void DuplicateClipMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = (FrameworkElement)sender;
                Clip clip = (Clip)element.DataContext;
                DuplicateClip(clip);
            }
            catch(Exception ex) { Error.Show(ex.Message); }
        }

        /// <summary>
        /// Adds new slideshow clip to project.
        /// </summary>
        public void AddSlideshowClip()
        {
            Project.Clips.Add(new SlideshowClip());
            ClipsView.Current.SelectClip(Project.Clips.LastOrDefault());
        }
    }
}
