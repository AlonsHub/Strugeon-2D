using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomBuildDisplayer : MonoBehaviour
{
    bool isBought = false;
    public int capacity = 2; //default capacity is 2
    //max capacity
    [SerializeField]
    GameObject buyButton;
    [SerializeField]
    GameObject ownedParent;
    [SerializeField]
    GameObject bedDisplayer;
    [SerializeField]
    Sprite fullyUpgradedSprite;
    [SerializeField]
    Sprite upgradeSprite;
     [SerializeField]
    Sprite bedSprite;
    // [SerializeField]
    //Sprite upgradeSprite;

    public int price;

    TMP_Text occupantsText;


    public void BuyMe()
    {
        
        //charge price to inventory
        buyButton.SetActive(false);
        ownedParent.SetActive(true);

        //set number of occupants
       
    }


    public void SetMe()
    {
        buyButton.SetActive(false);
        ownedParent.SetActive(true);
    }
    public void SetMe(int occupants)
    {
        buyButton.SetActive(false);
        ownedParent.SetActive(true);

        occupantsText.text = occupants.ToString();
    }
}
