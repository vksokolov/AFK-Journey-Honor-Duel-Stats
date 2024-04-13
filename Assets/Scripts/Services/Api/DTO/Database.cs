using System;
using System.Collections.Generic;
using System.Linq;
using Artifacts;
using Characters;
using Newtonsoft.Json;
using UnityEngine;
using Utils.Converters;

namespace Services.Api.DTO
{
    [Serializable]
    public class Database
    {
        private const float _epsilon = .001f;
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
            var data = JsonConvert.DeserializeObject<Database>(dataJson);
            Rows = data.Rows;
        }

        public static Database FromJson(string json) => 
            JsonUtility.FromJson<Database>(json);

        public string ToJson() => 
            JsonConvert.SerializeObject(this);

        public void AddRow(DatabaseRow row)
        {
            Rows.Add(row);
            OnDataChanged?.Invoke();
        }
        
        public void RemoveRow(DatabaseRow row)
        {
            var foundRow = Rows.FirstOrDefault(r => r.StarCount == row.StarCount && r.Artifact == row.Artifact && Math.Abs(r.AvgPlace - row.AvgPlace) < _epsilon && r.TeamComp == row.TeamComp);
            Rows.Remove(foundRow);
            OnDataChanged?.Invoke();
        }
    }

    [Serializable]
    [JsonConverter(typeof(DatabaseRowConverter))]
    public class DatabaseRow
    {
        [JsonProperty("p")] public float AvgPlace;
        [JsonProperty("a")] public ArtifactType Artifact;
        [JsonProperty("s")] public int StarCount;
        [JsonProperty("t")] public long TeamComp;
    }
}