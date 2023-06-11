using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDisplayer : BasicDisplayer, IPointerEnterHandler, IPointerExitHandler
{
    public MagicItem magicItem;
    public MercSheet mercSheet;
    public GameObject sellGroup;


    [SerializeField]
    Sprite comparisonArrowUpGreen;
    [SerializeField]
    Sprite comparisonArrowDownRed;
    [SerializeField]
    Sprite comparisonArrowEquals;

    //Gear displayer
    //[SerializeField]
    //static MercGearDisplayer mercGearDisplayer;
    [SerializeField]
    BasicDisplayer hoverDisplayer;

    
    [SerializeField]
    private Sprite emptySprite;


    Sprite _comparisonArrow;
    float _currentDifference;
    Color _col;

    private void OnDisable()
    {
        hoverDisplayer.gameObject.SetActive(false);
    }
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
  
    public void SetItem(MagicItem newItem, MercSheet sheet , BasicDisplayer bd)
    {
        mercSheet = sheet;
        magicItem = newItem;
        hoverDisplayer = bd;
        //SetMe(magicItem.magicItemName, magicItem.itemSprite); //no price on this one
        SetComparisonArrow();


        //SetMe(new List<string> { magicItem.magicItemName, magicItem._Benefit().Value().ToString(), magicItem._Benefit().BenefitStatName()}, new List<Sprite> {magicItem.itemSprite, _comparisonArrow});
        SetMe(new List<string> { magicItem.magicItemName, $"<color=#{ColorUtility.ToHtmlStringRGBA(_col)}>{(_currentDifference>0 ? "+" : "")}{_currentDifference}</color>", magicItem._Benefit().BenefitStatName()}, new List<Sprite> {magicItem.itemSprite, _comparisonArrow});
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
        SetComparisonArrow();

        Vector3 newPos = transform.position;
        //newPos.x = newPos.x > 400 ? newPos.x - 280f : newPos.x + 400f;
        newPos.x -= 350f;
        hoverDisplayer.transform.position = newPos;

        //Color col = _currentDifference > 0 ? Color.green : Color.red;
        //if (_currentDifference == 0)
        //    col = Color.white;


        hoverDisplayer.SetMe(new List<string> { magicItem.magicItemName, magicItem.fittingSlotType.ToString(), magicItem._Benefit().BenefitStatName(), magicItem._Benefit().Value().ToString(), $"<color=#{ColorUtility.ToHtmlStringRGBA(_col)}>({(_currentDifference>0 ? "+" : "")}{_currentDifference})</color>", magicItem.ItemDescription(), magicItem.goldValue.ToString() }, new List<Sprite> { magicItem.itemSprite, _comparisonArrow });
        hoverDisplayer.transform.SetParent(All_Canvases.FrontestCanvas.transform);
    }

    private void SetComparisonArrow()
    {
        if (mercSheet.gear.GetItemBySlot(magicItem.fittingSlotType)._Benefit().BenefitStatName() == magicItem._Benefit().BenefitStatName())
        {
            _currentDifference =  magicItem._Benefit().Value() - mercSheet.gear.GetItemBySlot(magicItem.fittingSlotType)._Benefit().Value();

            if (_currentDifference < 0)
            {
                _comparisonArrow = comparisonArrowDownRed;
            }
            else if (_currentDifference > 0)
            {
                _comparisonArrow = comparisonArrowUpGreen;
            }
            else
            {
                _comparisonArrow = comparisonArrowEquals;
            }
        }
        else
        {
            _comparisonArrow = emptySprite;
        }


        _col = _currentDifference > 0 ? Color.green : Color.red;
        if (_currentDifference == 0)
            _col = Color.white;
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
