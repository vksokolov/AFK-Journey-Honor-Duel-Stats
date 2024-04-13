using System.Collections.Generic;
using Characters;

namespace Services.Api.DTO
{
    public class TeamData
    {
        public float AvgPlace;
        public ArtifactData Artifact;
        public HashSet<HeroData> Heroes;
        
        public long ToBitMask()
        {
            long bitMask = 0;
            foreach (var hero in Heroes)
                bitMask |= hero.BitMask;
            return bitMask;
        }
    }
}
