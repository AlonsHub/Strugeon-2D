using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInfo 
{
    public bool isStartPin;

    TurnTaker turnTaker;
    //int beltIndex; //this is problematic if they move around, and they should be able to!

    //Data relevant to the machine for parsing
    //public bool DoDoubleTurn;
    public bool DoSkipTurn;

    public bool IsTurnDone => turnTaker.TurnDone;

    //Status effects?
    

    public TurnInfo(TurnTaker t)
    {
        turnTaker = t;
    }
    public TurnTaker GetTurnTaker()
    {
        return turnTaker;
    }

    
    public void TakeTurn()
    {
        //TBA Call events and things! 
        turnTaker.TakeTurn();
        
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
}
