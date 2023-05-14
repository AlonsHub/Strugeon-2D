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
    BasicDisplayer hoverDisplayer;
    //[SerializeField]
    //EquipInventoryManager inventoryDisplayManager;

    MagicItem magicItem;

    MercSheet mercSheet;
    LobbyMercDisplayer lobbyMercDisplayer;

    
    public void SetMeEmpty() //check if this is needed to be able to fight. TBF + gamedesign-wise, do we even allow illegal "unequips" and let the merc be useless?
    {
        base.SetMe(new List<string> { "Empty", "no benefit" }, new List<Sprite> {emptySprite});
    }
    public bool SetMeFull(MagicItem item, MercSheet ms, LobbyMercDisplayer lmd)
    {
        lobbyMercDisplayer = lmd;
           mercSheet = ms;
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
        //inventoryDisplayManager.gameObject.SetActive(true);
        //inventoryDisplayManager.FilterBySlot(slotType);
        Tavern.Instance.equipInventoryManager.gameObject.SetActive(true);
        Tavern.Instance.equipInventoryManager.FilterBySlot(slotType, mercSheet, lobbyMercDisplayer); //need to pass the slot/merc to equip to

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (magicItem == null || !hoverDisplayer)
            return;

        hoverDisplayer.SetMe(new List<string> { magicItem.magicItemName, magicItem.ItemDescription(), magicItem.fittingSlotType.ToString(), magicItem._Benefit().BenefitStatName(), magicItem._Benefit().Value().ToString(), magicItem.goldValue.ToString() }, new List<Sprite> { ((magicItem._Benefit() as StatBenefit).statToBenefit == StatToBenefit.MaxHP) ? PrefabArchive.Instance.healthSprite : PrefabArchive.Instance.swordSprite });
        Vector3 newPos = hoverDisplayer.transform.position;
        newPos.x = transform.position.x + 350f;
        

        hoverDisplayer.transform.SetParent(All_Canvases.FrontestCanvas.transform);
        hoverDisplayer.transform.position = newPos;

        hoverDisplayer.gameObject.SetActive(true);
        //HoverTextBoard.Instance.SetMe(new List<string> { magicItem.magicItemName, magicItem.ItemDescription(), magicItem.goldValue.ToString() }, new List<Sprite> { ((magicItem._Benefit() as StatBenefit).statToBenefit == StatToBenefit.MaxHP) ? PrefabArchive.Instance.healthSprite : PrefabArchive.Instance.swordSprite });
        //HoverTextBoard.Instance.SetMe(new List<string> { magicItem.magicItemName, $"+{magicItem._Benefit().Value()}", magicItem.ItemDescription(), $"{magicItem.goldValue} Gold" },
        //                            new List<Sprite> { SwitchOnBenefit((magicItem._Benefit() as StatBenefit).statToBenefit) });
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
        if (magicItem == null || !hoverDisplayer) 
            return;
        //HoverTextBoard.Instance.UnSetMe();
        hoverDisplayer.gameObject.SetActive(false);
        hoverDisplayer.transform.SetParent(transform.parent); //Doesnt really matter
    }
}
