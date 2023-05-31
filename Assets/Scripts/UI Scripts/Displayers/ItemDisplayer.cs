﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDisplayer : BasicDisplayer, IPointerEnterHandler, IPointerExitHandler
{
    public MagicItem magicItem;
    
    public GameObject sellGroup;

    //Gear displayer
    //[SerializeField]
    //static MercGearDisplayer mercGearDisplayer;
    [SerializeField]
    BasicDisplayer hoverDisplayer;

    
    [SerializeField]
    private Sprite emptySprite;

    public void OnClick()
    {
        if(magicItem ==null)
            return; //instead of caching button

        //if (!clickedOnce)
        //{
        //    StartCoroutine(nameof(DoubleClickCooldown));
        //}
        //else
        //{
            EquipInventoryManager.Instance.TryEquip(magicItem);
        //    //if (mercGearDisplayer) 
        //    //{
        //    //    mercGearDisplayer.TryEquipItem(magicItem);
        //    //}

        //}
    }
  
    public void SetItem(MagicItem newItem, BasicDisplayer bd)
    {
        magicItem = newItem;
        hoverDisplayer = bd;
        //SetMe(magicItem.magicItemName, magicItem.itemSprite); //no price on this one
        
        SetMe(new List<string> { magicItem.magicItemName, magicItem._Benefit().Value().ToString(), magicItem._Benefit().BenefitStatName()}, new List<Sprite> {magicItem.itemSprite});
    }
    public void SetItem()
    {
        magicItem = null;
        hoverDisplayer = null ; //just in case

        //SetMe(magicItem.magicItemName, magicItem.itemSprite); //no price on this one

        SetMe(new List<string> { "", ""}, new List<Sprite> {emptySprite});
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (magicItem == null || !hoverDisplayer)
            return;

        SetDisplayOn();
    }

    private void SetDisplayOn()
    {
        hoverDisplayer.gameObject.SetActive(true);

        Vector3 newPos = transform.position;
        //newPos.x = newPos.x > 400 ? newPos.x - 280f : newPos.x + 400f;
        newPos.x -= 350f;
        hoverDisplayer.transform.position = newPos;

        hoverDisplayer.SetMe(new List<string> { magicItem.magicItemName, magicItem.fittingSlotType.ToString(), magicItem._Benefit().BenefitStatName(), magicItem._Benefit().Value().ToString(), magicItem.ItemDescription(), magicItem.goldValue.ToString() }, new List<Sprite> { magicItem.itemSprite });
        hoverDisplayer.transform.SetParent(All_Canvases.FrontestCanvas.transform);
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
        if (magicItem == null)
            return;

        hoverDisplayer.transform.SetParent(transform.parent); //not sure we really need to do this
        hoverDisplayer.gameObject.SetActive(false);
    }
}
