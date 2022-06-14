﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : BaseInventory
{
   
    MagicItem selectedItem; //currently selected item to display/sell
    [SerializeField]
    BasicDisplayer selectedItemDesplayer;
   
    [SerializeField]
    UnityEngine.UI.Button sellButton;

    [SerializeField]
    [ColorUsage(true)]
    Color titleColor;
    [SerializeField]
    [ColorUsage(true)]
    Color slotColor;
    //private void OnEnable()
    //{
    //    RefreshInventory();
    //    Inventory.Instance.OnInventoryChange += RefreshInventory;

    //}

    //private void OnDisable()
    //{
    //    Inventory.Instance.OnInventoryChange -= RefreshInventory;

    //}

    public override void SetCurrentItem(MagicItem newItem)
    {
        selectedItem = newItem;
        sellButton.interactable = true;
        string titleColorHex = ColorUtility.ToHtmlStringRGBA(titleColor);
        string slotColorHex = ColorUtility.ToHtmlStringRGBA(slotColor);

        selectedItemDesplayer.SetMe(new List<string> { selectedItem.magicItemName, $"<color=#{slotColorHex}> {selectedItem.fittingSlotType} | </color>" +$"<color=#{titleColorHex}>" +
            $"{selectedItem._Benefit().BenefitStatName()} + {selectedItem._Benefit().Value()} </color>", 
            selectedItem.ItemDescription(), $"{selectedItem.goldValue} Gold"}, new List<Sprite> {selectedItem.itemSprite});
    }
    public void SetCurrentItem()
    {
        if (Inventory.Instance.inventoryItems.Count == 0)
        {
            selectedItem = null;
            //Set as nothing
            selectedItemDesplayer.SetMe(new List<string> { emptyItem.magicItemName, "" ,
            /*emptyItem.ItemDescription()*/ "Nothing to describe...", ""}, new List<Sprite> { emptyItem.itemSprite });
            sellButton.interactable = false;
        }
        else
        {
            SetCurrentItem(Inventory.Instance.inventoryItems[0]);
            displayers[0].OnMyClick();
        }
    }

    public void SellSelectedItem()
    {
        if(selectedItem == null)
        {
            Debug.LogError("no selected item to sell");
        }

        //sell selected item
        Inventory.Instance.SellItem(selectedItem);

    }

}
