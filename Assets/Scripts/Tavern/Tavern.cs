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

    //List<GameObject> roomButtons;
    List<RoomButton> _roomButtons;

    //[SerializeField]
    //Image ;
    //[SerializeField]
    //GameObject roomPanelPrefab;
    //[SerializeField]
    //GameObject roomPanelPrefab;

    List<RoomBuildDisplayer> roomDisplayers;


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

    //private void OnEnable()
    //{
    //    RefreshRooms();
    //}

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
            GameObject go = Instantiate(roomPanelPrefab, roomButtonParent);
            _roomButtons.Add(go.GetComponent<RoomButton>());
            //roomButtons.Add(go);
            _roomButtons[i].SetMe(PlayerDataMaster.Instance.currentPlayerData.rooms[i]);
        }
    }

    // public void ReadSaveData()
    // Takes the available mercs from current save data
    public void TryOpenNewSquadMenu()
    {
        if(PartyMaster.Instance.squads.Count == PlayerDataMaster.Instance.currentPlayerData.rooms.Count)
        {
            noMoreRoomsWindow.SetActive(true);
            return;
        }

        newSquadMenu.SetActive(true);
    }

    public void EditSquadMenu(Squad s)
    {
        //disassembles a squad
        PartyMaster.Instance.squads.Remove(s);
        //opens and sets up the newSquadWindow with the members of that squad in the PartySlots 
        newSquadMenu.SetActive(true);
        newSquadMenu.GetComponent<SquadBuilder>().EditSquadMode(s.pawns);
    }


}
