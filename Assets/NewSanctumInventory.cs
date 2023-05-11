using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSanctumInventory : MonoBehaviour
{
    [SerializeField]
    GameObject displayerPrefab;
    [SerializeField]
    Transform parent;

    List<SanctumItemDisplayer> itemDisplayers;

    private void Start()
    {

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
            itemDisplayers[i].SetMeFull(Inventory.Instance.inventoryItems[i]);
        }
    }

    private void EnsureOneDisplayerPerItem()
    {
        int delta = itemDisplayers.Count - Inventory.Instance.magicItemCount;
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
}
