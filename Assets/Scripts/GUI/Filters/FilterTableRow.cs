using System;
using Scripts.Filters;

namespace Gui.Filters
{
    public class FilterTableRow : TableEntryElement<FilterData>
    {
        public FilterElement FilterElement;
        
        public event Action<FilterData> OnFilterRemoved;
        
        public override void SetContent(FilterData data)
        {
            FilterElement.SetContent(data);
            FilterElement.OnRemoveButtonClicked += OnFilterElementRemoveButtonClicked;
        }

        private void OnFilterElementRemoveButtonClicked(FilterData filterData) => 
            OnFilterRemoved?.Invoke(filterData);
    }
}