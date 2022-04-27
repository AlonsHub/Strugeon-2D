using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [SerializeField]
    Pawn pawn;
    WeaponItem weaponItem;
    int level;

    //TEMP Enemy LEVEL-UP-SHEET
    #region LevelUpData
    [SerializeField]
    int minDamagePerLevel;
    [SerializeField]
    int maxDamagePerLevel;
    [SerializeField]
    int hpPerLevel;
    [SerializeField]
    int baseExpReward;
    [SerializeField]
    int expPerLevel;

    public int _ExpReward => baseExpReward + expPerLevel * (level -1);
    public int _ExpPerLevel => expPerLevel;
    public int _BaseExpReward => baseExpReward;

    #endregion
    //END TEMP
    public void SetEnemyLevel(int newLevel)
    {
        level = newLevel;

        //WORK WITH LEVEL-UP-SHEET INSTEAD! TBF
        pawn.maxHP += hpPerLevel * (newLevel-1);
        weaponItem.minDamage += minDamagePerLevel * (newLevel - 1);
        weaponItem.maxDamage += maxDamagePerLevel * (newLevel - 1);
        //SA_Item interface should also get a SetLevelSA_Item method
    }

}
