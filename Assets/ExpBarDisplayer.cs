using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarDisplayer : MonoBehaviour //EXCEPTION CLASS THAT IS NOT A BASIC DISPLAYER! it's too simple, honestly
{
    [SerializeField]
    TMP_Text currentLevelNumber;
    [SerializeField]
    TMP_Text nextLevelNumber;
    [SerializeField]
    TMP_Text expText; // is composed of {currentExp}/{requiredExpToNextLevel}


    //[SerializeField]
    //Image barFillPart;

    [SerializeField]
    Slider slider;
    public void SetBar(MercSheet mercSheet) //makes it easier, since it mostly takes MercSheets (if not just mercSheets)
    {
        Vector2 expFromTo = mercSheet._expFromAndToNextLevel;
        float fillAmount = (mercSheet._experience - expFromTo.x) / (expFromTo.y - expFromTo.x);
        SetBar(mercSheet._level, $"{mercSheet._experience}/{mercSheet._expFromAndToNextLevel.y}", fillAmount);
    }
    public void SetBar() //makes it easier, since it mostly takes MercSheets (if not just mercSheets)
    {
        
        
        SetBar(0, "0/0", 0f);
    }

    public void SetBar(int c, string exp, float fill) //the basic most setter - replaces the "base.SetMe(...)" part of this odd design pattern (see BasicDisplayer)
    {
        currentLevelNumber.text = c.ToString();
        if(nextLevelNumber)
        nextLevelNumber.text = (c+1).ToString();
        expText.text = exp;

        slider.value = fill;
    }
}
