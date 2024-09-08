using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AniFlow_.NET
{
    internal class BitmapFunctions
    {
        public static ImageSource Base64ToImageSource(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public string? ImageSourceToBase64String(System.Windows.Media.ImageSource imageSource)
        {
            if (imageSource is BitmapSource bitmapSource)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(memoryStream);

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            return null;
        }
    }
}
