using System;
using Scripts.Filters;
using Services.Api.DTO;

namespace Gui.Artifacts
{
    public class ArtifactTableRow : TableEntryElement<ArtifactData>, IFilterSource
    {
        public ArtifactElement ArtifactElement;

        public event Action<FilterData> OnFilterApplied;

        public override void SetContent(ArtifactData artifactData)
        {
            AvgPlaceElement.SetContent(artifactData.AvgPlace);
            ArtifactElement.SetContent(artifactData);
            ArtifactElement.SetButtonAction(() => ApplyFilter(ArtifactElement.Data.ToFilterData()));
        }

        private void ApplyFilter(FilterData toFilterData) => 
            OnFilterApplied?.Invoke(toFilterData);
    }
}