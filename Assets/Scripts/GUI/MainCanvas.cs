using System.Linq;
using Artifacts;
using Characters;
using Gui.Artifacts;
using Gui.Windows;
using Services.Api;
using Services.Api.DTO;
using UnityEngine;
using UnityEngine.Scripting;

namespace Gui
{
    public class MainCanvas : MonoBehaviour
    {
        public ArtifactTable ArtifactTable;
        public TeamTable TeamTable;
        [Space]
        public JsonWindow JsonWindow;
        public AddResultWindow AddResultWindow;
        
        private IApiEventReceiver _apiEventReceiver;
        private ArtifactPreset _artifactPreset;
        private HeroPreset _heroPreset;

        public void Init(
            IApiEventReceiver apiEventReceiver,
            ArtifactPreset artifactPreset,
            HeroPreset heroPreset)
        {
            _apiEventReceiver = apiEventReceiver;
            _artifactPreset = artifactPreset;
            _heroPreset = heroPreset;
            InitResultWindow(_heroPreset, _artifactPreset);
        }

        private void InitResultWindow(HeroPreset heroPreset, ArtifactPreset artifactPreset)
        {
            AddResultWindow.Init(heroPreset, artifactPreset);
        }

        [Preserve]
        public void OnLoadJsonButtonClicked() => 
            _apiEventReceiver.OnLoadDataButtonClicked(JsonWindow.JsonInputField.text);
        
        [Preserve]
        public void OnAddResultButtonClicked()
        {
            var team = AddResultWindow.SelectedTeam;
            if (team?.Heroes?.Distinct().Count() != 5)
                return;
            
            var artifact = AddResultWindow.SelectedArtifact;
            var place = AddResultWindow.Place;
            var starCount = AddResultWindow.StarCount;
            var row = new DatabaseRow()
            {
                Artifact = artifact,
                AvgPlace = place,
                StarCount = starCount,
                TeamComp = team.ToBitMask()
            };
            _apiEventReceiver.OnAddResultButtonClicked(row);
        }
        
        [Preserve]
        public void OnExportJsonButtonClicked()
        {
            JsonWindow.JsonInputField.text = _apiEventReceiver.ExportData();
            JsonWindow.gameObject.SetActive(true);
        }
    }
}