using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utils
{
    public static class ByteConverter
    {
        public static string ToBase64String<T>(T obj)
        {
            if (obj == null)
                return null;
            
            if (obj is byte[] byteArr)
                return Convert.ToBase64String(byteArr);
            
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return Convert.ToBase64String(ms.ToArray());
        }

        public static T FromBase64String<T>(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return default;
            var data = Convert.FromBase64String(base64);
            if (typeof(T) == typeof(byte[]))
                return (T)(object)data;
            
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream(data);
            var obj = bf.Deserialize(ms);
            return (T)obj;
        }
    }
}