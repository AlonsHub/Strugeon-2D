using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SortByMethod {Value, Type};

public class Archived_ItemComparers
{
    //public static IComparer ItemComparer_Value()
    //{
    //    return (IComparer)new ItemComparer_Value<MagicItem>();
    //}
   
}

public class ItemComparer_Value : IComparer<MagicItem>
{
    public int Compare(MagicItem x, MagicItem y)
    {
        int result = Mathf.Clamp(x.goldValue - y.goldValue, -1, 2);
        //if(result == 0)
        //{
        //    return string.Compare(x.magicItemName, y.magicItemName);
        //}
        return result !=0 ? result : string.Compare(x.magicItemName, y.magicItemName);

    }
}
public class ItemComparer_Name : IComparer<MagicItem>
{
    public int Compare(MagicItem x, MagicItem y)
    {
        int result = string.Compare(x.magicItemName, y.magicItemName); ;
        
        return result !=0 ? result : string.Compare(x.magicItemName, y.magicItemName);

    }
}
