using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
    [CreateAssetMenu(fileName = nameof(HeroPreset), menuName = "Presets/" + nameof(HeroPreset))]
    public class HeroPreset : ScriptableObject
    {
        private Dictionary<long, HeroData> _heroesByBitMaskId;
        private HashSet<HeroData> _heroes;

        [FormerlySerializedAs("Characters")] public List<HeroData> Heroes;

        public HashSet<HeroData> GetHeroes(long teamComp)
        {
            _heroes ??= new HashSet<HeroData>(Heroes);
            _heroesByBitMaskId ??= CreateHeroBitMaskMap(_heroes);
            
            var heroes = new HashSet<HeroData>();
            var remainingTeam = teamComp;
            while (remainingTeam > 0)
            {
                var heroBitMask = remainingTeam & -remainingTeam;
                remainingTeam &= ~heroBitMask;
                heroes.Add(_heroesByBitMaskId[heroBitMask]);
            }

            return heroes;
        }

        private Dictionary<long, HeroData> CreateHeroBitMaskMap(HashSet<HeroData> characters)
        {
            var heroesByBitMaskId = new Dictionary<long, HeroData>();
            foreach (var hero in characters)
                heroesByBitMaskId[hero.BitMask] = hero;
            return heroesByBitMaskId;
        }

        public HeroData GetHero(string heroName) => 
            Heroes.Find(hero => hero.Name == heroName);
    }
}