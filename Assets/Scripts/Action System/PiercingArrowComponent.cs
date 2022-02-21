using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PiercingArrowComponent : MonoBehaviour //not a weapon item
{
    Pawn pawn;
    WeaponItem weaponItem;
    List<Pawn> relevantTargets; // don't init, this is mostly for ref purposes

    [SerializeField]
    int minDamage, maxDamage;
    void Awake()
    {
        pawn = GetComponent<Pawn>();
        weaponItem = GetComponent<WeaponItem>();
    }

    private void OnEnable()
    {
        weaponItem.attackAction += HitRandomRelevantTarget;
    }
    private void OnDisable()// isnt really relevant at the moment, but good to have
    {
        weaponItem.attackAction -= HitRandomRelevantTarget;
    }


    bool CanPierceTarget(GridPoser tgt) //returns if piercing is valid (could be useful for weight calculations)
    {
        List<Pawn> collateralPawns = FloorGrid.Instance.GetNeighbourOccupantsByType<Pawn>(tgt.GetGridPos());

        if(collateralPawns.Count == 0)
            return false;
        
        relevantTargets = collateralPawns.Where(x => x.isEnemy == !pawn.isEnemy).ToList(); //these will only splinter at enemies, EVEN WHEN CHARM IS IN EFFECT

        if (relevantTargets.Count == 0)
            return false;

        //At this point we know we have relevant targets (so we return true, but not before we keep that list of relevant targets


        return true;

        //no math yet, just aoe check (simple vector math will do the trig here ;3) 
    }

    public void HitRandomRelevantTarget()
    {
        if (!CanPierceTarget(weaponItem.toHit)) //gathers relevant targets aswell
            return;

        if(relevantTargets.Count ==0)
        {
            Debug.LogError("no relevant targets and somehow still got this far");
            return;
        }
        relevantTargets[Random.Range(0, relevantTargets.Count - 1)].TakeDamage(Random.Range(minDamage, maxDamage+1)); //+1 to max, cause its exclusive
    }
}
