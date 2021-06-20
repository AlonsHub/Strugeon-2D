using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public List<Pawn> enemies;
    public List<Object> lootList; 


    public FloorGrid floorGrid;

    public GameObject censerPrefab;
    public List<Vector2Int> mercSpawns; // set in inspector

    [ContextMenu("Get tiles")]
    public void GetFloorTiles() 
    {
        if(floorGrid.floorTiles != null)
        {
            return;
        }

        Debug.Log("no floor tiles");
    }
        
}
