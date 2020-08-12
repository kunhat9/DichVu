using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace CORE.Helpers
{
    public static class ObjConvert
    {
        public static string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                // Convert byte[] to Base64 String
                string base64String = System.Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static Image GenerateQR(string userID)
        {
            try
            {
                var url = string.Format("https://chart.googleapis.com/chart?cht=qr&chs={1}x{2}&chl={0}", userID, "150", "150");
                WebResponse response = default(WebResponse);
                Stream remoteStream = default(Stream);
                StreamReader readStream = default(StreamReader);
                WebRequest request = WebRequest.Create(url);
                response = request.GetResponse();
                remoteStream = response.GetResponseStream();
                readStream = new StreamReader(remoteStream);
                Image img = Image.FromStream(remoteStream);
                return img;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        public static string GetImageToUrl(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return url;
                }
                else
                {
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(url);
                    string result = Convert.ToBase64String(bytes);
                    return result;
                }
            }
            catch (Exception ex)
            {
                return "";
                throw ex;
            }
        }
    }
}
