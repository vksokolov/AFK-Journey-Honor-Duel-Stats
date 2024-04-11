using System.Collections.Generic;
using Artifacts;
using Services.Api.DTO;
using TMPro;
using UnityEngine.UI;

namespace Gui.Artifacts
{
    public class ArtifactElement : SettableUiElement<ArtifactData>
    {
        private static Dictionary<int, string> _starCountToText = new()
        {
            // with non-filled star chars
            {0  , "☆☆"},
            {1  , "★☆"},
            {2  , "★★"},
        };
        public Image Portrait;
        public TMP_Text NameText;
        public TMP_Text StarCountText;
        
        public override void SetContent(ArtifactData artifactData)
        {
            var artifact = artifactData.Artifact;
            Portrait.sprite = artifact.Icon;
            NameText.text = artifact.Name;
            StarCountText.text = _starCountToText[artifactData.StarCount];
        }
    }
}