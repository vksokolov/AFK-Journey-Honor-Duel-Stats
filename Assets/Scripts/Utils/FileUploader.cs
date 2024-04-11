using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Utils
{
    public class FileUploader : MonoBehaviour
    {
        public event Action<string> OnContentReceived;
        
        public void OnUploadedContentReceived(string data) => 
            OnContentReceived?.Invoke(data);
    }
}