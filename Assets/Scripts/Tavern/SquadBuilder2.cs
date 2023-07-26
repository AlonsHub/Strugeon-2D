using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SquadBuilder2 : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text crewName;
    [SerializeField]
    List<RosterSlot> availableSlots;
    [SerializeField]
    Transform partySlotsParent;
    [SerializeField]
    RosterSlot[] partySlots; //limited by Room_Level (int) - from avishy's room logic

    public RosterSlot[] PartySlots { get => partySlots;}

    public Squad tempSquad => ToRoom.squad;
    Room toRoom;
    public Room ToRoom { get => toRoom; }
    [SerializeField]
    bool isEdit = false;
    bool isConfirmed = false;
    
    public MercDataDisplayer mercDataDisplayer;

    //[SerializeField]
    //TogglePopout myButton;

    [SerializeField]
    List<Pawn> uneditedSquadPawns;
    [SerializeField]
    TMPro.TMP_Text upgradePriceText;

    //[SerializeField]
    //GameObject confirmWindow;
    //[SerializeField]
    //GameObject cycleConfirmWindow;

    bool? confirmWindowAnswer;
    bool? confirmCycleAnswer;

    //temp AF TBF 
    int _currentRoomIndex = 0;
    int _roomCount => Tavern.Instance.RoomCount;

    public static System.Action OnAnyCrewChanges;

    
    private void OnEnable()
    {
        Init();
    }
    //private void Awake()
    //{
    //    Init();
    //}

    private void Init()
    {
        //Tavern.Instance.DisableWindowTier1(name);
        //confirmWindowAnswer = null;
        //confirmCycleAnswer = null;
        //isConfirmed = false; //no?
        //tempSquad = new Squad();

        //_currentRoomIndex = 0; //temp - If not room buttons exit - this makes sure to open the squad builder displaying the first crew

        //if (toRoom == null)
            //toRoom = Tavern.Instance.GetRoomByIndex(_currentRoomIndex);
            //if((toRoom = Tavern.Instance.GetRoomByIndex(_currentRoomIndex)) != null)
            //BetterSetToRoom(toRoom);
        BetterSetToRoom(Tavern.Instance.GetRoomByIndex(_currentRoomIndex));


        List<MercName> names = PlayerDataMaster.Instance.GetMercNamesByAssignment(MercAssignment.Available);
        //List<MercSheet> _sheets = PlayerDataMaster.Instance.GetMercSheetsByAssignment(MercAssignment.Available);
        //List<MercSheet> _sheets = PlayerDataMaster.Instance.GetMercSheetsByAssignments(new List<MercAssignment>{ MercAssignment.Available });

        for (int i = 0; i < names.Count; i++)
        {
            availableSlots[i].gameObject.SetActive(true);
            //availableSlots[i].SetMe(MercPrefabs.Instance.EnumToPawnPrefab(_sheets[i]));
            availableSlots[i].SetMe(PlayerDataMaster.Instance.GetMercSheetByName(names[i]));
        }
        //for (int i = _sheets.Count; i < availableSlots.Count; i++)
        //{
        //    availableSlots[i].gameObject.SetActive(false);
        //}
        for (int i = names.Count; i < availableSlots.Count; i++)
        {
            availableSlots[i].SetMe(); //empty
            availableSlots[i].gameObject.SetActive(false);
        }

        

        for (int i = 0; i < ToRoom.squad.pawns.Count; i++)
        {
            partySlots[i].SetMe(ToRoom.squad.pawns[i]);
        }
        for (int i = ToRoom.squad.pawns.Count; i < partySlots.Length; i++)
        {
            partySlots[i].ClearSlot(); //empty
        }


        //toRoom = new Room();
        //myButton.Toggle(true);
    }

    //public void SetConfirmDecision(bool decision)//called by buttons in inspector
    //{
    //    confirmWindowAnswer = decision;
    //}
    //public void SetCycleConfirmDecision(bool decision)//called by buttons in inspector
    //{
    //    confirmCycleAnswer = decision;
    //}


    ////A Special CloseMe() that WAITS FOR CONFIRMATION
    //public void CloseMe()
    //{
    //    //check if need to confirm edits:

    //    if (uneditedSquadPawns.Count == tempSquad.pawns.Count)
    //    {
    //        bool same = true;
    //        foreach (var item in tempSquad.pawns)
    //        {
    //            if (!uneditedSquadPawns.Contains(item))
    //                same = false;
    //        }

    //        if (same)
    //            confirmWindowAnswer = false; //stops the wait for answer
    //        else
    //            confirmWindow.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        confirmWindow.gameObject.SetActive(true); //turns on buttons that would decide true or false
    //    }
    //    //check confirm
    //    StartCoroutine(nameof(WaitForConfirmDecision));
    //}
    //A Special CloseMe() that WAITS FOR CONFIRMATION
    public void CloseMe()
    {
        ConfirmClose();
    }


    //IEnumerator WaitForConfirmDecision()
    //{
    //    yield return new WaitUntil(() => confirmWindowAnswer != null || !confirmWindow.activeInHierarchy);// !confirmWindow.activeInHierarchy also, if they hit something to close it accidently? idk

    //    if(confirmWindowAnswer == true)
    //    {
    //        //confirm
    //        ConfirmClose();
    //    }
    //    else
    //    {
    //        //even if null, don't confirm changes
    //        if (isEdit)
    //        {
    //            //isEdit = false;//confirm cancels edit!
    //            //Differences between tempSquad and uneditedSquadPawns needs to be unset:

    //            //pawns that dont exist in the uneditedSquadPawns need to be returned to AvailablePawns
    //            //pawns that do exist in the uneditedSquadPawns, but DONT exist in tempSquad need to be returned from AvailablePawns to tempSquad

    //            //to acheive this, uneditedSquadPawns will be the squad returned - and only mercs which appear in tempSquad need to be returned to available

    //            List<Pawn> backToAvailable = tempSquad.pawns.Where(x => !uneditedSquadPawns.Contains(x) && !PartyMaster.Instance.availableMercPrefabs.Contains(x)).ToList();

    //            PartyMaster.Instance.availableMercPrefabs.AddRange(backToAvailable);
    //            foreach (var item in backToAvailable)
    //            {
    //                tempSquad.RemoveMerc(item); //to reset merc assignments
    //            }
    //            //PartyMaster.Instance.AddNewSquadToRoom(uneditedSquadPawns, toRoom);
    //            tempSquad.pawns = uneditedSquadPawns;

    //            ConfirmClose(); 
    //        }
    //        else
    //        {
    //            toRoom.ClearRoom();
    //            //toRoom.roomButton.SetStatusText("Vacant");
    //            if (tempSquad.pawns.Count != 0) //maybe try something more wholistic like checking the tempSquad
    //            {
    //                foreach (var item in tempSquad.pawns)
    //                {
    //                    PartyMaster.Instance.availableMercPrefabs.Add(item);
    //                    item.mercSheetInPlayerData.SetToState(MercAssignment.Available, -1);
    //                }
    //            }
    //            //close squad menu
    //            gameObject.SetActive(false);
    //        }
    //    }

    //    //close squad builder
    //}

    //IEnumerator CycleWaitForConfirm()
    //{
    //    yield return new WaitUntil(() => confirmCycleAnswer.HasValue || !cycleConfirmWindow.activeInHierarchy);// !confirmWindow.activeInHierarchy also, if they hit something to close it accidently? idk

    //    //if (confirmCycleAnswer == true)
    //    //{
    //    //    //confirm
    //    //    CycleConfirm();
    //    //}
    //    if (confirmCycleAnswer == false)
    //    {
    //        //even if null, don't confirm changes
    //        if (isEdit)
    //        {
    //            //isEdit = false;//confirm cancels edit!
    //            //Differences between tempSquad and uneditedSquadPawns needs to be unset:

    //            //pawns that dont exist in the uneditedSquadPawns need to be returned to AvailablePawns
    //            //pawns that do exist in the uneditedSquadPawns, but DONT exist in tempSquad need to be returned from AvailablePawns to tempSquad

    //            //to acheive this, uneditedSquadPawns will be the squad returned - and only mercs which appear in tempSquad need to be returned to available

    //            List<Pawn> backToAvailable = tempSquad.pawns.Where(x => !uneditedSquadPawns.Contains(x) && !PartyMaster.Instance.availableMercPrefabs.Contains(x)).ToList();

    //            PartyMaster.Instance.availableMercPrefabs.AddRange(backToAvailable);
    //            foreach (var item in backToAvailable)
    //            {
    //                tempSquad.RemoveMerc(item); //to reset merc assignments
    //            }
    //            //PartyMaster.Instance.AddNewSquadToRoom(uneditedSquadPawns, toRoom);
    //            tempSquad.pawns = uneditedSquadPawns;

    //            CycleConfirm();
    //        }
    //        else
    //        {
    //            //toRoom.ClearRoom();
    //            Tavern.Instance.GetRoomByIndex(_currentRoomIndex).ClearRoom();
    //            //toRoom.roomButton.SetStatusText("Vacant");
    //            if (tempSquad.pawns.Count != 0) //maybe try something more wholistic like checking the tempSquad
    //            {
    //                foreach (var item in tempSquad.pawns)
    //                {
    //                    PartyMaster.Instance.availableMercPrefabs.Add(item);
    //                    item.mercSheetInPlayerData.SetToState(MercAssignment.Available, -1);
    //                }
    //            }
    //            //close squad menu
    //            //gameObject.SetActive(false);
    //            cycleConfirmWindow.SetActive(false);
    //        }
    //    }

    //    //close squad builder
    //}

    public void OnDisable()
    {
        //confirmWindowAnswer = null;
        //confirmWindow.SetActive(false);
        //mercDataDisplayer.gameObject.SetActive(false);

        //Tavern.Instance.RefreshRooms();

        foreach (var item in partySlots)
        {
            item.gameObject.SetActive(true); //turns all party slots (those will be closed on SetToRoom() before the Create/EditSquad mode is activated)
            item.ClearSlot();
        }
        //if(myButton)
        //myButton.Toggle(false);

        //SAVE?

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
    public void BetterSetToRoom(Room r) /// this is the problem, fix the room setting issue
    {
        gameObject.SetActive(true); //for when called by CrewBlock
        toRoom = r;
        _currentRoomIndex = r.roomNumber;
        
        if (r.squad != null && r.squad.pawns.Count > 0)
        {
            //set partySlots by toRoom.size
            for (int i = 0; i < r.squad.pawns.Count; i++)
            {
                //partySlots[i].SetMe(r.squad.pawns[i]);
                partySlots[i].SetMe(r.squad.pawns[i].mercSheetInPlayerData);
            }
            for (int i = r.squad.pawns.Count; i < partySlots.Length; i++)
            {
                partySlots[i].SetMe();
            }

            for (int i = toRoom.size; i < partySlots.Length; i++)
            {
                partySlots[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < partySlots.Length; i++)
            {
                partySlots[i].SetMe();
            }
            for (int i = toRoom.size; i < partySlots.Length; i++)
            {
                partySlots[i].gameObject.SetActive(false);
            }
        }

        Refresh();
    }

    public void ConfirmClose() //also called in inspector by the Assemble Squad buttons
    {
       
        //PlayerDataMaster.Instance.GrabAndSaveData();

        foreach (var item in partySlots)
        {
            item.ClearSlot();
        }
        foreach (var item in availableSlots)
        {
            item.ClearSlot();
        }

        gameObject.SetActive(false); //beacuse confirm is also called by the button, not only thorugh the "confirm?" window
    }
   public void CycleConfirm() //also called in inspector by the Assemble Squad buttons
    {
     

        PlayerDataMaster.Instance.GrabAndSaveData();

        foreach (var item in partySlots)
        {
            item.ClearSlot();
        }
        foreach (var item in availableSlots)
        {
            item.ClearSlot();
        }
      
    }

    public void Refresh()
    {

        //Room ToRoom = Tavern.Instance.GetRoomByIndex(_currentRoomIndex);
        if(ToRoom ==null)
        {
            Debug.LogError("ToRoom is null");
            return;
        }
        if (ToRoom.squad == null || ToRoom.squad.pawns.Count == 0)
        {
            crewName.text = $"Crew {_currentRoomIndex + 1}";
            upgradePriceText.text = Prices.UpgradeCrewPriceAsText(toRoom.size);

            LoadAvailables();
            //tempSquad = new Squad();
            for (int i = 0; i < partySlots.Length; i++)
            {
                if(i<ToRoom.size)
                {
                    partySlots[i].ClearSlot();
                    partySlots[i].gameObject.SetActive(true);
                }
                else
                {
                    partySlots[i].gameObject.SetActive(false);
                }
            }

            return;
        }

        crewName.text = Tavern.Instance.GetRoomByIndex(_currentRoomIndex).squad.squadName;
        LoadAvailables();

        for (int i = 0; i < ToRoom.squad.pawns.Count; i++)
        {
            partySlots[i].SetMe(ToRoom.squad.pawns[i].mercSheetInPlayerData);
        }
        for (int i = ToRoom.squad.pawns.Count; i < partySlots.Length; i++)
        {
            partySlots[i].ClearSlot(); //empty
        }
    }
    //System.Func<MercSheet, bool> pred;

    public void SetClassFilterOnRoster(MercClass newClassToFilter)
    {
        if(classFilter.HasValue && classFilter.Value == newClassToFilter)
        {
            //turn filter off
            classFilter = null;
        }
        else
        {
            classFilter = newClassToFilter;
        }
        LoadAvailables();
    }


    MercClass? classFilter;
    private void LoadAvailables()
    {
        List<MercSheet> sheets = PlayerDataMaster.Instance.GetMercSheetsByAssignments(new List<MercAssignment>{ MercAssignment.Available});

        if(classFilter.HasValue)
        {
            sheets = sheets.Where(x => x.mercClass == classFilter.Value).ToList();
        }

        for (int i = 0; i < sheets.Count; i++)
        {
            availableSlots[i].gameObject.SetActive(true);
            
            //availableSlots[i].SetMe(sheets[i].MyPawnPrefabRef<Pawn>()); //prefer Pawn setter to MercSheet setter
            availableSlots[i].SetMe(sheets[i]); //prefer Pawn setter to MercSheet setter
        }

        for (int i = sheets.Count; i < availableSlots.Count; i++)
        {
            availableSlots[i].gameObject.SetActive(false);
        }
    }

    public void TurnAllFramesOff()
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

    public void CycleCrews(int i)
    {
        StartCoroutine(CycleCrewCoroutine(i));

    }

    private IEnumerator CycleCrewCoroutine(int i) //TBF remove all confirm stuff and make this a method
    {
        yield return null;
        
        CycleConfirm();
        

        _currentRoomIndex += i;
        if (_currentRoomIndex < 0)
        {
            _currentRoomIndex = _roomCount - 1;
        }
        if (_currentRoomIndex >= _roomCount)
        {
            _currentRoomIndex = 0;
        }
        Room r = Tavern.Instance.GetRoomByIndex(_currentRoomIndex);
        BetterSetToRoom(r);
    }
    
    public void AddMercToParty(Pawn p)
    {
        ToRoom.squad.AddMerc(p);
        OnAnyCrewChanges?.Invoke();
        Refresh();
    }
    public void RemoveMercFromParty(Pawn p)
    {
        ToRoom.squad.RemoveMerc(p);
        OnAnyCrewChanges?.Invoke();
        Refresh();
    }

    public void TryUpgradeCurrentRoom()
    {
        if(!toRoom.TryUpgrade())
        {
            Debug.LogError("Can't upgrade room!");
            return;
        }

        BetterSetToRoom(ToRoom);
        OnAnyCrewChanges?.Invoke();
        //Refresh();
    }
}