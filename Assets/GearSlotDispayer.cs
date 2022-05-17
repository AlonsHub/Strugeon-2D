﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSlotDispayer : BasicDisplayer
{
    //needs a SetMeFull that accepts some reference to the item?
    [SerializeField]
    EquipSlotType slotType;
    [SerializeField]
    Sprite emptySprite;

    [SerializeField]
    EquipInventoryManager inventoryDisplayManager;

    public void SetMeEmpty() //check if this is needed to be able to fight. TBF + gamedesign-wise, do we even allow illegal "unequips" and let the merc be useless?
    {
        base.SetMe(new List<string> { "Empty", "no benefit" }, new List<Sprite> {emptySprite});
    }
    public bool SetMeFull(MagicItem item)
    {
        if (!item.itemSprite && !item.FetchSprite())
                return false;

        //Now sets the data to a HIDDEN(i.e. disabled gameobject called "HoverBox - TEMP"
        return base.SetMe(new List<string> {$"{item.magicItemName} of {item._BenefitsProperNoun()}" , $"{item._BenefitsStat()} +{item._Benefit().Value()}" }, new List<Sprite> {item.itemSprite});
    }

    //add an OnHover mechanic to enable HoverBox temporarily


    public void OnMyClick()
    {
        //ask the gear displayer to open ItemInventory and filter items by class and slot
        inventoryDisplayManager.gameObject.SetActive(true);
        inventoryDisplayManager.FilterBySlot(slotType);

    }
}
