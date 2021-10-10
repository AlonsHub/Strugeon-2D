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
    [SerializeField]
    TMP_Text capacityText;
    [SerializeField]
    Room room;

    public void BuyMe()
    {
        
        //charge price to inventory
        buyButton.SetActive(false);
        ownedParent.SetActive(true);
        isBought = true;
        //set number of occupants
       
    }


    public void SetMe(Room r)
    {
        room = r;
        buyButton.SetActive(false);
        ownedParent.SetActive(true);

        capacityText.text = room.size.ToString();
    }

    public void TryUpgrade()
    {
        if(room.TryUpgrade())
        {
            capacityText.text = room.size.ToString();
        }
    }
    //public void SetMe(int occupants)
    //{
    //    buyButton.SetActive(false);
    //    ownedParent.SetActive(true);

    //    capacityText.text = occupants.ToString();
    //}
}
