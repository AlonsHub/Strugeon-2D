using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySheetAddon : MonoBehaviour //TEMP sheet for monsters, so they don't have to get their own character sheets just yet (and so I don't have to seperate 
{
    public void SetMe(Pawn p, int level)
    {
        p.maxHP += level * GameStats.maxHpBonusPerLevel;
        GetComponent<WeaponItem>().ApplySheet(level* GameStats.minDmgPerLevel, level * GameStats.maxDmgPerLevel);

        Destroy(this); //kills this component
    }
}
