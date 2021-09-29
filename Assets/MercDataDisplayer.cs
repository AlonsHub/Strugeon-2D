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
    Image saImage;

    //public Image mercImage;
    //public Image mercImage;

    void Start()
    {
        
    }

    public void SetMe(Pawn merc)
    {
        nameText.text = merc.Name; //+ suffix/monicer
        mercImage.sprite = merc.FullPortraitSprite;

        SA_Item sA_Item = merc.forDisplayPurposesOnly as SA_Item;
        //TEMP AF
        specialAbilityTitle.text = merc.SA_Title;
        specialAbilityDescription.text = merc.SA_Description;
        //end TEMP AF
        specialAbilityIcon.sprite = merc.SASprite;
    }
}
