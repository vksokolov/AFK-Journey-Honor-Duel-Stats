using System;
using System.Collections.Generic;
using System.Linq;
using Gui.Artifacts;
using Scripts.Filters;
using Services.Api.DTO;
using UnityEngine.UI;

namespace Gui
{
    public class TeamTableRow : TableEntryElement<TeamData>, IFilterSource
    {
        public ArtifactElement ArtifactElement;
        public List<HeroElement> HeroElements;
        public Button DeleteButton;

        public event Action<FilterData> OnFilterApplied;
        
        public TeamData Data { get; private set; }


        public override void SetContent(TeamData teamData)
        {
            Data = teamData;
            ArtifactElement.SetContent(teamData.Artifact);
            ArtifactElement.SetButtonAction(() => ApplyFilter(teamData.Artifact.ToFilterData()));
            AvgPlaceElement.SetContent(teamData.AvgPlace);
            
            var heroes = teamData.Heroes.ToList();
            for (var i = 0; i < heroes.Count; i++)
            {
                var index = i;
                HeroElements[i].SetContent(heroes[i]);
                HeroElements[i].SetButtonAction(() => ApplyFilter(heroes[index].ToFilterData()));
            }
        }

        public void SetDeleteButtonActive(bool active) => 
            DeleteButton.gameObject.SetActive(active);

        public void SetDeleteButtonAction(Action<TeamData> action)
        {
            DeleteButton.onClick.RemoveAllListeners();
            DeleteButton.onClick.AddListener(() => action(Data));
        }

        private void ApplyFilter(FilterData toFilterData) => 
            OnFilterApplied?.Invoke(toFilterData);
    }
}