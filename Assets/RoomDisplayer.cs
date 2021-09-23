using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomDisplayer : MonoBehaviour
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
        if (Inventory.Instance.Gold >= price)
            Inventory.Instance.Gold -= price;
        else
            return; //indicate not enough gold

        //charge price to inventory
        buyButton.SetActive(false);
        ownedParent.SetActive(true);

        PlayerDataMaster.Instance.currentPlayerData.totalSquadRooms +=1;

        Tavern.Instance.RefreshRooms();

        //set number of occupants
       
    }


    public void SetMe()
    {
        buyButton.SetActive(false);
        ownedParent.SetActive(true);

        //occupantsText.text = occupants.ToString();
    }
    public void SetMe(int occupants)
    {
        buyButton.SetActive(false);
        ownedParent.SetActive(true);

        occupantsText.text = occupants.ToString();
    }
}
