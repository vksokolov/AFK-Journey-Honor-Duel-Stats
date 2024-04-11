using System.Collections.Generic;
using System.Linq;
using Artifacts;
using Characters;
using Gui.Artifacts;
using Services.Api.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;

namespace Gui.Windows
{
    public class AddResultWindow : MonoBehaviour
    {
        private readonly Dictionary<int, ArtifactType> _artifactMap = new();
        
        public HeroSelectorWindow HeroSelectorWindow;
        public ArtifactSelectorWindow ArtifactSelectorWindow;
        public ArtifactElement ArtifactElement;
        public TMP_Dropdown ArtifactStarsDropdown;
        public List<HeroElement> HeroElements;
        public TMP_InputField TotalWinsInputField;

        private HeroPreset _heroPreset;
        private ArtifactPreset _artifactPreset;

        public ArtifactType SelectedArtifact => ArtifactElement.Data.Artifact.ArtifactType;
        public float Place => float.Parse(TotalWinsInputField.text);
        public int StarCount => ArtifactStarsDropdown.value;

        public TeamData SelectedTeam
        {
            get
            {
                var selectedHeroes = HeroElements.Select(element => element.Data).ToList();
                return new TeamData
                {
                    AvgPlace = Place,
                    Heroes = selectedHeroes
                };
            }
        }
        
        public void Init(HeroPreset heroPreset, ArtifactPreset artifactPreset)
        {
            _heroPreset = heroPreset;
            _artifactPreset = artifactPreset;
            HeroSelectorWindow.Init(heroPreset);
            ArtifactSelectorWindow.Init(artifactPreset);
            InitArtifactButton(_artifactPreset.Artifacts);
            InitHeroButtons(_heroPreset.Heroes);
        }

        private void InitArtifactButton(IReadOnlyList<Artifact> artifacts)
        {
            _artifactMap.Clear();
            for (var i = 0; i < artifacts.Count; i++)
                _artifactMap.Add(i, artifacts[i].ArtifactType);
            
            ArtifactElement.SetButtonAction(() => OnArtifactButtonClicked(ArtifactElement));
        }

        private void InitHeroButtons(IReadOnlyList<HeroData> heroes)
        {
            for (var i = 0; i < HeroElements.Count; i++)
            {
                var heroElement = HeroElements[i];
                heroElement.SetContent(heroes[i]);
                heroElement.SetButtonAction(() => OnHeroButtonClicked(heroElement));
            }
        }
        
        private void OnHeroButtonClicked(HeroElement heroElement) => 
            HeroSelectorWindow.SelectHero(heroElement.SetContent);
        
        private void OnArtifactButtonClicked(ArtifactElement artifactElement) => 
            ArtifactSelectorWindow.SelectArtifact(artifactElement.SetContent);
    }
}