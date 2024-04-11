using UnityEngine;

namespace Gui
{
    public abstract class SettableUiElement<TContent> : MonoBehaviour
    {
        public abstract void SetContent(TContent data);
    }
}