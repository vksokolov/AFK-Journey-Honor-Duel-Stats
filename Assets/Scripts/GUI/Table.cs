using System.Collections.Generic;
using UnityEngine;

namespace Gui
{
    public class Table<TElement, TData> : MonoBehaviour where TElement : SettableUiElement<TData>
    {
        public Transform ElementRoot;
        public TElement Prefab;
        
        private List<TElement> _elements = new List<TElement>();

        public void SetElements(List<TData> elements)
        {
            for (var i = 0; i < elements.Count; i++)
            {
                if (i < _elements.Count)
                {
                    _elements[i].SetContent(elements[i]);
                    _elements[i].gameObject.SetActive(true);
                }
                else
                {
                    var element = Instantiate(Prefab, ElementRoot);
                    element.SetContent(elements[i]);
                    _elements.Add(element);
                }
            }
            
            for (var i = elements.Count; i < _elements.Count; i++) 
                _elements[i].gameObject.SetActive(false);
        }

        public void HideAll()
        {
            foreach (var element in _elements)
                element.gameObject.SetActive(false);
        }
    }
}