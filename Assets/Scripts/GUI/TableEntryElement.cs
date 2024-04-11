using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gui
{
    public abstract class TableEntryElement<TContent> : SettableUiElement<TContent>
    {
        public AvgPlaceElement AvgPlaceElement;
        public Transform ContentRoot;
    }
}
