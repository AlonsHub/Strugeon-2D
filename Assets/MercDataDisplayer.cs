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

    //[SerializeField]
    //Image saImage;

    //public Image mercImage;
    //public Image mercImage;

    void Start()
    {
        
    }

    public void SetMe(Pawn merc)
    {
        nameText.text = merc.Name; //+ suffix/monicer
        mercImage.sprite = merc.FullPortraitSprite;

        specialAbilityTitle.text = merc.saItems[0].ToString();
        specialAbilityDescription.text = merc.saItems[0].ToString(); // DESCRIBE!
        specialAbilityIcon.sprite = merc.SASprite;
    }
}
