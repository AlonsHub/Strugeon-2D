using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NulBar : Bar
{
    [SerializeField]
    PsionNulElement nulElement;

    public void SetMe(PsionNulElement element)
    {
        nulElement = element;

        //set correct color and icon?

        //set values
        maxValue = element.maxValue;
        currentValue = element.value;
        regenRate = element.regenRate;

        ShowValue();
    }

}
