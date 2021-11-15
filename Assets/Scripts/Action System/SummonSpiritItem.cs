using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSpiritItem : ActionItem
{
    [SerializeField]
    GameObject drownedSpiritPrefab;

    [SerializeField]
    int cooldown; //start full
    [SerializeField]
    int maxCooldown;
    [SerializeField]
    GameObject summonVFX;

    [ContextMenu("Summon")]
    public override void Action(GameObject tgt)
    {
        //tgt is the spawn-tile 
        GameObject go = Instantiate(drownedSpiritPrefab);
        Pawn newSpirit = go.GetComponent<Pawn>();

        GameObject go2 = Instantiate(summonVFX, go.transform);

        go.transform.position = FloorGrid.Instance.GetRandomFreeTile().transform.position;

        //Vector3 v = (Vector3)Vector2.one * (FloorGrid.Instance.tileSize.x + FloorGrid.Instance.gapSize.x);

        //Random.Range(-4, 4);

        //go.transform.position += v; 
        //Battlelog entry! // ADD CHARM AND SUMMON TO ENUM with repectvie sprites
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
            actionVariations.Add(new ActionVariation(this,FloorGrid.Instance.GetTileByIndex(new Vector2Int(3,3)).gameObject , 1000));
        }
        else
        {

            cooldown--;
        }
    }
    //summon the shit

}
