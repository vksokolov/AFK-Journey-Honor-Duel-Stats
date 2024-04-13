using System.Collections.Generic;
using Scripts.Filters;
using UnityEngine;

namespace Gui
{
    public class Table<TElement, TData> : MonoBehaviour where TElement : SettableUiElement<TData>
    {
        public Transform ElementRoot;
        public TElement Prefab;
        
        [SerializeField] protected List<TElement> Elements = new List<TElement>();

        public virtual void SetElements(List<TData> elements)
        {
            for (var i = 0; i < elements.Count; i++)
            {
                if (i < Elements.Count)
                {
                    Elements[i].SetContent(elements[i]);
                    Elements[i].gameObject.SetActive(true);
                }
                else
                {
                    var element = Instantiate(Prefab, ElementRoot);
                    element.SetContent(elements[i]);
                    Elements.Add(element);
                }
            }
            
            for (var i = elements.Count; i < Elements.Count; i++) 
                Elements[i].gameObject.SetActive(false);
            
            OnElementsUpdated();
        }

        protected virtual void OnElementsUpdated() { }

        public void HideAll()
        {
            foreach (var element in Elements)
                element.gameObject.SetActive(false);
        }
    }
}