﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionItem : MonoBehaviour
{
    //public string itemName;
    //Cost in this case is considered a positive "weight"-like calculation
    public Pawn pawn;
    //baseCost is the natural weight of any action to occur. Should usually remain as 1, but can change as 
    //a character/item levels up.
    public int baseweight; //Weight, probabillity of being picked
                             //Default = 1. Synthetic Cancel/"Don't Use" = 0.
                             //Will be modified by the characters themselves by multiplication, so will be "turned off"
                             //by *0, if need be. Synthetically-cancel actions with base 0 in cases of "mute/atropy"

    
    public List<ActionVariation> actionVariations;
    
    public bool doTargetAllies; 
    //public List<Character> relevantTargets; // An interesting idea, add actions to the pool per relevant target - regardless of the action type
    //but we should have all possible targets somere

    List<BehaveVariable> behaveVariables;

    public virtual void Awake()
    {
        //actionVariations = new List<ActionVariation>(); //Why is this commented out? TBF look at all overrides of this
        //if(!pawn) MUST HAPPEN! the ref is the prefab to begin with
        pawn = GetComponent<Pawn>(); //MUST
        behaveVariables = new List<BehaveVariable>();
    }
    public virtual void CalculateVariations()
    {

    }
    public virtual void Action(ActionVariation av)
    {

    }

    public void AddBehaveVariable(BehaveVariable behaveVariable)
    {
        behaveVariables.Add(behaveVariable);
    }

    public void CallBehaveVariables()
    {
        foreach (var item in behaveVariables)
        {
            item.ConditionallyApplyMod();
        }
    }


    //for inspector use!
    [ContextMenu("get pawn")]
    public void GetPawn()
    {
        pawn = GetComponent<Pawn>();
    }

    public override string ToString()
    {
        return this.GetType().Name.Replace("Item", "");
    }
}