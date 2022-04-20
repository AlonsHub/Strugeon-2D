﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    MercGearDisplayer mercGearDisplayer;

    List<MagicItem> relevantItems;

    bool sellModeIsOn;
    private void OnEnable()
    {
        SetSellMode(false);

        if(!mercGearDisplayer)
        {
            Debug.LogError("No gear displayer!"); //TBF need to decide if this will be used for other inventory views or not
            return;
        }
        
        RefreshInventory();

        Inventory.Instance.OnInventoryChange += RefreshInventory;
        mercGearDisplayer.OnMercChange += RefreshInventory;
    }

    public void RefreshInventory()
    {
        if (mercGearDisplayer.GetMercSheet != null)
            relevantItems = Inventory.Instance.inventoryItems.Where(x => x.relevantClasses.Contains(mercGearDisplayer.GetMercSheet.mercClass)).ToList();
        else
            return;
        //    relevantItems = Inventory.Instance.inventoryItems;
        EnsureOneDisplayerPerMagicItem();

        //Draw items
        for (int i = 0; i < relevantItems.Count; i++)
        {
            displayers[i].SetItem(relevantItems[i]);
        }
    }

    private void EnsureOneDisplayerPerMagicItem()
    {

        //int diff = displayers.Count - Inventory.Instance.magicItemCount;
        int diff = displayers.Count - relevantItems.Count;

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
            //for (int i = Inventory.Instance.magicItemCount; i < displayers.Count; i++)
            for (int i = relevantItems.Count; i < displayers.Count; i++)
            {
                Destroy(displayers[i].gameObject); //just disable them dude... TBF
            }
            displayers.RemoveRange(relevantItems.Count, diff);
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
        mercGearDisplayer.OnMercChange -= RefreshInventory;
    }


}
