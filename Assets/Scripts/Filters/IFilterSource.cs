using System;

namespace Scripts.Filters
{
    public interface IFilterSource
    {
        event Action<FilterData> OnFilterApplied;
    }
}