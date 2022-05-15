using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemDisplayer : BasicDisplayer
{
    public static InventoryItemDisplayer ActiveDisplayer;

    MagicItem magicItem;
    bool isDisplayed = false;

    SimpleInventory simpleInventory;

    [SerializeField]
    UnityEngine.UI.Image bgImg;

    [SerializeField]
    Sprite bgOn;
    [SerializeField]
    Sprite bgOff;

    public bool SetMeFull(MagicItem item, SimpleInventory si)
    {
        simpleInventory = si;
        magicItem = item;
        return base.SetMe(new List<string> {magicItem.magicItemName, magicItem.goldValue.ToString()}, new List<Sprite> { magicItem.itemSprite });
    }
    //When clicked 
    public string GetItemDescription() => magicItem.ItemDescription();
    public void OnMyClick()
    {
        if(!isDisplayed)
        {
            if(ActiveDisplayer)
            ActiveDisplayer.BackgroundGlow(false);
            ActiveDisplayer = this;
            ActiveDisplayer.BackgroundGlow(true);

            simpleInventory.SetCurrentItem(magicItem);
            //Turn on selected glow behind button 
        }
    }
    public void BackgroundGlow(bool on)
    {
        bgImg.sprite = on ? bgOn : bgOff;
    }
}
