using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarGraphSlice : MonoBehaviour
{
    public NulColour myColour;

    [SerializeField]
    float maxValue;
    [SerializeField]
    float currentValue;
    [SerializeField]
    Image fillImg;

    //relevant for when displaying a value with no obvious maximum
    float defaultMaxNool = 10f;

    public void SetMe(PsionNulElement element)
    {
        //set values
        maxValue = element.maxValue;
        currentValue = element.value;
        ShowValue();
    }
    /// <summary>
    /// Shows a max of 10f as default
    /// </summary>
    /// <param name="value"></param>
    public void SetMe(float value)
    {
        //set values
        maxValue = defaultMaxNool;
        currentValue = value;
        ShowValue();
    }

    public virtual void ShowValue()
    {
        
        fillImg.fillAmount = currentValue / maxValue;
    }
}
