using System;
using System.Collections.Generic;
using Artifacts;
using Characters;
using Utils;

namespace Scripts.Filters
{
    public class FilterModel : IFilterEventReceiver
    {
        public event Action OnFilterChanged;

        public long HeroFilterBitmask { get; private set; }
        
        private HashSet<HeroFilter> _heroFilters = new();
        public ArtifactFilter ArtifactFilter { get; private set; }
        public IReadOnlyHashSet<HeroFilter> HeroFilters;
        
        public FilterModel()
        {
            HeroFilters = new ReadOnlyHashSet<HeroFilter>(_heroFilters);
        }

        public void ApplyFilter(FilterData filterData)
        {
            if (filterData is HeroFilter heroFilter)
            {
                var newFilter = HeroFilterBitmask | heroFilter.Filter.BitMask;

                if (HeroFilterBitmask == newFilter) 
                    return;
                
                HeroFilterBitmask = newFilter;
                _heroFilters.Add(heroFilter);
                OnFilterChanged?.Invoke();
            }
            else if (filterData is ArtifactFilter artifactTypeFilter)
            {
                if (ArtifactFilter != null)
                    return;
                
                ArtifactFilter = artifactTypeFilter;
                OnFilterChanged?.Invoke();
            }
        }
        
        public void RemoveFilter(FilterData filterData)
        {
            if (filterData is HeroFilter heroFilter)
            {
                var newFilter = HeroFilterBitmask & ~heroFilter.Filter.BitMask;
                
                if (HeroFilterBitmask != newFilter)
                {
                    HeroFilterBitmask = newFilter;
                    _heroFilters.Remove(heroFilter);
                    OnFilterChanged?.Invoke();
                }
            }
            else if (filterData is ArtifactFilter artifactTypeFilter)
            {
                if (ArtifactFilter != artifactTypeFilter)
                    return;
                
                ArtifactFilter = null;
                OnFilterChanged?.Invoke();
            }
        }
        
        public void AddFilterSource(IFilterSource filterSource)
        {
            filterSource.OnFilterApplied += ApplyFilter;
        }

        public List<FilterData> GetFilters()
        {
            var filters = new List<FilterData>();
            if (ArtifactFilter != null)
                filters.Add(ArtifactFilter);
            filters.AddRange(_heroFilters);
            return filters;
        }
    }
}
