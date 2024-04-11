using System;
using System.Collections.Generic;
using Gui.Artifacts;
using Services.Api.DTO;
using UnityEngine.UI;

namespace Gui
{
    public class TeamTableRow : TableEntryElement<TeamData>
    {
        public ArtifactElement ArtifactElement;
        public List<HeroElement> HeroElements;
        public Button DeleteButton;
        
        public TeamData Data { get; private set; }

        public override void SetContent(TeamData teamData)
        {
            Data = teamData;
            ArtifactElement.SetContent(teamData.Artifact);
            AvgPlaceElement.SetContent(teamData.AvgPlace);
            
            for (var i = 0; i < teamData.Heroes.Count; i++) 
                HeroElements[i].SetContent(teamData.Heroes[i]);
        }
        
        public void SetDeleteButtonActive(bool active) => 
            DeleteButton.gameObject.SetActive(active);
        
        public void SetDeleteButtonAction(Action<TeamData> action)
        {
            DeleteButton.onClick.RemoveAllListeners();
            DeleteButton.onClick.AddListener(() => action(Data));
        }
    }
}