using TMPro;

namespace Gui
{
    public class AvgPlaceElement : SettableUiElement<float>
    {
        public TMP_Text AvgPlaceText;
        
        public override void SetContent(float avgPlace) => 
            AvgPlaceText.text = avgPlace.ToString("F2");
    }
}