using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SquadBuilder : MonoBehaviour
{
    [SerializeField]
    RosterSlot[] availableSlots;
    [SerializeField]
    Transform partySlotsParent;
    [SerializeField]
    RosterSlot[] partySlots; //limited by Room_Level (int) - from avishy's room logic

    public RosterSlot[] PartySlots { get => partySlots;}

    public Squad tempSquad;
    Room toRoom;
    public Room ToRoom { get => toRoom; }
    [SerializeField]
    bool isEdit = false;
    bool isConfirmed = false;
    
    public MercDataDisplayer mercDataDisplayer;

    [SerializeField]
    TogglePopout myButton;

    [SerializeField]
    List<Pawn> uneditedSquadPawns;
    //private void Awake()
    //{
    //    //gameObject.SetActive
    //}
    private void OnEnable()
    {
        Tavern.Instance.DisableWindowTier1(name);

        tempSquad = new Squad();
        mercDataDisplayer.gameObject.SetActive(true);

        //set/instantiate empty party-slots by Room_level

        //print all availables:
        for (int i = 0; i < PartyMaster.Instance.availableMercPrefabs.Count; i++)
        {
            availableSlots[i].SetMe(PartyMaster.Instance.availableMercPrefabs[i]); //could also go with mercSheets.Where(x => x.assignment == MercAssignment.Available)
        }
        if(availableSlots[0].isOccupied)
        {
            mercDataDisplayer.SetMe(availableSlots[0].pawn);
        }
        else
        {
            Debug.LogError("You have no mercs... or at least, you don't have a FIRST merc to show... which is just weird");
        }
        for (int i = PartyMaster.Instance.availableMercPrefabs.Count; i < availableSlots.Length; i++)
        {
            availableSlots[i].SetMe(); //empty
        }

        for (int i = 0; i < tempSquad.pawns.Count; i++)
        {
            partySlots[i].SetMe(tempSquad.pawns[i]);
        }
        for (int i = tempSquad.pawns.Count; i < partySlots.Length; i++)
        {
            partySlots[i].ClearSlot(); //empty
        }
        myButton.Toggle(true);

    }
    public void OnDisable()
    {

        //if(isEdit)
        //{
        //    isEdit = false;
        //    if (partySlots.Any(x => x.isOccupied))
        //    {
        //        Confirm();
        //    }
        //    else
        //    {
        //        toRoom.SetStatusText("Vacant");
        //    }
        //}
        //if (partySlots.Any(x => x.isOccupied)) //maybe try something more wholistic like checking the tempSquad
        if (!isConfirmed)
        {

            if (isEdit)
            {
                //isEdit = false;//confirm cancels edit!
                //Differences between tempSquad and uneditedSquadPawns needs to be unset:

                //pawns that dont exist in the uneditedSquadPawns need to be returned to AvailablePawns
                //pawns that do exist in the uneditedSquadPawns, but DONT exist in tempSquad need to be returned from AvailablePawns to tempSquad

                //to acheive this, uneditedSquadPawns will be the squad returned - and only mercs which appear in tempSquad need to be returned to available

                List<Pawn> backToAvailable = tempSquad.pawns.Where(x => !uneditedSquadPawns.Contains(x) && !PartyMaster.Instance.availableMercPrefabs.Contains(x)).ToList();

                PartyMaster.Instance.availableMercPrefabs.AddRange(backToAvailable);

                //PartyMaster.Instance.AddNewSquadToRoom(uneditedSquadPawns, toRoom);
                tempSquad.pawns = uneditedSquadPawns;

                Confirm();
            }
            else 
            {
            toRoom.roomButton.SetStatusText("Vacant");
                if (tempSquad.pawns.Count != 0) //maybe try something more wholistic like checking the tempSquad
                {
                    foreach (var item in tempSquad.pawns)
                    {
                        PartyMaster.Instance.availableMercPrefabs.Add(item);
                    }
                }
            }
        }
        else
        {
            if(isEdit)
            {

            }
            isConfirmed = false;
        }
        
                //tempSquad = null;
        //    else 
        //{
        //    foreach (var p in tempSquad.pawns)
        //    {
        //        PartyMaster.Instance.availableMercs.Add(p);
        //    }
        //}
            
        

        mercDataDisplayer.gameObject.SetActive(false);
        foreach (var item in partySlots)
        {
            item.gameObject.SetActive(true); //turns all party slots (those will be closed on SetToRoom() before the Create/EditSquad mode is activated)
            item.ClearSlot();
        }
        if(myButton)
        myButton.Toggle(false);
    }
    public void SetToRoom(Room r) /// this is the problem, fix the room setting issue
    {
        toRoom = r;

        //set partySlots by toRoom.size
        for (int i = toRoom.size; i < partySlots.Length; i++)
        {
                partySlots[i].gameObject.SetActive(false);
        }
    }

    public void Confirm()
    {
        //isEdit = false; //just making sure that we won't double confirm
        isConfirmed = true;
        //PartyMaster.Instance.squads.Add(new Squad(tempSquad.pawns)); //to avoid referencing the tempSquad, which will be cleared soon after this.
        if (tempSquad.pawns.Count > 0)
        PartyMaster.Instance.AddNewSquadToRoom(tempSquad.pawns, toRoom); //this needs to change to (tempSquad.pawns, tempSquad.roomNumber)
        //toRoom.squad = PartyMaster.Instance.squads[PartyMaster.Instance.squads.Count];

        PlayerDataMaster.Instance.GrabAndSaveData();

        foreach (var item in partySlots)
        {
            item.ClearSlot();
        }
        foreach (var item in availableSlots)
        {
            item.ClearSlot();
        }

        Tavern.Instance.RefreshRooms();


        gameObject.SetActive(false);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("OverlandMapScene");
    }
    public void SetMercDisplayer(Pawn merc)
    {
        mercDataDisplayer.SetMe(merc);
    }
    public void Refresh()
    {
        for (int i = 0; i < PartyMaster.Instance.availableMercPrefabs.Count; i++)
        {
            availableSlots[i].SetMe(PartyMaster.Instance.availableMercPrefabs[i]);
        }
        for (int i = PartyMaster.Instance.availableMercPrefabs.Count; i < availableSlots.Length; i++)
        {
            availableSlots[i].ClearSlot();
        }

        for (int i = 0; i < tempSquad.pawns.Count; i++)
        {
            partySlots[i].SetMe(tempSquad.pawns[i]);
        }
        for (int i = tempSquad.pawns.Count; i < partySlots.Length; i++)
        {
            partySlots[i].ClearSlot();
        }
    }
    public void EditSquadMode(List<Pawn> oldSquad, Room room)
    {
        //for (int i = 0; i < oldSquad.Count; i++)
        //{
        //    partySlots[i].SetMe(oldSquad[i]);
        //}
        tempSquad.pawns = oldSquad;
        uneditedSquadPawns = new List<Pawn>(oldSquad);
        isEdit = true;

        SetToRoom(room);

        Refresh();
    }

    public void TurnAllOff()
    {
        foreach (var item in partySlots)
        {
            item.FrameToggle(false);
        }
        foreach (var item in availableSlots)
        {
            item.FrameToggle(false);
        }
    }
}