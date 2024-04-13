using System.Runtime.InteropServices;

namespace Utils
{
    public class DownloadHelper
    {
        [DllImport("__Internal")]
        private static extern void DownloadFile(byte[] array, int byteLength, string fileName);

        public static void ExportData(string json)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            DownloadFile(bytes, bytes.Length, "afk-journey-honor-duel-stats.json");
        }
    }
}
