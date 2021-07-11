﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyTranslator : MonoBehaviour
{
    public static DifficultyTranslator Instance;
    //public Dictionary<LairDifficulty,Pawn> 
    public List<Pawn> easySet;
    public List<Pawn> mediumSet;
    public List<Pawn> hardSet;

    public List<Object> easyRewardSet;
    public List<Object> mediumRewardSet;
    public List<Object> hardRewardSet;

    public int[] goldRewards; //0,1,2 index - easy, medium, hard

    //public GameObject easyPrefab;
    //public GameObject mediumPrefab;
    //public GameObject hardPrefab;

    //presets
    //public List<Pawn>[] presets;

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
                break;
            case LairDifficulty.Medium:
                return mediumSet;

                break;
            case LairDifficulty.Hard:
                return hardSet;

                break;
            default:
                return null;
                break;
        }
    }
    public List<Object> DifficultyToRewardPreset(LairDifficulty difficulty)
    {
        //return presets[(int)difficulty];
        switch (difficulty)
        {
            case LairDifficulty.Easy:
                return easyRewardSet;
                break;
            case LairDifficulty.Medium:
                return mediumRewardSet;

                break;
            case LairDifficulty.Hard:
                return hardRewardSet;

                break;
            default:
                return null;
                break;
        }
    }
    public int DifficultyToGoldReward(LairDifficulty difficulty)
    {
        //return presets[(int)difficulty];
        return goldRewards[(int)difficulty] + Random.Range(-5, 6);
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