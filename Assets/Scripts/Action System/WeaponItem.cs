using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum DamageType {Slashing, Bludgening, Piercing, Pure}; // Pure? True? 

public class WeaponItem : ActionItem
{
    //WeaponPrefs weaponPrefs; //is it better to cache once from character sheet, or access directly to sheet?
    WeaponPrefs weaponPrefs => pawn._mercSheet.basicPrefs.weaponPrefs; //is it better to cache once from character sheet, or access directly to sheet?
    TargetHealthPrefs targetHealthPrefs => pawn._mercSheet.basicPrefs.targetHealthPrefs; //is it better to cache once from character sheet, or access directly to sheet?

    public FeetItem feetItem; //does the walking as an actionvariation added by weaponitem, but holding the feetItem in the List entry of actionsvariations 

    //added params:Range, Damage, ?
    //public int range => statBlock.range;
    public int range;

    //I WANT THIS BACK TBD TBF
    //public int maxDamage => pawn._mercSheet._maxDamage;
    //public int minDamage => pawn._mercSheet._minDamage;
    //I WANT THIS BACK

    //THIS SHOULD NOT STAY! TBD TBF
    public int maxDamage;
    public int minDamage;


    //TEMP! This should just be the weapon-related part of the block TBD, seperate Stat Block to components/modules
    StatBlock statBlock;
    

    //temp? BONUSES and Benefits? should refer to the items? can they be canacelled, destoried, amplified, muted during a fight? if so... then yes.

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
    public Pawn pawnToHit;

    LookAtter la;

    public System.Action attackAction; //not sure if this is even used //holy shit is it ever boiii! thank you! <3... this is weird coming from myself
    public System.Action hitAction;

    
     public GameObject cachedProjectile; //public for addcomponent only!

 

    public override void Awake()
    {
        actionVariations = new List<ActionVariation>();
        hasEffect = false;
        hasRedBuff = false;
        feetItem = GetComponent<FeetItem>();
        //weaponPrefs = pawn._mercSheet.basicPrefs.weaponPrefs; //weaponPrefs is NOT a struct, so this REF does NOT duplicate memory, 
        //but currently this approach is abandoned

        la = GetComponentInChildren<LookAtter>();


        base.Awake();
    }
    private void Start()
    {
        if (pawn.isEnemy)
            targets = RefMaster.Instance.mercs;
        else
            targets = RefMaster.Instance.enemyInstances;
    }
    public void Init(List<Pawn> tgts, StatBlock sb)
    {
        //if (pawn.isEnemy)
        //    targets = RefMaster.Instance.mercs;
        //else
        //    targets = RefMaster.Instance.enemyInstances;
        statBlock = sb;

        //targets = tgts;

        //TEMP! SHOULD BE THAT lamda expression ALL THE WAY
        maxDamage = pawn._mercSheet._maxDamage;
        minDamage = pawn._mercSheet._minDamage;
        range = statBlock.range;
        //TEMP! SHOULD BE THAT lamda expression ALL THE WAY

        if (feetItem)
            feetItem.currentRangeInTiles = range; // maybe do this at Start()? // Yup... if at all
        // arrowSpawn = pawn.arrow
        //la = GetComponentInChildren<LookAtter>();

        if(range > 1 && arrowGfx)
        {
            LoadAndCacheProjectile();
        }
    }

    private void LoadAndCacheProjectile()
    {
        cachedProjectile = Instantiate(arrowGfx, arrowSpawn.position, arrowSpawn.rotation);
        cachedProjectile.SetActive(false);
    }

    public override void Action(ActionVariation av)
    {
        pawnToHit = av.target.GetComponent<Pawn>();

        if (!pawnToHit)
        {
            Debug.Log("no tgt to hit");
            return;
        }


        //int dist = pawn.tileWalker.currentNode.GetDistanceToTarget(toHit.tileWalker.currentNode);
        int dist = pawn.tileWalker.GetDistanceFromMeToYou(pawnToHit.tileWalker);



        if (dist > range*14)
        {
            StartCoroutine(WalkThenAttack(pawnToHit));
            return;
        }

        if (av.target && la)
            la.LookOnce(av.target.transform);




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
                la.LookOnce(tgt.transform);


            attackAction?.Invoke();
            pawn.anim.SetTrigger("Attack"); // sets TurnDone via animation behaviour
        }
    }

    
    public void ShootProjectile() //called by animation event
    {

        //cachedProjectile = Instantiate(arrowGfx, arrowSpawn.position, arrowSpawn.rotation);
        cachedProjectile.transform.position = arrowSpawn.position;
        cachedProjectile.transform.rotation = arrowSpawn.rotation;
        //cachedProjectile.GetComponent<Arrow>().tgt = toHit.transform;
        cachedProjectile.GetComponent<Arrow>().SetTarget(pawnToHit.transform);

        cachedProjectile.SetActive(true);

        StartCoroutine(WaitForArrowToHit(cachedProjectile));
    }// called by animation events exclusively (11/11/2021)

    IEnumerator WaitForArrowToHit(GameObject arrow) //or die, currently always hits
    {
        yield return new WaitUntil(() => (arrow == null)); //maybe check if arrow ==null || toHit ==null in case the target dies somehow TBF
        //Debug.Log("Arrow hit");
        //cachedProjectile = null;
        DamageTarget(); 
        LoadAndCacheProjectile();
    }
    public void DamageTarget()
    {
        
        float rolledDamage = Random.Range(minDamage, maxDamage+1); //+1 to max, since it is still rolling ints not floats - so its exclusive
        
        //TBF - has effect should add to variable that resets here: //int bonusElementalDamage.

        if (hasEffect)
        {
            int bonusDamage = effectData.bonusDamage + Random.Range(-5, 6);
            // Instantiate(effectData.effectGFXPrefab, go.transform.GetChild(0).GetChild(0));

            effectData.currentUses--;
            pawnToHit.TakeElementalDamage(bonusDamage, effectColour); // Should be toHit.TakeElementalDamage //should really just add to the rolled damamge and report both separatly
            if (effectData.currentUses <= 0)
            {
                hasEffect = false;
                effectData = null;
                pawn.RemoveIconByName("fireBuff");
                effectColour = Color.black;
            }

            if(pawnToHit == null || pawnToHit.currentHP <= 0)
            {
                BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Attack, pawnToHit.Name, (int)rolledDamage, Color.red);

                la.tgt = null;
                //pawn.TurnDone = true;
                pawn.FinishAnimation();
                return;
            }
        }
        //if (hasRedBuff)
        //{
        //    rolledDamage *= 1.5f;
        //    hasRedBuff = false;
        //    pawn.RemoveIconByName("redBuff");
        //}

        if (rolledDamage < 0)
            rolledDamage = 0;
        
        //status effect run
        StatusEffect[] outgoingDamageEffects = pawn.GetStatusEffectsByPredicate(x => x is I_StatusEffect_OutgoingDamageMod);
        if(outgoingDamageEffects != null && outgoingDamageEffects.Length !=0 )
        {
            foreach (var item in outgoingDamageEffects)
            {
                rolledDamage = (item as I_StatusEffect_OutgoingDamageMod).OperateOnDamage(rolledDamage);
            }
        }

        //TEMP Check HitGrazeCrit here for now? 

        int toHitRoll = Random.Range(1, 101); //1-100
        if(toHitRoll < statBlock.grazeChance)
        {
            //graze!
            rolledDamage *= statBlock.grazeDamagePercentage / 100f; 
        }
        else if(toHitRoll >= 100 - statBlock.critChance)
        {
            rolledDamage *= statBlock.critDamagePercentage / 100f; 
        }
        //else! normal hit, change nothing
        

        //End HitGrazeCrit

        pawnToHit.TakeDamage((int)rolledDamage); // add time delay to reduce HP only after hit (atm this is done in TakeDamage and ReduceHP methods in character)

        hitAction?.Invoke();

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Attack, pawnToHit.Name, (int)rolledDamage ,Color.red);

        la.tgt = null;
        //pawn.TurnDone = true;
        pawn.FinishAnimation();
        //go.transform.LookAt(tgt.transform);
        //GetComponent<LookAt>().lookAtTargetPosition = tgt.transform.position;
    }
    public void ExtraDamageTarget(int minDmg, int maxDmg)
    {
        float rolledDamage = Random.Range(minDmg, maxDmg);

        pawnToHit.TakeDamage((int)rolledDamage); // add time delay to reduce HP only after hit (atm this is done in TakeDamage and ReduceHP methods in character)
        //BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Attack, toHit.Name, (int)rolledDamage, Color.red);

        //pawn.TurnDone = true; // this may be needed if turns get stuck around Ezra


        //go.transform.LookAt(tgt.transform);
        //GetComponent<LookAt>().lookAtTargetPosition = tgt.transform.position;
        la.tgt = null;
    }
    public void AddEffect(WeaponEffectAddon effectAddon)
    {
        hasEffect = true;
        if(effectData == null)
        {
            effectData = new EffectAddonDataType();
        }
        effectData.SetMe(effectAddon.data);
        
        //pawn.TurnDone = true;
        pawn.FinishAnimation();
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

        foreach (Pawn p in targets)
        {
            if (p.currentHP <= 0) //just in-case a dead enemy is still in the list somehow
                continue;

            int weight = weaponPrefs.baseAttackValue;

            //int currentDistance = pawn.tileWalker.currentNode.GetDistanceToTarget(p.tileWalker.currentNode);
            int currentDistance = pawn.tileWalker.GetDistanceFromMeToYou(p.tileWalker);

            if(range <= 14 && pawn.tileWalker.elevation != p.tileWalker.elevation) //melee
            {
                continue;
            }

            switch (currentDistance)
            {
                case 0:
                    weight *= weaponPrefs.attackFoesAtRange0_modifier; // generally high, since it's ON YOU
                    break;
                case 10:
                    weight *= weaponPrefs.attackFoesAtRange1_modifier; // 0 for ranged-attackers currently, until we have positioning  
                    break;
                case 14:
                    weight *= weaponPrefs.attackFoesAtRange1_modifier; // 0 for ranged-attackers currently, until we have positioning  
                    break;
                default: // distance of 2 or more
                    if(currentDistance <= range)
                    {
                        weight *= weaponPrefs.attackFoesWithinAttackRange_modifier;
                    }
                    else
                    {
                        weight *= weaponPrefs.approachFoesOutOfAttackRange_modifier; ///i.e. walk to attack
                    }
                    break;
            }

            if (weight == 0) //don't calculate if it's going to be end up as 0 anyways
                continue;

            #region Method 1: Stacking. 
            // method 1: Stacking. 
            // simply checks if the condition applies - and modifies if so ("stacking")
            //foreach (var pair in targetHealthPrefs.percentModParis)
            //{
            //    if(p.currentHP <= (p.maxHP / 100 * pair.percentOfHealth))
            //    {
            //        weight *= pair.modifier;
            //    }
            //}
            #endregion

            #region Method 2: Threshold
            // method 2: Threshold. 
            // only applies the mod for the "greatest" relevant threshold met 
            for (int i = 0; i < targetHealthPrefs.percentModParis.Length; i++)
            {
                if(p.currentHP <= (p.maxHP/100*targetHealthPrefs.percentModParis[i].percentOfHealth)) //Checks current pair
                {
                    if (i == targetHealthPrefs.percentModParis.Length - 1) //chevcks if i is last pair
                    {
                        //apply this (i) mod!
                        weight *= targetHealthPrefs.percentModParis[i].modifier;
                    }
                    else if(p.currentHP <= (p.maxHP / 100 * targetHealthPrefs.percentModParis[i + 1].percentOfHealth)) //Checks if next pair is relevant
                    {
                        continue; //
                    }
                    else
                    {
                        weight *= targetHealthPrefs.percentModParis[i].modifier;
                        //apply this (i) mod, and break
                        break;
                    }
                }
            }

            //END METHOD 2
            #endregion

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

        minDamage += ms._minDamage;
        maxDamage += ms._maxDamage;
    }
    public void ApplySheet(int addMin, int addMax)
    {
        minDamage += addMin;
        maxDamage += addMax;
    }
    //public void SetDamage(int addMin, int addMax)
    //{
    //    minDamage = addMin;
    //    maxDamage = addMax;
    //}
    public void SetStatBlockAndBenefits(StatBlock _statBlock/*, int minDmgBonus, int maxDmgBonus*/)
    {
        statBlock = _statBlock; //TBD should only grab the weapon related data (should probably be sturct) TEMP
    }

    void OnAttack() //just something to have in the attackAction. Currently holds nothing
    {
        Debug.Log("ON ATTACK ACTION " + name);
    }

    
}
