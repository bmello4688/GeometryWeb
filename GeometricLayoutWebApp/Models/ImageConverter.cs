using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GeometricLayout.Models
{
    public static class ImageConverter
    {
        public static string ConvertTo(Bitmap image)
        {
            using (MemoryStream m = new MemoryStream())
            {
                image.Save(m, ImageFormat.Png);
                byte[] imageBytes = m.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static Bitmap ConvertFrom(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);

            using (MemoryStream m = new MemoryStream(bytes))
            {
                return Image.FromStream(m) as Bitmap;
            }
        }
    }
}