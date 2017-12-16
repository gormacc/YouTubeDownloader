using Caliburn.Micro;

namespace YouTubeDownloader
{
    public class GlobalVariables
    {
        private GlobalVariables()
        {
            Songs = new BindableCollection<Song>();
        }

        public BindableCollection<Song> Songs;

        public static GlobalVariables Instance { get; } = new GlobalVariables();
    }
}
