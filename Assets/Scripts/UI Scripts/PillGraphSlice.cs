using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PillGraphSlice : StarGraphSlice
{
    [SerializeField, Tooltip("Basically, the value if the pill was as full as can show")]
    float topMax = 5f;

    [SerializeField]
    Image negativeFill;


    public void SetMeFull(PsionNulElement element)
    {
        maxValue = element.maxValue;
        negativeFill.fillAmount = 1- maxValue/topMax;
        //positiveFill.fillAmount = maxValue / topMax;

        currentValue = element.value;

        fillImg.fillAmount = currentValue / topMax; // the positive does not change size, hence the full 1f fillamount amounts to a full topMax (and not the current capacity)
        base.SetMyDisplayer(new List<string> { $"{currentValue} / {maxValue}" }, new List<Sprite> ());
    }

    

}
