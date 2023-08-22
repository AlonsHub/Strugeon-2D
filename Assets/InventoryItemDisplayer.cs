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
    ClassEggPanel eggPanel;


    [SerializeField]
    Sprite bgOn;
    [SerializeField]
    Sprite bgOff;

    public bool SetMeFull(MagicItem item, BaseInventory si)
    {
        connectedInventory = si; //sadly no longer in use? tbd
        magicItem = item;

        if (eggPanel)
            eggPanel.SetEggs(item.relevantClasses);
        else
            Debug.LogError("egg panel required");

        return base.SetMe(new List<string> {magicItem.magicItemName, 
            magicItem.goldValue.ToString(), 
            magicItem.fittingSlotType.ToString(), 
            magicItem._BenefitsStat()}, 
            new List<Sprite> {magicItem.itemSprite});
    }
    //When clicked 
    public string GetItemDescription() => magicItem.ItemDescription();
    public void OnMyClick()
    {
        if(!isDisplayed)
        {
           
            connectedInventory.SetCurrentItem(magicItem);

            // NEEDS A DISPLAYER OR BETTER SOLUTION FOR 
            //Turn on selected glow behind button 
        }
    }
   
}
