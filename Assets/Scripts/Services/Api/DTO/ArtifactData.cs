using Artifacts;
using Scripts.Filters;

namespace Services.Api.DTO
{
    public class ArtifactData
    {
        public Artifact Artifact;
        public int StarCount;
        public float AvgPlace;
        
        public FilterData ToFilterData() => 
            new ArtifactFilter() {Filter = this};
    }
}