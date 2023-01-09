using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SummonSpiritItem : ActionItem, SA_Item
{
    [SerializeField]
    GameObject drownedSpiritPrefab;
    //[SerializeField]
    int drownedSpiritLevel => (int)Mathf.Clamp(((float)pawn.enemyLevel / 3f), 1f, pawn.enemyLevel); //LEVEL UP ADVANCEMENT!

    [SerializeField]
    int cooldown; //start full
    [SerializeField]
    int maxCooldown;
    [SerializeField]
    GameObject summonVFX;

    //public int _currentCooldown;
    public int saCooldown;
    [SerializeField]
    Sprite summonSprite;

    LookAtter la;
   
    private void Start()
    {
        la = GetComponentInChildren<LookAtter>();
    }

        GameObject instantiatedPawn;
    [ContextMenu("Summon")]
    public override void Action(GameObject tgt)
    {
        //tgt is the spawn-tile 
        //GameObject go;

        FloorTile floorTile;


        List<FloorTile> adjTiles = new List<FloorTile>();
        //NEW
        if (pawn.isEnemy)
        {
            pawn.targets = RefMaster.Instance.mercs;
        }
        else
        {
            pawn.targets = RefMaster.Instance.enemyInstances;
        }

        foreach (var merc in pawn.targets)
        {
            adjTiles.AddRange(FloorGrid.Instance.GetNeighbours(merc.tileWalker.gridPos));
        }

        if (adjTiles.Count == 0)
        {
            //FloorTile ft = FloorGrid.Instance.GetRandomFreeTile();
            floorTile = FloorGrid.Instance.GetRandomFreeTile();
            //go = FloorGrid.Instance.SpawnObjectOnGrid(drownedSpiritPrefab, floorTile.gridIndex);
        }
        else
        {
            List<FloorTile> emptyAdjTiles = adjTiles.Where(x => x.isEmpty).ToList();

            //go.transform.position = FloorGrid.Instance.GetRandomFreeTile().transform.position;
            //FloorTile ft = emptyAdjTiles[Random.Range(0, emptyAdjTiles.Count)];
            if(emptyAdjTiles.Count != 0)
            floorTile = emptyAdjTiles[Random.Range(0, emptyAdjTiles.Count)];
            else
                floorTile = FloorGrid.Instance.GetRandomFreeTile();


            //go = FloorGrid.Instance.SpawnObjectOnGrid(drownedSpiritPrefab, ft.gridIndex);
        }
        instantiatedPawn = FloorGrid.Instance.SpawnObjectOnGrid(drownedSpiritPrefab, floorTile.gridIndex);
        if(la && instantiatedPawn)
        la.LookOnce(instantiatedPawn.transform);
        //List<FloorTile> adjTiles = new List<FloorTile>();
        ////NEW
        //if (pawn.isEnemy)
        //{
        //    pawn.targets = RefMaster.Instance.mercs;
        //}
        //else
        //{
        //    pawn.targets = RefMaster.Instance.enemyInstances;
        //}

        //foreach (var merc in pawn.targets)
        //{
        //    //
        //    adjTiles.AddRange(FloorGrid.Instance.GetNeighbours(merc.tileWalker.gridPos));
        //}
        //if (adjTiles.Count == 0)
        //{
        //    FloorTile ft = FloorGrid.Instance.GetRandomFreeTile();
        //    go =FloorGrid.Instance.SpawnObjectOnGrid(drownedSpiritPrefab, ft.gridIndex);
        //}
        //else
        //{
        //    List<FloorTile> emptyAdjTiles = adjTiles.Where(x => x.isEmpty).ToList();

        //    //go.transform.position = FloorGrid.Instance.GetRandomFreeTile().transform.position;
        //    FloorTile ft = emptyAdjTiles[Random.Range(0, emptyAdjTiles.Count)];

        //    go = FloorGrid.Instance.SpawnObjectOnGrid(drownedSpiritPrefab, ft.gridIndex);
        //}
        //Pawn newSpirit = go.GetComponent<Pawn>();
        //GameObject go2 = Instantiate(summonVFX, go.transform);

        //BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Summon, newSpirit.Name);

        //RefMaster.Instance.enemyInstances.Add(newSpirit);
        //TurnMaster.Instance.AddNewTurnTaker(newSpirit);
        //cooldown = maxCooldown;


        pawn.anim.SetTrigger("Summon"); //plays summon animation which triggers Summon() by animation event

        //endturn
    }

    public void Summon() //called by animationEvent in summon animation
    {
        
        Pawn newSpirit = instantiatedPawn.GetComponent<Pawn>();

        //LEVEL ADVANCEMENT
        newSpirit.GetComponent<EnemyInfo>().SetEnemyLevel(drownedSpiritLevel);

        GameObject go2 = Instantiate(summonVFX, instantiatedPawn.transform);

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Summon, newSpirit.Name);

        RefMaster.Instance.enemyInstances.Add(newSpirit);
        //TurnMaster.Instance.AddNewTurnTaker(newSpirit);
        TurnMachine.Instance.InsertTurnTakerAsNext(newSpirit);
        cooldown = maxCooldown;

        //la.tgt = null; not needed after LookOnce was introduced.

        pawn.TurnDone = true;
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear();

        if (cooldown <= 0)
        {
            int weight = baseweight;

            if (pawn.isEnemy) //honestly should also check what happens if Mavka is ever charmed and then summons a spirit... Almost ironically, charms should work just fine when she's charmed
            {
                if (RefMaster.Instance.mercs.Count > 1)
                {
                    weight *= 10;
                }
            }
            else //doesn't ever happen as of now, but who knows?
            {
                if (RefMaster.Instance.enemyInstances.Count > 1)
                {
                    weight *= 10;
                }
            }
            if(pawn.maxHP/pawn.currentHP >2)
                weight *= 10;

            //actionVariations.Add(new ActionVariation(this,FloorGrid.Instance.GetTileByIndex(new Vector2Int(3,3)).gameObject , baseCost));
            actionVariations.Add(new ActionVariation(this,gameObject , weight)); //tgt is meaningless here
        }
        else
        {

            cooldown--;
        }
    }

    public bool SA_Available()
    {
        return !(cooldown > 0);
    }

    public int CurrentCooldown()
    {
        return cooldown;
    }

    public void StartCooldown()
    {
        cooldown = saCooldown;
    }

    public Sprite SA_Sprite()
    {
        return summonSprite;
    }
    public string SA_Name()
    {
        return "Summon Drowned Spirit";
    }

    public string SA_Description()
    {
        return "is this ever relevant?"; //we'll see...
    }

    public void SetToLevel(int level)
    {
        //do nothing! drownedSpiritLevel is calculated by mavka level/3
    }
}
