using System;
using Scripts.Filters;

namespace Gui
{
    public class FilteringTable<TElement, TData> : Table<TElement, TData>, IFilterSource where TElement : SettableUiElement<TData>, IFilterSource
    {
        protected IFilterEventReceiver FilterEventReceiver;

        public event Action<FilterData> OnFilterApplied;
        
        public void SetFilterEventReceiver(IFilterEventReceiver filterEventReceiver) => 
            FilterEventReceiver = filterEventReceiver;

        protected override void OnElementsUpdated()
        {
            base.OnElementsUpdated();
            foreach (var element in Elements)
                element.OnFilterApplied += OnFilterAppliedByElement;
        }
        
        private void OnFilterAppliedByElement(FilterData filterData) => 
            OnFilterApplied?.Invoke(filterData);
    }
}