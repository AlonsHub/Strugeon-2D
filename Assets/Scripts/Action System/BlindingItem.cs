using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindingItem : ActionItem //, SA_Item
{
    [SerializeField]
    int duration;
    int currentCooldown = 0;
    [SerializeField]
    int maxCooldown;

    public List<Pawn> targets;


    private void Start()
    {
        targets = pawn.targets;
    }

    // Start is called before the first frame update
    public override void Action(GameObject tgt)
    {
        Blinded blinded = tgt.gameObject.AddComponent<Blinded>();
        blinded.SetMe(tgt.GetComponent<WeaponItem>(), duration);

        //pawn.anim.SetTrigger(""); //normal attack? have some vfx?
        currentCooldown = maxCooldown;
        StartCoroutine(CountDownCool());
        pawn.TurnDone = true;
    }

    IEnumerator CountDownCool()
    {
        while (currentCooldown > 0)
        {
            yield return new WaitUntil(() => !pawn.TurnDone);
            currentCooldown--;
            yield return new WaitUntil(() => pawn.TurnDone);
        }
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear(); //Most important to do this! when a merc looks for actionVariations - he may find stale ones here (even if returned on cooldown > 0)

        if (currentCooldown > 0)
        {
            return;
        }

        if (targets.Count <= 0)
        {
            Debug.Log(name + " Found no enemies, no weapon action variations added");
            return;// end match
        }

        int weight = baseCost;

        foreach (Pawn p in targets)
        {
            actionVariations.Add(new ActionVariation(this, p.gameObject, weight));
        }
    }
}
