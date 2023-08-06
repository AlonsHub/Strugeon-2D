using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSD_Comparer : Comparer<BasicSpellData>
{
    public override int Compare(BasicSpellData x, BasicSpellData y)
    {
        return (int)(x.noolCost - y.noolCost);
    }
}
