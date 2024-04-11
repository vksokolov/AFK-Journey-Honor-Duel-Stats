using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Artifacts
{
    [CreateAssetMenu(fileName = nameof(ArtifactPreset), menuName = "Presets/" + nameof(ArtifactPreset))]
    public class ArtifactPreset : ScriptableObject
    {
        public Sprite MissingSprite;
        public List<Artifact> Artifacts;

        private Dictionary<ArtifactType, Artifact> _artifactsByType;
        
        public Artifact GetArtifact(ArtifactType artifactType)
        {
            _artifactsByType ??= Artifacts.ToDictionary(art => art.ArtifactType);
            return _artifactsByType.TryGetValue(artifactType, out var artifact) ? artifact : null;
        }
    }
}
