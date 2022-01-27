using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeItem : ActionItem
{
    [SerializeField]
    float escapeThreshold;
    [SerializeField]
    int escapeWeight;
    
    void Start()
    {
        actionVariations = new List<ActionVariation>();
    }

    public override void Action(GameObject tgt)
    {

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Escape);

        PlayerDataMaster.Instance.currentPlayerData.cowardMercs++; // WILL BE A PROBLEM IS MERCS TRY TO ESCAPE BUT CANT!

        StartCoroutine("WalkToEscape");
        //pawn.Escape()
    }
    IEnumerator WalkToEscape()
    {
        //walk to edge of screen and disappear!
        pawn.tileWalker.StartNewPathWithRange(ArenaMaster.Instance.escapeTile);

        //tilewalk to PlayerDataMaster.Instance.currentLevel.escapeTile
        yield return new WaitUntil(() => !pawn.tileWalker.hasPath);

        pawn.Escape();
        //disappear

        pawn.TurnDone = true;
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear();

        //CHECK IF EVEN POSSIBLE!!!!!!

        //IF THEY HAVE NO PATH, THEY CAN'T ESCAPEEEE!!@!!

        if(pawn.currentHP <= pawn.maxHP/escapeThreshold)
        {
            actionVariations.Add(new ActionVariation(this, gameObject, escapeWeight));
        }
    }
}
