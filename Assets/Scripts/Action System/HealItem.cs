using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : ActionItem
{
    public int range;
    public int healAmount;
    public float healDelay;
    [SerializeField]
    private LayerMask targetMask;

    //public Character ownerCharacter;
    public TileWalker tileWalker;

    public GameObject healEffect;
    FeetItem feetItem;

    public List<Pawn> targets;

    //public int _currentCooldown;
    //public int saCooldown;


    public override void Awake()
    {
        //ownerCharacter = GetComponent<Character>();
        tileWalker = GetComponent<TileWalker>();
        actionVariations = new List<ActionVariation>(); //not needed?
        feetItem = GetComponent<FeetItem>();
        base.Awake();
    }
    private void Start()
    {
        pawn._currentCooldown = 0;

        if (!pawn.isEnemy)
            targets = RefMaster.Instance.mercs;
        else
            targets = RefMaster.Instance.enemies;
    }

    public override void Action(GameObject tgt)
    {
        
        pawn._currentCooldown = pawn.saCooldown;
        Pawn tgtPawn = tgt.GetComponent<Pawn>();
        int dist = tileWalker.currentNode.GetDistanceToTarget(tgtPawn.tileWalker.currentNode);

        if(dist > range)
        {
            StartCoroutine(WalkThenHeal(tgtPawn));
            return;
        }

        //if dist <= 1 - just do this:

        
        int healRoll = Random.Range(-5, 5) + healAmount;

        tgtPawn.Heal(healRoll);

        GameObject go = Instantiate(healEffect, tgt.transform);
        Destroy(go, 2);
        
        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Heal, tgtPawn.Name, healRoll, Color.green);

        Invoke("CharacterHeal", healDelay);
    }

    IEnumerator WalkThenHeal(Pawn tgt)
    {
        pawn.tileWalker.StartNewPathWithRange(tgt.tileWalker, range);
        
        yield return new WaitUntil(() => !pawn.tileWalker.hasPath);
        
        Pawn p = tgt.GetComponent<Pawn>();
        int healRoll = Random.Range(-5, 5) + healAmount;

        p.Heal(healRoll);

        GameObject go = Instantiate(healEffect, tgt.transform);
        Destroy(go, 2);

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Heal, p.Name, healRoll, Color.green);

        Invoke("CharacterHeal", healDelay);
    }

    void CharacterHeal()
    {
        pawn.TurnDone = true;
    }

    public override void CalculateVariations()
    {
        actionVariations = new List<ActionVariation>();


        if (targets.Count <= 0 || pawn._currentCooldown > 0)
        {
            pawn._currentCooldown--;
            Debug.Log("No targest in List<Pawn> mercs or cooldown: " + pawn._currentCooldown.ToString());
            return;
        }



        foreach (Pawn p in targets)
        {
            if (p.currentHP >= p.maxHP) // instead of setting weight to 0, we just don't add this ActionVariation
            {
                continue;
            }

             //int weight = (1 - (p.currentHP / p.maxHP)) * baseCost;
             int weight = 10 * baseCost;
            //bool isMe


            if (p.currentHP < p.maxHP / 2) //if under half max
            {
                weight *= 8; //multiply again by 8
                if(p.name == name)
                {
                    weight += 2*(weight/8); //effectively makes it "times 10" [by adding (2/8 of 8x) we make it 10x]
                }
            }

            //Adjacency check 

            int currentDistance = tileWalker.currentNode.GetDistanceToTarget(FloorGrid.Instance.GetTileByIndex(p.tileWalker.gridPos));
            if (currentDistance <= 14) // one tile
            {
                weight *= 4;
                actionVariations.Add(new ActionVariation(this, p.gameObject, weight));
            }
            else
            {
                actionVariations.Add(new ActionVariation(this , p.gameObject, weight, true));
            }

            //else
            //{
            //    weight /= 2;
            //    actionVariations.Add(new ActionVariation(feetItem, p.gameObject, weight));
            //}
        }
    }
}
