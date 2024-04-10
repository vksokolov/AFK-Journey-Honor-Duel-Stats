using System.Collections.Generic;
using UnityEngine;

namespace Factions
{
    [CreateAssetMenu(fileName = "FactionPreset", menuName = "Presets/Faction Preset")]
    public class FactionPreset : ScriptableObject
    {
        public List<FactionData> Factions;
    }
}
