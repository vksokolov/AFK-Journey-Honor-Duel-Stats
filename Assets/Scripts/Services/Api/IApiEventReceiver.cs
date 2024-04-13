using Services.Api.DTO;

namespace Services.Api
{
    public interface IApiEventReceiver
    {
        void OnLoadDataButtonClicked(string json);
        void OnAddResultButtonClicked(DatabaseRow databaseRow);
        void ExportData();
        void ImportData();
        void SetCombining(bool isOn);
        void OnDeleteAllDataButtonClicked();
    }
}