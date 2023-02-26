using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inits the TurnBelt, (?) and feeds the belt to the TurnMachine (?)
/// The ONLY class that can edit the TurnBelt.
/// Provides access to the TurnBelt if needed.
/// </summary>
public class BeltManipulator : MonoBehaviour
{
    public static BeltManipulator Instance;

    [SerializeField]
    TurnBelt turnBelt;

    int _currentIndex;

    [SerializeField]
    HorizontalPlateGroup horizontalPlateGroup;

    //public HorizontalPlateGroup GetHorizontalPlateGroup { get => horizontalPlateGroup; }
    //public System.Action OnBeltChange;

    //public System.Action PreTurn;
    //public System.Action PostTurn;
   
    private void Awake()
    {
        if(Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Receives TurnTakers and creates TurnInfos for them.
    /// Rolls their initiative (simple 1d20 for now)
    /// Sorts them in initiative order
    /// And sets the _current counter to 0.
    /// </summary>
    /// <param name="allTakers"></param>
    public void InitManipulator(List<TurnTaker> allTakers)
    {
        //roll and sort
        foreach (TurnTaker tt in allTakers)
        {
            tt.Initiative = RollDx(20);
        }
        allTakers.Sort(SortByInitiativeScore);

        //Generate TurnInfo for each turnTaker -> don't forget to place start-pin at the 0 position
        List<TurnInfo> turnInfos = new List<TurnInfo>();

        //Adding Start-Pin before all turnTakers
        turnInfos.Add(new TurnInfo());

        foreach (var item in allTakers)
        {
            TurnInfo ti = new TurnInfo(item);
            turnInfos.Add(ti);
            //item.TurnInfo = ti; //kind of annoying they know eachother TBF
        }

        turnBelt.InitBelt(turnInfos);

        horizontalPlateGroup.Init(turnInfos);

        _currentIndex = 0;
    }

    public TurnInfo NextTurn()
    {
        _currentIndex++;

        //loop back after max
        if (_currentIndex >= turnBelt.infoCount)
            _currentIndex = 0;

        //OnBeltChange?.Invoke(_currentIndex);
        horizontalPlateGroup.RefreshPortraits(_currentIndex);
        return turnBelt.GetTurnInfo(_currentIndex);
    }
    //public void InsertTurnInfo(TurnInfo ti, int index)
    //{
    //    turnBelt.InsertTurnInfo(ti, index);
    //}
    public void InsertTurnTaker(TurnTaker tt, int index)
    {
        TurnInfo ti = new TurnInfo(tt);
        tt.TurnInfo = ti;
        if(index < _currentIndex)
        {
            _currentIndex++;
        }
        turnBelt.InsertTurnInfo(ti, index);
        //horizontalPlateGroup.AddChildByTurnInfo(ti);
        //horizontalPlateGroup.RefreshPortraits(_currentIndex);

    }
    public void InsertTurnTakerAsNext(TurnTaker tt)
    {
        //TurnInfo ti = new TurnInfo(tt);
        int index = (_currentIndex + 1 >= turnBelt.infoCount) ? 0 : _currentIndex+1;


        InsertTurnTaker(tt, index);

        //turnBelt.InsertTurnTaker(tt, index);
        //horizontalPlateGroup.RefreshPortraits(_currentIndex);

    }

    //add turn info??
    public void InsertTurnInfo(TurnInfo ti, int index)
    {
        if (index < _currentIndex)
        {
            _currentIndex++;
        }

        turnBelt.InsertTurnInfo(ti, index);

        //horizontalPlateGroup.AddChildByTurnInfo(ti);
        //horizontalPlateGroup.AddChild();
        horizontalPlateGroup.RefreshPortraits(_currentIndex);
    }
    public void MoveTurnInfoTo(TurnInfo ti, int index)
    {
        turnBelt.MoveTurnInfo(ti, index);
        horizontalPlateGroup.RefreshPortraits(_currentIndex);
    }
    public void MoveTurnInfoToBeNext(TurnInfo ti) //REALLY specific, but it's a cleaner and it uses MoveTurnInfoTo so keep it
    {
        int next = (_currentIndex+1 == turnBelt.infoCount) ? 0 : _currentIndex + 1;
        MoveTurnInfoTo(ti, next);
    }
    /// <summary>
    /// Call after killing the Pawn. Use with GetTurnInfoByTaker if you must
    /// </summary>
    /// <param name="ti"></param>
    public void RemoveTurnInfo(TurnInfo ti) //REALLY specific, but it's a cleaner and it uses MoveTurnInfoTo so keep it
    {

        int index = GetIndexOfTurnInfo(ti);

        turnBelt.RemoveTurnInfo(ti);
        if(index < _currentIndex) //in the case of a turntakers death AFTER their turn in this round - casuing indecies to be shifted down by 1 (as opposed to turntakers who die BEFORE their turn came this round)
        {
            _currentIndex--;
        }
        
        horizontalPlateGroup.RefreshPortraits(_currentIndex);
    }
    public void RemoveALLInfoByTaker(TurnTaker tt) //REALLY specific, but it's a cleaner and it uses MoveTurnInfoTo so keep it
    {

        int index = GetIndexOfTurnInfo(tt.TurnInfo);

        turnBelt.RemoveALLInfoByTaker(tt);
        if(index < _currentIndex) //in the case of a turntakers death AFTER their turn in this round - casuing indecies to be shifted down by 1 (as opposed to turntakers who die BEFORE their turn came this round)
        {
            _currentIndex--;
        }
        
        horizontalPlateGroup.RefreshPortraits(_currentIndex);
    }

    public TurnInfo GetTurnInfoByTaker(TurnTaker tt)
    {
        return turnBelt.GetTurnInfoByPredicate(x => x.GetTurnTaker == tt);
    }
     public List<TurnInfo> GetTurnInfos()
    {
        return turnBelt.GetAllTurnInfos();
    }
     
    int RollDx(int x)
    {
        return Random.Range(1, x + 1);
    }
    int SortByInitiativeScore(TurnTaker a, TurnTaker b)
    {
        return -a.Initiative.CompareTo(b.Initiative);
    }

    public int GetIndexOfTurnInfo(TurnInfo ti)
    {
        return turnBelt.GetAllTurnInfos().IndexOf(ti);
    }

    public TurnTaker GetCurrentTurnTaker()
    {
        return turnBelt.GetTurnInfo(_currentIndex).GetTurnTaker;
    }

    public void SetPortraitColour(TurnInfo ti, Color col)
    {
        DisplayPlate dp = horizontalPlateGroup.GetChildren.Where(x => x.turnInfo == ti).FirstOrDefault();

        dp.SetPortraitOverlayColour(col);
    }


    
}

