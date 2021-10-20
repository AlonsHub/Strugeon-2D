using System.Collections;
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

    public RoomManager roomManager;
    //List<GameObject> roomButtons;
    List<RoomButton> _roomButtons;

    //[SerializeField]
    //Image ;
    //[SerializeField]
    //GameObject roomPanelPrefab;
    //[SerializeField]
    //GameObject roomPanelPrefab;

    List<RoomBuildDisplayer> roomDisplayers;

    [SerializeField]
    SquadRoomDisplayer squadRoomDisplayer;


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

    public void RefreshRooms()
    {
        if(_roomButtons != null)
        {
            for (int i = _roomButtons.Count-1; i >= 0; i--)
            {
                Destroy(_roomButtons[i].gameObject);
            }
            //roomButtons.Clear();
        }
           
        //roomButtons = new List<GameObject>();
        _roomButtons = new List<RoomButton>();
        //set up empty rooms
        for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.rooms.Count; i++)
        {
            RoomButton go = Instantiate(roomPanelPrefab, roomButtonParent).GetComponent<RoomButton>();
            _roomButtons.Add(go);
            //roomButtons.Add(go);
            //go.room.roomNumber = i; //RENUMBERS ROOMS?!
            
            go.SetMe(PlayerDataMaster.Instance.currentPlayerData.rooms[i], i);
          
        }
    }

    // public void ReadSaveData()
    // Takes the available mercs from current save data
    public void TryOpenNewSquadMenu(Room r)
    {
        newSquadMenu.SetActive(true);

        newSquadMenu.GetComponent<SquadBuilder>().SetToRoom(r);
    }



    public void TryOpenNewSquadMenu()
    {
        if(PartyMaster.Instance.squads.Count == PlayerDataMaster.Instance.currentPlayerData.rooms.Count)
        {
            noMoreRoomsWindow.SetActive(true);
            return;
        }

        for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.rooms.Count; i++)
        {
            if (!PlayerDataMaster.Instance.currentPlayerData.rooms[i].isOccupied)
            {
                newSquadMenu.SetActive(true);

                newSquadMenu.GetComponent<SquadBuilder>().SetToRoom(PlayerDataMaster.Instance.currentPlayerData.rooms[i]);
                break;
            }
        }
        //decide which room to fill and send to newSquadMenu
    }

    public void EditSquadMenu(Squad s, Room room)
    {
        //disassembles a squad
        //opens and sets up the newSquadWindow with the members of that squad in the PartySlots 
            newSquadMenu.SetActive(true);
            //newSquadMenu.GetComponent<SquadBuilder>().SetToRoom(room);
        if (s != null)
        {
            PartyMaster.Instance.squads.Remove(s);
            newSquadMenu.GetComponent<SquadBuilder>().EditSquadMode(s.pawns, room);
            return;
        }

        TryOpenNewSquadMenu(room);
    }

    RoomButton activeRoomButton;
    public void SquadRoomSetup(RoomButton roomButton)
    {
        if (!squadRoomDisplayer.gameObject.activeSelf)
            squadRoomDisplayer.gameObject.SetActive(true);

        squadRoomDisplayer.SetMe(roomButton.room);
        activeRoomButton = roomButton;
    }

    public void EditActiveSquad()
    {
        activeRoomButton.EditMe();
    }

    //private void OnDisable()
    //{
    //    PlayerDataMaster.Instance.GrabAndSaveData();
    //}
}
