using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class SorterFilter 
{
    public static List<T> FilterListBy<T>(List<T> things, System.Func<T,bool> pred)
    {
        return things.Where(pred).ToList();
    }
    public static List<T> SortListBy<T>(List<T> things,IComparer<T> comparer)
    {
        things.Sort(comparer);
        return things;
    }

}
