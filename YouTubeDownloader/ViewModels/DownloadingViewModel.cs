using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Caliburn.Micro;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace YouTubeDownloader.ViewModels
{
    public sealed class DownloadingViewModel : PropertyChangedBase
    {
        private string _currentSongDownloaded = string.Empty;
        private string _downloadPath = Directory.GetCurrentDirectory();
        private double _progressPercentage;
        private bool _showDownloadingProgress;

        public BindableCollection<Song> Songs { get; set; }

        public string CurrentSongDownloaded
        {
            get { return _currentSongDownloaded; }
            set
            {
                _currentSongDownloaded = value;
                NotifyOfPropertyChange();
            }
        }
        
        public bool CanStartDownloading => (Songs != null && Songs.Count > 0);

        public bool ShowDownloadingProgress
        {
            get{ return _showDownloadingProgress; }
            set
            {
                _showDownloadingProgress = value;
                NotifyOfPropertyChange();
            }
        }

        public double ProgressPercentage
        {
            get { return _progressPercentage; }
            set
            {
                _progressPercentage = value;
                NotifyOfPropertyChange();
            }
        }

        public DownloadingViewModel(BindableCollection<Song> songs)
        {
            Songs = songs;
            NotifyOfPropertyChange(nameof(CanStartDownloading));
        }

        public async Task StartDownloading()
        {
            if (Songs != null)
            {
                //SelectDownloadingPath();

                await StartDownloadingAllSongs();

                ShowEndDownloadingInformation();
            }
        }

        private void SelectDownloadingPath()
        {
            var fbd = new FolderBrowserDialog();
            var resultOfDialog = fbd.ShowDialog();
            if (resultOfDialog == DialogResult.OK)
            {
                _downloadPath = fbd.SelectedPath;
            }
        }

        private async Task StartDownloadingAllSongs()
        {
            ShowDownloadingProgress = true;
            await Task.Run(() => DownloadAllSongs());
            ShowDownloadingProgress = false;
        }


        private void DownloadAllSongs()
        {   
            double numberOfSongsToDownload = Songs.Count(s => s.IsGonnaBeDownloaded);

            for (int i = 0; i < Songs.Count; i++)
            {
                if (Songs[i].IsGonnaBeDownloaded)
                {
                    DownloadSingleElement(Songs[i], (100 * (i + 1 )) / numberOfSongsToDownload);   // procent wysłanych piosenek
                }                
            }

            CurrentSongDownloaded = String.Empty;           
        }

        private void DownloadSingleElement(Song song , double percentage)
        {
            CurrentSongDownloaded = song.Author + " - " + song.Title;

            try
            {
                Run(song.Url);  // faktyczne pobieranie
            }
            catch (Exception)
            {
                MessageBox.Show(Application.Current.MainWindow, "Nie powiodło się", "Błąd", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }            

            ProgressPercentage = percentage;
        }

        private void ShowEndDownloadingInformation()
        {
            MessageBox.Show(Application.Current.MainWindow, "Zakończono pobieranie plików", "Pobieranie", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public void Run(string url)
        {
            string args = " -x --audio-format mp3 " + url;
            var p = new Process();
            //string path = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            p.StartInfo = new ProcessStartInfo("youtube-dl", args)
            {
                UseShellExecute = false
            };
            p.Start();
            p.WaitForExit();
        }

    }
}
