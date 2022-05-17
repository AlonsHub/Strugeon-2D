using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplayer : BasicDisplayer
{
    public MagicItem magicItem;
    
    public GameObject sellGroup;

    //Gear displayer
    [SerializeField]
    static MercGearDisplayer mercGearDisplayer;

    private void OnEnable()
    {
        if(!mercGearDisplayer) 
        {
            mercGearDisplayer = FindObjectOfType<MercGearDisplayer>(); //since it's static, it only happens once //TBF horrible
        }
    }


    bool clickedOnce = false;
    [SerializeField]
    private Sprite emptySprite;

    public void OnClick()
    {
        if(magicItem ==null)
            return; //instead of caching button

        if (!clickedOnce)
        {
            StartCoroutine(nameof(DoubleClickCooldown));
        }
        else
        {
            if (mercGearDisplayer) 
            {
                mercGearDisplayer.TryEquipItem(magicItem);
            }
        }
    }
    IEnumerator DoubleClickCooldown()
    {
        clickedOnce = true;
        yield return new WaitForSecondsRealtime(GeneralInputSettings.doubleClickWindow);
        clickedOnce = false;
    }
    public void SetItem(MagicItem newItem)
    {
        magicItem = newItem;

        //SetMe(magicItem.magicItemName, magicItem.itemSprite); //no price on this one

        SetMe(new List<string> { magicItem.magicItemName, magicItem.goldValue.ToString() }, new List<Sprite> {magicItem.itemSprite});
    }
    public void SetItem()
    {
        magicItem = null;

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

    public void TryEquipItem(IEquipable toEquip)
    {
        if (!mercGearDisplayer)
            return;

        mercGearDisplayer.TryEquipItem(toEquip as MagicItem);
        
    }
}
