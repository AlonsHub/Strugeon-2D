using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : BaseInventory
{
   
    MagicItem selectedItem; //currently selected item to display/sell
    [SerializeField]
    BasicDisplayer selectedItemDesplayer;

    [SerializeField]
    StarGraph starGraph;
   
    [SerializeField]
    UnityEngine.UI.Button sellButton;

    [SerializeField]
    [ColorUsage(true)]
    Color titleColor;
    [SerializeField]
    [ColorUsage(true)]
    Color slotColor;

    private void Start()
    {
        SetCurrentItem();
        //starGraph.SetToItem(emptyItem);
        starGraph.SetAllToValue(0f);
    }
    public override void SetCurrentItem(MagicItem newItem)
    {
        selectedItem = newItem;
        sellButton.interactable = true;
        string titleColorHex = ColorUtility.ToHtmlStringRGBA(titleColor);
        string slotColorHex = ColorUtility.ToHtmlStringRGBA(slotColor);

        selectedItemDesplayer.SetMe(new List<string> { selectedItem.magicItemName, 
            $"<color=#{slotColorHex}> {selectedItem.fittingSlotType} | </color>",
            $"<color=#{titleColorHex}> {selectedItem._Benefit().BenefitStatName()}</color>", 
            $"<color=#{titleColorHex}>{selectedItem._Benefit().Value()} </color>", 
            selectedItem.ItemDescription(), $"{selectedItem.goldValue} Gold"}, 
            //now sprites:
            new List<Sprite> {selectedItem.itemSprite});

        starGraph.SetToItem(selectedItem);
    }
    public void SetCurrentItem()
    {
        if (Inventory.Instance.inventoryItems.Count == 0)
        {
            selectedItem = null;

            //starGraph.SetToItem(emptyItem);
            starGraph.SetAllToValue(0f);


            //Set as nothing
            string titleColorHex = ColorUtility.ToHtmlStringRGBA(titleColor);
            string slotColorHex = ColorUtility.ToHtmlStringRGBA(slotColor);

            selectedItemDesplayer.SetMe(new List<string> { emptyItem.magicItemName,
            $"<color=#{slotColorHex}> {emptyItem.fittingSlotType} | </color>",
            $"<color=#{titleColorHex}> {emptyItem._Benefit().BenefitStatName()}</color>",
            $"<color=#{titleColorHex}>{emptyItem._Benefit().Value()} </color>",
            emptyItem.ItemDescription(), $"{emptyItem.goldValue} Gold"},
            //now sprites:
            new List<Sprite> { emptyItem.itemSprite });
            sellButton.interactable = false;
        }
        else
        {
            SetCurrentItem(Inventory.Instance.inventoryItems[0]);
            displayers[0].OnMyClick();
        }
    }

    public void SellSelectedItem()
    {
        if(selectedItem == null)
        {
            Debug.LogError("no selected item to sell");
        }

        //sell selected item
        Inventory.Instance.SellItem(selectedItem);
        SetCurrentItem();
    }

    public void SortBy(SortByMethod sortByMethod)
    {
        switch (sortByMethod)
        {
            case SortByMethod.Value:

                break;
            case SortByMethod.Type:
                break;
            default:
                break;
        }
    }

  
}
