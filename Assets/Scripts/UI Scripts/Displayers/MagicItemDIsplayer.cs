using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicItemDIsplayer : BasicDisplayer
{
    public MagicItem magicItem;
    
    public GameObject sellGroup;

    public void SetItem(MagicItem newItem)
    {
        magicItem = newItem;

        //SetMe(magicItem.magicItemName, magicItem.itemSprite); //no price on this one

        SetMe(new List<string> { magicItem.magicItemName, magicItem.goldValue.ToString() }, new List<Sprite> {magicItem.itemSprite});
    }

    public void SellMe() //button refs this in inspector
    {
        int goldValue = magicItem.goldValue;
        //remove from inventory 
        if (!Inventory.Instance.RemoveMagicItem(magicItem))
        {
            Debug.LogError($"couldn't remove {magicItem.magicItemName}");
            return;
        }
        //else

        Inventory.Instance.AddGold(goldValue);
    }
}
