using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatBlock 
{
    //health
    [Header("Health:")]
    [Space(10f)]
    public int maxHp;
    //public int armor; //?
    //public int evasion; //?

    [Header("Weapon:")]
    [Space(10f)]
    
    //weapon
    public int maxDamage;
    public int minDamage;
    public int range; //in tiles //can even improve???
    //public int accuracy; //?

    [Header("Advancement Scheme:"), Space(5f)]
    public int maxHPPerLevel;
    public int maxDamagePerLevel;
    public int minDamagePerLevel;

    public float maxHPPerLevelAsPercentage;
    public float maxDamagePerLevelAsPercentage;
    public int minDamagePerLevelAsPercentage;


    //public int MinDamage(int level)
    //{
    //    return minDamage + (minDamagePerLevel * (level - 1));
    //}
    public int MinDamage(int level)
    {
        int toReturn = minDamage;
        for (int i = 0; i < level; i++)
        {
            toReturn += toReturn * (minDamagePerLevelAsPercentage / 100); 
        }
        return toReturn;
    }

    public float MaxDamage(int level)
    {
        return MinDamage(level) * (1 + maxDamagePerLevelAsPercentage/100f);
    }
    //public int MaxDamage(int level)
    //{
    //    return maxDamage + (maxDamagePerLevel * (level - 1));
    //}
    public float MaxHP(int level)
    {
        float toReturn = maxHp;
        for (int i = 0; i < level; i++)
        {
            toReturn += toReturn * (maxHPPerLevelAsPercentage / 100);
        }
        return toReturn;
    }
    //public int MaxHP(int level)
    //{
    //    return maxHp + (maxHPPerLevel * (level - 1));
    //}

}
