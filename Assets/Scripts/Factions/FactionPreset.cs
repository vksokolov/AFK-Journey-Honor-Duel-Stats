using System.Collections.Generic;
using UnityEngine;

namespace Factions
{
    [CreateAssetMenu(fileName = nameof(FactionPreset), menuName = "Presets/" + nameof(FactionPreset))]
    public class FactionPreset : ScriptableObject
    {
        public List<FactionData> Factions;
    }
}
