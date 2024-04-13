using Gui.Filters;

namespace Scripts.Filters
{
    public interface IFilterEventReceiver
    {
        void ApplyFilter(FilterData filterData);
    }
}