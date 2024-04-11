using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Utils
{
    public static class FileUploaderHelper
    {
        [DllImport("__Internal")]
        private static extern string BrowserTextUpload(string extFilter, string gameObjName, string dataSinkFn);

        private static FileUploader _fileUploader;
        private static Action<string> _callback;
        
        public static void UploadFile(Action<string> callback)
        {
            _fileUploader ??= CreateFileUploaderGameObject();
            _callback = callback;
            BrowserTextUpload(".txt, .json", nameof(FileUploader), nameof(FileUploader.OnUploadedContentReceived));
        }

        private static FileUploader CreateFileUploaderGameObject()
        {
            var go = new GameObject(nameof(FileUploader));
            var uploader = go.AddComponent<FileUploader>();
            uploader.OnContentReceived += OnContentReceived;
            return uploader;
        }

        private static void OnContentReceived(string obj) => 
            _callback?.Invoke(obj);
    }
}