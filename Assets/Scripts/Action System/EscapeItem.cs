using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeItem : ActionItem
{
    [SerializeField]
    float escapeThreshold;
    [SerializeField]
    int escapeWeight;
    
    void Start()
    {
        actionVariations = new List<ActionVariation>();
    }

    public override void Action(GameObject tgt)
    {
        //walk to edge of screen and disappear!

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Escape);


    }

    public override void CalculateVariations()
    {
        if(pawn.currentHP >= pawn.maxHP/escapeThreshold)
        {
            actionVariations.Add(new ActionVariation(this, gameObject, escapeWeight));
        }
    }
}
