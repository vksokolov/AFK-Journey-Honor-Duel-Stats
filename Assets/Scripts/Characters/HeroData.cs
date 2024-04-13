using System;
using Factions;
using Scripts.Filters;
using UnityEngine;

namespace Characters
{
    [Serializable]
    public class HeroData
    {
        public string Name;
        public int Id;
        public Faction Faction;
        public Sprite Portrait;

        public long BitMask => 1L << Id - 1; // 1, 2, 4, 8, ...
        
        public FilterData ToFilterData() => 
            new HeroFilter() {Filter = this};
    }
}
