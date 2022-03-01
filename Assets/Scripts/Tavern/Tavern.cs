using System.Collections;
using System.Linq;
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
    public SquadBuilder squadBuilder;

    [SerializeField]
    GameObject roomPanelPrefab;

    [SerializeField]
    Transform roomButtonParent;

    public RoomManager roomManager;
    //List<GameObject> roomButtons;
    List<RoomButton> _roomButtons;

    List<RoomBuildDisplayer> roomDisplayers;

    [SerializeField]
    SquadRoomDisplayer squadRoomDisplayer;


    /// <summary>
    /// UI tricks
    /// 
    /// 1) Have a list of the main "tier" of windows. such that when one element within the group is activeated - the rest MUST be deactivated
    /// </summary>
    
    [SerializeField]
    List<GameObject> windowTier1;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _roomButtons = new List<RoomButton>();

        //windowTier1 = new List<GameObject>();

        windowTier1.Add(newSquadMenu);
        windowTier1.Add(squadRoomDisplayer.gameObject);
        //windowTier1.Add(roomManager.transform.parent.gameObject); //adds the parent, which also holds the Exit button
        windowTier1.Add(roomManager.gameObject); //adds the parent, which also holds the Exit button

        RefreshRooms();
        squadBuilder = newSquadMenu.GetComponent<SquadBuilder>();

        HirelingMaster.Instance.LoadExistingHireablesToLog();
        Invoke("TryPromptNewHireling", 1);

        //Populate with existing hireables
        //Maybe test here if they are still hireable or if they left the tavern for greater adventures elsewhere?
    }

    public void DisableWindowTier1(string dontDisable)
    {
        foreach (var item in windowTier1)
        {
            if (item.activeInHierarchy && !item.name.Equals(dontDisable))
            {
                if (item.name.Equals(squadBuilder.name))
                {
                    squadBuilder.CloseMe();
                }
                else
                {
                    item.SetActive(false);
                }
            }
        }
    }

    public void RefreshRooms()
    {
        //if(_roomButtons != null && _roomButtons.Count > 0)
        //{
        //    for (int i = _roomButtons.Count-1; i >= 0; i--)
        //    {
        //        Destroy(_roomButtons[i].gameObject);
        //    }
        //    //roomButtons.Clear();
        //}
        //_roomButtons.Clear();

        //if(_roomButtons.Count != PlayerDataMaster.Instance.currentPlayerData.rooms.Count)
        //{
        //    if(_roomButtons.Count > PlayerDataMaster.Instance.currentPlayerData.rooms.Count)
        //    {
        //        for (int i = _roomButtons.Count-1; i > PlayerDataMaster.Instance.currentPlayerData.rooms.Count; i--)
        //        {
        //            Destroy(_roomButtons[i]);
        //            _roomButtons.RemoveAt(i);
        //        }
        //    }
        //    else
        //    {
        //        for (int i = _roomButtons.Count; i < PlayerDataMaster.Instance.currentPlayerData.rooms.Count; i++)
        //        {
        //            RoomButton go = Instantiate(roomPanelPrefab, roomButtonParent).GetComponent<RoomButton>();
        //            _roomButtons.Add(go);
        //        }
        //    }
        //}
        
        for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.rooms.Count; i++)
        {
            if (_roomButtons.Count - 1 < i)
            {
                RoomButton go = Instantiate(roomPanelPrefab, roomButtonParent).GetComponent<RoomButton>();
                _roomButtons.Add(go);
                go.SetMe(PlayerDataMaster.Instance.currentPlayerData.rooms[i], i);
            }
            else
            {
                _roomButtons[i].SetMe(PlayerDataMaster.Instance.currentPlayerData.rooms[i], i);
            }
        }
    }

    // public void ReadSaveData()
    // Takes the available mercs from current save data
    public void TryOpenNewSquadMenu(Room r)
    {
        newSquadMenu.SetActive(true);

        newSquadMenu.GetComponent<SquadBuilder>().SetToRoom(r); //squadBuilder.SetToRoom(r);?
    }

    public void TryOpenNewSquadMenu()
    {
        
        if(PartyMaster.Instance.squads.Count > PlayerDataMaster.Instance.currentPlayerData.rooms.Count)
        {
            Debug.LogError("There are more squads than there are rooms at the moment. This shouldn't happen");
        }

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
        if (s != null && s.pawns.Count > 0)
        {
            if(PartyMaster.Instance.squads.Remove(PartyMaster.Instance.squads.Where(x => x.Equals(s)).SingleOrDefault()))
            {

                newSquadMenu.GetComponent<SquadBuilder>().EditSquadMode(s.pawns, room);
            }
            else
            {
                Debug.LogError("Failed to remove party from room");
            }
            
           
            return;
        }

        TryOpenNewSquadMenu(room);
    }

    RoomButton activeRoomButton;
    public void SquadRoomSetup(RoomButton roomButton)
    {

        if (roomButton.isOccupied)
        {

            if (!squadRoomDisplayer.gameObject.activeSelf)
                squadRoomDisplayer.gameObject.SetActive(true);
            //else
            //{
            //    //if it IS on
            //    squadRoomDisplayer.SetMe();
            //}
            squadRoomDisplayer.SetMe(roomButton.room);
            activeRoomButton = roomButton;
        }
        else
        {
            TryOpenNewSquadMenu(roomButton.room);
        }
    }

    public void EditActiveSquad()
    {
        activeRoomButton.EditMe();
    }

    //SUPER TEMP
    void TryPromptNewHireling()
    {
        //SHOULD HAAPEN EVERY X AND AGAIN
        HirelingMaster.Instance.PromptNewHireling();
    }
}
