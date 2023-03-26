using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class TurnInfo 
{
    public bool isStartPin;

    TurnTaker turnTaker;

    //DisplayPlate displayPlate;
    //int beltIndex; //this is problematic if they move around, and they should be able to!

    public TurnTaker GetTurnTaker => turnTaker;
   
    public bool IsTurnDone { get => GetTurnTaker.TurnDone; set => GetTurnTaker.TurnDone = value; }
    //public Sprite GetPortraite {get => }

    #region Maybe Zone
    //Data relevant to the machine for parsing
    public System.Action OnTurnBegin;
    //public System.Action OnTurnSkip; //
    public System.Action OnTurnEnd; //add to this cooldowns and effects that need to remove themselves
    #endregion

    //Status effects?
    List<TurnInfoEffect> turnInfoEffects;


    public Color colour;


    /// <summary>
    /// Create a TurnInfo for this TurnTaker
    /// </summary>
    /// <param name="t"></param>
    public TurnInfo(TurnTaker t)
    {
        turnTaker = t;
        if (t.TurnInfo.isStartPin)
        {
            t.TurnInfo = this;
            isStartPin = false;
        }
        turnInfoEffects = new List<TurnInfoEffect>();

        colour = new Color(0, 0, 0, 0);
    }
    /// <summary>
    /// Creates a Start-Pin TurnInfo
    /// </summary>
    public TurnInfo()
    {
        isStartPin = true;

        turnInfoEffects = new List<TurnInfoEffect>(); //TBD this could be fun! double/skip turn on the Start-Pin
        turnTaker = null;
    }


    public virtual void TakeTurn()
    {
        OnTurnBegin?.Invoke(); 

        turnTaker.TakeTurn();

        //OnTurnEnd?.Invoke(); 
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

    /// <summary>
    /// Add effects that ARE ALREADY inited and applied
    /// </summary>
    /// <param name="turnInfoEffect"></param>
    public void AddEffect(TurnInfoEffect turnInfoEffect)
    {
        if (turnInfoEffects == null)
            turnInfoEffects = new List<TurnInfoEffect>();
        turnInfoEffects.Add(turnInfoEffect);
        //GFX! TBA
    }
    /// <summary>
    /// Removes and ENDS effects
    /// </summary>
    /// <param name="turnInfoEffect"></param>
    public void RemoveEffect(TurnInfoEffect turnInfoEffect)
    {
        if (turnInfoEffects == null)
            return;
        turnInfoEffects.Remove(turnInfoEffect);
        turnInfoEffect.EndEffect(); //GFX! remove here or in EndEffect? TBA
    }

    public bool GetEffectOfType<T>(out T export) where T: TurnInfoEffect
    {
        export = null;

        if (turnInfoEffects.Count == 0)
            return false;

        foreach(var ti in turnInfoEffects)
        {
            if (ti is T)
            {
                export = ti as T;
                return true;
            }
        }
        Debug.Log($"NOT FOUND");
        return false;
        //return turnInfoEffects.Where(x => x.GetType().Equals(T)).SingleOrDefault();

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
