using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewSanctumInventory : MonoBehaviour, ISearchBarable
{
    [SerializeField]
    GameObject displayerPrefab;
    [SerializeField]
    Transform parent;

    List<SanctumItemDisplayer> itemDisplayers;
    List<MagicItem> _localItems;


    private void Start()
    {
        _localItems = Inventory.Instance.inventoryItems;

        Refresh();
    }
    private void OnEnable()
    {
        Inventory.Instance.OnInventoryChange += Refresh;
    }
    private void OnDisable()
    {
        Inventory.Instance.OnInventoryChange -= Refresh;
    }

    public void Refresh()
    {
        if (itemDisplayers == null)
            itemDisplayers = new List<SanctumItemDisplayer>();

        EnsureOneDisplayerPerItem();

        for (int i = 0; i < itemDisplayers.Count; i++)
        {
            itemDisplayers[i].SetMeFull(_localItems[i]);
        }
    }

    private void EnsureOneDisplayerPerItem()
    {
        int delta = itemDisplayers.Count - _localItems.Count;
        if (delta > 0)
        {
            for (int i = 0; i < delta; i++)
            {
                Destroy( itemDisplayers[itemDisplayers.Count-1].gameObject);
                itemDisplayers.RemoveAt(itemDisplayers.Count - 1);
            }
        }
        else if (delta < 0)
        {
            for (int i = 0; i < Mathf.Abs(delta); i++)
            {
                //SanctumItemDisplayer sanctumItemDisplayer = Instantiate(displayerPrefab, parent).GetComponent<SanctumItemDisplayer>();
                itemDisplayers.Add(Instantiate(displayerPrefab, parent).GetComponent<SanctumItemDisplayer>());
            }
        }
    }

    public void Search(string searchWord)
    {
        _localItems = Inventory.Instance.inventoryItems.Where
            (x => x.magicItemName.IndexOf(searchWord, 0, x.magicItemName.Length, System.StringComparison.OrdinalIgnoreCase) != -1).ToList();
        Refresh();
    }

    public void ClearSearch()
    {
        _localItems = Inventory.Instance.inventoryItems;
        Refresh();

    }
}
