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

    [SerializeField]
    ItemInhaler itemInhaler;

    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //    //itemInhaler.OnInhale += RefreshInventory;
    //}
    //protected override void OnDisable()
    //{
    //    base.OnDisable();
    //    //itemInhaler.OnInhale -= RefreshInventory;
    //}
    public override void SetCurrentItem(MagicItem itemToSet)
    {
        if (itemInhaler.inhaling)
            return;

        string titleColorHex = ColorUtility.ToHtmlStringRGBA(titleColor);
        string slotColorHex = ColorUtility.ToHtmlStringRGBA(slotColor);

        itemInhaler.SelectItem(itemToSet);

        sanctumSelectedItemDisplayer.SetMeFull(new List<string> { itemToSet.magicItemName, $"<color=#{slotColorHex}> {itemToSet.fittingSlotType} | </color>" +$"<color=#{titleColorHex}>" +
            $"{itemToSet._Benefit().BenefitStatName()} + {itemToSet._Benefit().Value()} </color>",
            itemToSet.ItemDescription(), $"{itemToSet.goldValue} Gold"}, new List<Sprite> { itemToSet.itemSprite }, itemToSet);

        //base.SetCurrentItem(itemToSet);
    }
    public  void SetCurrentItem() //empty
    {
        string titleColorHex = ColorUtility.ToHtmlStringRGBA(titleColor);
        string slotColorHex = ColorUtility.ToHtmlStringRGBA(slotColor);

        itemInhaler.SelectItem(null);

        sanctumSelectedItemDisplayer.SetMeFull(new List<string> { emptyItem.magicItemName, "" ,
            "Nothing to describe", "0 Gold"}, new List<Sprite> { emptyItem.itemSprite }, null);

        //base.SetCurrentItem(itemToSet);
    }

    public override void RefreshInventory()
    {
        //SetCurrentItem(emptyItem);
        SetCurrentItem();
        base.RefreshInventory();

        //if just ihaled?
        //sanctumSelectedItemDisplayer.SetAllNulBarsTo(0f);


        for (int i = 0; i < Inventory.Instance.magicItemCount; i++)
        {
            if (!Inventory.Instance.inventoryItems[i].FetchSprite())
                continue;

            displayers[i].BackgroundGlow(false);
        }
    }
}
