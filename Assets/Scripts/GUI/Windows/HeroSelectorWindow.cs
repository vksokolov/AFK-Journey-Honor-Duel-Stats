using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using UnityEngine.Scripting;

namespace Gui.Windows
{
    public class HeroSelectorWindow : MonoBehaviour
    {
        public List<HeroElement> HeroElements;

        private Action<HeroData> _callback;
        
        public void Init(HeroPreset heroPreset)
        {
            for (var i = 0; i < HeroElements.Count; i++)
            {
                var element = HeroElements[i];
                if (i < heroPreset.Heroes.Count)
                {
                    element.SetContent(heroPreset.Heroes[i]);
                    element.SetButtonAction(() => OnHeroButtonClicked(element));
                }
                else
                    element.gameObject.SetActive(false);
            }
        }
        
        public void SelectHero(Action<HeroData> onHeroSelected)
        {
            gameObject.SetActive(true);
            _callback = onHeroSelected;
        }

        public void OnHeroButtonClicked(HeroElement heroElement)
        {
            _callback?.Invoke(heroElement.Data);
            gameObject.SetActive(false);
        }
    }
}