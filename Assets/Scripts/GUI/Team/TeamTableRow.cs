using System.Collections.Generic;
using Gui.Artifacts;
using Services.Api.DTO;

namespace Gui
{
    public class TeamTableRow : TableEntryElement<TeamData>
    {
        public ArtifactElement ArtifactElement;
        public List<HeroElement> HeroElements;
        
        public override void SetContent(TeamData teamData)
        {
            ArtifactElement.SetContent(teamData.Artifact);
            AvgPlaceElement.SetContent(teamData.AvgPlace);
            
            for (var i = 0; i < teamData.Heroes.Count; i++) 
                HeroElements[i].SetContent(teamData.Heroes[i]);
        }
    }
}