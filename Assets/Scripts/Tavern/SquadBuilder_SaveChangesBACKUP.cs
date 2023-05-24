//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;

//public class SquadBuilder_SaveChangesBACKUP : MonoBehaviour
//{
//    [SerializeField]
//    TMPro.TMP_Text crewName;
//    [SerializeField]
//    RosterSlot[] availableSlots;
//    [SerializeField]
//    Transform partySlotsParent;
//    [SerializeField]
//    RosterSlot[] partySlots; //limited by Room_Level (int) - from avishy's room logic

//    public RosterSlot[] PartySlots { get => partySlots; }

//    public Squad tempSquad;
//    Room toRoom;
//    public Room ToRoom { get => toRoom; }
//    [SerializeField]
//    bool isEdit = false;
//    bool isConfirmed = false;

//    public MercDataDisplayer mercDataDisplayer;

//    //[SerializeField]
//    //TogglePopout myButton;

//    [SerializeField]
//    List<Pawn> uneditedSquadPawns;

//    [SerializeField]
//    GameObject confirmWindow;
//    [SerializeField]
//    GameObject cycleConfirmWindow;

//    bool? confirmWindowAnswer;
//    bool? confirmCycleAnswer;

//    //temp AF TBF 
//    int _currentRoomIndex;
//    int _roomCount => Tavern.Instance.RoomCount;


//    private void OnEnable()
//    {
//        Init();
//    }
//    //private void Awake()
//    //{
//    //    Init();
//    //}

//    private void Init()
//    {
//        Tavern.Instance.DisableWindowTier1(name);
//        confirmWindowAnswer = null;
//        confirmCycleAnswer = null;
//        //isConfirmed = false; //no?
//        tempSquad = new Squad();

//        _currentRoomIndex = 0; //temp - If not room buttons exit - this makes sure to open the squad builder displaying the first crew

//        //if (toRoom == null)
//        //toRoom = Tavern.Instance.GetRoomByIndex(_currentRoomIndex);
//        BetterSetToRoom(Tavern.Instance.GetRoomByIndex(_currentRoomIndex));


//        List<MercName> names = PlayerDataMaster.Instance.GetMercNamesByAssignment(MercAssignment.Available);

//        for (int i = 0; i < names.Count; i++)
//        {
//            availableSlots[i].SetMe(MercPrefabs.Instance.EnumToPawnPrefab(names[i]));
//        }

//        //if (availableSlots[0].isOccupied)
//        //{
//        //    mercDataDisplayer.SetMe(availableSlots[0].pawn);
//        //}

//        //for (int i = PartyMaster.Instance.availableMercPrefabs.Count; i < availableSlots.Length; i++)
//        for (int i = names.Count; i < availableSlots.Length; i++)
//        {
//            availableSlots[i].SetMe(); //empty
//        }

//        for (int i = 0; i < tempSquad.pawns.Count; i++)
//        {
//            partySlots[i].SetMe(tempSquad.pawns[i]);
//        }
//        for (int i = tempSquad.pawns.Count; i < partySlots.Length; i++)
//        {
//            partySlots[i].ClearSlot(); //empty
//        }


//        //toRoom = new Room();
//        //myButton.Toggle(true);
//    }

//    public void SetConfirmDecision(bool decision)//called by buttons in inspector
//    {
//        confirmWindowAnswer = decision;
//    }
//    public void SetCycleConfirmDecision(bool decision)//called by buttons in inspector
//    {
//        confirmCycleAnswer = decision;
//    }


//    public void CloseMe()
//    {
//        //check if need to confirm edits:

//        if (uneditedSquadPawns.Count == tempSquad.pawns.Count)
//        {
//            bool same = true;
//            foreach (var item in tempSquad.pawns)
//            {
//                if (!uneditedSquadPawns.Contains(item))
//                    same = false;
//            }

//            if (same)
//                confirmWindowAnswer = false; //stops the wait for answer
//            else
//                confirmWindow.gameObject.SetActive(true);
//        }
//        else
//        {
//            confirmWindow.gameObject.SetActive(true); //turns on buttons that would decide true or false
//        }
//        //check confirm
//        StartCoroutine(nameof(WaitForConfirmDecision));
//    }
//    IEnumerator WaitForConfirmDecision()
//    {
//        yield return new WaitUntil(() => confirmWindowAnswer != null || !confirmWindow.activeInHierarchy);// !confirmWindow.activeInHierarchy also, if they hit something to close it accidently? idk

//        if (confirmWindowAnswer == true)
//        {
//            //confirm
//            Confirm();
//        }
//        else
//        {
//            //even if null, don't confirm changes
//            if (isEdit)
//            {
//                //isEdit = false;//confirm cancels edit!
//                //Differences between tempSquad and uneditedSquadPawns needs to be unset:

//                //pawns that dont exist in the uneditedSquadPawns need to be returned to AvailablePawns
//                //pawns that do exist in the uneditedSquadPawns, but DONT exist in tempSquad need to be returned from AvailablePawns to tempSquad

//                //to acheive this, uneditedSquadPawns will be the squad returned - and only mercs which appear in tempSquad need to be returned to available

//                List<Pawn> backToAvailable = tempSquad.pawns.Where(x => !uneditedSquadPawns.Contains(x) && !PartyMaster.Instance.availableMercPrefabs.Contains(x)).ToList();

//                PartyMaster.Instance.availableMercPrefabs.AddRange(backToAvailable);
//                foreach (var item in backToAvailable)
//                {
//                    tempSquad.RemoveMerc(item); //to reset merc assignments
//                }
//                //PartyMaster.Instance.AddNewSquadToRoom(uneditedSquadPawns, toRoom);
//                tempSquad.pawns = uneditedSquadPawns;

//                Confirm();
//            }
//            else
//            {
//                toRoom.ClearRoom();
//                //toRoom.roomButton.SetStatusText("Vacant");
//                if (tempSquad.pawns.Count != 0) //maybe try something more wholistic like checking the tempSquad
//                {
//                    foreach (var item in tempSquad.pawns)
//                    {
//                        PartyMaster.Instance.availableMercPrefabs.Add(item);
//                        item.mercSheetInPlayerData.SetToState(MercAssignment.Available, -1);
//                    }
//                }
//                //close squad menu
//                gameObject.SetActive(false);
//            }
//        }

//        //close squad builder
//    }

//    IEnumerator CycleWaitForConfirm()
//    {
//        yield return new WaitUntil(() => confirmCycleAnswer.HasValue || !cycleConfirmWindow.activeInHierarchy);// !confirmWindow.activeInHierarchy also, if they hit something to close it accidently? idk

//        //if (confirmCycleAnswer == true)
//        //{
//        //    //confirm
//        //    CycleConfirm();
//        //}
//        if (confirmCycleAnswer == false)
//        {
//            //even if null, don't confirm changes
//            if (isEdit)
//            {
//                //isEdit = false;//confirm cancels edit!
//                //Differences between tempSquad and uneditedSquadPawns needs to be unset:

//                //pawns that dont exist in the uneditedSquadPawns need to be returned to AvailablePawns
//                //pawns that do exist in the uneditedSquadPawns, but DONT exist in tempSquad need to be returned from AvailablePawns to tempSquad

//                //to acheive this, uneditedSquadPawns will be the squad returned - and only mercs which appear in tempSquad need to be returned to available

//                List<Pawn> backToAvailable = tempSquad.pawns.Where(x => !uneditedSquadPawns.Contains(x) && !PartyMaster.Instance.availableMercPrefabs.Contains(x)).ToList();

//                PartyMaster.Instance.availableMercPrefabs.AddRange(backToAvailable);
//                foreach (var item in backToAvailable)
//                {
//                    tempSquad.RemoveMerc(item); //to reset merc assignments
//                }
//                //PartyMaster.Instance.AddNewSquadToRoom(uneditedSquadPawns, toRoom);
//                tempSquad.pawns = uneditedSquadPawns;

//                CycleConfirm();
//            }
//            else
//            {
//                //toRoom.ClearRoom();
//                Tavern.Instance.GetRoomByIndex(_currentRoomIndex).ClearRoom();
//                //toRoom.roomButton.SetStatusText("Vacant");
//                if (tempSquad.pawns.Count != 0) //maybe try something more wholistic like checking the tempSquad
//                {
//                    foreach (var item in tempSquad.pawns)
//                    {
//                        PartyMaster.Instance.availableMercPrefabs.Add(item);
//                        item.mercSheetInPlayerData.SetToState(MercAssignment.Available, -1);
//                    }
//                }
//                //close squad menu
//                //gameObject.SetActive(false);
//                cycleConfirmWindow.SetActive(false);
//            }
//        }

//        //close squad builder
//    }

//    public void OnDisable()
//    {
//        confirmWindowAnswer = null;
//        confirmWindow.SetActive(false);
//        mercDataDisplayer.gameObject.SetActive(false);

//        //Tavern.Instance.RefreshRooms(); 

//        foreach (var item in partySlots)
//        {
//            item.gameObject.SetActive(true); //turns all party slots (those will be closed on SetToRoom() before the Create/EditSquad mode is activated)
//            item.ClearSlot();
//        }
//        //if(myButton)
//        //myButton.Toggle(false);
//    }
//    public void SetToRoom(Room r) /// this is the problem, fix the room setting issue
//    {
//        toRoom = r;

//        //set partySlots by toRoom.size
//        for (int i = toRoom.size; i < partySlots.Length; i++)
//        {
//            partySlots[i].gameObject.SetActive(false);
//        }
//    }
//    public void BetterSetToRoom(Room r) /// this is the problem, fix the room setting issue
//    {
//        toRoom = r;
//        //temp af TBF
//        //if (r.squad == null || r.squad.pawns.Count ==0)
//        //return;
//        tempSquad = r.squad;
//        isEdit = false; //Will be set true in EditSquadMode if relevant

//        if (tempSquad != null && tempSquad.pawns.Count > 0)
//        {
//            EditSquadMode(tempSquad.pawns, r);
//        }

//        if (tempSquad != null && tempSquad.pawns.Count > 0)
//        {
//            //set partySlots by toRoom.size
//            for (int i = 0; i < r.squad.pawns.Count; i++)
//            {
//                partySlots[i].SetMe(r.squad.pawns[i]);
//            }
//            for (int i = r.squad.pawns.Count; i < partySlots.Length; i++)
//            {
//                partySlots[i].SetMe();
//            }

//            for (int i = toRoom.size; i < partySlots.Length; i++)
//            {
//                partySlots[i].gameObject.SetActive(false);
//            }
//        }
//        Refresh();
//    }

//    public void Confirm() //also called in inspector by the Assemble Squad buttons
//    {
//        //isEdit = false; //just making sure that we won't double confirm
//        isConfirmed = true;
//        //PartyMaster.Instance.squads.Add(new Squad(tempSquad.pawns)); //to avoid referencing the tempSquad, which will be cleared soon after this.
//        if (tempSquad.pawns.Count > 0)
//            PartyMaster.Instance.AddNewSquadToRoom(tempSquad.pawns, toRoom); //this needs to change to (tempSquad.pawns, tempSquad.roomNumber)
//        //toRoom.squad = PartyMaster.Instance.squads[PartyMaster.Instance.squads.Count];

//        PlayerDataMaster.Instance.GrabAndSaveData();

//        foreach (var item in partySlots)
//        {
//            item.ClearSlot();
//        }
//        foreach (var item in availableSlots)
//        {
//            item.ClearSlot();
//        }

//        gameObject.SetActive(false); //beacuse confirm is also called by the button, not only thorugh the "confirm?" window
//    }
//    public void CycleConfirm() //also called in inspector by the Assemble Squad buttons
//    {
//        //isEdit = false; //just making sure that we won't double confirm
//        //isConfirmed = true;
//        //PartyMaster.Instance.squads.Add(new Squad(tempSquad.pawns)); //to avoid referencing the tempSquad, which will be cleared soon after this.
//        if (tempSquad.pawns.Count > 0)
//            PartyMaster.Instance.AddNewSquadToRoom(tempSquad.pawns, Tavern.Instance.GetRoomByIndex(_currentRoomIndex)); //this needs to change to (tempSquad.pawns, tempSquad.roomNumber)
//        //toRoom.squad = PartyMaster.Instance.squads[PartyMaster.Instance.squads.Count];

//        PlayerDataMaster.Instance.GrabAndSaveData();

//        foreach (var item in partySlots)
//        {
//            item.ClearSlot();
//        }
//        foreach (var item in availableSlots)
//        {
//            item.ClearSlot();
//        }
//        cycleConfirmWindow.SetActive(false);
//        confirmCycleAnswer = null;
//        //gameObject.SetActive(false); //beacuse confirm is also called by the button, not only thorugh the "confirm?" window
//    }

//    public void Refresh()
//    {
//        List<MercName> names = PlayerDataMaster.Instance.GetMercNamesByAssignment(MercAssignment.Available);
//        if (Tavern.Instance.GetRoomByIndex(_currentRoomIndex).squad != null || Tavern.Instance.GetRoomByIndex(_currentRoomIndex).squad.pawns.Count > 0)
//            crewName.text = Tavern.Instance.GetRoomByIndex(_currentRoomIndex).squad.squadName;
//        else
//            crewName.text = $"Crew {_currentRoomIndex + 1}";


//        for (int i = 0; i < names.Count; i++)
//        {
//            availableSlots[i].SetMe(MercPrefabs.Instance.EnumToPawnPrefab(names[i]));
//        }

//        if (availableSlots[0].isOccupied)
//        {
//            mercDataDisplayer.SetMe(availableSlots[0].pawn);
//        }

//        //for (int i = PartyMaster.Instance.availableMercPrefabs.Count; i < availableSlots.Length; i++)
//        for (int i = names.Count; i < availableSlots.Length; i++)
//        {
//            availableSlots[i].SetMe(); //empty
//        }

//        for (int i = 0; i < tempSquad.pawns.Count; i++)
//        {
//            partySlots[i].SetMe(tempSquad.pawns[i]);
//        }
//        for (int i = tempSquad.pawns.Count; i < partySlots.Length; i++)
//        {
//            partySlots[i].ClearSlot(); //empty
//        }
//    }
//    public void EditSquadMode(List<Pawn> oldSquad, Room room)
//    {
//        //for (int i = 0; i < oldSquad.Count; i++)
//        //{
//        //    partySlots[i].SetMe(oldSquad[i]);
//        //}
//        tempSquad.pawns = oldSquad;
//        uneditedSquadPawns = new List<Pawn>(oldSquad);
//        isEdit = true;

//        SetToRoom(room);

//        Refresh();
//    }

//    public void TurnAllFramesOff()
//    {
//        foreach (var item in partySlots)
//        {
//            item.FrameToggle(false);
//        }
//        foreach (var item in availableSlots)
//        {
//            item.FrameToggle(false);
//        }
//    }

//    public void CycleCrews(int i)
//    {
//        StartCoroutine(CycleCrewCoroutine(i));

//    }

//    private IEnumerator CycleCrewCoroutine(int i)
//    {
//        Room currentRoom = Tavern.Instance.GetRoomByIndex(_currentRoomIndex);
//        if (currentRoom.squad != null && currentRoom.squad.pawns.Count > 0) //TBF, this is how we check if is occupied
//        {
//            if (uneditedSquadPawns.Count == tempSquad.pawns.Count)
//            {
//                bool same = true;
//                foreach (var item in tempSquad.pawns)
//                {
//                    if (!uneditedSquadPawns.Contains(item))
//                        same = false;
//                }

//                if (same)
//                    confirmCycleAnswer = false; //stops the wait for answer
//                else
//                    cycleConfirmWindow.gameObject.SetActive(true);
//            }
//            else
//            {
//                cycleConfirmWindow.gameObject.SetActive(true); //turns on buttons that would decide true or false
//            }

//            yield return StartCoroutine(nameof(CycleWaitForConfirm));

//            if (confirmCycleAnswer.HasValue && confirmCycleAnswer.Value == true)
//            {
//                CycleConfirm();
//            }
//        }

//        _currentRoomIndex += i;
//        if (_currentRoomIndex < 0)
//        {
//            _currentRoomIndex = _roomCount - 1;
//        }
//        if (_currentRoomIndex >= _roomCount)
//        {
//            _currentRoomIndex = 0;
//        }
//        Room r = Tavern.Instance.GetRoomByIndex(_currentRoomIndex);
//        BetterSetToRoom(r);

//        Refresh();
//    }
//}