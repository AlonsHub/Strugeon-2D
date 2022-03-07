using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayManager : MonoBehaviour
{
    //parent
    [SerializeField]
    Transform inventoryParent;
    //prefab with bsicdisplayer
    [SerializeField]
    GameObject itemDisplayerPrefab;
    [SerializeField] //just to see them
    List<MagicItemDisplayer> displayers = new List<MagicItemDisplayer>();

    bool sellModeIsOn;
    private void OnEnable()
    {
        SetSellMode(false);
        RefreshInventory();

        Inventory.Instance.OnInventoryChange += RefreshInventory;
    }

    public void RefreshInventory()
    {
        EnsureOneDisplayerPerMagicItem();

        //Draw items
        for (int i = 0; i < displayers.Count; i++)
        {
            displayers[i].SetItem(Inventory.Instance.inventoryItems[i]);
        }
    }

    private void EnsureOneDisplayerPerMagicItem()
    {
        int diff = displayers.Count - Inventory.Instance.magicItemCount;

        if (diff == 0)
            return;

        if (diff < 0)
        {
            for (int i = 0; i < diff * -1; i++) //*-1 since diff is < 0, but is still the difference
            {
                GameObject displayerObject = Instantiate(itemDisplayerPrefab, inventoryParent);
                displayers.Add(displayerObject.GetComponent<MagicItemDisplayer>());
            }
        }
        else
        {
            for (int i = Inventory.Instance.magicItemCount; i < displayers.Count; i++)
            {
                Destroy(displayers[i].gameObject);
            }
            displayers.RemoveRange(Inventory.Instance.magicItemCount, diff);
        }
    }

    public void SetSellMode(bool onOrOff) //on is true, set in inspector
    {
        sellModeIsOn = onOrOff;
        foreach (var item in displayers)
        {
            item.sellGroup.SetActive(onOrOff);
        }
    }
    public void ToggleSellMode() //set in inspector
    {
        SetSellMode(!sellModeIsOn);
    }

    private void OnDisable()
    {
        Inventory.Instance.OnInventoryChange -= RefreshInventory;
    }


}
