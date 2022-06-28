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

    [SerializeField]
    TMPro.TMP_Text yellowText;

    public void SetMe(PsionNulElement element)
    {
        //nulElement = element;
        if (!myColour.HasValue || myColour != element.GetNulColour)
        {
            SetColour(element.GetNulColour);
        }
        //myColour = element.nulColour;
        //set correct color and icon?
        //img.sprite = PrefabArchive.Instance.GetElementFillBar((int)element.nulColour);
        //iconImg.sprite = PrefabArchive.Instance.ElementIcon((int)element.nulColour);

        //set values
        maxValue = element.maxValue;
        //currentValue = element.value;
        regenRate = element.regenRate;

        AnimatedSetValue(element.value, 1f);

        //ShowValue();
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
        SetMax(10f);
        //currentValue = element.value;
        StartCoroutine(AnimatedValueChange(element.value, 1f));

        //max and regen values could be relevant here - but shold they be here?
        //regen should operate somewhere else

        //ShowValue();
    }

    private void SetMax(float max)
    {
        maxValue = max;
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
    public void AnimatedSetValue(float newVal, float duration)
    {
        StartCoroutine(AnimatedValueChange(newVal, duration));
    }
    IEnumerator AnimatedValueChange(float goal, float duration)
    {
        float t = 0f;
        float initialValue = currentValue;
        while (t <= duration)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            SetValue(Mathf.Lerp(initialValue, goal, t/duration));
        }
    }

    public void SetText(string t)
    {
        if (!yellowText)
            return;

        yellowText.text = t;
        TextOnOff(true);
    }

    public void TextOnOff(bool on)
    {
        yellowText.gameObject.SetActive(on);
    }
    //public override void AddValue(float value)
    //{


    //    base.AddValue(value);
    //}

}
