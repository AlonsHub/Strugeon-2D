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
    [SerializeField]
    public Vector2Int escapeTile;
    
    public float startDelayAmount;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDisable()
    {
        Instance = null;
    }

    private void Start()
    {
        //Invoke(nameof(LoadNewLevel), startDelayAmount);
        Invoke(nameof(LoadNewLevelTurnMachine), startDelayAmount);
    }

    //public void LoadNewLevel(LevelData newLevelData)
    //public void LoadNewLevel()
    //{
    //    levelData = LevelRef.Instance.currentLevel.levelData;

    //    escapeTile = levelData.escapeTile;
    //    //instantiate level prefab for GFX/Environment assets ONLY
    //    //levelGFX = Instantiate(levelData.levelPrefab);
    //    List<Pawn> newEnemies = new List<Pawn>();
    //    List<Pawn> newMercs = new List<Pawn>();

    //    //temp AF

    //    List<StaticObastacle> staticObastacles = FindObjectsOfType<StaticObastacle>().ToList();
    //    foreach (var item in staticObastacles)
    //    {
    //        item.SpawnMeOnGrid();
    //    }
    //    //end temp AF

    //    //= levelData.enemies;
    //    //build floor:
    //    if (enemySpawnTiles.Count > 0)
    //    {
    //        //Spawn enemies:
    //        //int amount = levelData.enemies.Count;
    //        foreach (var p in levelData.enemies)
    //        {
    //            int rnd = Random.Range(0, enemySpawnTiles.Count); //draw random pos
    //            GameObject go = Instantiate(p.gameObject, p.gameObject.transform.position, p.gameObject.transform.rotation); // have enemy PARENT
    //            newEnemies.Add(go.GetComponent<Pawn>());
    //            FloorGrid.Instance.PlaceObjectOnGrid(go, enemySpawnTiles[rnd]);
    //            enemySpawnTiles.RemoveAt(rnd);
    //            //go.name.("(Clone)");
    //        }
    //        //RefMaster.Instance.enemyInstances = newEnemies; //sends enemies to refmaster to be kept there as the common ref for all components to use (AND NOT CACHED HERE)
    //        //RefMaster.Instance.enemyLevels = new List<int>(levelData.enemyLevels); //sends enemy levels to ref master (just making sure the list is not a ref to the level data list - since that may re-roll at some point, and we don't want any changes to carry over from the site to the arena after this moment anyways
    //        RefMaster.Instance.SetEnemyCharacters(newEnemies, new List<int>(levelData.enemyLevels)); //inits enemies
    //    }

    //        //Spawn mercs:
    //    if (mercSpawnTiles.Count > 0)
    //    {
    //        //foreach (var p in RefMaster.Instance.mercs)

    //        //foreach (var p in RefMaster.Instance.mercs)
    //        foreach (var p in PartyMaster.Instance.currentMercParty)
    //        {
    //            int rnd = Random.Range(0, mercSpawnTiles.Count); //draw random pos

    //            GameObject go = Instantiate(p.gameObject); // have merc PARENT
    //            //RefMaster.Instance.mercs.Add(go);
    //            newMercs.Add(go.GetComponent<Pawn>()); 

    //            FloorGrid.Instance.PlaceObjectOnGrid(go, mercSpawnTiles[rnd]);
    //            mercSpawnTiles.RemoveAt(rnd);
    //        }
    //        RefMaster.Instance.mercs = newMercs; //sends newMercs to refmaster to be kept there as the common ref for all components to use (AND NOT CACHED HERE)
    //        RefMaster.Instance.SetMercPawns(); //inits all mercs

    //        FloorGrid.Instance.SpawnObjectOnGrid(FloorGrid.Instance.censerPrefab, FloorGrid.Instance.censerGridPos); //move that data for censerGridPos to the levelData please TBF

    //        FloorGrid.Instance.SpawnObstacles(); //Different from StaticObstacles! this spawns random rocks and such

    //        StartCoroutine(nameof(DelayedStart));
    //    }

        
    //}
    public void LoadNewLevelTurnMachine()
    {
        levelData = LevelRef.Instance.currentLevel.levelData;

        escapeTile = levelData.escapeTile;
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
                GameObject go = Instantiate(p.gameObject, p.gameObject.transform.position, p.gameObject.transform.rotation); // have enemy PARENT
                newEnemies.Add(go.GetComponent<Pawn>());
                FloorGrid.Instance.PlaceObjectOnGrid(go, enemySpawnTiles[rnd]);
                enemySpawnTiles.RemoveAt(rnd);
                //go.name.("(Clone)");
            }
            //RefMaster.Instance.enemyInstances = newEnemies; //sends enemies to refmaster to be kept there as the common ref for all components to use (AND NOT CACHED HERE)
            //RefMaster.Instance.enemyLevels = new List<int>(levelData.enemyLevels); //sends enemy levels to ref master (just making sure the list is not a ref to the level data list - since that may re-roll at some point, and we don't want any changes to carry over from the site to the arena after this moment anyways
            RefMaster.Instance.SetEnemyCharacters(newEnemies, new List<int>(levelData.enemyLevels)); //inits enemies
        }

            //Spawn mercs:
        if (mercSpawnTiles.Count > 0)
        {
            //foreach (var p in RefMaster.Instance.mercs)

            //foreach (var p in RefMaster.Instance.mercs)
            foreach (var p in PartyMaster.Instance.currentMercParty)
            {
                int rnd = Random.Range(0, mercSpawnTiles.Count); //draw random pos

                GameObject go = Instantiate(p.gameObject); // have merc PARENT
                //RefMaster.Instance.mercs.Add(go);
                newMercs.Add(go.GetComponent<Pawn>()); 

                FloorGrid.Instance.PlaceObjectOnGrid(go, mercSpawnTiles[rnd]);
                mercSpawnTiles.RemoveAt(rnd);
            }
            RefMaster.Instance.mercs = newMercs; //sends newMercs to refmaster to be kept there as the common ref for all components to use (AND NOT CACHED HERE)
            RefMaster.Instance.SetMercPawns(); //inits all mercs

            FloorGrid.Instance.SpawnObjectOnGrid(FloorGrid.Instance.censerPrefab, FloorGrid.Instance.censerGridPos); //move that data for censerGridPos to the levelData please TBF

            FloorGrid.Instance.SpawnObstacles(); //Different from StaticObstacles! this spawns random rocks and such

            StartCoroutine(nameof(DelayedStartTurnMachine));
        }

        
    }

    //IEnumerator DelayedStart()
    //{
    //    yield return new WaitForSeconds(startDelayAmount);
    //    TurnMaster.Instance.GetReady();
    //}
    IEnumerator DelayedStartTurnMachine()
    {
        yield return new WaitForSeconds(startDelayAmount);
        TurnMachine.Instance.GetReady();
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

