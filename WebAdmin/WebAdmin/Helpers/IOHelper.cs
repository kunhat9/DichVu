using System.IO;

namespace WebAdmin.Helpers
{
    public class IOHelper
    {
        public static bool CheckFolderExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool CheckFileExists(string path)
        {
            return File.Exists(path);
        }

        public static void CreateFolder(string path)
        {
            if (!CheckFolderExists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}