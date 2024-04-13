#if UNITY_EDITOR
#define DEBUG_OR_PC
#endif

using System;
using System.IO;
using Gui.Windows;
using UnityEngine;


namespace Utils
{
    public class SaveDataHelper
    {
        private static string _saveFilePath = Application.persistentDataPath + "/afk-journey-honor-duel-stats.json";
        private readonly JsonWindow _jsonWindow;
        
        public SaveDataHelper(JsonWindow jsonWindow)
        {
            _jsonWindow = jsonWindow;
        }
        
        public void ImportData(Action<string> callback)
        {
#if DEBUG_OR_PC
            _jsonWindow.gameObject.SetActive(true);
#else
            FileUploaderHelper.UploadFile(callback);
#endif
        }

        public void ExportData(string json)
        {
#if DEBUG_OR_PC
            _jsonWindow.JsonInputField.text = json;
            _jsonWindow.gameObject.SetActive(true);
#else
            DownloadHelper.ExportData(json);
#endif
        }
    }
}