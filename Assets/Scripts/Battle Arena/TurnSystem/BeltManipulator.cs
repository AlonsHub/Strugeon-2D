﻿using System.Collections;
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
        foreach (var item in allTakers)
        {
            turnInfos.Add(new TurnInfo(item));
        }

        turnBelt.InitBelt(turnInfos);

        _currentIndex = 0;
    }

    public TurnInfo NextTurn()
    {
        _currentIndex++;

        //loop back after max
        if (_currentIndex >= turnBelt.infoCount)
            _currentIndex = 0;

        return turnBelt.GetTurnInfo(_currentIndex);
    }

    //add turn info??
    public void InsertTurnInfo(TurnInfo ti, int index)
    {
        turnBelt.InsertTurnInfo(ti, index);
    }

    public TurnInfo GetTurnInfoByTaker(TurnTaker tt)
    {
        return turnBelt.GetTurnInfoByPredicate(x => x.GetTurnTaker == tt);
    }
     
    int RollDx(int x)
    {
        return Random.Range(1, x + 1);
    }
    int SortByInitiativeScore(TurnTaker a, TurnTaker b)
    {
        return -a.Initiative.CompareTo(b.Initiative);
    }


    ///// <summary>
    ///// AMAZING TEST THAT WORKEDDDD!
    ///// </summary>
    //[ContextMenu("Skip the index ToSkip")]
    //public void Skip()
    //{
    //    SkipTurn_Effect skipTurn_Effect = new SkipTurn_Effect(turnBelt.GetTurnInfo(toSkip));
    //    skipTurn_Effect.ApplyEffect();
    //}
}

