using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archived_ItemComparers
{
   
}

public class ItemComparer_Value : IComparer
{
    public int Compare(object x, object y)
    {
        MagicItem itemX = (MagicItem)x;
        MagicItem itemY = (MagicItem)y;

        int result = itemX.goldValue - itemY.goldValue;
        return Mathf.Clamp(result,-1, 2);
    }
}
