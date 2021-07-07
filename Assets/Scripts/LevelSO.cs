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

    public void SetLevelData(LairDifficulty difficulty)
    {
        //enemies.Clear();
        //rewards.Clear();
        enemies = DifficultyTranslator.Instance.DifficultyToEnemyPreset(difficulty);
        rewards = DifficultyTranslator.Instance.DifficultyToRewardPreset(difficulty);
        goldReward = DifficultyTranslator.Instance.DifficultyToGoldReward(difficulty);
        //levelPrefab = DifficultyTranslator.Instance.DiffcultyToLevelPrefab(difficulty);



    }

    //public int risk; //Experimental! might be better as enum



    //[HideInInspector]
    //public Vector2Int enemySpawnTiles; //set only via SpawnZones
    //[HideInInspector]
    //public Vector2Int mercSpawnTiles; //set only via SpawnZones

}