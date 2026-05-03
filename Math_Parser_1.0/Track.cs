using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Math_Parser_1._0
{
    public class Track
    {
        public Track(string title, string artist, string url, string cover, double volume = 1)
        {
            Title = title;
            Artist = artist;
            Url = url;
            Volume = volume;
            Cover = new BitmapImage(
    new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cover)));
     
        }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Url { get; set; }
        public double Volume { get; set; }

        public ImageSource Cover { get; set; }
    }
}
