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
        if (!pawn.isEnemy)
            targets = RefMaster.Instance.mercs;
        else
            targets = RefMaster.Instance.enemies;
    }

    public override void Action(GameObject tgt)
    {
        //BattleLog.Instance.AddLine(name + " Healed: " + tgt.name);
        
        //Character c = tgt.GetComponent<Character>();
        Pawn p = tgt.GetComponent<Pawn>();
        int healRoll = Random.Range(-5, 5) + healAmount;

        p.Heal(healRoll);

        GameObject go = Instantiate(healEffect, tgt.transform);
        //go.transform.position += Vector3.up * .2f;
        Destroy(go, 2);

        //pawn.anim.SetTrigger("Heal");
        //BatllelogVerticalGroup.Instance.AddEntry(pawn.name, ActionIcon.Heal, p.name);
        BattleLogVerticalGroup.Instance.AddEntry(pawn.name, ActionSymbol.Heal, tgt.name);


        Invoke("CharacterHeal", healDelay);
        // StartCoroutine("CharacterHeal");
    }

    void CharacterHeal()
    {
        pawn.TurnDone = true;
    }

    public override void CalculateVariations()
    {
        if (targets.Count <= 0)
        {
            Debug.Log("No targest in List<Pawn> mercs");
            return;
        }
       
        actionVariations = new List<ActionVariation>();

        foreach (Pawn p in targets)
        {
           

            if (p.currentHP >= p.maxHP)
            {
                continue;
            }
            int currentDistance = tileWalker.currentNode.GetDistanceToTarget(FloorGrid.Instance.GetTileByIndex(p.tileWalker.gridPos));

            int weight = (1 - (p.currentHP / p.maxHP)) * baseCost;

            if (p.currentHP < p.maxHP / 2)
            {
                weight *= 10;
                if(p.name == name)
                {
                    weight *= 2;
                   // Debug.LogError("Double weighting myself: " + weight);
                }
            }
            //if (currentDistance <= range)
            //{
                actionVariations.Add(new ActionVariation(this, p.gameObject, weight));
            //}
            //else
            //{
            //    weight /= 2;
            //    actionVariations.Add(new ActionVariation(feetItem, p.gameObject, weight));
            //}
        }
    }
}
