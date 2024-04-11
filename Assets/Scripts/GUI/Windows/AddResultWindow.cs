using System.Collections.Generic;
using Artifacts;
using Characters;
using Services.Api.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;

namespace Gui.Windows
{
    public class AddResultWindow : MonoBehaviour
    {
        private readonly Dictionary<int, ArtifactType> _artifactMap = new();
        
        public TMP_Dropdown ArtifactDropdown;
        public TMP_Dropdown ArtifactStarsDropdown;
        public TMP_Dropdown HeroDropdown1;
        public TMP_Dropdown HeroDropdown2;
        public TMP_Dropdown HeroDropdown3;
        public TMP_Dropdown HeroDropdown4;
        public TMP_Dropdown HeroDropdown5;
        public TMP_InputField PlaceInputField;

        private HeroPreset _heroPreset;
        private ArtifactPreset _artifactPreset;
        
        public ArtifactType SelectedArtifact { get; private set; }
        public float Place => float.Parse(PlaceInputField.text);
        public int StarCount => ArtifactStarsDropdown.value;

        public TeamData SelectedTeam
        {
            get
            {
                var selectedHeroes = new List<HeroData>()
                {
                    _heroPreset.GetHero(HeroDropdown1.options[HeroDropdown1.value].text),
                    _heroPreset.GetHero(HeroDropdown2.options[HeroDropdown2.value].text),
                    _heroPreset.GetHero(HeroDropdown3.options[HeroDropdown3.value].text),
                    _heroPreset.GetHero(HeroDropdown4.options[HeroDropdown4.value].text),
                    _heroPreset.GetHero(HeroDropdown5.options[HeroDropdown5.value].text)
                };
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
            SetArtifactDropdownOptions(_artifactPreset.Artifacts);
            SetHeroDropdownOptions(_heroPreset.Heroes);
        }

        public void SetArtifactDropdownOptions(List<Artifact> artifacts)
        {
            _artifactMap.Clear();
            for (var i = 0; i < artifacts.Count; i++)
                _artifactMap.Add(i, artifacts[i].ArtifactType);
            
            ArtifactDropdown.ClearOptions();
            ArtifactDropdown.AddOptions(CreateOptions());

            List<TMP_Dropdown.OptionData> CreateOptions()
            {
                var options = new List<TMP_Dropdown.OptionData>();
                foreach (var artifact in artifacts)
                    options.Add(new TMP_Dropdown.OptionData(artifact.Name, artifact.Icon));

                return options;
            }
        }
        
        public void SetHeroDropdownOptions(List<HeroData> heroes)
        {
            HeroDropdown1.ClearOptions();
            HeroDropdown2.ClearOptions();
            HeroDropdown3.ClearOptions();
            HeroDropdown4.ClearOptions();
            HeroDropdown5.ClearOptions();
            
            HeroDropdown1.AddOptions(CreateOptions());
            HeroDropdown2.AddOptions(CreateOptions());
            HeroDropdown3.AddOptions(CreateOptions());
            HeroDropdown4.AddOptions(CreateOptions());
            HeroDropdown5.AddOptions(CreateOptions());

            List<TMP_Dropdown.OptionData> CreateOptions()
            {
                var options = new List<TMP_Dropdown.OptionData>();
                foreach (var hero in heroes)
                    options.Add(new TMP_Dropdown.OptionData(hero.Name, hero.Portrait));

                return options;
            }
        }
        
        [Preserve]
        public void OnArtifactDropdownValueChanged(int index) => 
            SelectedArtifact = _artifactMap[index];
    }
}