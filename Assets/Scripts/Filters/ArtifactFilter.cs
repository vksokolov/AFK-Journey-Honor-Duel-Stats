using Artifacts;
using Services.Api.DTO;

namespace Scripts.Filters
{
    public class ArtifactFilter : FilterData<ArtifactData>
    {
        public bool Check(Artifact artifact) => 
            Filter.Artifact.ArtifactType == artifact.ArtifactType;

        public bool Check(ArtifactType artifactType) => 
            Filter.Artifact.ArtifactType == artifactType;
    }
}