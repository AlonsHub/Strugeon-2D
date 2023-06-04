using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BaseInventory : MonoBehaviour, ISearchBarable
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

    List<MagicItem> _localItems;
   
    protected virtual void OnEnable()
    {
        _localItems = Inventory.Instance.inventoryItems;
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

        int diff = displayers.Count - _localItems.Count;
        //int diff = displayers.Count - Inventory.Instance.magicItemCount;
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
            for (int i = _localItems.Count; i < displayers.Count; i++)
            {
                Destroy(displayers[i].gameObject); //just disable them dude... TBF
            }
            displayers.RemoveRange(_localItems.Count, diff);
        }
    }

    //This should operate with a local _list that can be ordered on its own.
    public virtual void RefreshInventory()
    {
        EnsureOneDisplayerPerMagicItem();

        //Draw items
        for (int i = 0; i < _localItems.Count; i++)
        {
            //if (!_localItems[i].FetchSprite())
            //    continue;

            displayers[i].SetMeFull(_localItems[i], this);
        }
    }

    public virtual void SortItemsByValue()
    {

        //_localItems = SorterFilter.SortListBy(_localItems, new ItemComparer_Value());
        //_localItems = _localItems.Sort(Archived_ItemComparers.ItemComparer_Value());
        //_localItems.Sort(Archived_ItemComparers.ItemComparer_Value());
        _localItems.Sort(new ItemComparer_Value());

        RefreshInventory();
    }

    public void Search(string searchWord)
    {
        _localItems = Inventory.Instance.inventoryItems.Where
            (x => x.magicItemName.IndexOf(searchWord, 0, x.magicItemName.Length, System.StringComparison.OrdinalIgnoreCase) != -1).ToList();
        RefreshInventory();
    }

    public void ClearSearch()
    {
        _localItems = Inventory.Instance.inventoryItems;
        RefreshInventory();
    }
}
