using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inits the TurnBelt, (?) and feeds the belt to the TurnMachine (?)
/// The ONLY class that can edit the TurnBelt.
/// Provides access to the TurnBelt if needed.
/// </summary>
public class BeltManipulator : MonoBehaviour
{
    [SerializeField]
    TurnBelt turnBelt;


    int _currentIndex;

    /// <summary>
    /// Receives TurnTakers and creates TurnInfos for them.
    /// Rolls their initiative (simple 1d20 for now)
    /// Sorts them in initiative order
    /// And sets the _current counter to 0.
    /// </summary>
    /// <param name="allTakers"></param>
    public void InitBelt(List<TurnTaker> allTakers)
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

        turnBelt.Set(turnInfos);

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

    int RollDx(int x)
    {
        return Random.Range(1, x + 1);
    }
    int SortByInitiativeScore(TurnTaker a, TurnTaker b)
    {
        return -a.Initiative.CompareTo(b.Initiative);
    }
}

