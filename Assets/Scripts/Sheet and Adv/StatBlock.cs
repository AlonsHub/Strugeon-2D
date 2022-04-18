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

    public int MinDamage(int level)
    {
        return minDamage + (minDamagePerLevel * (level - 1));
    }
    public int MaxDamage(int level)
    {
        return maxDamage + (maxDamagePerLevel * (level - 1));
    }
    public int MaxHP(int level)
    {
        return maxHp + (maxHPPerLevel * (level - 1));
    }
}
