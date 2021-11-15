using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HirelingWindow : MonoBehaviour
{
    [SerializeField]
    Image portrait;
    [SerializeField]
    TMP_Text nameText;
    [SerializeField]
    TMP_Text priceText;

    MercName mercName;

    public int price;

    public HirelingMaster hirelingMaster;

    public void SetMe(MercName newMerc, HirelingMaster hm)
    {
        mercName = newMerc;
        nameText.text = newMerc.ToString(); //interesting if this works or not

        portrait.sprite = MercPrefabs.Instance.EnumToPawnPrefab(mercName).PortraitSprite;
        //price
        price = PlayerDataMaster.Instance.currentPlayerData.mercPrice;
        priceText.text = price.ToString();
        hirelingMaster = hm;
    }


    public void TryHire()
    {
        if(!Inventory.Instance.TryRemoveGold(price))
        {
            Debug.LogWarning("Not enough money");
            return;
        }

        //success
        //PartyMaster.Instance.availableMercs.Add(MercPrefabs.Instance.EnumToPawnPrefab(mercName));
        PlayerDataMaster.Instance.HireMerc(mercName);

        if (transform.parent.childCount <= 1) // other hireling windows
        {
            HirelingMaster.Instance.CloseMe();
        }

        Destroy(gameObject);

        //close and destroy stuff
    }
    public void TryGiveFreeDrink()
    {
        //reduce hiring price?
    }
}
