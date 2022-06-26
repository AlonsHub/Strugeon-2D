using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NulBar : Bar
{
    //[SerializeField]
    //PsionNulElement nulElement; //may not need this TBF

    //NulElement n;

    NulColour? myColour;

    [SerializeField]
    UnityEngine.UI.Image iconImg;

    public void SetMe(PsionNulElement element)
    {
        //nulElement = element;
        if (!myColour.HasValue || myColour != element.nulColour)
        {
            SetColour(element.nulColour);
        }
        //myColour = element.nulColour;
        //set correct color and icon?
        //img.sprite = PrefabArchive.Instance.GetElementFillBar((int)element.nulColour);
        //iconImg.sprite = PrefabArchive.Instance.ElementIcon((int)element.nulColour);

        //set values
        maxValue = element.maxValue;
        currentValue = element.value;
        regenRate = element.regenRate;

        ShowValue();
    }

    public void SetMe(NulElement element)
    {
        //n = element;

        if (!myColour.HasValue || myColour != element.nulColour)
        {
            SetColour(element.nulColour);
        }

        //set correct color and icon?
        //img.sprite = PrefabArchive.Instance.GetElementFillBar((int)element.nulColour);
        //iconImg.sprite = PrefabArchive.Instance.ElementIcon((int)element.nulColour);
        //set values

        //maxValue = element.maxValue; //What should max be defualted to? TBF TBD gdd
        maxValue = 10f;
        currentValue = element.value;
        
        //max and regen values could be relevant here - but shold they be here?
        //regen should operate somewhere else

        ShowValue();
    }

    public void SetColour(NulColour colour)
    {
        myColour = colour;
        img.sprite = PrefabArchive.Instance.GetElementFillBar((int)colour);
        iconImg.sprite = PrefabArchive.Instance.ElementIcon((int)colour);
    }

    /// <summary>
    /// After initial SetMe - only set value
    /// </summary>
    public void SetValue(float newVal)
    {
        currentValue = newVal;
        ShowValue();

    }
    //public override void AddValue(float value)
    //{


    //    base.AddValue(value);
    //}

}
