using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace YouTubeDownloader
{
    public static class DataCollectionLoader
    {

        public static void SaveCollection<T>(T[] values, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T[]));
            string filePathWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            if (filePathWithoutExtension != null)
            {
                using (var writer = new StreamWriter(filePathWithoutExtension))
                    serializer.Serialize(writer, values);
            }
        }

        public static T[] LoadCollection<T>(string filePath)
        {
            if (File.Exists(filePath))
                try
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        var serializer = new XmlSerializer(typeof(T[]));
                        return (T[])serializer.Deserialize(reader);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    // ignored
                }
            return new T[0];
        }
    }
}