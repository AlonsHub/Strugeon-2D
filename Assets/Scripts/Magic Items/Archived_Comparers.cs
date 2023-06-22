using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SortByMethod {Value, Type};

public class Archived_Comparers
{
    //public static IComparer ItemComparer_Value()
    //{
    //    return (IComparer)new ItemComparer_Value<MagicItem>();
    //}
   
}

public class ItemComparer_ValueLowToHigh : IComparer<MagicItem>
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
public class ItemComparer_ValueHighToLow :IComparer<MagicItem>
{
    public int Compare(MagicItem x, MagicItem y)
    {
        int result = Mathf.Clamp(y.goldValue - x.goldValue, -1, 2);
        //if(result == 0)
        //{
        //    return string.Compare(x.magicItemName, y.magicItemName);
        //}
        return result !=0 ? result : string.Compare(x.magicItemName, y.magicItemName);

    }
}

public class ItemComparer_NameAtoZ : IComparer<MagicItem>
{
    public int Compare(MagicItem x, MagicItem y)
    {
        int result = string.Compare(x.magicItemName, y.magicItemName); 

        return result != 0 ? result : x.goldValue - y.goldValue;
    }
}
public class ItemComparer_NameZtoA : IComparer<MagicItem>
{
    public int Compare(MagicItem x, MagicItem y)
    {
        int result = string.Compare(y.magicItemName, x.magicItemName); 
        
        return result !=0 ? result : y.goldValue - x.goldValue;

    }
}
public class ItemComparer_AquisitionOrderEarlyToLate : IComparer<MagicItem>
{
    public int Compare(MagicItem x, MagicItem y)
    {
        int result = System.DateTime.Compare(x.acquisitionDate, y.acquisitionDate); 

        return result != 0 ? result : x.goldValue - y.goldValue;
    }
}
public class ItemComparer_AquisitionOrderLateToEarly : IComparer<MagicItem>
{
    public int Compare(MagicItem x, MagicItem y)
    {
        int result = System.DateTime.Compare(y.acquisitionDate, x.acquisitionDate);

        return result !=0 ? result : y.goldValue - x.goldValue;

    }
}

public class MercComparer_LevelHighToLow : IComparer<MercSheet>
{
    public int Compare(MercSheet x, MercSheet y)
    {
        //int result = Mathf.Clamp(y._level - x._level, -1, 2);
        return y._level - x._level;
    }
}
public class MercComparer_LevelLowToHigh : IComparer<MercSheet>
{
    public int Compare(MercSheet x, MercSheet y)
    {
        //int result = Mathf.Clamp(y._level - x._level, -1, 2);
        return x._level - y._level;
    }
}
public class MercComparer_NameAtoZ : IComparer<MercSheet>
{
    public int Compare(MercSheet x, MercSheet y)
    {
        return string.Compare(x.characterName.ToString(),y.characterName.ToString());
    }
}
public class MercComparer_NameZtoA : IComparer<MercSheet>
{
    public int Compare(MercSheet x, MercSheet y)
    {
        return string.Compare(y.characterName.ToString(),x.characterName.ToString());
    }
}
public class MercComparer_DamageLowToHigh : IComparer<MercSheet>
{
    public int Compare(MercSheet x, MercSheet y)
    {
        return x._maxDamage - y._maxDamage;
    }
}
public class MercComparer_DamageHighToLow : IComparer<MercSheet>
{
    public int Compare(MercSheet x, MercSheet y)
    {
        return y._maxDamage -x._maxDamage;
    }
}
public class MercComparer_HPLowToHigh : IComparer<MercSheet>
{
    public int Compare(MercSheet x, MercSheet y)
    {
        return x._maxHp - y._maxHp;
    }
}
public class MercComparer_HPHighToLow : IComparer<MercSheet>
{
    public int Compare(MercSheet x, MercSheet y)
    {
        return y._maxHp - x._maxHp;
    }
}
