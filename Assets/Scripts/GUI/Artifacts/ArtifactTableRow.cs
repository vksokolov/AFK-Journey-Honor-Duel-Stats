using Services.Api.DTO;

namespace Gui.Artifacts
{
    public class ArtifactTableRow : TableEntryElement<ArtifactData>
    {
        public ArtifactElement ArtifactElement;
        
        public override void SetContent(ArtifactData artifactData)
        {
            AvgPlaceElement.SetContent(artifactData.AvgPlace);
            ArtifactElement.SetContent(artifactData);
        }
    }
}