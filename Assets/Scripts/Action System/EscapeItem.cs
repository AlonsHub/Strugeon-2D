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

        pawn.TurnDone = true;
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear();

        //CHECK IF EVEN POSSIBLE!!!!!!

        //IF THEY HAVE NO PATH, THEY CAN'T ESCAPEEEE!!@!!
        if (pawn.HasRoot)
            return;
        if(pawn.currentHP <= pawn.maxHP/escapeThreshold)
        {
            //check here if there is a path to escape - by either trying to get a route there, or inversely checking if hes not surrounded
            actionVariations.Add(new ActionVariation(this, gameObject, escapeWeight)); //* consider formulating escapeWeight
        }
    }
}
