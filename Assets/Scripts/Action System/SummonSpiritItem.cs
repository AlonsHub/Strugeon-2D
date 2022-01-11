﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SummonSpiritItem : ActionItem, SA_Item
{
    [SerializeField]
    GameObject drownedSpiritPrefab;

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

    [ContextMenu("Summon")]
    public override void Action(GameObject tgt)
    {
        //tgt is the spawn-tile 
        GameObject go;



        List<FloorTile> adjTiles = new List<FloorTile>();
        //NEW
        if (pawn.isEnemy)
        {
            pawn.targets = RefMaster.Instance.mercs;
        }
        else
        {
            pawn.targets = RefMaster.Instance.enemies;
        }

        foreach (var merc in pawn.targets)
        {
            //
            adjTiles.AddRange(FloorGrid.Instance.GetNeighbours(merc.tileWalker.gridPos));
        }
        if (adjTiles.Count == 0)
        {
            FloorTile ft = FloorGrid.Instance.GetRandomFreeTile();
            go =FloorGrid.Instance.SpawnObjectOnGrid(drownedSpiritPrefab, ft.gridIndex);
        }
        else
        {
            List<FloorTile> emptyAdjTiles = adjTiles.Where(x => x.isEmpty).ToList();

            //go.transform.position = FloorGrid.Instance.GetRandomFreeTile().transform.position;
            FloorTile ft = emptyAdjTiles[Random.Range(0, emptyAdjTiles.Count)];

            go = FloorGrid.Instance.SpawnObjectOnGrid(drownedSpiritPrefab, ft.gridIndex);
        }
        Pawn newSpirit = go.GetComponent<Pawn>();
        GameObject go2 = Instantiate(summonVFX, go.transform);

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Summon, newSpirit.Name);

        RefMaster.Instance.enemies.Add(newSpirit);
        TurnMaster.Instance.AddNewTurnTaker(newSpirit);
        cooldown = maxCooldown;
        pawn.anim.SetTrigger("Summon");

        //endturn
        pawn.TurnDone = true;
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear();

        if (cooldown <= 0)
        {
            int weight = baseCost;

            if (pawn.isEnemy) //honestly should also check what happens if Mavka is ever charmed and then summons a spirit... Almost ironically, charms should work just fine when she's charmed
            {
                if (RefMaster.Instance.mercs.Count > 1)
                {
                    weight *= 10;
                }
            }
            else //doesn't ever happen as of now, but who knows?
            {
                if (RefMaster.Instance.enemies.Count > 1)
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

}
