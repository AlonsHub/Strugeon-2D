using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : ActionItem, SA_Item
{
    public int range;
    public int minHeal;
    public int maxHeal;
    public int healAmount;
    public float healDelay;
    [SerializeField]
    private LayerMask targetMask;

    //public Character ownerCharacter;
    public TileWalker tileWalker;

    public GameObject healEffect;
    FeetItem feetItem;

    public List<Pawn> targets;

    public int _currentCooldown;
    public int saCooldown;
    [SerializeField]
    Sprite healSprite;

    LookAtter la;

    public override void Awake()
    {
        //ownerCharacter = GetComponent<Character>();
        tileWalker = GetComponent<TileWalker>();
        actionVariations = new List<ActionVariation>(); //not needed?
        feetItem = GetComponent<FeetItem>();
        la = GetComponentInChildren<LookAtter>();

        targetAllies = true;

        base.Awake();
    }
    private void Start()
    {
        _currentCooldown = 0;

        //targets = pawn.isEnemy ? RefMaster.Instance.enemyInstances : RefMaster.Instance.mercs; //target acquistion should be generic as-well, by including bool targetAllies
        targets = (pawn.isEnemy == targetAllies) ? RefMaster.Instance.enemyInstances : RefMaster.Instance.mercs;//should be lowered as-is to base start

        //base.Start();
    }

    public override void Action(GameObject tgt)
    {

        StartCooldown();

        Pawn tgtPawn = tgt.GetComponent<Pawn>();
        int dist = tileWalker.currentNode.GetDistanceToTarget(tgtPawn.tileWalker.currentNode);

        if(dist > range * 14)
        {
            StartCoroutine(WalkThenHeal(tgtPawn));
            return;
        }
        //if dist <= 1 - just do the following below:
        la.LookOnce(tgt.transform);

        //HEAL ANIMATION????
        
        int healRoll = Random.Range(minHeal, maxHeal);

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
        int healRoll = Random.Range(minHeal, maxHeal);

        p.Heal(healRoll);

        GameObject go = Instantiate(healEffect, tgt.transform);
        Destroy(go, 2); //effects should selfdestruct TBF

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Heal, p.Name, healRoll, Color.green);

        Invoke(nameof(CharacterHeal), healDelay); //TBF!!!
    }

    void CharacterHeal()
    {
        pawn.TurnDone = true;//TBF!!!
    }

    public override void CalculateVariations()
    {
        actionVariations = new List<ActionVariation>();


        if (targets.Count <= 0 || _currentCooldown > 0)
        {
            _currentCooldown--;
            Debug.Log("No targest in List<Pawn> mercs or cooldown: " + _currentCooldown.ToString());
            return;
        }


        foreach (Pawn p in targets)
        {
            if (p.currentHP >= p.maxHP) // instead of setting weight to 0, we just don't add this ActionVariation
            {
                continue;
            }
            int weight = baseCost;

            int currentDistance = tileWalker.currentNode.GetDistanceToTarget(FloorGrid.Instance.GetTileByIndex(p.tileWalker.gridPos));
            if (currentDistance <= 14) // one tile
            {
                weight *= 4; //if adjcent
            }
            else if (pawn.HasRoot)
            {
                continue;
            }


            if (p.currentHP < p.maxHP / 2) //if under half max
            {
                if (p.name == name)
                {
                    weight *= 10;
                }
                else
                {
                    weight *= 5;
                }

                if (p.currentHP < p.maxHP / 5) //if under 20%
                {
                    weight *= 10;
                }
            }

            actionVariations.Add(new ActionVariation(this, p.gameObject, weight));

        }
    }

    public bool SA_Available()
    {
        return !(_currentCooldown > 0);
    }

    public int CurrentCooldown()
    {
        return _currentCooldown;
    }

    public void StartCooldown()
    {
        _currentCooldown = saCooldown;
    }

    public Sprite SA_Sprite()
    {
        return healSprite;
    }

    public string SA_Name()
    {
        return "Heal Ally";
    }

    public string SA_Description()
    {
        return "Smadi heals an ally or herself because she is an amazing person and we should all aspire to be more like her.";
    }
}
