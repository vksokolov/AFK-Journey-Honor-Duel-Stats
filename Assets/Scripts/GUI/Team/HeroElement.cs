using Characters;
using TMPro;
using UnityEngine.UI;

namespace Gui
{
    public class HeroElement : SettableUiElement<HeroData>
    {
        public Image Portrait;
        public TMP_Text NameText;
        
        public override void SetContent(HeroData heroData)
        {
            Portrait.sprite = heroData.Portrait;
            NameText.text = heroData.Name;
        }
    }
}