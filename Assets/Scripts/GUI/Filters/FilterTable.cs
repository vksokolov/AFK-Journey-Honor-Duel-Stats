using System;
using System.Collections.Generic;
using Scripts.Filters;

namespace Gui.Filters
{
    public class FilterTable : Table<FilterTableRow, FilterData>
    {
        private const int MaxArtifactFilters = 1;
        private const int MaxHeroFilters = 5;
        private const int HeroElementsOffset = 1;
        
        private const int ArtifactElementIndex = 0;
        private FilterTableRow ArtifactRow => Elements[ArtifactElementIndex];
        
        public event Action<FilterData> OnFilterRemoved; 

        public override void SetElements(List<FilterData> elements)
        {
            var artifactFilters = 0;
            var heroFilters = 0;
            for (var i = 0; i < elements.Count; i++)
            {
                if (elements[i] is HeroFilter heroFilter)
                {
                    if (heroFilters > MaxHeroFilters)
                        continue;

                    if (i < Elements.Count)
                        SetHeroFilter(heroFilter, heroFilters + HeroElementsOffset);
                    
                    heroFilters++;
                }
                else if (elements[i] is ArtifactFilter artifactFilter)
                {
                    artifactFilters++;
                    if (artifactFilters > MaxArtifactFilters)
                        continue;

                    if (i < Elements.Count)
                        SetArtifactFilter(artifactFilter);
                }
            }
            
            if (artifactFilters == 0)
                ArtifactRow.gameObject.SetActive(false);
            
            if (heroFilters < MaxHeroFilters)
            {
                for (var i = heroFilters + HeroElementsOffset; i < MaxHeroFilters + HeroElementsOffset; i++)
                    Elements[i].gameObject.SetActive(false);
            }
            
            OnElementsUpdated();
        }

        private void SetArtifactFilter(ArtifactFilter artifactFilter)
        {
            ArtifactRow.SetContent(artifactFilter);
            ArtifactRow.gameObject.SetActive(true);
            ArtifactRow.OnFilterRemoved += OnRemoveFilterButtonClicked;
        }

        private void OnRemoveFilterButtonClicked(FilterData obj) => 
            OnFilterRemoved?.Invoke(obj);

        private void SetHeroFilter(HeroFilter heroFilter, int index)
        {
            var element = Elements[index];
            element.SetContent(heroFilter);
            element.gameObject.SetActive(true);
            element.OnFilterRemoved += OnRemoveFilterButtonClicked;
        }
    }
}