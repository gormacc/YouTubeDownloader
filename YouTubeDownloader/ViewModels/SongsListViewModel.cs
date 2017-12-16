using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Caliburn.Micro;
using Microsoft.Win32;

namespace YouTubeDownloader.ViewModels
{
    public class SongsListViewModel : PropertyChangedBase
    {
        public BindableCollection<Song> Songs { get; set; }
        

        private static readonly string AppPath = Directory.GetCurrentDirectory();
        //private BackgroundWorker _bgWorker;

        public SongsListViewModel(BindableCollection<Song> songsList)
        {
            Songs = songsList;
        }


        public void CommitChangesInDataGrid(object sender, RoutedEventArgs eventArgs)
        {
            var grid = (DataGrid)sender;
            grid.CommitEdit(DataGridEditingUnit.Row, true);
        }

        public void SaveSongsList()
        {
            string fileName = SaveNewXMLFile();

            DataCollectionLoader.SaveCollection(Songs.Select(s => new Song
            {
                Url = s.Url,
                Title = s.Title,
                Author = s.Author,
                IsGonnaBeDownloaded = s.IsGonnaBeDownloaded
            }).ToArray(),fileName);
        }

        public static string SaveNewXMLFile()
        {
            var sfd = new SaveFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                InitialDirectory = Path.Combine(AppPath)
            };
            XmlDocument doc = new XmlDocument();
            XmlElement elem = doc.CreateElement("ArrayOfSongs");
            doc.AppendChild(elem);

            bool? resultOfDialog = sfd.ShowDialog();
            string fileName = resultOfDialog == true
                ? sfd.FileName
                : Path.Combine(AppPath, "DefaultName.xml");
            doc.Save(fileName);
            return fileName + ".xml";
        }

        public void LoadSongsList()
        {
            string nameOfSelectedFile;

            if (!LoadXMLFile(out nameOfSelectedFile))
            {
                MessageBox.Show(Application.Current.MainWindow, "Błąd wczytywania pliku", "BŁĄD", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                Songs.Clear();
                Songs.AddRange(DataCollectionLoader.LoadCollection<Song>(nameOfSelectedFile));
                NotifyOfPropertyChange(nameof(Songs));
            }
        }

        public static bool LoadXMLFile(out string fileName)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                InitialDirectory = Path.Combine(AppPath),
                Multiselect = false
            };

            fileName = "";

            bool? resultOfDialog = ofd.ShowDialog();
            if (resultOfDialog == true)
            {
                var fileNamePath = ofd.SafeFileName;
                if (Path.GetExtension(fileNamePath) == ".xml")
                {
                    fileName = ofd.FileName;
                    return true;
                }
            }
            return false;
        }

        public void AddSongsList()
        {
            string nameOfSelectedFile;

            if (!LoadXMLFile(out nameOfSelectedFile))
            {
                MessageBox.Show(Application.Current.MainWindow, "Błąd wczytywania pliku", "BŁĄD", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                Songs.AddRange(DataCollectionLoader.LoadCollection<Song>(nameOfSelectedFile));
                NotifyOfPropertyChange(nameof(Songs));
            }
        }







        //BackGroundWorker jeszcze nie wiem czy będzie


        //public void StartDownloading()
        //{
        //    //((ShellView) Application.Current.MainWindow).MyStatusBar.Visibility = Visibility.Visible; // inaczej się powinno to robic
        //    //CanStartDownloading = false;
        //    //NotifyOfPropertyChange(nameof(CanStartDownloading));

        //    //try
        //    //{
        //    //    _bgWorker = new BackgroundWorker();
        //    //    _bgWorker.DoWork += PretendDownloading;
        //    //    _bgWorker.ProgressChanged += PrentedDownloadingProgress;
        //    //    _bgWorker.RunWorkerCompleted += AfterPretendDownloading;
        //    //    _bgWorker.WorkerReportsProgress = true;

        //    //    _bgWorker.RunWorkerAsync();
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    Console.WriteLine(e);  // dodaj info o błędzie
        //    //    throw;
        //    //}


        //}

        //public void PretendDownloading(object sender , DoWorkEventArgs eventArgs)
        //{
        //    for (int i = 1; i <= 10; i++)
        //    {              
        //        _bgWorker.ReportProgress(i*10);
        //        Thread.Sleep(500);
        //    }
        //}

        //public void PrentedDownloadingProgress(object sender , ProgressChangedEventArgs eventArgs)
        //{
        //    ((ShellView) Application.Current.MainWindow).MyProgressBar.Value = eventArgs.ProgressPercentage;
        //}

        //public void AfterPretendDownloading(object sender, RunWorkerCompletedEventArgs eventArgs)
        //{
        //    ((ShellView)Application.Current.MainWindow).MyStatusBar.Visibility = Visibility.Collapsed;
        //    ((ShellView) Application.Current.MainWindow).MyProgressBar.Value = 0;
        //}

    }
}
