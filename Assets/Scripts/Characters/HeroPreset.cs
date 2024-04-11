using System.Collections.Generic;
using Services.Api.DTO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
    [CreateAssetMenu(fileName = nameof(HeroPreset), menuName = "Presets/" + nameof(HeroPreset))]
    public class HeroPreset : ScriptableObject
    {
        private Dictionary<long, HeroData> _heroesByBitMaskId;
        
        [FormerlySerializedAs("Characters")] public List<HeroData> Heroes;

        public List<HeroData> GetHeroes(long teamComp)
        {
            _heroesByBitMaskId ??= CreateHeroBitMaskMap(Heroes);
            
            var heroes = new List<HeroData>();
            var remainingTeam = teamComp;
            while (remainingTeam > 0)
            {
                var heroBitMask = remainingTeam & -remainingTeam;
                remainingTeam &= ~heroBitMask;
                heroes.Add(_heroesByBitMaskId[heroBitMask]);
            }

            return heroes;
        }

        private Dictionary<long, HeroData> CreateHeroBitMaskMap(List<HeroData> characters)
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