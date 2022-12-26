using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Try to keep this DATA class light. Only manipulate via BeltManipulator -> don't add methods to this class (if you can avoid it)
/// </summary>
[System.Serializable]
public class TurnBelt
{
    public List<TurnInfo> turnInfos; //This should really be a special collection
                                     //

    public int infoCount => turnInfos.Count;

    int _current;

   
    public void Set(List<TurnInfo> infos)
    {
        turnInfos = infos;
    }

    public void Clear()
    {
        turnInfos.Clear();

        //Also unsubscribe from stuff?
    }
    /// <summary>
    /// Do you really need to use this right now? Is the current TurnTaker not cached where you are right now? and if not, why?
    /// </summary>
    /// <returns></returns>
    public TurnInfo GetTurnInfo(int index) //Don't really need this, as it should be cached in the TurnMachine - but it may come in handy.
    {
        return turnInfos[index]; 
    }

}
