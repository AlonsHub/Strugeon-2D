using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSlotDispayer : BasicDisplayer
{
    //needs a SetMeFull that accepts some reference to the item?
    [SerializeField]
    EquipSlotType slotType;
    [SerializeField]
    Sprite emptySprite;

    public void SetMeEmpty() //check if this is needed to be able to fight. TBF + gamedesign-wise, do we even allow illegal "unequips" and let the merc be useless?
    {
        base.SetMe(new List<string> { "Empty", "no benefit" }, new List<Sprite> {emptySprite});
    }
    public bool SetMeFull(MagicItem item)
    {
        if (!item.itemSprite && !item.FetchSprite())
                return false;

        return base.SetMe(new List<string> {$"{item.magicItemName} of {item._BenefitsProperNoun()}" , $"Provides {item._BenefitsProperNoun()} +{item._Benefit().Value()}" }, new List<Sprite> {item.itemSprite});
    }

    public void Click()
    {
        MercSheet merc;
        //merc.gear.TryRemoveItemFromSlot()
    }
}
