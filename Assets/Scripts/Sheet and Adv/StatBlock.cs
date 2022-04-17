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

    
    
}
