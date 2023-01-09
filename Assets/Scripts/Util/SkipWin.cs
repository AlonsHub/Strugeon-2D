using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipWin : MonoBehaviour
{
    public void Win()
    {
        //TurnMaster.Instance.StopTurning();
        TurnMachine.Instance.StopTurnSequence();
    }
}
