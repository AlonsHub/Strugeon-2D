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

    [ContextMenu("Summon")]
    public override void Action(GameObject tgt)
    {
        //tgt is the spawn-tile 
        GameObject go = Instantiate(drownedSpiritPrefab);
        Pawn p = go.GetComponent<Pawn>();

        TurnMaster.Instance.AddNewTurnTaker(p);
        cooldown = maxCooldown;
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
