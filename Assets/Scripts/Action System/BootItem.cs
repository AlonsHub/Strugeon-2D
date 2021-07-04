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

    public override void Awake()
    {
        actionVariations = new List<ActionVariation>();
        //tileWalker = GetComponent<TileWalker>();
        // pawn = GetComponent<Pawn>();
        weaponItem = GetComponent<WeaponItem>();
        base.Awake(); // getcomponent pawn
    }

    public override void Action(GameObject tgt)
    {
        pawn.tileWalker.Step(tgt.GetComponent<FloorTile>());
        StartCoroutine("CharacterStep");

    }
    IEnumerator CharacterStep()
    {
        //at this moment, a step moves the hero to another tile, and then they attack a random enemy character
        int randomEnemy = UnityEngine.Random.Range(0, RefMaster.Instance.enemies.Count - 1); //Exclusive?
        //weaponItem.Action(RefMaster.Instance.enemies[randomEnemy].gameObject);
        weaponItem.Action(pawn.targets[randomEnemy].gameObject);

        Debug.Log("Stepped!");
       // BattleLog.Instance.AddLine(name + " Step-back attacked: " + RefMaster.Instance.enemies[randomEnemy].name);
        yield return new WaitForSeconds(.1f);
        //pawn.TurnDone = true;
        //TurnOrder.Instance.NextTurn();
    }
    public override void CalculateVariations()
    {
        actionVariations = new List<ActionVariation>();

        //List<FloorTile> legitTiles = new List<FloorTile>();
        bool hasEnemyAtMelee = false;
        List<FloorTile> surroundingTiles = FloorGrid.Instance.GetNeighbours(pawn.tileWalker.currentNode);
        
        foreach (FloorTile ft in surroundingTiles)
        {
            if (ft.isEmpty)
            {
                actionVariations.Add(new ActionVariation(this, ft.gameObject, baseCost));
            }
            else if (ft.myOccupant != null)
            {
                if(ft.myOccupant.CompareTag("Enemy"))
                {
                    hasEnemyAtMelee = true;
                }
                
            }
        }

        if (!hasEnemyAtMelee)
        {
            actionVariations.Clear();

            Debug.LogError("No enemy in melee range");
            //Turn off the SA icon
            return;//?
        }

    }
}
