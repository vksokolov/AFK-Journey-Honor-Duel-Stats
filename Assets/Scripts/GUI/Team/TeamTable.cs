using System;
using Services.Api.DTO;

namespace Gui
{
    public class TeamTable : FilteringTable<TeamTableRow, TeamData>
    {
        private Action<TeamData> _deleteAction;
        
        public void ShowDeleteButtons(bool active)
        {
            foreach (var tableRow in Elements)
                tableRow.SetDeleteButtonActive(active);
        }
        
        public void SetDeleteButtonAction(Action<TeamData> action)
        {
            _deleteAction = action;
        }

        protected override void OnElementsUpdated()
        {
            base.OnElementsUpdated();
            foreach (var tableRow in Elements)
                tableRow.SetDeleteButtonAction(_deleteAction);
        }
    }
}