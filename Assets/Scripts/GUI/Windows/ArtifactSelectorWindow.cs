using System;
using System.Collections.Generic;
using Artifacts;
using Characters;
using Gui.Artifacts;
using Services.Api.DTO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gui.Windows
{
    public class ArtifactSelectorWindow : MonoBehaviour
    {
        public List<ArtifactElement> ArtifactElements;

        private Action<ArtifactData> _callback;
        
        public void Init(ArtifactPreset artifactPreset)
        {
            var sortedArtifacts = artifactPreset.Artifacts;
            sortedArtifacts.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
            for (var i = 0; i < ArtifactElements.Count; i++)
            {
                var element = ArtifactElements[i];
                if (i < artifactPreset.Artifacts.Count)
                {
                    var artifactData = new ArtifactData()
                    {
                        Artifact =  sortedArtifacts[i],
                        AvgPlace = 0,
                        StarCount = -1,
                    };
                    element.SetContent(artifactData);
                    element.SetButtonAction(() => OnArtifactButtonClicked(element));
                }
                else
                    element.gameObject.SetActive(false);
            }
        }
        
        public void SelectArtifact(Action<ArtifactData> onArtifactSelected)
        {
            gameObject.SetActive(true);
            _callback = onArtifactSelected;
        }

        public void OnArtifactButtonClicked(ArtifactElement artifactElement)
        {
            _callback?.Invoke(artifactElement.Data);
            gameObject.SetActive(false);
        }
    }
}