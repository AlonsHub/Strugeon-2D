using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyTranslator : MonoBehaviour
{
    public static DifficultyTranslator Instance;
    //public Dictionary<LairDifficulty,Pawn> 
    //public List<Pawn> easyEnemySet;
    //public List<Pawn> mediumEnemySet;
    //public List<Pawn> hardEnemySet;

    public EnemySetSO easyEnemySet;
    public EnemySetSO mediumEnemySet;
    public EnemySetSO hardEnemySet;

    public ItemSetSO easyItemSetSO;
    public ItemSetSO mediumItemSetSO;
    public ItemSetSO hardItemSetSO;

    public int[] goldRewards; //0,1,2 index - easy, medium, hard

    public int[] expRewards;//0,1,2 index - easy, medium, hard

    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public List<Pawn> DifficultyToEnemyPreset(LairDifficulty difficulty)
    {
        //return presets[(int)difficulty];
        switch (difficulty)
        {
            case LairDifficulty.Easy:
                return easyEnemySet.enemySet.enemyPrefabs;
                //break;
            case LairDifficulty.Medium:
                return mediumEnemySet.enemySet.enemyPrefabs; 

                //break;
            case LairDifficulty.Hard:
                return hardEnemySet.enemySet.enemyPrefabs;
                //break;
            default:
                return null;
                //break;
        }
    }

    
    public MagicItem DifficultyToSingleReward(LairDifficulty difficulty)
    {
        switch (difficulty)
        {
            case LairDifficulty.Easy:
                //return easyRewardSet[Random.Range(0, easyRewardSet.Count)].magicItem;
                return WeightedRollOnItemSetSO(easyItemSetSO);
                //break;
            case LairDifficulty.Medium:
                return WeightedRollOnItemSetSO(mediumItemSetSO);
            //break;
            case LairDifficulty.Hard:
                return WeightedRollOnItemSetSO(hardItemSetSO);
            //break;
            default:
                return null;
                //break;
        }
    }

    
    MagicItem WeightedRollOnItemSetSO(ItemSetSO itemSetSO)
    {
        int[] weights = new int[itemSetSO.itemSet.itemSOs.Count];
        int total = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            total += itemSetSO.itemSet.perItemWeight[i];
            weights[i] = total;
        }

        int rand = Random.Range(0, total);
        Debug.Log($"Random: {rand}/{total}");
        for (int i = 0; i < weights.Length; i++)
        {
            if (rand <= weights[i])
            {
                Debug.Log($"Item: {itemSetSO.itemSet.itemSOs[i].magicItem.magicItemName}");
                return itemSetSO.itemSet.itemSOs[i].magicItem;
            }
        }

        return null;
    }

    public int DifficultyToGoldReward(LairDifficulty difficulty)
    {
        //return presets[(int)difficulty];
        return goldRewards[(int)difficulty] + Random.Range(0, 6);
    }
    public int DifficultyToExp(LairDifficulty difficulty)
    {
        return expRewards[(int)difficulty];
    }
    //public GameObject DiffcultyToLevelPrefab(LairDifficulty difficulty)
    //{
    //    switch (difficulty)
    //    {
    //        case LairDifficulty.Easy:
    //            return easyPrefab;
    //            break;
    //        case LairDifficulty.Medium:
    //            return mediumPrefab;

    //            break;
    //        case LairDifficulty.Hard:
    //            return hardPrefab;

    //            break;
    //        default:
    //            return null;
    //            break;
    //    }
    //}


}
