using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : MonoBehaviour
{
    //parent
    [SerializeField]
    Transform inventoryParent;
    //prefab with bsicdisplayer
    [SerializeField]
    GameObject itemDisplayerPrefab;

    [SerializeField] //just to see them
    List<InventoryItemDisplayer> displayers = new List<InventoryItemDisplayer>();

    MagicItem selectedItem; //currently selected item to display/sell
    [SerializeField]
    BasicDisplayer selectedItemDesplayer;
    [SerializeField]
    MagicItemSO emptyItemSO; //the data for what to show when no items are available
    MagicItem emptyItem => emptyItemSO.magicItem; //the data for what to show when no items are available
    [SerializeField]
    UnityEngine.UI.Button sellButton;

    [SerializeField]
    [ColorUsage(true)]
    Color titleColor;
    [SerializeField]
    [ColorUsage(true)]
    Color slotColor;
    private void OnEnable()
    {
        RefreshInventory();
        Inventory.Instance.OnInventoryChange += RefreshInventory;

    }

    private void OnDisable()
    {
        Inventory.Instance.OnInventoryChange -= RefreshInventory;

    }
    private void EnsureOneDisplayerPerMagicItem()
    {

        int diff = displayers.Count - Inventory.Instance.magicItemCount;
        //int diff = displayers.Count - Inventory.Instance.inventoryItems.Count;

        if (diff == 0)
            return;

        if (diff < 0)
        {
            for (int i = 0; i < diff * -1; i++) //*-1 since diff is < 0, but is still the difference
            {
                GameObject displayerObject = Instantiate(itemDisplayerPrefab, inventoryParent);
                displayers.Add(displayerObject.GetComponent<InventoryItemDisplayer>());
            }
        }
        else
        {
            //for (int i = Inventory.Instance.magicItemCount; i < displayers.Count; i++)
            for (int i = Inventory.Instance.magicItemCount; i < displayers.Count; i++)
            {
                Destroy(displayers[i].gameObject); //just disable them dude... TBF
            }
            displayers.RemoveRange(Inventory.Instance.magicItemCount, diff);
        }
    }

    public void RefreshInventory()
    {
        EnsureOneDisplayerPerMagicItem();

        //Draw items
        for (int i = 0; i < Inventory.Instance.magicItemCount; i++)
        {
            if (!Inventory.Instance.inventoryItems[i].FetchSprite())
                continue;

            //displayers[i].SetMe(new List<string> { }, new List<Sprite> { Inventory.Instance.inventoryItems[i].itemSprite });
            displayers[i].SetMeFull(Inventory.Instance.inventoryItems[i], this);
        }

        SetCurrentItem();
    }

    public void SetCurrentItem(MagicItem newItem)
    {
        selectedItem = newItem;
        sellButton.interactable = true;
        string titleColorHex = ColorUtility.ToHtmlStringRGBA(titleColor);
        string slotColorHex = ColorUtility.ToHtmlStringRGBA(slotColor);

        selectedItemDesplayer.SetMe(new List<string> { selectedItem.magicItemName, $"<color=#{slotColorHex}> {selectedItem.fittingSlotType} | </color>" +$"<color=#{titleColorHex}>" +
            $"{selectedItem._Benefit().BenefitStatName()} + {selectedItem._Benefit().Value()} </color>", 
            selectedItem.ItemDescription(), $"{selectedItem.goldValue} Gold"}, new List<Sprite> {selectedItem.itemSprite});
    }
    public void SetCurrentItem()
    {
        if (Inventory.Instance.inventoryItems.Count == 0)
        {
            selectedItem = null;
            //Set as nothing
            selectedItemDesplayer.SetMe(new List<string> { emptyItem.magicItemName, "" ,
            /*emptyItem.ItemDescription()*/ "Nothing to describe...", ""}, new List<Sprite> { emptyItem.itemSprite });
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

    }

}
