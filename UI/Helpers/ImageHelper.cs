using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace UI.Helpers
{

    public static class ImageHelper
    {
        public static void DeleteFileFromAllLocations(string imgPath, string relativePath)
        {
            imgPath = Path.Combine("Assets", imgPath);


            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            try
            {
                string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                if (File.Exists(outputPath))
                    File.Delete(outputPath);

                string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
                string sourcePath = Path.Combine(projectRoot, imgPath);
                if (File.Exists(sourcePath))
                    File.Delete(sourcePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при изтриване на файл:\n{ex.Message}", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
