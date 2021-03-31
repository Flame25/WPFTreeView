using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;  

namespace WPFTreeView
{
    [ValueConversion(typeof(string),typeof(BitmapImage))] 
    class HeaderToImage : IValueConverter
    {
        public static HeaderToImage Instance = new HeaderToImage(); 

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = (string)value;
            if (path == null)
                return null;
            var image = "image/file.png";
            var name = MainWindow.GetFolderName(path); 
            if(string.IsNullOrEmpty(name))
            {
                image = "image/drive.png"; 
            }
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
            {
                image = "image/folder-closed.png"; 
            }

            return new BitmapImage(new Uri($"pack://application:,,,/{image}")); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
