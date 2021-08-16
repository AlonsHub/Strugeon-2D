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
    public List<Pawn> enemies; //Ref and if we want random placement or easier picking from a range
    public List<Object> rewards; //Objects TBD

    public GameObject levelPrefab; //currently holds enemies
    public int goldReward;

    public LairDifficulty difficulty;

    public void SetLevelData(LairDifficulty newDifficulty)
    {
        //enemies.Clear();
        //rewards.Clear();
        difficulty = newDifficulty;
        enemies = DifficultyTranslator.Instance.DifficultyToEnemyPreset(newDifficulty);
        rewards = DifficultyTranslator.Instance.DifficultyToRewardPreset(newDifficulty);
        goldReward = DifficultyTranslator.Instance.DifficultyToGoldReward(newDifficulty);
        //levelPrefab = DifficultyTranslator.Instance.DiffcultyToLevelPrefab(difficulty);



    }

    //public int risk; //Experimental! might be better as enum



    //[HideInInspector]
    //public Vector2Int enemySpawnTiles; //set only via SpawnZones
    //[HideInInspector]
    //public Vector2Int mercSpawnTiles; //set only via SpawnZones

}