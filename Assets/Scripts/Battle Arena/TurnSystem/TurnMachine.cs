using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurnMachine : MonoBehaviour
{
    //[SerializeField]
    //TurnBelt turnBelt;

    [SerializeField]
    BeltManipulator beltManipulator;
    [SerializeField]
    DisplayBelt displayBelt;
    [SerializeField]
    float startOfTurnDelay;
    /// <summary>
    /// Keeps the turnsequence while loop going.
    /// </summary>
    bool isActiveBattle= false;


    //REMOVE ALL OF THIS!
    private void Start()
    {
        List<TurnTaker> tts = new List<TurnTaker>();
        TEST_TurnTaker[] tests = FindObjectsOfType<TEST_TurnTaker>();
        foreach (var item in tests)
        {
            tts.Add(item as TurnTaker);
        }
        SetMachine(tts);

        Invoke(nameof(StartBattle), 2f);
    }

    public void SetMachine(List<TurnTaker> allTakers)
    {
        beltManipulator.InitManipulator(allTakers);
    }
    /// <summary>
    /// For now, the Machine recieves the first order of buisness. Recieving All TurnTakers, passing it on to the BeltManipulator to init the belt.
    /// The machine will them wait patiently till "StartBattle" is called(?)
    /// </summary>
    /// 


    public void StartBattle()
    {
        //TBA a priliminary sequence just before StartBattle for setup such as this

        displayBelt.Init(beltManipulator); //TBF do this earlier


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
                Debug.Log("Round Restarted!");
                //call events for StartPin! (On Round Restart) //should call for the on start and end events for start pin? they could be useful
                continue;
            }

            DoubleTurn_Effect de;
            if (currentTurnInfo.GetEffectOfType(out de))
            {
                //currentTurnInfo.OnTurnSkip?.Invoke();
                currentTurnInfo.RemoveEffect(de);

                Debug.Log($"{currentTurnInfo.GetTurnTaker.Name}'s turn was skipped!");
                //TBD figure out if turn-skips also count against Cooldowns, but I suppose that can be done by sub/unsubbing from on turn start events? OR simply not calling them... ergh
                // ^^^ Solved -> Add a method to TurnTaker, which performs a "skipped-turn's" logic... if needed, a RemoveHold/CountDurationOfEffect method will sub to an event like "OnSkippedTurn" if relevant
                // 
                // Call OnSkippedTurn action here
                continue;
            }

            yield return new WaitForSeconds(startOfTurnDelay);

            currentTurnInfo.IsTurnDone = false;
            currentTurnInfo.TakeTurn();

            yield return new WaitUntil(() => currentTurnInfo.IsTurnDone);
            currentTurnInfo.OnTurnEnd?.Invoke();
        }

    }    
}
