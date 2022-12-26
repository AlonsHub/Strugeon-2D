using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TurnInfo 
{
    public bool isStartPin;

    TurnTaker turnTaker;
    //int beltIndex; //this is problematic if they move around, and they should be able to!

    //Data relevant to the machine for parsing
    public bool DoDoubleTurn;
    public bool DoSkipTurn;

    public TurnTaker GetTurnTaker => turnTaker;
   
    public bool IsTurnDone => GetTurnTaker.TurnDone;


    public System.Action OnTurnBegin;
    public System.Action OnTurnEnd; //add to this cooldowns and effects that need to remove themselves
    //Status effects?
    

    public TurnInfo(TurnTaker t)
    {
        turnTaker = t;
    }

    
    public void TakeTurn()
    {
        OnTurnBegin?.Invoke(); 

        turnTaker.TakeTurn();

        OnTurnEnd?.Invoke(); 
    }

    /// <summary>
    /// Returns null in case of the turnTaker not being a Pawn
    /// </summary>
    /// <returns></returns>
    public Pawn TryGetPawn()
    {
        if (turnTaker is Pawn)
            return turnTaker as Pawn;

        return null;
    }


    // TBD if this is really a problem?
    ///// <summary>
    ///// IMPORTANT! not clearing may cause all turninfos to remain in RAM if they still have listeners.?
    ///// </summary>
    //public void Clear()
    //{
    //    OnTurnBegin.
    //}
}
