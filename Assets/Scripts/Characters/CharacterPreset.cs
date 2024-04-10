using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "CharacterPreset", menuName = "Presets/Character Preset")]
    public class CharacterPreset : ScriptableObject
    {
        public List<CharacterData> Characters;
    }
}