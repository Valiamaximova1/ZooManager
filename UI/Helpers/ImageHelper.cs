using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace UI.Helpers
{

    public static class ImageHelper
    {
        public static BitmapImage LoadImageSafe(string path)
        {
            if (!File.Exists(path))
                return null;

            var bitmap = new BitmapImage();
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // зарежда всичко в паметта
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }
            bitmap.Freeze(); // прави го достъпен от други нишки и безопасен
            return bitmap;
        }
    }
}
