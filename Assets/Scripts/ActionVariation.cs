﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ActionVariation
{
    public ActionItem relevantItem;
    public GameObject target;
    public int weight; //cost?

    //public int specialRange;
    public ActionVariation(ActionItem rItem, GameObject tgt, int actWeight)
    {
        relevantItem = rItem; //usually, the performing item will be the relevantItem
        target = tgt;
        weight = actWeight;

        
    }

    
    //public ActionVariation(ActionItem rItem, GameObject tgt, int actWeight, int newSpecialRange)
    //{
    //    relevantItem = rItem; //usually, the performing item will be the relevantItem
    //    target = tgt;
    //    weight = actWeight;
    //    specialRange = newSpecialRange;
    //}
    public void PerformActionOnTarget()
    {
        // relevantItem.Action(target.GetComponent<Character>());
        relevantItem.Action(target);
    }

}
