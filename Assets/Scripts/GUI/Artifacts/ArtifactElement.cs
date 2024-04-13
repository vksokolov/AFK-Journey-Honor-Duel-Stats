using System;
using System.Collections.Generic;
using Scripts.Filters;
using Services.Api.DTO;
using TMPro;
using UnityEngine.UI;

namespace Gui.Artifacts
{
    public class ArtifactElement : SettableUiElement<ArtifactData>
    {
        private static Dictionary<int, string> _starCountToText = new()
        {
            { -1, string.Empty },
            { 0, "☆☆" },
            { 1, "★☆" },
            { 2, "★★" },
        };
        public Image Portrait;
        public TMP_Text NameText;
        public TMP_Text StarCountText;
        public Button Button;
        
        public ArtifactData Data { get; private set; }

        public override void SetContent(ArtifactData artifactData)
        {
            Data = artifactData;
            var artifact = artifactData.Artifact;
            Portrait.sprite = artifact.Icon;
            NameText.text = artifact.Name;
            StarCountText.text = _starCountToText[artifactData.StarCount];
        }
        
        public void SetButtonAction(Action action)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => action());
        }
    }
}