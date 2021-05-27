using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject
{
    public LevelData level;
}
[System.Serializable]
public struct LevelData
{
    public List<Pawn> enemies; //Ref and if we want random placement or easier picking from a range
    public List<Object> rewards; //Objects TBD

    public GameObject levelPrefab; //currently holds enemies

    public int risk; //Experimental! might be better as enum

    //[HideInInspector]
    //public Vector2Int enemySpawnTiles; //set only via SpawnZones
    //[HideInInspector]
    //public Vector2Int mercSpawnTiles; //set only via SpawnZones

}