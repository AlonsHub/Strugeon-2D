using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningTileEffect : TileEffect
{
    int minDamage, maxDamage;
    

    public void SetEffect(FloorTile ft, int minDmg, int maxDmg)
    {
        base.SetBase(ft);
        minDamage =minDmg;
        maxDamage =maxDmg;

        TryApplyEffectToOccupant();
        
    }
    public void SetEffect(Vector2Int gridpos, int minDmg, int maxDmg)
    {
        base.SetBase(gridpos);
        minDamage = minDmg;
        maxDamage = maxDmg;
    }

    public override void TryApplyEffectToOccupant()
    {
        if (floorTile.isEmpty)
            return;
        Pawn p;
        if(p = floorTile.myOccupant.GetComponent<Pawn>())
        {
            int rolledDamage = Random.Range(minDamage, maxDamage);
            p.TakeElementalDamage(rolledDamage, Color.yellow);

            BattleLogVerticalGroup.Instance.AddEntry("Fire", ActionSymbol.Censer, p.Name, rolledDamage, Color.yellow);
        }
    }
}
