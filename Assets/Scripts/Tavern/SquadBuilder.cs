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

    bool isEdit = false;
    
    public MercDataDisplayer mercDataDisplayer;

    [SerializeField]
    TogglePopout myButton;
    //private void Awake()
    //{
    //    //gameObject.SetActive
    //}
    private void OnEnable()
    {

        tempSquad = new Squad();

        //set/instantiate empty party-slots by Room_level

        //print all availables:
        for (int i = 0; i < PartyMaster.Instance.availableMercs.Count; i++)
        {
            availableSlots[i].SetMe(PartyMaster.Instance.availableMercs[i]);
        }
        for (int i = PartyMaster.Instance.availableMercs.Count; i < availableSlots.Length; i++)
        {
            availableSlots[i].SetMe();
        }
        myButton.Toggle(true);

    }
    public void OnDisable()
    {
        if(isEdit)
        {
            isEdit = false;
            if (partySlots.Any(x => x.isOccupied))
            {
                Confirm();
            }
        }

        mercDataDisplayer.gameObject.SetActive(false);
        foreach (var item in partySlots)
        {
            item.gameObject.SetActive(true); //turns all party slots (those will be closed on SetToRoom() before the Create/EditSquad mode is activated)
            item.ClearSlot();
        }
        if(myButton)
        myButton.Toggle(false);
    }
    public void SetToRoom(Room r)
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
        //PartyMaster.Instance.squads.Add(new Squad(tempSquad.pawns)); //to avoid referencing the tempSquad, which will be cleared soon after this.

        if(tempSquad.pawns.Count > 0)
        PartyMaster.Instance.AddNewSquadToRoom(tempSquad.pawns, toRoom);
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
        for (int i = 0; i < PartyMaster.Instance.availableMercs.Count; i++)
        {
            availableSlots[i].SetMe(PartyMaster.Instance.availableMercs[i]);
        }
        for (int i = PartyMaster.Instance.availableMercs.Count; i < availableSlots.Length; i++)
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