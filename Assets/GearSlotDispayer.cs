using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GearSlotDispayer : BasicDisplayer, IPointerEnterHandler, IPointerExitHandler
{
    //needs a SetMeFull that accepts some reference to the item?
    [SerializeField]
    EquipSlotType slotType;
    [SerializeField]
    Sprite emptySprite;

    [SerializeField]
    EquipInventoryManager inventoryDisplayManager;

    MagicItem magicItem;

    public void SetMeEmpty() //check if this is needed to be able to fight. TBF + gamedesign-wise, do we even allow illegal "unequips" and let the merc be useless?
    {
        base.SetMe(new List<string> { "Empty", "no benefit" }, new List<Sprite> {emptySprite});
    }
    public bool SetMeFull(MagicItem item)
    {
        if (!item.itemSprite && !item.FetchSprite())
                return false;
        magicItem = item;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (magicItem == null)//unlikely
            return;
        //HoverTextBoard.Instance.SetMe(new List<string> { magicItem.magicItemName, magicItem.ItemDescription(), magicItem.goldValue.ToString() }, new List<Sprite> { ((magicItem._Benefit() as StatBenefit).statToBenefit == StatToBenefit.MaxHP) ? PrefabArchive.Instance.healthSprite : PrefabArchive.Instance.swordSprite });
        HoverTextBoard.Instance.SetMe(new List<string> { magicItem.magicItemName, $"+{magicItem._Benefit().Value()}", magicItem.ItemDescription(), $"{magicItem.goldValue} Gold" },
                                    new List<Sprite> { SwitchOnBenefit((magicItem._Benefit() as StatBenefit).statToBenefit) });
    }
    Sprite SwitchOnBenefit(StatToBenefit statToBenefit)
    {
        switch (statToBenefit)
        {
            case StatToBenefit.MaxHP:
                return PrefabArchive.Instance.healthSprite;
                break;
            case StatToBenefit.FlatDamage:
                return PrefabArchive.Instance.swordSprite;
                break;
            default:
                return null;
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (magicItem == null) //unlikely
            return;
        HoverTextBoard.Instance.UnSetMe();
    }
}
