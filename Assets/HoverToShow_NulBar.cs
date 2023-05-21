using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverToShow_NulBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler // TBF - SEE "Hoverable.cs" - I already solved this
{
    enum StatToShow { MaxValue, CurrentValue, CurrentAndMaxValue};
    [SerializeField]
    StatToShow statToShow;

    NulBar nulBar;
    private void OnEnable()
    {
        if(!nulBar)
        nulBar = GetComponent<NulBar>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (statToShow)
        {
            case StatToShow.MaxValue:
                nulBar.SetText(nulBar.maxValue.ToString());
                break;
            case StatToShow.CurrentValue:
                nulBar.SetText(nulBar.currentValue.ToString());
                break;
            case StatToShow.CurrentAndMaxValue:
                nulBar.SetText($"{(int)nulBar.currentValue}/{nulBar.maxValue}");
                break;
            default:
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nulBar.TextOnOff(false);
    }
}
