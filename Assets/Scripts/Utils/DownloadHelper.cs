using System.Runtime.InteropServices;

namespace Utils
{
    public class DownloadHelper
    {
        [DllImport("__Internal")]
        public static extern void DownloadFile(byte[] array, int byteLength, string fileName);
    }
}
