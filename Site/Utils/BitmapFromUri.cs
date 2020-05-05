using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Site.Utils
{
    public static class BitmapFromUri
    {
        public static ImageSource BitmapFromPath(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
    }
}
