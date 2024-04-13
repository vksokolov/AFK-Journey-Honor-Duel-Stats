using System;
using Artifacts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Api.DTO;
using UnityEngine;

namespace Utils.Converters
{
    public class DatabaseRowConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var row = (DatabaseRow)value;
            JObject obj = new()
            {
                ["a"] = (int)row.Artifact,
                ["p"] = row.AvgPlace,
                ["s"] = row.StarCount,
                ["t"] = row.TeamComp,
            };
            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            var artifactTypeToken = obj["a"] ?? obj["Artifact"];
            return new DatabaseRow()
            {
                Artifact = (ArtifactType)(int)artifactTypeToken,
                AvgPlace = (float)(obj["p"] ?? obj["AvgPlace"]),
                StarCount = (int)(obj["s"] ?? obj["StarCount"]),
                TeamComp = (long)(obj["t"] ?? obj["TeamComp"]),
            };
        }

        public override bool CanConvert(Type objectType) => 
            objectType == typeof(DatabaseRow);
    }
}