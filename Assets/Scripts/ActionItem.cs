using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionItem : MonoBehaviour
{
    //public string itemName;
    //Cost in this case is considered a positive "weight"-like calculation
    public Pawn pawn;
    //baseCost is the natural weight of any action to occur. Should usually remain as 1, but can change as 
    //a character/item levels up.
    public int baseCost = 1; //Weight, probabillity of being picked
                             //Default = 1. Synthetic Cancel/"Don't Use" = 0.
                             //Will be modified by the characters themselves by multiplication, so will be "turned off"
                             //by *0, if need be. Synthetically-cancel actions with base 0 in cases of "mute/atropy"

    //Each action has a 
    public int currentModifier = 1;
    public List<ActionVariation> actionVariations;
    public bool isWeapon; // CAN REMOVE!
    //public List<Character> relevantTargets; // An interesting idea, add actions to the pool per relevant target - regardless of the action type
    //but we should have all possible targets somere
    public virtual void Awake()
    {
        //actionVariations = new List<ActionVariation>();
        pawn = GetComponent<Pawn>();
    }
    public virtual void CalculateVariations()
    {

    }
    public virtual void Action(GameObject tgt)
    {

    }

}