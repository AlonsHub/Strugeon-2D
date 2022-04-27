using System.Collections;
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
    TMP_Text expText;

    [SerializeField]
    Image saImage;
    [SerializeField]
    Image expSlider;

    public void SetMe(Pawn merc)
    {
        int maxHP_benefit, maxDamage_benefit, minDamage_benefit;
        maxHP_benefit = maxDamage_benefit = minDamage_benefit = 0;

        foreach (var benefit in merc.mercSheetInPlayerData.gear.GetAllBenefits())
        {
            switch ((benefit as StatBenefit).statToBenefit)
            {
                case StatToBenefit.MaxHP:
                    maxHP_benefit += benefit.Value();
                    break;
                case StatToBenefit.FlatDamage:
                    maxDamage_benefit += benefit.Value();
                    minDamage_benefit += benefit.Value();
                    break;
                default:
                    break;
            }
        }

        nameText.text = merc.Name; //+ suffix/monicer
        mercImage.sprite = merc.FullPortraitSprite;
        MercSheet ms = merc.mercSheetInPlayerData;
        maxHPText.text = (ms._maxHp + maxHP_benefit).ToString();
        WeaponItem wi = merc.GetComponent<WeaponItem>();
        //if (!wi)
        //{
        //    Debug.LogError("Merc with no WEAPON!!!");
        //}
        //else
        //{
            //all is good, merc has weapon
            dmgRangeText.text = (ms._minDamage+ minDamage_benefit).ToString() + "-" + (ms._maxDamage+ maxDamage_benefit).ToString(); //MUST CHANGE ACCESS TO DAMAGE
        //}


        //SA_Item sA_Item = merc.forDisplayPurposesOnly as SA_Item;
        //TEMP AF
        specialAbilityTitle.text = merc.SA_Title;
        specialAbilityDescription.text = merc.SA_Description;
        //end TEMP AF
        specialAbilityIcon.sprite = merc.SASprite;

        currentLevelText.text = ms._level.ToString();
        nextLevelText.text = (ms._level+1).ToString();

        Vector2 fromAndTo = ms._expFromAndToNextLevel; //casted into Vector2 (not int) just to make sure they float 
        expSlider.fillAmount = (ms._experience - fromAndTo.x)/(fromAndTo.y - fromAndTo.x); //current EXP, minus previous threshold

        expText.text = ms._experience.ToString() + " / " + fromAndTo.y;


    }
}
