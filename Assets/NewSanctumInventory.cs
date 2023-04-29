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
            for (int i = itemDisplayers.Count - 1; i > itemDisplayers.Count - 1 - delta; i++)
            {
                //itemDisplayers[i].gameObject.SetActive(false);
                Destroy( itemDisplayers[i].gameObject);
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
