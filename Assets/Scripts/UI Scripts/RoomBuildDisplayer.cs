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

    //public int price;
    [SerializeField]
    TMP_Text capacityText;
    [SerializeField]
    TMP_Text buyPriceText;
    [SerializeField]
    TMP_Text upgradePriceText;
    [SerializeField]
    TMP_Text roomNumberText;
    [SerializeField]
    GameObject roomNumberParent;

    [SerializeField]
    Room room;
    [SerializeField]
    RoomManager roomManager;

    public void SetBuyPriceText(int price)
    {
        //price = (PlayerDataMaster.Instance.RoomCount * Prices.roomBasePrice);
        buyPriceText.text = price.ToString();
        roomNumberParent.SetActive(false);
    }

    public void BuyMe()
    {
        
        //charge price to inventory
        buyButton.SetActive(false);
        ownedParent.SetActive(true);
        isBought = true;
        //set number of occupants
        roomNumberParent.SetActive(true);


    }


    public void SetMe(Room r)
    {
        room = r;
        buyButton.SetActive(false);
        ownedParent.SetActive(true);
        //capacity = r.size;
        roomNumberParent.SetActive(true);

        capacityText.text = room.size.ToString();
        roomNumberText.text = (room.roomNumber+1).ToString();
        if (room.size < GameStats.maxRoomSize)
        {
            upgradePriceText.text = (room.size * Prices.upgradeRoomBasePrice).ToString();
        }
        else
        {
            GameObject upgradeButton = GetComponentInParent<UnityEngine.UI.Button>().gameObject;


            upgradePriceText.text = "Max";
            upgradePriceText.GetComponentInParent<UnityEngine.UI.Button>().colors = new UnityEngine.UI.ColorBlock { colorMultiplier = 1f, selectedColor = Color.black, normalColor = Color.red, highlightedColor = Color.gray, disabledColor = Color.red };
            upgradePriceText.GetComponentInParent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    public void TryUpgrade()
    {
        if(room.TryUpgrade())
        {
            //capacityText.text = room.size.ToString();
            roomManager.RefreshAllRooms();
        }
    }
    //public void SetMe(int occupants)
    //{
    //    buyButton.SetActive(false);
    //    ownedParent.SetActive(true);

    //    capacityText.text = occupants.ToString();
    //}
}
