using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctumInventory : BaseInventory
{
    //This class better find soem things to do soon... prehaps have it manage all sanctum systems?
    [SerializeField]
    SanctumSelectedItemDisplayer sanctumSelectedItemDisplayer; //this MAY be redunant, but let's go with it 

    [SerializeField]
    [ColorUsage(true)]
    Color titleColor;
    [SerializeField]
    [ColorUsage(true)]
    Color slotColor;


    public override void SetCurrentItem(MagicItem itemToSet)
    {
        string titleColorHex = ColorUtility.ToHtmlStringRGBA(titleColor);
        string slotColorHex = ColorUtility.ToHtmlStringRGBA(slotColor);

        sanctumSelectedItemDisplayer.SetMeFull(new List<string> { itemToSet.magicItemName, $"<color=#{slotColorHex}> {itemToSet.fittingSlotType} | </color>" +$"<color=#{titleColorHex}>" +
            $"{itemToSet._Benefit().BenefitStatName()} + {itemToSet._Benefit().Value()} </color>",
            itemToSet.ItemDescription(), $"{itemToSet.goldValue} Gold"}, new List<Sprite> { itemToSet.itemSprite }, itemToSet);

        //base.SetCurrentItem(itemToSet);
    }
}
