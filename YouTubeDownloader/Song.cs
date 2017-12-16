using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace YouTubeDownloader
{
    [DataContract]
    public class Song : XmlSerializablePropertyChangedBase
    {
        private string _url;
        private string _title;
        private string _author;
        private bool _isGonnaBeDownloaded;

        [DataMember, XmlAttribute]
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                NotifyOfPropertyChange();
            }
        }

        [DataMember, XmlAttribute]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange();
            }
        }

        [DataMember, XmlAttribute]
        public string Author
        {
            get { return _author; }
            set
            {
                _author = value;
                NotifyOfPropertyChange();
            }
        }

        [DataMember, XmlAttribute]
        public bool IsGonnaBeDownloaded
        {
            get { return _isGonnaBeDownloaded; }
            set
            {
                _isGonnaBeDownloaded = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
