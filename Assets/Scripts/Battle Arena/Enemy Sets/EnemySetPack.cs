using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class EnemySetPack : ScriptableObject
{
    //several relevant enemysets, per difficulty 
    public List<EnemySetSO> easySetsSO;
    public List<EnemySetSO> mediumSetsSO;
    public List<EnemySetSO> hardSetsSO;

    public EnemySet GetRandomEnemySetOfDificulty(LairDifficulty difficulty)
    {
        switch (difficulty)
        {
            case LairDifficulty.Easy:
                return easySetsSO[Random.Range(0, easySetsSO.Count - 1)].enemySet;
                break;
            case LairDifficulty.Medium:
                return mediumSetsSO[Random.Range(0, mediumSetsSO.Count - 1)].enemySet;
                break;
            case LairDifficulty.Hard:
                return hardSetsSO[Random.Range(0, hardSetsSO.Count - 1)].enemySet;
                break;
            default:
                break;
        }
        return null;
    }

    //should also do the same for rewards, question is - if it should happen here or in it's own-separate scriptable setup like enemies
}
