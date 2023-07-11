﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatBlock 
{
    //health
    [Header("Health:")]
    [Space(5f)]
    public int maxHp;
    //public int armor; //?
    //public int evasion; //?

    [Header("Weapon:")]
    [Space(5f)]
    public int range; //in tiles //can even improve???
    public int minDamage;
    public int maxDamage;

    //TEMP BAD DONT LIKE THIS!
    public int mercLevel; //for calculation purposes
    public int minDamageBenefit; //from items!
    public int maxDamageBenefit; //from items!
    //TEMP BAD DONT LIKE THIS!

    //ToHit
    public int grazeChance;
    public int grazeDamagePercentage;
    public int critChance;
    public int critDamagePercentage;
    

    [Header("Advancement Scheme:"), Space(5f)]
    //public int maxHPPerLevel;
    //public int maxDamagePerLevel;
    //public int minDamagePerLevel;

    public float maxHPPerLevelAsPercentage;
    public float maxDamagePerLevelAsPercentage;
    public float minDamagePerLevelAsPercentage;


    #region External Setters:

    public void CopyAdvancementPercentages(StatBlock toCopy)
    {
        maxHPPerLevelAsPercentage = toCopy.maxHPPerLevelAsPercentage;
        minDamagePerLevelAsPercentage = toCopy.minDamagePerLevelAsPercentage;
        maxDamagePerLevelAsPercentage = toCopy.maxDamagePerLevelAsPercentage;
    }

    #endregion

    public float MinDamage(int level)
    {
        float toReturn = minDamage;
        for (int i = 0; i < level; i++)
        {
            toReturn += toReturn * (minDamagePerLevelAsPercentage / 100f) + minDamageBenefit; 
        }
        return toReturn;
    }

    public float MaxDamage(int level)
    {
        return MinDamage(level) * (1 + maxDamagePerLevelAsPercentage/100f) + maxDamageBenefit;
    }
    
    public float MaxHP(int level)
    {
        float toReturn = maxHp;
        for (int i = 0; i < level; i++)
        {
            toReturn += toReturn * (maxHPPerLevelAsPercentage / 100f);
        }
        return toReturn;
    }
   


    
}
