﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipInventoryManager : MonoBehaviour
{
    public static EquipInventoryManager Instance;
    //parent
    [SerializeField]
    Transform inventoryParent;
    //prefab with bsicdisplayer
    [SerializeField]
    GameObject itemDisplayerPrefab;
    // [SerializeField]
    //GameObject emptyDisplayerPrefab;
     
    [SerializeField] //just to see them
    List<ItemDisplayer> displayers = new List<ItemDisplayer>(); //assume 16 to begin with, add by 4's whenever needed!
    //List<GameObject> empties = new List<GameObject>();

    
    //MercGearDisplayer mercGearDisplayer;

    List<MagicItem> relevantItems;

    bool sellModeIsOn;
    EquipSlotType relevantSlot;


    //
    MercSheet relevantMercSheet;
    LobbyMercDisplayer lobbyMercDisplayer;
    private void Awake()
    {
        Instance = this;
    }
    //private void Awake()
    //{
    //    if (!mercGearDisplayer)
    //    {
    //        Debug.LogError("No gear displayer!"); //TBF need to decide if this will be used for other inventory views or not
    //        return;
    //    }
    //    //EnsureOneDisplayerPerMagicItem();
    //}
    //private void OnEnable()
    //{
    //    //SetSellMode(false);

    //    if(relevantMercSheet == null)
    //    {
    //        Debug.LogError("No relevantMercSheet!"); //TBF need to decide if this will be used for other inventory views or not
    //        return;
    //    }

    //    RefreshInventory();

    //    Inventory.Instance.OnInventoryChange += RefreshInventory;
    //    //mercGearDisplayer.OnMercChange += RefreshInventory;
    //}
    private void OnDisable()
    {
        //for (int i = empties.Count-1; i >= 0; i--)
        //{
        //    Destroy(empties[i]);
        //}
        Inventory.Instance.OnInventoryChange -= RefreshInventory;
        //if (mercGearDisplayer)
        //    mercGearDisplayer.OnMercChange -= RefreshInventory;
    }
    public void FilterBySlot(EquipSlotType slotType, MercSheet ms, LobbyMercDisplayer lmd)
    {
        lobbyMercDisplayer = lmd;
        relevantMercSheet = ms;
        //gsd = gearSlotDispayer;
        relevantSlot = slotType;
        

        Inventory.Instance.OnInventoryChange += RefreshInventory;
        RefreshInventory();
    }
    public void RefreshInventory()
    {
        if (relevantMercSheet != null)
            relevantItems = Inventory.Instance.inventoryItems.Where(x => x.fittingSlotType == relevantSlot && x.relevantClasses.Contains(relevantMercSheet.mercClass)).ToList();
        else
            return;

        
        
        EnsureOneDisplayerPerMagicItem();

        foreach (var item in displayers)
        {
            item.gameObject.SetActive(true);
        }
        if(relevantItems.Count > displayers.Count)
        {
            int diff = relevantItems.Count - displayers.Count;
            diff = (diff % 4 == 0) ? diff : diff +(4-diff%4); //if it does not divide neatly by 4, add 1... could cast as float
            //diff = round up ((float)diff/4f)
            for (int i = 0; i < diff; i++)
            {
                displayers.Add(Instantiate(itemDisplayerPrefab, inventoryParent).GetComponent<ItemDisplayer>());
            }
        }
        else if (displayers.Count > relevantItems.Count) //excluding the displayer.count == relevantItems.count option
        {
            int diff = displayers.Count - relevantItems.Count;
            if(diff>=4)
            {
                int leftover = 4- relevantItems.Count % 4;
                if(leftover == 4)
                {
                    leftover = 0;
                }

                for (int i = relevantItems.Count + leftover; i < displayers.Count; i++)
                {
                    displayers[i].gameObject.SetActive(false);
                }
            }
        }

        //Draw items
        for (int i = 0; i < relevantItems.Count; i++)
        {
            if (!relevantItems[i].FetchSprite())
                continue;

            displayers[i].SetItem(relevantItems[i]);
        }
        for (int i = relevantItems.Count; i < displayers.Count; i++)
        {
         
            displayers[i].SetItem();
        }
        //while (displayers.Count + empties.Count < 16 || (displayers.Count + empties.Count) % 4 != 0)
        //{
        //    empties.Add( Instantiate(emptyDisplayerPrefab, inventoryParent));
        //}

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
                displayers.Add(displayerObject.GetComponent<ItemDisplayer>());
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

   public void TryEquip(MagicItem item)
    {
        if (relevantMercSheet == null)
            return;
        Inventory.Instance.RemoveMagicItem(item);
        IEquipable removedItem;
        if ((removedItem = relevantMercSheet.gear.TryEquipItemToSlot(item)) != null)
        {
            //add to inventory
            Inventory.Instance.AddMagicItem(removedItem as MagicItem);
        }
        if(lobbyMercDisplayer)
        lobbyMercDisplayer.DisplayGear(); //refreshes
        else if(MercGearDisplayer.Instance.isActiveAndEnabled)
        {
            MercGearDisplayer.Instance.SetMeFully(relevantMercSheet);
        }
    }


}
