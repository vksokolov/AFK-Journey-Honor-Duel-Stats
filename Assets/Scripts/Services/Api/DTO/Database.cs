using System;
using System.Collections.Generic;
using Artifacts;
using Characters;
using UnityEngine;

namespace Services.Api.DTO
{
    [Serializable]
    public class Database
    {
        public event Action OnDataChanged;
        public List<DatabaseRow> Rows = new();

        public void SetData(
            string dataJson)
        {
            ParseJson(dataJson);
            OnDataChanged?.Invoke();
        }

        private void ParseJson(string dataJson)
        {
            var data = JsonUtility.FromJson<Database>(dataJson);
            Rows = data.Rows;
        }

        public static Database FromJson(string json) => 
            JsonUtility.FromJson<Database>(json);

        public string ToJson() => 
            JsonUtility.ToJson(this);

        public void AddRow(DatabaseRow row)
        {
            Rows.Add(row);
            OnDataChanged?.Invoke();
        }
    }

    [Serializable]
    public class DatabaseRow
    {
        public float AvgPlace;
        public ArtifactType Artifact;
        public int StarCount;
        public long TeamComp;
    }
}