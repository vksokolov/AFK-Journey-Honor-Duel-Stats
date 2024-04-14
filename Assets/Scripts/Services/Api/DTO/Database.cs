using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Artifacts;
using Characters;
using Newtonsoft.Json;
using UnityEngine;
using Utils;
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
            string exportString)
        {
            try
            {
                Rows = FromExportString(exportString).ToList();
            }
            catch
            {
                Rows = JsonConvert.DeserializeObject<Database>(exportString).Rows;
            }

            OnDataChanged?.Invoke();
        }

        public void AddRow(DatabaseRow row)
        {
            Rows.Add(row);
            OnDataChanged?.Invoke();
        }

        public void RemoveRow(DatabaseRow row)
        {
            var foundRow = Rows.FirstOrDefault(r =>
                r.StarCount == row.StarCount && r.Artifact == row.Artifact &&
                Math.Abs(r.AvgPlace - row.AvgPlace) < _epsilon && r.TeamComp == row.TeamComp);
            Rows.Remove(foundRow);
            OnDataChanged?.Invoke();
        }

        public void Clear()
        {
            Rows.Clear();
            OnDataChanged?.Invoke();
        }

        public string ToExportString() =>
            ByteConverter.ToBase64String(DatabaseRow.ArrayToByteArray(Rows.ToArray()));

        public DatabaseRow[] FromExportString(string exportString) =>
            DatabaseRow
                .ArrayFromByteArray(ByteConverter.FromBase64String<byte[]>(exportString));
    }

    [Serializable]
    [JsonConverter(typeof(DatabaseRowConverter))]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DatabaseRow
    {
        public const byte ByteSize = 10;
        
        [JsonProperty("p")] public float AvgPlace;
        [JsonProperty("a")] public ArtifactType Artifact;
        [JsonProperty("s")] public int StarCount;
        [JsonProperty("t")] public long TeamComp;

        public int AvgPlaceInt => (int)AvgPlace;

        public static byte[] ToByteArray(DatabaseRow row)
        {
            byte[] buffer = new byte[ByteSize]; // 2 bytes for AvgPlace, Artifact, and StarCount, 8 bytes for TeamComp
            ushort packedValue = 0;
            packedValue |= (ushort)(row.AvgPlaceInt & 0xF); // Lower 4 bits for AvgPlace
            packedValue |= (ushort)((row.AvgPlaceInt & 0x7F) << 4); // Next 7 bits for Artifact
            packedValue |= (ushort)((row.StarCount & 0x3) << 11); // Next 2 bits for StarCount
            buffer[0] = (byte)(packedValue & 0xFF); // Low byte
            buffer[1] = (byte)((packedValue >> 8) & 0xFF); // High byte
            Buffer.BlockCopy(BitConverter.GetBytes(row.TeamComp), 0, buffer, 2, 8);
            return buffer;
        }

        public static DatabaseRow FromByteArray(byte[] buffer)
        {
            ushort packedValue = (ushort)(buffer[0] | (buffer[1] << 8));
            DatabaseRow row;
            row.AvgPlace = packedValue & 0xF; // Extract AvgPlace
            row.Artifact = (ArtifactType)((packedValue >> 4) & 0x7F); // Extract Artifact
            row.StarCount = (packedValue >> 11) & 0x3; // Extract StarCount
            row.TeamComp = BitConverter.ToInt64(buffer, 2);
            return row;
        }

        public static byte[] ArrayToByteArray(DatabaseRow[] rows)
        {
            var buffer = new byte[rows.Length * ByteSize];
            for (var i = 0; i < rows.Length; i++)
            {
                var rowArr = ToByteArray(rows[i]);
                Buffer.BlockCopy(rowArr, 0, buffer, i * ByteSize, ByteSize);
            }

            return buffer;
        }

        public static DatabaseRow[] ArrayFromByteArray(byte[] buffer)
        {
            var rows = new DatabaseRow[buffer.Length / ByteSize];
            for (var i = 0; i < rows.Length; i++)
            {
                var rowArr = new byte[ByteSize];
                Buffer.BlockCopy(buffer, i * ByteSize, rowArr, 0, ByteSize);
                rows[i] = FromByteArray(rowArr);
            }

            return rows;
        }
    }
}