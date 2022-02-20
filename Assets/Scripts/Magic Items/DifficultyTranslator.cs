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

    public List<EnemySetSO> easyEnemySets; //also SOs
    public List<EnemySetSO> mediumEnemySets; //also SOs
    public List<EnemySetSO> hardEnemySets; //also SOs

    public List<ItemSetSO> easyItemSetSOs;
    public List<ItemSetSO> mediumItemSetSOs;
    public List<ItemSetSO> hardItemSetSOs;

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
        int rand;
        //return presets[(int)difficulty];
        switch (difficulty)
        {
            case LairDifficulty.Easy:
                rand = Random.Range(0, easyEnemySets.Count);
                return easyEnemySets[rand].enemySet.enemyPrefabs;
                //break;
            case LairDifficulty.Medium:
                rand = Random.Range(0, mediumEnemySets.Count);
                return mediumEnemySets[rand].enemySet.enemyPrefabs;
            //break;
            case LairDifficulty.Hard:
                rand = Random.Range(0, hardEnemySets.Count);
                return hardEnemySets[rand].enemySet.enemyPrefabs;
            //break;
            default:
                return null;
                //break;
        }
    }
     public EnemySet DifficultyToEnemySet(LairDifficulty difficulty)
    {
        int rand;
        //return presets[(int)difficulty];
        switch (difficulty)
        {
            case LairDifficulty.Easy:
                rand = Random.Range(0, easyEnemySets.Count);
                return easyEnemySets[rand].enemySet;
                //break;
            case LairDifficulty.Medium:
                rand = Random.Range(0, mediumEnemySets.Count);
                return mediumEnemySets[rand].enemySet;
            //break;
            case LairDifficulty.Hard:
                rand = Random.Range(0, hardEnemySets.Count);
                return hardEnemySets[rand].enemySet;
            //break;
            default:
                return null;
                //break;
        }
    }

    
    public MagicItem DifficultyToSingleReward(LairDifficulty difficulty)
    {
        int rand;

        switch (difficulty)
        {
            case LairDifficulty.Easy:
                //return easyRewardSet[Random.Range(0, easyRewardSet.Count)].magicItem;
                rand = Random.Range(0, easyItemSetSOs.Count);

                return WeightedRollOnItemSetSO(easyItemSetSOs[rand]);
                //break;
            case LairDifficulty.Medium:
                rand = Random.Range(0, mediumItemSetSOs.Count);

                return WeightedRollOnItemSetSO(mediumItemSetSOs[rand]);
            //break;
            case LairDifficulty.Hard:
                rand = Random.Range(0, hardItemSetSOs.Count);

                return WeightedRollOnItemSetSO(hardItemSetSOs[rand]);
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

        int rand = Random.Range(1, total+1);
        Debug.Log($"Random: {rand}/{total}");
        for (int i = 0; i < weights.Length; i++)
        {
            if (rand <= weights[i])
            {
                Debug.Log($"Item: {itemSetSO.itemSet.itemSOs[i].magicItem.magicItemName}");
                return itemSetSO.itemSet.itemSOs[i].magicItem;
            }
        }

        ////only case if if it rolled the max number!
        //Debug.Log("Rolled MAX on items! get another item?");

        return itemSetSO.itemSet.itemSOs[itemSetSO.itemSet.itemSOs.Count-1].magicItem;
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
