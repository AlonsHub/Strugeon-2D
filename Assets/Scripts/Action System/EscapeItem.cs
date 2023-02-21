using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeItem : ActionItem
{
    [SerializeField]
    float escapeThreshold;
    [SerializeField]
    int escapeWeight;
    [SerializeField]
    bool healthEffectsWegiht;
    [SerializeField, Tooltip("The full weight added (theoretically) if HP is at 0. Only relevant if healthEffectWeight is true.")]
    int maxAddedWeight;

    [SerializeField, Tooltip("E.g. Smadi only escapes if she's the last merc around -> Escapes Last = true")]
    bool escapesLast; //E.g. Smadi only escapes if she's the last merc around
    //[SerializeField]
    //bool modif
    [SerializeField, Tooltip("Added weight per dead ally")]
    int weightPerDeadAlly;
    [SerializeField, Tooltip("Added weight per escaped ally")]
    int weightPerEscapedAlly;

    void Start()
    {
        actionVariations = new List<ActionVariation>();
    }

    public override void Action(GameObject tgt)
    {

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Escape);

        PlayerDataMaster.Instance.currentPlayerData.cowardMercs++; // WILL BE A PROBLEM IS MERCS TRY TO ESCAPE BUT CANT!

        StartCoroutine(nameof(WalkToEscape));
        //pawn.Escape()
    }
    IEnumerator WalkToEscape()
    {
        //walk to edge of screen and disappear!
        pawn.tileWalker.StartNewPathWithRange(ArenaMaster.Instance.escapeTile); //should just be "walk to one edge of the screen"

        //tilewalk to PlayerDataMaster.Instance.currentLevel.escapeTile
        yield return new WaitUntil(() => !pawn.tileWalker.hasPath);

        //what happenes if not?!

        pawn.Escape();
        //disappear

        //pawn.TurnDone = true;
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear();

        //IF THEY HAVE NO PATH, THEY CAN'T ESCAPEEEE!!@!!
        if (pawn.HasRoot)
            return;

        if (escapesLast) //ONLY IF ESCAPELAST
        {
            if (!(RefMaster.Instance.mercs.Count == 1 && RefMaster.Instance.mercs[0].Name == pawn.Name))
            {
                //if NOT the last merc, stop considering escape
                return;
            }
        }

        if (pawn.currentHP > pawn.maxHP / escapeThreshold)
        {
            return;
        }

        int weight = escapeWeight;

        //if (weightPerDeadAlly != 0 && TurnMaster.Instance.theDead.Count > 0)
        if (weightPerDeadAlly != 0 && RefMaster.Instance.GetTheDead.Count > 0)
        {
            weight += weightPerDeadAlly * RefMaster.Instance.GetTheDead.Count;
        }
        if (weightPerDeadAlly != 0 && RefMaster.Instance.GetTheCowardly.Count > 0)
        {
            weight += weightPerDeadAlly * RefMaster.Instance.GetTheCowardly.Count;
        }

        if (healthEffectsWegiht)
        {
            weight += (int)((1f - ((float)pawn.currentHP / (float)pawn.maxHP)) * maxAddedWeight);
        }

        //actionVariations.Add(new ActionVariation(this, gameObject, escapeWeight)); //* consider formulating escapeWeight// 
        actionVariations.Add(new ActionVariation(this, gameObject, weight)); //* consider formulating escapeWeight// did :)
    }
}
