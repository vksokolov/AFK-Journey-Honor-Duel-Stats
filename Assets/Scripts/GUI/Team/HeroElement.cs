using System;
using Characters;
using TMPro;
using UnityEngine.UI;

namespace Gui
{
    public class HeroElement : SettableUiElement<HeroData>
    {
        public Image Portrait;
        public TMP_Text NameText;
        public Button Button;
        
        public HeroData Data { get; private set; }
        
        public override void SetContent(HeroData heroData)
        {
            Data = heroData;
            Portrait.sprite = heroData.Portrait;
            NameText.text = heroData.Name;
        }
        
        public void SetButtonAction(Action action)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => action());
        }
    }
}