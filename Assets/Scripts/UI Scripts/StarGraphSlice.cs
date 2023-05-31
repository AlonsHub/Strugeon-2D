using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarGraphSlice : HoverableWithBox
{
    public NoolColour myColour;

    [SerializeField]
    protected float maxValue;
    [SerializeField]
    protected float currentValue;
    [SerializeField]
    protected Image fillImg;

    //relevant for when displaying a value with no obvious maximum //TBD TBF
    protected float defaultMaxNool = 10f;


    //public virtual void SetMe(PsionNulElement element)
    //{
    //    //set values
    //    maxValue = element.maxValue;
    //    currentValue = element.value;
    //    ShowValue();
    //}
    /// <summary>
    /// Shows a max of 10f as default
    /// </summary>
    /// <param name="value"></param>
    public virtual void SetMe(float value)
    {
        float tempNewTopMax = defaultMaxNool;
        //set values
        while (value >= tempNewTopMax)
        {
             tempNewTopMax += defaultMaxNool;

        }
        maxValue = tempNewTopMax;
        currentValue = value;
        ShowValue();
    }


    public virtual void ShowValue()
    {
        fillImg.fillAmount = currentValue / maxValue;
    }
}
