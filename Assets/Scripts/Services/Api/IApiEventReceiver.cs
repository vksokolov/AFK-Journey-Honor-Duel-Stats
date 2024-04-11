using Services.Api.DTO;

namespace Services.Api
{
    public interface IApiEventReceiver
    {
        void OnLoadDataButtonClicked(string json);
        void OnAddResultButtonClicked(DatabaseRow databaseRow);
        string ExportData();
    }
}