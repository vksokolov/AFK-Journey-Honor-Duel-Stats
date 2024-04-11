using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Artifacts
{
    [Serializable]
    public class Artifact
    {
        public string Name;
        public ArtifactType ArtifactType;
        public string Description;
        public Sprite Icon;
    }
}
