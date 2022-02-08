using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum DamageType {Slashing, Bludgening, Piercing, Pure}; // Pure? True? 

public class WeaponItem : ActionItem
{
    public FeetItem feetItem; //does the walking as an actionvariation added by weaponitem, but holding the feetItem in the List entry of actionsvariations 

    //added params:Range, Damage, ?
    public int range;
    public int minDamage, maxDamage;

    public GameObject arrowGfx; //arrow or spear
    [SerializeField]
    bool isRanged;
    public Transform arrowSpawn; // 

    public Transform effectSpawn;

    [SerializeField]
    private LayerMask targetMask;

    //public TileWalker tileWalker;

   
    public GameObject elementIconObj;
    

    public bool hasEffect; //default: false
    public Color effectColour; //default: red

    [SerializeField]
    EffectAddonDataType effectData; //GET WHAT EVER SETS THIS
    public DamageType damageType;

    public List<Pawn> targets;
    public bool hasRedBuff;
    public Pawn toHit;

    LookAtter la;

    public System.Action attackAction; //not sure if this is even used



    public override void Awake()
    {
        actionVariations = new List<ActionVariation>();
        isWeapon = true;
        hasEffect = false;
        hasRedBuff = false;
        feetItem = GetComponent<FeetItem>();

        attackAction += OnAttack;

        base.Awake();
    }

    private void Start()
    {
        if (pawn.isEnemy)
            targets = RefMaster.Instance.mercs;
        else
            targets = RefMaster.Instance.enemies;

        // arrowSpawn = pawn.arrow
        la = GetComponentInChildren<LookAtter>();

    }
    public override void Action(GameObject tgt)
    {
        toHit = tgt.GetComponent<Pawn>();

        if (!toHit)
        {
            Debug.Log("no tgt to hit");
            return;
        }


        int dist = pawn.tileWalker.currentNode.GetDistanceToTarget(toHit.tileWalker.currentNode);

        if (dist > range*14)
        {
            StartCoroutine(WalkThenAttack(toHit));
            return;
        }

        if (tgt && la)
        la.tgt = tgt.transform;


       
        attackAction?.Invoke();
        //pawn.transform.LookAt(tgt.transform);
        //pawn.transform.rotation = Quaternion.Euler(0, pawn.transform.eulerAngles.y, 0);
        pawn.anim.SetTrigger("Attack"); // sets TurnDone via animation behaviour
    }

    IEnumerator WalkThenAttack(Pawn tgt)
    {
        pawn.tileWalker.StartNewPathWithRange(tgt.tileWalker, range);

        yield return new WaitUntil(() => !pawn.tileWalker.hasPath || pawn.TurnDone); // 

        if (!pawn.TurnDone) //in case of step limiters
        {
            if (tgt && la)
                la.tgt = tgt.transform;

            attackAction?.Invoke();
            pawn.anim.SetTrigger("Attack"); // sets TurnDone via animation behaviour
        }
    }

    
    public void ShootProjectile()
    {
        GameObject effectGo = Instantiate(arrowGfx, arrowSpawn.position, arrowSpawn.rotation);
        effectGo.GetComponent<Arrow>().tgt = toHit.transform;
        StartCoroutine(WaitForArrowToHit(effectGo));
    }// called by animation events exclusively (11/11/2021)

    IEnumerator WaitForArrowToHit(GameObject arrow) //or die, currently always hits
    {
        yield return new WaitUntil(() => (arrow == null));
        Debug.Log("Arrow hit");
        DamageTarget();
    }
    public void DamageTarget()
    {
        //float rolledDamage = damage + Random.Range(-5, 6);
        float rolledDamage = Random.Range(minDamage, maxDamage);
        if (toHit == null || toHit.currentHP <= 0)
        {
            BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Attack, toHit.Name, (int)rolledDamage, Color.red);

            la.tgt = null;
            pawn.TurnDone = true;
            return;
        }

        if (hasEffect)
        {
            int bonusDamage = effectData.bonusDamage + Random.Range(-5, 6);
            // Instantiate(effectData.effectGFXPrefab, go.transform.GetChild(0).GetChild(0));

            effectData.currentUses--;
            toHit.TakeElementalDamage(bonusDamage, effectColour); // Should be toHit.TakeElementalDamage
            if (effectData.currentUses <= 0)
            {
                hasEffect = false;
                effectData = null;
                pawn.RemoveIconByColor("fireBuff");
                effectColour = Color.black;
            }

            if(toHit == null || toHit.currentHP <= 0)
            {
                BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Attack, toHit.Name, (int)rolledDamage, Color.red);

                la.tgt = null;
                pawn.TurnDone = true;
                return;
            }
        }
        if (hasRedBuff)
        {
            rolledDamage *= 1.5f;
            hasRedBuff = false;
            pawn.RemoveIconByColor("redBuff");
        }

        if (rolledDamage < 0)
            rolledDamage = 0;

        toHit.TakeDamage((int)rolledDamage); // add time delay to reduce HP only after hit (atm this is done in TakeDamage and ReduceHP methods in character)
        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Attack, toHit.Name, (int)rolledDamage ,Color.red);

        la.tgt = null;
        pawn.TurnDone = true;
        //go.transform.LookAt(tgt.transform);
        //GetComponent<LookAt>().lookAtTargetPosition = tgt.transform.position;
    }
    public void ExtraDamageTarget(int minDmg, int maxDmg)
    {
        float rolledDamage = Random.Range(minDmg, maxDmg);

        toHit.TakeDamage((int)rolledDamage); // add time delay to reduce HP only after hit (atm this is done in TakeDamage and ReduceHP methods in character)
        //BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Attack, toHit.Name, (int)rolledDamage, Color.red);

        pawn.TurnDone = true;
        //go.transform.LookAt(tgt.transform);
        //GetComponent<LookAt>().lookAtTargetPosition = tgt.transform.position;
        la.tgt = null;
    }
    public void AddEffect(WeaponEffectAddon effectAddon)
    {
        hasEffect = true;
        effectData = effectAddon.data;

        pawn.TurnDone = true;
    }
    public override void CalculateVariations()
    {
        //actionVariations = new List<ActionVariation>();
        actionVariations.Clear();

        if (targets.Count == 0)
        {
            Debug.Log(name + " Found no enemies, no weapon action variations added");
            return;// end match
        }


        if(feetItem)
        feetItem.currentRangeInTiles = range; // maybe do this at Start()?

        int weight = baseCost;
        foreach(Pawn p in targets)
        {
            int currentDistance = pawn.tileWalker.currentNode.GetDistanceToTarget(p.tileWalker.currentNode);

            if (p.currentHP <= 0)
                continue;
            //if(p.currentHP > 0)
            //{
            //    weight *= 5;
                
            //}

            if (currentDistance <= range *14)
            {
                weight = baseCost;
                if (isRanged && currentDistance <= 14) //14 is one tile - makes sure you're not in melee range with target
                {
                    //according to GDD this should multiply by 10 and shouldn't "continue" (meaning the loop should not break)
                    continue;
                }
                weight *= 2; // changed from 20

                if (p.currentHP <= p.maxHP / 2.5f) //40%
                {
                    weight *= 10;
                }
            }

            if (weight != 0)
            {
                actionVariations.Add(new ActionVariation(this, p.gameObject, weight));
            }
        }

        CallBehaveVariables();

    }

    public void ApplySheet(MercSheet ms)
    {
        //takes the whole MercSheet just in case we want to get more info later

        minDamage += ms._minDamageBonus;
        maxDamage += ms._maxDamageBonus;
    }

    void OnAttack() //just something to have in the attackAction. Currently holds nothing
    {
        Debug.Log("ON ATTACK ACTION " + name);
    }
}
