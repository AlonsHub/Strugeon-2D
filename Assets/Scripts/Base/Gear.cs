using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The data-set which holds all items held my a merc.
/// Should this be "saveable"? it should definitely be serializeable...
/// </summary>
public enum EquipSlotType {Body, Hand, Trinket};
/// 

[System.Serializable]
public class Gear
{
    IEquipable[] equipables;
    public Gear() //mages - empty
    {
        equipables = new IEquipable[System.Enum.GetValues(typeof(EquipSlotType)).Length]; //as the number of EquipSlotType {Body=0, Hand=1, Trinket=2}; 
    }
     public Gear(string defaultWeaponName)
    {
        equipables = new IEquipable[System.Enum.GetValues(typeof(EquipSlotType)).Length]; //as the number of EquipSlotType {Body=0, Hand=1, Trinket=2}; 

        LoadItemsFromString(null, defaultWeaponName, null);
    }

    public Gear(string body, string hand, string trinket)
    {
        equipables = new IEquipable[System.Enum.GetValues(typeof(EquipSlotType)).Length]; //as the number of EquipSlotType {Body=0, Hand=1, Trinket=2}; 
        //load one to each by ID
        LoadItemsFromString(body, hand,trinket);

    }

    //void Equip(IEquipable equipable)
    //{
    //    equipables[(int)equipable._EquipSlotType()] = equipable;
    //}
    //IEquipable UnEquip(IEquipable equipable)
    //{
    //    IEquipable
    //    equipables[(int)equipable._EquipSlotType()] = null;
    //}

    public void LoadItemsFromString(string body, string hand, string trinket)
    {
        if(equipables == null)
        equipables = new IEquipable[System.Enum.GetValues(typeof(EquipSlotType)).Length]; //as the number of EquipSlotType {Body=0, Hand=1, Trinket=2}; 

        //load one to each by ID
        if (body != null && ItemDatabase.IDtoItemSO.ContainsKey(body))
            equipables[(int)EquipSlotType.Body] = ItemDatabase.IDtoItemSO[body].magicItem;

        if (hand != null && ItemDatabase.IDtoItemSO.ContainsKey(hand))
            equipables[(int)EquipSlotType.Hand] = ItemDatabase.IDtoItemSO[hand].magicItem;

        if (trinket != null && ItemDatabase.IDtoItemSO.ContainsKey(trinket))
            equipables[(int)EquipSlotType.Trinket] = ItemDatabase.IDtoItemSO[trinket].magicItem;
    }
    
    


    /// <summary>
    /// Just returns them, doesn NOT apply/provide benefits!!!
    /// </summary>
    /// <returns></returns>
    public List<IBenefit> GetAllBenefits()
    {
        if(equipables == null)
        {
            Debug.LogError("somehow equipables == null");
            return null;
        }

        List<IBenefit> toReturn = new List<IBenefit>();

        foreach (var equipable in equipables)
        {
            toReturn.Add(equipable._Benefit()); //single benefit items

            if(equipable._Benefits() != null && equipable._Benefits().Count != 0) //multiple benefits
            {
                toReturn.AddRange(equipable._Benefits());
            }
        }

        return toReturn;
    }

    /// <summary>
    /// Tries to Remove the item from slotToRemoveFrom.
    /// If successful, returns true
    /// If no item is found, removedItem is null
    /// </summary>
    /// <param name="slotToRemoveFrom"></param>
    /// <returns></returns>
    public bool TryRemoveItemFromSlot(EquipSlotType slotToRemoveFrom, out IEquipable removedItem)
    {
        if(equipables == null)// || equipables[(int)slotToRemoveFrom] == null) if this is true, it will return null
        {
            removedItem = null;
            return false;
        }

        removedItem = equipables[(int)slotToRemoveFrom]; //can be null! and should sometime should be

        equipables[(int)slotToRemoveFrom] = null;
        return true;
    }
    /// <summary>
    /// Tries to insert item - returns the removed item?
    /// should probably just return bool if success/fail to equip item
    /// </summary>
    /// <param name="toEquip"></param>
    /// <returns></returns>
    public IEquipable TryEquipItemToSlot(IEquipable toEquip)
    {
        if(equipables[(int)toEquip._EquipSlotType()] == null)
        {
            //just insert/equip here, if there are no further issues
            equipables[(int)toEquip._EquipSlotType()] = toEquip;
            return null; //there was not removed item
        }
        else
        {
            IEquipable removedItem; 
            if(TryRemoveItemFromSlot(toEquip._EquipSlotType(), out removedItem))

            equipables[(int)toEquip._EquipSlotType()] = toEquip;
            return removedItem;
        }

    }

    public IEquipable GetItemBySlot(EquipSlotType byType)
    {
        if (equipables == null) // || equipables[(int)slot] == null)
            throw new System.Exception();

        return equipables[(int)byType]; //null if empty
    }
}
