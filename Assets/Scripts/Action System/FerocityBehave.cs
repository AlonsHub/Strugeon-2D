using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerocityBehave : BehaveVariable
{
    [Tooltip("The % of hp a foe must have left to trigger this added weight")]
    [SerializeField]
    float hpThresholdPercent;
    public override void ConditionallyApplyMod()
    {
        if(affectedBehaviour.actionVariations.Count == 0)
        {
            return; //do nothing
        }

        foreach (var av in affectedBehaviour.actionVariations)
        {
            Pawn tgt = av.target.GetComponent<Pawn>();
            if (!tgt)
                continue;

            if (tgt.currentHP <= tgt.maxHP / (100f/hpThresholdPercent))
            {
                av.weight *= modifier;
                Debug.Log("FEROCITY ADDED!");
            }
        }
    }
}
