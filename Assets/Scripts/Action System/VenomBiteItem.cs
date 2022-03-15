using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomBiteItem : ActionItem, SA_Item
{
    [SerializeField]
    Sprite venomBiteSprite;
    [SerializeField]
    string venomBiteSpriteName;

    [SerializeField]
    int minDamage, maxDamage; //this sets them all

    int _currentCooldown;

    [SerializeField]
    int fullCooldown;

    [SerializeField]
    int posionDuration;

    List<Pawn> targets;
    public bool SA_Available()
    {
        return !(_currentCooldown > 0);
    }

    public string SA_Description()
    {
        return "poisons stuff [I don't think this ever appears anyhere]";
    }

    public string SA_Name()
    {
        return "Venom Bite";
    }

    public Sprite SA_Sprite()
    {
        return venomBiteSprite;
    }

    public void StartCooldown()
    {
        _currentCooldown = fullCooldown;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentCooldown = 0;
    }
    public override void Action(GameObject tgt)
    {
        PoisonAttacher pa = tgt.AddComponent<PoisonAttacher>();
        pa.SetMeFull(tgt.GetComponent<Pawn>(), "poison", posionDuration, minDamage, maxDamage);
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear();

        if (targets.Count == 0)
        {
            Debug.Log(name + " Found no enemies, no weapon action variations added");
            return;// end match
        }


        //if (feetItem)
        //    feetItem.currentRangeInTiles = range; // maybe do this at Start()?

        foreach (Pawn p in targets)
        {
            int weight = baseCost;

            int currentDistance = pawn.tileWalker.currentNode.GetDistanceToTarget(p.tileWalker.currentNode);

            if (p.currentHP <= 0)
                continue;
            //if(p.currentHP > 0)
            //{
            //    weight *= 5;

            //}

            if (currentDistance <= range * 14) //melee=14 ranged is more
            {
                if (isRanged && currentDistance <= 14) //14 is one tile - makes sure you're not in melee range with target
                {
                    //according to GDD this should multiply by 10 
                    continue;
                }
                //melee attacker only have 1 range, so this means adjacent
                weight *= 20; // changed from 20 to 2 //changed back to 20

            }
            if (p.currentHP <= p.maxHP / 2.5f) //40%
            {
                weight *= 10;
            }

            if (weight != 0)
            {
                actionVariations.Add(new ActionVariation(this, p.gameObject, weight));
            }
        }

    }
}
