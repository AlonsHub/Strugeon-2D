using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmItem : ActionItem //,SAITEM!!!!
{
    [SerializeField]
    int charmDuration; //in number of turns. Defualt value is 2
    [SerializeField]
    int initialWeight;
    public List<Pawn> targets;

    [SerializeField]
    GameObject charmVFX;

    //COOLDOWN ITEMS SHOULD JUST BE THIER OWN INHERITED CLASS BUT FINE FOR NOW WHATEVER IT DONT CARE... I do... I care a lot actually... I just really can't care about this specific detail right now. Be brave, and sorry future me if your unfucking this now and kind of upset with me, that being past you... who still sucks a bit, but getting better... god I hope you're better, but don't fret nothing if you aren't - it is hard to improve on something so polished yet fundementally flawed. I love you.
    
    int currentCooldown = 0;
    [SerializeField]
    int maxCooldown; //set in inspector

    private void Start()
    {
        targets = RefMaster.Instance.mercs; //because they are the only ones who can be charmed... should still be more dynamic though
    }

    public override void Action(GameObject tgt)
    {
        Charmed charmed = tgt.AddComponent<Charmed>();
        charmed.SetMe(tgt.GetComponent<WeaponItem>(), charmDuration);

        pawn.anim.SetTrigger("Charm");

        GameObject go = Instantiate(charmVFX, tgt.transform);

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Charm, tgt.GetComponent<Pawn>().Name);

        currentCooldown = maxCooldown;
        StartCoroutine(CountDownCool());
        pawn.TurnDone = true;
    }

    IEnumerator CountDownCool()
    {
        while(currentCooldown>0)
        {
            yield return new WaitUntil(() => !pawn.TurnDone);
            currentCooldown--;
            yield return new WaitUntil(() => pawn.TurnDone);
        }
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear(); //Most important to do this! when a merc looks for actionVariations - he may find stale ones here (even if returned on cooldown > 0)

        if(currentCooldown > 0)
        {
            return;
        }

        if (targets.Count <= 0)
        {
            Debug.Log(name + " Found no enemies, no weapon action variations added");
            return;// end match
        }

        int weight = initialWeight;

        foreach (Pawn p in targets)
        {
            actionVariations.Add(new ActionVariation(this, p.gameObject, weight));
        }
    }
}
