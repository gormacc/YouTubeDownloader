using Caliburn.Micro;

namespace YouTubeDownloader.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private PropertyChangedBase _selectedView;

        public PropertyChangedBase SelectedView
        {
            get { return _selectedView; }
            private set
            {
                if (Equals(value, _selectedView)) return;
                _selectedView = value;
                NotifyOfPropertyChange();
            }
        }

        //public static ShellViewModel CurrentInstance { get; private set; }

        public ShellViewModel()
        {
            //CurrentInstance = this;
            SelectedView = new WelcomeViewModel();
        }

        public void AddLink()
        {
            if (SelectedView is AddLinkViewModel)
                return;
            SelectedView = new AddLinkViewModel(GlobalVariables.Instance.Songs);
        }

        public void SongsList()
        {
            if (SelectedView is SongsListViewModel)
                return;
            SelectedView = new SongsListViewModel(GlobalVariables.Instance.Songs);
        }

        public void Settings()
        {
            if (SelectedView is SettingsViewModel)
                return;
            SelectedView = new SettingsViewModel();
        }

        public void Downloading()
        {
            if (SelectedView is DownloadingViewModel)
                return;
            SelectedView = new DownloadingViewModel(GlobalVariables.Instance.Songs);
        }

    }
}
