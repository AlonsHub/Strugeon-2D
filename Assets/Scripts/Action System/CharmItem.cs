using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharmItem : ActionItem , SA_Item //,SAITEM!!!!
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

    public int saCooldown;
    [SerializeField]
    Sprite charmSprite;

    LookAtter la;

    private void Start()
    {
        targets = RefMaster.Instance.mercs; //because they are the only ones who can be charmed... should still be more dynamic though
        la = GetComponentInChildren<LookAtter>();
    }

    public override void Action(ActionVariation av)
    {
        CharmEffect charmEffect = new CharmEffect(av.target.GetComponent<Pawn>(), charmSprite, pawn); //passing off "pawn" being the mavka, as sourceCaster

        la.LookOnce(av.target.transform);

        pawn.anim.SetTrigger("Charm");

        GameObject go = Instantiate(charmVFX, av.target.transform);

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Charm, av.target.GetComponent<Pawn>().Name);

        currentCooldown = maxCooldown;
        StartCoroutine(CountDownCool());
        pawn.FinishAnimation();
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

        bool isFlanked = FloorGrid.Instance.GetNeighbours(pawn.tileWalker.gridPos).Where(x => x.myOccupant && x.myOccupant.CompareTag("Merc")).ToList().Count > 1;

        foreach (Pawn p in targets)
        {
            int weight = initialWeight;
            //if (pawn.tileWalker.currentNode.GetDistanceToTarget(p.tileWalker.currentNode) / 14 == 1)
            if (pawn.tileWalker.GetDistanceFromMeToYou(p.tileWalker) / 14 == 1)
            {
                weight *= 10;
                if(pawn.maxHP/pawn.currentHP >=2) //50%
                {
                    weight *= 10;
                }
                if (isFlanked)
                    weight *= 5;
            }

            actionVariations.Add(new ActionVariation(this, p.gameObject, weight));
        }
    }

    public bool SA_Available()
    {
        return !(currentCooldown > 0);
    }

    public int CurrentCooldown()
    {
        return currentCooldown;
    }

    public void StartCooldown()
    {
        currentCooldown = saCooldown;
    }

    public Sprite SA_Sprite()
    {
        return charmSprite;
    }
    public string SA_Name()
    {
        return "Summon Drowned Spirit";
    }

    public string SA_Description()
    {
        return "is this ever relevant?"; //we'll see...
    }

    public void SetToLevel(int level)
    {
        //do nothing
    }
}
