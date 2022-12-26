using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMachine : MonoBehaviour
{
    //[SerializeField]
    //TurnBelt turnBelt;

    [SerializeField]
    BeltManipulator beltManipulator;
    [SerializeField]
    float startOfTurnDelay;
    /// <summary>
    /// Keeps the turnsequence while loop going.
    /// </summary>
    bool isActiveBattle= false;
    //public void FeedBelt(List<TurnTaker> allTakers)
    //{
    //    turnBelt.InitialSet(allTakers); 

    //    ProcessTurnInfo(turnBelt.NextTurn()); //starts the loop! AND skips the first processing of the StartPin
    //}

    /// <summary>
    /// For now, the Machine recieves the first order of buisness. Recieving All TurnTakers, passing it on to the BeltManipulator to init the belt.
    /// The machine will them wait patiently till "StartBattle" is called(?)
    /// </summary>
    public void SetMachine(List<TurnTaker> allTakers)
    {
        beltManipulator.InitManipulator(allTakers);
    }

    public void StartBattle()
    {
        //no need to null check things that must be here - nor do we want to getComponent if not dragged. DRAG what needs to be dragged, or ERROR

        //CALL OnBattleBegin!
        isActiveBattle = true;
        StartCoroutine(nameof(StartTurnSequence));
    }
    IEnumerator StartTurnSequence()
    {
        TurnInfo currentTurnInfo;
        while (isActiveBattle)
        {
            currentTurnInfo = beltManipulator.NextTurn();//Skipping the first processing of the Start-Pin, allowing us to differentiate events such as: "On Round Restart" and "On Begin Battle"

            if (currentTurnInfo.isStartPin)
            {
                //call events for StartPin! (On Round Restart) //should call for the on start and end events for start pin? they could be useful
                continue;
            }

            if (currentTurnInfo.DoSkipTurn)
            {
                Debug.Log($"{currentTurnInfo.GetTurnTaker.Name}'s turn was skipped!");
                //TBD figure out if turn-skips also count against Cooldowns, but I suppose that can be done by sub/unsubbing from on turn start events? OR simply not calling them... ergh
                // ^^^ Solved -> Add a method to TurnTaker, which performs a "skipped-turn's" logic... if needed, a RemoveHold/CountDurationOfEffect method will sub to an event like "OnSkippedTurn" if relevant
                // 
                // Call OnSkippedTurn action here
                continue;
            }

            yield return new WaitForSeconds(startOfTurnDelay);

            currentTurnInfo.TakeTurn();

            yield return new WaitUntil(() => currentTurnInfo.IsTurnDone);



        }

    }
    //void ProcessTurnInfo(TurnInfo ti)
    //{
    //    if(ti.isStartPin)
    //    {
    //        //call events for StartPin
    //        return;
    //    }

    //    if(ti.DoSkipTurn)
    //    {
    //        //perform skip and(?!) remove the status effect?
    //    }

    //    //CALL OnTurnStart!

    //    //Perform turn
    //    ti.TakeTurn();

    //    //WAIT FOR TURN DONE

    //}
}
