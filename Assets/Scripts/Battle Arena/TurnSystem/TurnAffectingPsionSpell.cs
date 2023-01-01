using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAffectingPsionSpell : MonoBehaviour
{
    [SerializeField]
    BeltManipulator beltManipulator;
    public void SkipTurn(TurnTaker target)
    {
        TurnInfo ti= beltManipulator.GetTurnInfoByTaker(target);
        SkipTurn_Effect skipTurn_Effect = new SkipTurn_Effect(ti);
        skipTurn_Effect.ApplyEffect();

    }
     public void DoubleTurn(TurnTaker target)
    {
        TurnInfo ti= beltManipulator.GetTurnInfoByTaker(target);
        DoubleTurn_Effect doubleTurn_Effect = new DoubleTurn_Effect(ti);
        doubleTurn_Effect.ApplyEffect();
    }
        
}
