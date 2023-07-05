using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatSetter : ScriptableObject
{
    [SerializeField]
    List<Pawn> pawns;
    [SerializeField]
    BaseStatBlock rogueStatBlock;
    [SerializeField]
    BaseStatBlock priestStatBlock;
    [SerializeField]
    BaseStatBlock figherStatBlock;
    [SerializeField]
    BaseStatBlock mageStatBlock;

    [ContextMenu("Set BaseStatBlock for all mercs, by class")]
    public void SetMercsStatBlockByClass()
    {
        //foreach pawns -> switch on class to decide which stat block to apply (or list, and enum as index)
    }
}
