using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInventory : MonoBehaviour
{
    //parent
    [SerializeField]
    Transform inventoryParent;
    //prefab with bsicdisplayer
    [SerializeField]
    GameObject itemDisplayerPrefab;

    [SerializeField] //just to see them
    protected List<InventoryItemDisplayer> displayers = new List<InventoryItemDisplayer>();

    [SerializeField]
    MagicItemSO emptyItemSO; //the data for what to show when no items are available
    public MagicItem emptyItem => emptyItemSO.magicItem; //the data for what to show when no items are available
    
   
    protected virtual void OnEnable()
    {
        RefreshInventory();
        Inventory.Instance.OnInventoryChange += RefreshInventory;

    }

    protected virtual void OnDisable()
    {
        Inventory.Instance.OnInventoryChange -= RefreshInventory;

    }

    public virtual void SetCurrentItem(MagicItem itemToSet)
    {

    }
    public virtual void EnsureOneDisplayerPerMagicItem()
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

    public virtual void RefreshInventory()
    {
        EnsureOneDisplayerPerMagicItem();

        //Draw items
        for (int i = 0; i < Inventory.Instance.magicItemCount; i++)
        {
            if (!Inventory.Instance.inventoryItems[i].FetchSprite())
                continue;

            displayers[i].SetMeFull(Inventory.Instance.inventoryItems[i], this);
        }
    }

    

}
