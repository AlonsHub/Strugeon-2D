using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class EnemySetSO : ScriptableObject
{
    public LairDifficulty setDifficulty; //TBF seems useless as long as EnemySetPacks have 3 lists 
                                        //(1 for each difficulty... could be made into 1 list that can be filtered each time, using linq
    public EnemySet enemySet;
}
