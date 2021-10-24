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

        HirelingMaster.Instance.CloseMe();

        //close and destroy stuff
    }
    public void TryGiveFreeDrink()
    {
        //reduce hiring price?
    }
}
