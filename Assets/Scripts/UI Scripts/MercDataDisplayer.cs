﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MercDataDisplayer : MonoBehaviour
{
    [SerializeField]
    Image mercImage;
    [SerializeField]
    TMP_Text nameText;
    [SerializeField]
    TMP_Text specialAbilityDescription;
    [SerializeField]
    Image specialAbilityIcon;
    [SerializeField]
    TMP_Text specialAbilityTitle;
    [SerializeField]
    TMP_Text currentLevelText;
    [SerializeField]
    TMP_Text nextLevelText;
     [SerializeField]
    TMP_Text maxHPText; 
    [SerializeField]
    TMP_Text dmgRangeText;

    [SerializeField]
    Image saImage;
    [SerializeField]
    Image expSlider;

    public void SetMe(Pawn merc)
    {
        nameText.text = merc.Name; //+ suffix/monicer
        mercImage.sprite = merc.FullPortraitSprite;
        MercSheet ms = merc.GetCharacterSheet;
        maxHPText.text = (merc.maxHP + ms._maxHpBonus).ToString();
        WeaponItem wi = merc.GetComponent<WeaponItem>();
        if (!wi)
        {
            Debug.LogError("Merc with no WEAPON!!!");
        }
        else
        {
            //all is good, merc has weapon
            dmgRangeText.text = (wi.minDamage + ms._minDamageBonus).ToString() + "-" + (wi.maxDamage + ms._maxDamageBonus).ToString(); //MUST CHANGE ACCESS TO DAMAGE
        }
        SA_Item sA_Item = merc.forDisplayPurposesOnly as SA_Item;
        //TEMP AF
        specialAbilityTitle.text = merc.SA_Title;
        specialAbilityDescription.text = merc.SA_Description;
        //end TEMP AF
        specialAbilityIcon.sprite = merc.SASprite;

        currentLevelText.text = ms._level.ToString();
        nextLevelText.text = (ms._level+1).ToString();

        Vector2 fromAndTo = ms._expFromAndToNextLevel; //casted into Vector2 (not int) just to make sure they float 
        expSlider.fillAmount = (ms._experience - fromAndTo.x)/(fromAndTo.y - fromAndTo.x); //current EXP, minus previous threshold
        
    }
}
