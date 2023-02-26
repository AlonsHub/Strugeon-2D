using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Try to keep this DATA class light. Only manipulate via BeltManipulator -> don't add methods to this class (if you can avoid it)
/// </summary>
[System.Serializable]
public class TurnBelt
{
    [SerializeField]
    List<TurnInfo> turnInfos; 
                                     

    public int infoCount => turnInfos.Count;

    public Action OnBeltChange;
   
    public void InitBelt(List<TurnInfo> infos)
    {
        turnInfos = infos;
    }
    public void Clear()
    {
        turnInfos.Clear();

        //Call OnBeltChange? does this count as change?
        //Also unsubscribe from stuff?

        OnBeltChange = null;
    }

    public void InsertTurnInfo(TurnInfo ti, int index)
    {
        if(index >= turnInfos.Count)
        {
            turnInfos.Add(ti);
        }
        else
        {
            turnInfos.Insert(index, ti);
        }
    }

    public void AddTurnInfo(TurnInfo ti)
    {
        turnInfos.Add(ti);
    }
     public void MoveTurnInfo(TurnInfo ti, int index)
    {
        turnInfos.Remove(ti);
        
        Debug.Log(index);
        if(index ==0)
        {
            index++;
        }
            turnInfos.Insert(index, ti);

    }

    public void RemoveTurnInfo(TurnInfo ti)
    {
        turnInfos.Remove(ti);
        //handle index?
    }

    /// <summary>
    /// Do you really need to use this right now? Is the current TurnTaker not cached where you are right now? and if not, why?
    /// </summary>
    /// <returns></returns>
    public TurnInfo GetTurnInfo(int index) //Don't really need this, as it should be cached in the TurnMachine - but it may come in handy.
    {
        return turnInfos[index]; 
    }
     public List<TurnInfo> GetAllTurnInfos()
    {
        return turnInfos; 
    }

    public TurnInfo GetTurnInfoByPredicate(System.Func<TurnInfo,bool> predicate)
    {
        return turnInfos.Where(predicate).SingleOrDefault();
    }

}
