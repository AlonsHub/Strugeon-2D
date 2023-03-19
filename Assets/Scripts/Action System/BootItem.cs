using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BootItem : ActionItem
{
    //public TileWalker tileWalker;
   // Pawn pawn;


    Pawn myTarget;

    WeaponItem weaponItem;
    ActionVariation actionVariation;
    public override void Awake()
    {
        actionVariations = new List<ActionVariation>();
        //tileWalker = GetComponent<TileWalker>();
        // pawn = GetComponent<Pawn>();
        weaponItem = GetComponent<WeaponItem>();
        base.Awake(); // getcomponent pawn
    }

    public override void Action(ActionVariation av)
    {
        actionVariation = av;
        pawn.tileWalker.Step(actionVariation.secondaryTarget.GetComponent<FloorTile>());
        //StartCoroutine(nameof(CharacterStep));
        weaponItem.Action(actionVariation);

        //Debug.Log("Stepped!");

    }
    //IEnumerator CharacterStep()
    //{
    //    //at this moment, a step moves the hero to another tile, and then they attack a random enemy character
    //    int randomEnemy = UnityEngine.Random.Range(0, weaponItem.targets.Count - 1); //Exclusive?
        
    //    //weaponItem.Action(weaponItem.targets[randomEnemy].gameObject);
        
    //   // BattleLog.Instance.AddLine(name + " Step-back attacked: " + RefMaster.Instance.enemies[randomEnemy].name);
    //    yield return new WaitForSeconds(.1f);
    //    //pawn.TurnDone = true;
    //    //TurnOrder.Instance.NextTurn();
    //}
    public override void CalculateVariations()
    {
        actionVariations = new List<ActionVariation>();
        if (pawn.HasRoot)
            return;
        //List<FloorTile> legitTiles = new List<FloorTile>();
        //bool hasEnemyAtMelee = false;
        List<FloorTile> surroundingTiles = FloorGrid.Instance.GetNeighbours(pawn.tileWalker.currentNode);
        List<GameObject> enemyTargets = new List<GameObject>();
        List<GameObject> tileTargets = new List<GameObject>();


        foreach (FloorTile ft in surroundingTiles)
        {
            if (ft.isEmpty)
            {
                tileTargets.Add(ft.gameObject);
                //actionVariations.Add(new ActionVariation(this, ft.gameObject, baseweight));
            }
            else if (ft.myOccupant != null && ft.myOccupant.CompareTag("Enemy"))
            {
                enemyTargets.Add(ft.myOccupant);
                    //hasEnemyAtMelee = true;
            }
        }

        if (enemyTargets.Count<=0)
        {
            actionVariations.Clear();
            //Turn off the SA icon
            return;//?
        }

        foreach (var enemy in enemyTargets)
        {
            foreach (var tile in tileTargets)
            {
                actionVariations.Add(new ActionVariation(this, enemy,tile.gameObject, baseweight));
            }
        }
    }
}
