﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctumInventory : BaseInventory
{
    //This class better find soem things to do soon... prehaps have it manage all sanctum systems?
    [SerializeField]
    SanctumSelectedPanel sanctumSelectedPanel; //this MAY be redunant, but let's go with it 

    
    //public override void SetCurrentItem(MagicItem itemToSet)
    //{
      

    //    //string titleColorHex = ColorUtility.ToHtmlStringRGBA(titleColor);
    //    //string slotColorHex = ColorUtility.ToHtmlStringRGBA(slotColor);



    //    //sanctumSelectedPanel.SetMeFull(new List<string> { itemToSet.magicItemName, $"<color=#{slotColorHex}> {itemToSet.fittingSlotType} | </color>" +$"<color=#{titleColorHex}>" +
    //    //    $"{itemToSet._Benefit().BenefitStatName()} + {itemToSet._Benefit().Value()} </color>",
    //    //    itemToSet.ItemDescription(), $"{itemToSet.goldValue} Gold"}, new List<Sprite> { itemToSet.itemSprite }, itemToSet);
    //    //sanctumSelectedPanel.SetMeFull(itemToSet);

    //    //base.SetCurrentItem(itemToSet);
    //}
    //public  void SetCurrentItem() //empty
    //{
    //    //string titleColorHex = ColorUtility.ToHtmlStringRGBA(titleColor);
    //    //string slotColorHex = ColorUtility.ToHtmlStringRGBA(slotColor);

       

    //    //sanctumSelectedPanel.SetMeFull(new List<string> { emptyItem.magicItemName, "" ,
    //    //    "Nothing to describe", "0 Gold"}, new List<Sprite> { emptyItem.itemSprite }, null);

    //    //base.SetCurrentItem(itemToSet);
    //}

    public override void RefreshInventory()
    {
        //SetCurrentItem(emptyItem);
        //SetCurrentItem();
        base.RefreshInventory();

        //if just ihaled?
        //sanctumSelectedItemDisplayer.SetAllNulBarsTo(0f);


        //for (int i = 0; i < Inventory.Instance.magicItemCount; i++)
        //{
        //    //if (!Inventory.Instance.inventoryItems[i].FetchSprite())
        //    //{
        //    //    Debug.LogError($"{Inventory.Instance.inventoryItems[i].magicItemName} failed fetch. With spritename: {Inventory.Instance.inventoryItems[i].spriteName}");
        //    //    continue;
        //    //}
        //    //displayers[i].SetMeFull(Inventory.Instance.inventoryItems[i]);

        //    displayers[i].BackgroundGlow(false);
        //}
    }
}
