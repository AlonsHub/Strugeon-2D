using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaMaster : MonoBehaviour
{
    public static ArenaMaster Instance;
    //    RefMaster characterMaster;
    //    TurnMaster turnMaster;
    FloorGrid floorGrid;
    public GameObject levelGFX; //exists if level is loaded

    public LevelData levelData;

    public List<Vector2Int> mercSpawnTiles;
    public List<Vector2Int> enemySpawnTiles;
    
    public float startDelayAmount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Invoke("LoadNewLevel", startDelayAmount);
    }

    //public void LoadNewLevel(LevelData newLevelData)
    public void LoadNewLevel()
    {
        levelData = LevelRef.Instance.currentLevel.levelData;
        //instantiate level prefab for GFX/Environment assets ONLY
        //levelGFX = Instantiate(levelData.levelPrefab);
        List<Pawn> newEnemies = new List<Pawn>();
        List<Pawn> newMercs = new List<Pawn>();

        //temp AF

        List<StaticObastacle> staticObastacles = FindObjectsOfType<StaticObastacle>().ToList();
        foreach (var item in staticObastacles)
        {
            item.SpawnMeOnGrid();
        }
        //end temp AF

        //= levelData.enemies;
        //build floor:
        if (enemySpawnTiles.Count > 0)
        {
            //Spawn enemies:
            //int amount = levelData.enemies.Count;
            foreach (var p in levelData.enemies)
            {
                int rnd = Random.Range(0, enemySpawnTiles.Count); //draw random pos
                GameObject go = Instantiate(p.gameObject);
                newEnemies.Add(go.GetComponent<Pawn>());
                FloorGrid.Instance.PlaceObjectOnGrid(go, enemySpawnTiles[rnd]);
                enemySpawnTiles.RemoveAt(rnd);
                //go.name.("(Clone)");
            }
            RefMaster.Instance.enemies = newEnemies; //sends enemies to refmaster to be kept there as the common ref for all components to use (AND NOT CACHED HERE)
            RefMaster.Instance.SetEnemyCharacters(); //inits enemies
        }

            //Spawn mercs:
        if (mercSpawnTiles.Count > 0)
        {
            //foreach (var p in RefMaster.Instance.mercs)

            //foreach (var p in RefMaster.Instance.mercs)
            foreach (var p in PartyMaster.Instance.currentMercParty)
            {
                int rnd = Random.Range(0, mercSpawnTiles.Count); //draw random pos

                GameObject go = Instantiate(p.gameObject);
                //RefMaster.Instance.mercs.Add(go);
                newMercs.Add(go.GetComponent<Pawn>());

                FloorGrid.Instance.PlaceObjectOnGrid(go, mercSpawnTiles[rnd]);
                mercSpawnTiles.RemoveAt(rnd);
            }
            RefMaster.Instance.mercs = newMercs; //sends newMercs to refmaster to be kept there as the common ref for all components to use (AND NOT CACHED HERE)
            RefMaster.Instance.SetMercPawns(); //inits all mercs

            FloorGrid.Instance.SpawnObstacles();

            StartCoroutine("DelayedStart");
        }

        
    }
    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(startDelayAmount);
        TurnMaster.Instance.GetReady();
    }

    //public void SpawnPawnOnTile(GameObject pawnObject, Vector2Int tilePos)
    //{

    //}
    //        characterMaster = RefMaster.Instance; //good idea? to have cached singeltons?
    //        turnMaster = TurnMaster.Instance;
    //    }
    //    private void Start()
    //    {
    //        LoadLevelSO(TransferSceneToArena.levelSO);

    //    }
    //    public void LoadLevelSO(LevelSO levelSO)
    //    {
    //        GameObject go = Instantiate(levelSO.levelPrefab);
    //        Level level = go.GetComponent<Level>();
    //        floorGrid = level.floorGrid;
    //        //spawn mercs:
    //        List<Pawn> mercs = new List<Pawn>();
    //        int i = 0;
    //        foreach(Pawn p in PartyMaster.Instance.currentMercParty)
    //        {
    //            Pawn newMerc = Instantiate(p.gameObject, floorGrid.floorTiles[level.mercSpawns[i].x, level.mercSpawns[i].y].transform.position, Quaternion.identity).GetComponent<Pawn>();
    //            //newMerc.Init(); //Initiated in CharacterMaster!
    //            newMerc.name = newMerc.name.Substring(0, newMerc.name.IndexOf('('));
    //            mercs.Add(newMerc);
    //            i++;
    //        }
    //        RefMaster.Instance.SetNewPawns(mercs, false); //inits mercs [isEnemies = false]
    //        RefMaster.Instance.SetNewPawns(level.enemies, true); //inits enemies [isEnemies = true]

    //        TurnMaster.Instance.GetReady();
    //        //Check for stuff?

    //        //GrabTargets()


    //        TurnMaster.Instance.StartTurning();

    //    }

    //    // Update is called once per frame
    //    void Update()
    //    {

    //    }
}

