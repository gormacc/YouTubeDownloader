using System;
using System.Windows;
using Caliburn.Micro;
using System.Web;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace YouTubeDownloader.ViewModels
{
    public class AddLinkViewModel : PropertyChangedBase
    {
        private string _songUrl;
        private string _playlistUrl;

        public AddLinkViewModel(BindableCollection<Song> songs)
        {
            Songs = songs;
        }

        public BindableCollection<Song> Songs { get; set; }

        public string SongUrl
        {
            get { return _songUrl; }
            set
            {
                _songUrl = value;
                NotifyOfPropertyChange();
            }
        }

        public string PlaylistUrl
        {
            get { return _playlistUrl; }
            set
            {
                _playlistUrl = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddSongUrl()
        {
            try
            {
                Songs.Add(new Song()
                {
                    Url = SongUrl
                });        

                MessageBox.Show(Application.Current.MainWindow, SongUrl, "Dodano piosenkę", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show(Application.Current.MainWindow, "Nie powiodło się", "Błąd", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            
        }

        public void AddPlaylistUrl()
        {
            try
            {

                CollectUrlFromPlaylist();

                MessageBox.Show(Application.Current.MainWindow, PlaylistUrl, "Dodano piosenkę", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show(Application.Current.MainWindow, "Nie powiodło się", "Błąd", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void CollectUrlFromPlaylist()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCSpgs4vtyPALGIXsKU3-bnjh5Q2CHHOYg",
                ApplicationName = this.GetType().ToString()
            });
            var request = youtubeService.PlaylistItems.List("snippet");
            request.Key = "AIzaSyCSpgs4vtyPALGIXsKU3-bnjh5Q2CHHOYg";
            request.PlaylistId = GetArgs(PlaylistUrl, "list", '?'); ;
            request.MaxResults = 50;
            var response = request.Execute();

            foreach (var item in response.Items)
            {
                Songs.Add(new Song { Url = GetUrlFromSongId(item.Snippet.ResourceId.VideoId) });
            }
        }

        private string GetUrlFromSongId(string id)
        {
            return $"https://www.youtube.com/watch?v={id}";
        }

        private string GetArgs(string args, string key, char query)
        {
            var iqs = args.IndexOf(query);
            return iqs == -1
                ? string.Empty
                : HttpUtility.ParseQueryString(iqs < args.Length - 1
                    ? args.Substring(iqs + 1) : string.Empty)[key];
        }

    }
}
