using System;
using Gui.Artifacts;
using Scripts.Filters;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace Gui.Filters
{
    public class FilterElement : SettableUiElement<FilterData>
    {
        public ArtifactElement ArtifactElement;
        public HeroElement HeroElement;
        public Button RemoveButton;
        
        public FilterData Data { get; private set; }
        
        public event Action<FilterData> OnRemoveButtonClicked;

        private void Awake()
        {
            ArtifactElement.SetButtonAction(null);
            HeroElement.SetButtonAction(null);
        }

        public override void SetContent(FilterData filterData)
        {
            Data = filterData;
            
            if (filterData is HeroFilter heroFilter)
                SetHeroFilter(heroFilter);
            else if (filterData is ArtifactFilter artifactFilter) 
                SetArtifactFilter(artifactFilter);
        }

        private void SetArtifactFilter(ArtifactFilter artifactFilter)
        {
            ArtifactElement.SetContent(artifactFilter.Filter);
            HeroElement.gameObject.SetActive(false);
        }

        private void SetHeroFilter(HeroFilter heroFilter)
        {
            HeroElement.SetContent(heroFilter.Filter);
            ArtifactElement.gameObject.SetActive(false);
        }

        [Preserve]
        public void OnRemoveFilterButtonClicked() => 
            OnRemoveButtonClicked?.Invoke(Data);
    }
}