using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmItem : ActionItem
{
    [SerializeField]
    int charmDuration; //in number of turns. Defualt value is 2
    [SerializeField]
    int initialWeight;
    public List<Pawn> targets;
    private void Start()
    {
        targets = RefMaster.Instance.mercs;
    }

    public override void Action(GameObject tgt)
    {
        Charmed charmed = tgt.AddComponent<Charmed>();
        charmed.SetMe(tgt.GetComponent<WeaponItem>(), charmDuration);
        pawn.TurnDone = true;
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear();

        

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
