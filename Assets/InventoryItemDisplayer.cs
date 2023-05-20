using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemDisplayer : BasicDisplayer
{
    public static InventoryItemDisplayer ActiveDisplayer; //also not really in use

    MagicItem magicItem;
    bool isDisplayed = false;

    BaseInventory connectedInventory;

    [SerializeField]
    UnityEngine.UI.Image bgImg;

    [SerializeField]
    Sprite bgOn;
    [SerializeField]
    Sprite bgOff;

    public bool SetMeFull(MagicItem item, BaseInventory si)
    {
        connectedInventory = si; //sadly no longer in use? tbd
        magicItem = item;
        return base.SetMe(new List<string> {magicItem.magicItemName, magicItem.goldValue.ToString()}, new List<Sprite> { magicItem.itemSprite });
    }
    //When clicked 
    public string GetItemDescription() => magicItem.ItemDescription();
    public void OnMyClick()
    {
        if(!isDisplayed)
        {
            //if(ActiveDisplayer)
            //ActiveDisplayer.BackgroundGlow(false);

            //ActiveDisplayer = this;
            //ActiveDisplayer.BackgroundGlow(true);

            //if(connectedInventory is SimpleInventory)
            //((SimpleInventory)connectedInventory).SetCurrentItem(magicItem);
            //else if (connectedInventory is SanctumInventory)
            connectedInventory.SetCurrentItem(magicItem);

            // NEEDS A DISPLAYER OR BETTER SOLUTION FOR 
            //Turn on selected glow behind button 
        }
    }
    //public void BackgroundGlow(bool on)
    //{
    //    bgImg.sprite = on ? bgOn : bgOff;
    //}
}
