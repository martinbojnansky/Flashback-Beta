using Flashback.Models;
using Helpers.Dialogs;
using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Flashback.ViewModels
{
    public partial class ProjectViewModel : ViewModelBase
    {
        public async Task PickTrackAsync()
        {
            try
            {
                ProgressObject.Show("Waiting");

                FileOpenPicker picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".mp3");
                picker.FileTypeFilter.Add(".wav");
                picker.FileTypeFilter.Add(".flac");
                StorageFile file = await picker.PickSingleFileAsync();

                if (file == null)
                    return;

                ProgressObject.Show("Initializing audio file");

                CreateTrack();
                await Project.Track.InitializeAsync(file);
            }
            catch(Exception ex)
            {
                Error.Show(ex.Message);
            }
            finally
            {
                ProgressObject.Hide();
            }
        }
        public async void PickTrack() { await PickTrackAsync(); }

        public void CreateTrack()
        {
            Project.Track = new Track();
        }
    }
}
