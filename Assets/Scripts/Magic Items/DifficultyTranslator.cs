using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyTranslator : MonoBehaviour
{
    public static DifficultyTranslator Instance;
    //public Dictionary<LairDifficulty,Pawn> 
    public List<Pawn> easySet;
    public List<Pawn> mediumSet;
    public List<Pawn> hardSet;

    public List<MagicItemSO> easyRewardSet;
    public List<MagicItemSO> mediumRewardSet;
    public List<MagicItemSO> hardRewardSet;

    public int[] goldRewards; //0,1,2 index - easy, medium, hard

    public int[] expRewards;//0,1,2 index - easy, medium, hard

    //public GameObject easyPrefab;
    //public GameObject mediumPrefab;
    //public GameObject hardPrefab;

    //presets
    //public List<Pawn>[] presets;
    [SerializeField]
    ItemSet easyItemSet;
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
                return easySet;
                //break;
            case LairDifficulty.Medium:
                return mediumSet;

                //break;
            case LairDifficulty.Hard:
                return hardSet;

                //break;
            default:
                return null;
                //break;
        }
    }

    public List<MagicItemSO> DifficultyToRewardPreset(LairDifficulty difficulty)
    {
        //return presets[(int)difficulty];
        switch (difficulty)
        {
            case LairDifficulty.Easy:
                return easyRewardSet;
                //break;
            case LairDifficulty.Medium:
                return mediumRewardSet;

                //break;
            case LairDifficulty.Hard:
                return hardRewardSet;

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
                return WeightedRollOnItemSet(easyRewardSet);
                //break;
            case LairDifficulty.Medium:
                return WeightedRollOnItemSet(mediumRewardSet);
            //break;
            case LairDifficulty.Hard:
                return WeightedRollOnItemSet(hardRewardSet);
            //break;
            default:
                return null;
                //break;
        }
    }

    MagicItem WeightedRollOnItemSet(List<MagicItemSO> itemSet)
    {
        int[] weights = new int[itemSet.Count];
        int total = 0;
        for (int i = 0; i < itemSet.Count; i++)
        {
            total += itemSet[i].magicItem.dropRateWeight;
            weights[i] = total;
        }

        int rand = Random.Range(0, total);

        for (int i = 0; i < weights.Length; i++)
        {
            if(rand <= weights[i])
            {
                return itemSet[i].magicItem;
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
