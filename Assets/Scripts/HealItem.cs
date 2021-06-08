﻿using System.Collections;
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
        //BattleLog.Instance.AddLine(name + " Healed: " + tgt.name);
        pawn._currentCooldown = pawn.saCooldown;
        //Character c = tgt.GetComponent<Character>();
        Pawn p = tgt.GetComponent<Pawn>();
        int healRoll = Random.Range(-5, 5) + healAmount;

        p.Heal(healRoll);

        GameObject go = Instantiate(healEffect, tgt.transform);
        //go.transform.position += Vector3.up * .2f;
        Destroy(go, 2);

        //pawn.anim.SetTrigger("Heal");
        //BatllelogVerticalGroup.Instance.AddEntry(pawn.name, ActionIcon.Heal, p.name);

        //BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Heal, tgt.name, healRoll, Color.green);
        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Heal, p.Name, healRoll, Color.green);


        Invoke("CharacterHeal", healDelay);
        // StartCoroutine("CharacterHeal");
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
