﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tavern : MonoBehaviour
{

    public static Tavern Instance;
    //SHOULD destroy on load
    //on entering the tavern, it should read the current SaveData and display it
    [SerializeField]
    GameObject noMoreRoomsWindow;
    [SerializeField]
    GameObject newSquadMenu;

    [SerializeField]
    GameObject roomPanelPrefab;

    [SerializeField]
    Transform roomButtonParent;

    List<GameObject> roomButtons;

    //[SerializeField]
    //Image ;
    //[SerializeField]
    //GameObject roomPanelPrefab;
    //[SerializeField]
    //GameObject roomPanelPrefab;

    List<RoomDisplayer> roomDisplayers;


    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        RefreshRooms();
    }

    private void OnEnable()
    {
        RefreshRooms();
    }

    public void RefreshRooms()
    {
        if(roomButtons != null)
        {
            for (int i = roomButtons.Count-1; i >= 0; i--)
            {
                Destroy(roomButtons[i]);
            }
            //roomButtons.Clear();
        }
           
        roomButtons = new List<GameObject>();
        //set up empty rooms
        for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.totalSquadRooms; i++)
        {
            GameObject go = Instantiate(roomPanelPrefab, roomButtonParent);
            roomButtons.Add(go);
        }
        int c = 0;
        //load squads into rooms (if applicable)
        foreach (var squad in PartyMaster.Instance.squads)
        {
            roomButtons[c].GetComponentInChildren<Image>().color = Color.red;
            c++;
        }
    }

    // public void ReadSaveData()
    // Takes the available mercs from current save data
    public void TryOpenNewSquadMenu()
    {
        if(PartyMaster.Instance.squads.Count == PlayerDataMaster.Instance.currentPlayerData.totalSquadRooms)
        {
            noMoreRoomsWindow.SetActive(true);
            return;
        }

        newSquadMenu.SetActive(true);
    }

}
