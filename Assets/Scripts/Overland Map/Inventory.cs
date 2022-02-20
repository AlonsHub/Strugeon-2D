﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField]
    TMP_Text goldDisplayer;
    public int Gold { get => PlayerDataMaster.Instance.currentPlayerData.gold; 
                     private set => PlayerDataMaster.Instance.currentPlayerData.gold = value; } // not sure setter is needed
    [SerializeField]
    GoldUpdater goldUpdater;

    public List<MagicItem> inventoryItems { get => PlayerDataMaster.Instance.currentPlayerData.magicItems; private set => PlayerDataMaster.Instance.currentPlayerData.magicItems = value; }
    public int magicItemCount => inventoryItems.Count;
    public System.Action OnInventoryChange;
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        OnInventoryChange += TEST_OnInventoryChange;

        //If magicItems(changed name)-> inventoryItems (aka PlayerDataMaster.Instance.currentPlayerData.magicItems) is null, it means there was nothing there when it was saved - or that it is a new save
        if (inventoryItems == null)
        inventoryItems = new List<MagicItem>();

        //foreach (var item in inventoryItems)
        //{
        //    if(!item.FecthSprite())
        //    {
        //        Debug.LogError($"item {item.magicItemName} could not find a sprite");
        //    }
        //}
    }
    void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {

        goldUpdater = GameObject.Find("Gold Displayer").GetComponent<GoldUpdater>();
        if (!goldUpdater)
        {
            Debug.LogError("no gold updater");
        }
    }

    private void Start()
    {
        goldUpdater = GameObject.Find("Gold Displayer").GetComponent<GoldUpdater>();
        if (!goldUpdater)
        {
            Debug.LogError("no gold updater");
        }

        foreach (var item in inventoryItems)
        {
            if (!item.FecthSprite())
            {
                Debug.LogError($"item {item.magicItemName} could not find a sprite");
            }
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        goldUpdater.RefreshGold();
    }
    public bool TryRemoveGold(int amount)
    {
        if(amount > Gold)
        {
            //insufficient funds
            return false;
        }

        Gold -= amount;
        goldUpdater.RefreshGold();
        return true;
    }
    
    public void AddMagicItem(MagicItem magicItem)
    {
        Debug.Log($"Item: {magicItem.magicItemName} was added!");
        inventoryItems.Add(magicItem);

        //displayer update? 
        //call an event that it maybe registered to, if it is enabled? sounds CORRECT TBD
        OnInventoryChange.Invoke();
    }
    public bool RemoveMagicItem(MagicItem magicItem)
    {
        if(inventoryItems.Remove(magicItem))
        {

            Debug.Log($"Item: {magicItem.magicItemName} was removed!");
            OnInventoryChange.Invoke();
            return true;
        }
        else
        {

            Debug.Log($"Item: {magicItem.magicItemName} was not found, so it could not be removed!"); //still invoke OnInventoryChange?
            return false;
        }

        //displayer update? 
        //call an event that it maybe registered to, if it is enabled? sounds CORRECT TBD
    }

    void TEST_OnInventoryChange()
    {
        Debug.Log("On inventory change");
    }
}
