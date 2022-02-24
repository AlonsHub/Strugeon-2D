using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LairDifficulty {Easy, Medium, Hard};

[CreateAssetMenu()]
public class LevelSO : ScriptableObject
{
    public LevelData levelData;

    
}
[System.Serializable]
public struct LevelData
{
    public List<Pawn> enemies => enemySet.enemyPrefabs; //Ref and if we want random placement or easier picking from a range
    public List<int> enemyLevels => enemySet.enemyLevels;
    [SerializeField]
    EnemySet enemySet;
    //public List<MagicItemSO> magicItems; 
    public MagicItem magicItem; 
    public int expReward;
    public GameObject levelPrefab; //currently holds enemies
    public int goldReward;

    public LairDifficulty difficulty;
    public Sprite siteIcon;

    public Vector2Int escapeTile;

    
    public TimeSpan waitTime; //this is not serializable, or at least it doesn't show up in inspector
    public int waitTimeHours, waitTimeMinutes, waitTimeSeconds;

    public bool isSet;

    public void SetLevelData(LairDifficulty newDifficulty)
    {
        //enemies.Clear();
        //rewards.Clear();
        difficulty = newDifficulty;

         //this just needs to be one method... 
        //enemies = DifficultyTranslator.Instance.DifficultyToEnemyPreset(newDifficulty); // here!!!!!
        enemySet = DifficultyTranslator.Instance.DifficultyToEnemySet(newDifficulty); // here!!!!!
        magicItem = DifficultyTranslator.Instance.DifficultyToSingleReward(newDifficulty);
        goldReward = DifficultyTranslator.Instance.DifficultyToGoldReward(newDifficulty);
        expReward = DifficultyTranslator.Instance.DifficultyToExp(newDifficulty);
        //this just needs to be one method... [end]

        waitTime = new TimeSpan(waitTimeHours, waitTimeMinutes, waitTimeSeconds);
        //levelPrefab = DifficultyTranslator.Instance.DiffcultyToLevelPrefab(difficulty);

        isSet = true;

    }

    //public int risk; //Experimental! might be better as enum



    //[HideInInspector]
    //public Vector2Int enemySpawnTiles; //set only via SpawnZones
    //[HideInInspector]
    //public Vector2Int mercSpawnTiles; //set only via SpawnZones

}