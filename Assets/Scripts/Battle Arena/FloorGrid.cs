using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorGrid : MonoBehaviour
{
    [SerializeField]
    bool doBuildFloorOnStart;

    public Transform startingPoint;
    //public Vector3 startingPoint;
    public Vector2Int floorSize;
    public Vector2 tileSize;
    public Vector2 gapSize;
    //[SerializeField]
    //float[,] tileHeightMap //holds the height for each tile (by x,y positions as indexes... indecies?) with a float value as height in units
    [SerializeField]
    Transform tilePlacerPointer;

    //[SerializeField]
    public FloorTile[,] floorTiles;
    public TileWalker doorWalker;

    public Vector2Int censerGridPos;
    public int numberOfRocks; //obstacles, perhaps meaningless

    public GameObject tilePrefab;
    public GameObject censerPrefab;

    [Header("Obstacles: ")]
    public GameObject obstaclePrefab;
    public int minObstacles, maxObstacles;

    public static FloorGrid Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
        Instance = this;

        if (doBuildFloorOnStart) BuildFloor();
        
    }

    [ContextMenu("BuildFloor")]
    public void BuildFloor()
    {
        floorTiles = new FloorTile[floorSize.x, floorSize.y];
        tilePlacerPointer.position = startingPoint.position;

        for (int i = 0; i < floorSize.x; i++)
        {
            for (int k = 0; k < floorSize.y; k++)
            {
                GameObject go = Instantiate(tilePrefab, tilePlacerPointer.position, tilePrefab.transform.rotation);
                go.transform.parent = transform;
                FloorTile ft = go.GetComponent<FloorTile>();
                if(ft != null)
                {
                    floorTiles[i,k] = ft;
                    ft.gridIndex.x = i;
                    ft.gridIndex.y = k;
                }
                else
                {
                    Debug.LogError("floorTilePrefab has no FloorTile component");
                }
                tilePlacerPointer.position += new Vector3(0, 0, tileSize.y + gapSize.y);
            }
            tilePlacerPointer.position -= new Vector3(0, 0, tileSize.y + gapSize.y) * floorSize.y;
            tilePlacerPointer.position += new Vector3(tileSize.x + gapSize.x, 0, 0);
        }

        //FIND BETTER PLACE FOR THE CENSER POSITIONING and SPAWN 
        SpawnObjectOnGrid(censerPrefab, censerGridPos);
       
        //List<FloorTile> availableTiles = GetAvailableTiles();



        //for (int i = 0; i < numberOfRocks; i++)
        //{
        //    SpawnObjectOnGrid(obstaclePrefab, availableTiles[Random.Range(0, availableTiles.Count)].gridIndex);
        //    availableTiles.Remove(availableTiles[Random.Range(0, availableTiles.Count)]);
        //}
    }

    public List<FloorTile> GetAvailableTiles()
    {
        List<FloorTile> toReturn = new List<FloorTile>();
        foreach (FloorTile ft in floorTiles)
        {
            if(!ft.isEmpty) continue;
            
            toReturn.Add(ft);
        }
        return toReturn;
    }

    public List<FloorTile> GetNeighbours(FloorTile node)
    {
        List<FloorTile> toReturn = new List<FloorTile>();
        for (int x = -1; x <=1 ; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = x + node.gridIndex.x;
                int checkY = y + node.gridIndex.y;
                if(checkX >= 0 && checkX < floorSize.x && checkY >= 0 && checkY < floorSize.y)
                toReturn.Add(floorTiles[checkX, checkY]);
            }
        }
        return toReturn;
    }
    public List<FloorTile> GetNeighbours(Vector2Int pos)
    {
        List<FloorTile> toReturn = new List<FloorTile>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = x + pos.x;
                int checkY = y + pos.y;
                if (checkX >= 0 && checkX < floorSize.x && checkY >= 0 && checkY < floorSize.y)
                    toReturn.Add(floorTiles[checkX, checkY]);
            }
        }
        return toReturn;
    }

    public List<FloorTile> path;

    public FloorTile GetTileByIndex(Vector2Int index)
    {
        return floorTiles[index.x, index.y];
    }

    public GameObject SpawnObjectOnGrid(GameObject toSpawn, Vector2Int gridPos)
    {
        GameObject go = Instantiate(toSpawn, floorTiles[gridPos.x, gridPos.y].transform.position, toSpawn.transform.rotation);
        //go.GetComponent<GridPoser>().SetGridPos(new Vector2Int(gridPos.x, gridPos.y)); //grid poser might be best for snapping and positioning
        //go.GetComponent<Censer>().SetGridPos(gridPos);
        go.GetComponent<GridPoser>().SetGridPos(gridPos);

        floorTiles[gridPos.x, gridPos.y].isEmpty = false; 
        floorTiles[gridPos.x, gridPos.y].myOccupant = go;

        return go;
    }

    public void PlaceObjectOnGrid(GameObject toPlace, Vector2Int gridPos)
    {
        toPlace.transform.position = floorTiles[gridPos.x, gridPos.y].transform.position;

        floorTiles[gridPos.x, gridPos.y].isEmpty = false;
        floorTiles[gridPos.x, gridPos.y].myOccupant = toPlace;
    }

    [SerializeField]
    GameObject obstacleRock;
     public void SpawnObstacles() //currently, only ROCKS
    {
        List<FloorTile> availTiles = GetAvailableTiles();

        int amountOfObstacles = Random.Range(minObstacles, maxObstacles);

        for (int i = 0; i < amountOfObstacles; i++)
        {
            int rnd = Random.Range(0, availTiles.Count);
            SpawnObjectOnGrid(obstacleRock, availTiles[rnd].gridIndex);
            availTiles.RemoveAt(rnd);
        }
    }

    public void SetTileToStaticObstacle(Vector2Int pos, GameObject obstacle)
    {
        // check if pos is in-range for the grid...

        floorTiles[pos.x, pos.y].myOccupant = obstacle;
        floorTiles[pos.x, pos.y].isEmpty = false;
    }

    public FloorTile GetRandomFreeTile()
    {
        List<FloorTile> freeTiles = new List<FloorTile>();

        foreach (var ft in floorTiles) 
        {
            if(ft.isEmpty)
            {
                freeTiles.Add(ft);
            }
        }

        return freeTiles[Random.Range(0, freeTiles.Count)];
    }

    //private void OnDrawGizmos()
    //{
    //    if(path.Count >0)
    //    {
    //       foreach(FloorTile pathNode in path)
    //        {
    //            Gizmos.color = Color.red;
    //            Gizmos.DrawCube(pathNode.transform.position, Vector3.one);
    //        }
    //    }
    //}
}
