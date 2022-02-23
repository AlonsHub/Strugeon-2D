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
        Tavern.Instance.squadBuilder.Refresh();

        IdleLog.Instance.CloseIfEmptyCheck(1); //only 1 message is to be destroyed and will still count against logParent.ChildCount
        //if (transform.parent.childCount <= 1) // other hireling windows (1 being this hireling window, to be destroyed in a few lines)
        //{
        //    HirelingMaster.Instance.CloseMe(); //special case since this message deletes itself - otherwise IdleLog has a CloseIfEmptyCheck
        //}

        Destroy(gameObject);

        //close and destroy stuff
    }
    public void TryGiveFreeDrink()
    {
        //reduce hiring price?
    }
}
