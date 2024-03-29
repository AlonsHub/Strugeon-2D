﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Pawn : LiveBody, TurnTaker, GridPoser, PurpleTarget
{
    //public PawnStats _stats;
    //public CharacterSheet characterSheet; //either enemy or merc
    [SerializeField]
    private string pawnName; //character name (not GameObject name)! //chagne the only use of this to print the enums name (mercName)


    [SerializeField]
    GameObject GFX;

    [SerializeField]
    TurnInfo _turnInfo;
    


    public bool isEnemy;

    public MercName mercName;
    [SerializeField]
    //MercSheet _characterSheet;
    public MercSheet _mercSheet; //only to access by mercSheet once! - TBF! find a better place to hold the base stats than in THIS field of the prefab
    public MercSheet mercSheetInPlayerData { get =>  PlayerDataMaster.Instance.SheetByName(mercName);} //these are created and constructed as level 1 with 0 exp when they are created.
                                                                                                  //in any other case they are loaded as data and not constructed at all
                                                                                                  //TBF! caching a refernece once might be problematic, since it starts with a value
                                                                                                  //may use this getter once, since pawns dont really need to provide this kind of request to anyone
    //bool isSheetInit = false;
    int initiative;
    [SerializeField]
    int initiativeBonus;

    bool turnDone;
    bool actionDone;

    [SerializeField]
    public bool hasSAs;
    public SA_Item[] saItems;

    //public ActionItem forDisplayPurposesOnly;

    public List<Pawn> targets;

    public TileWalker tileWalker;

    public Animator anim;

    [SerializeField]
    private Sprite portraitSprite;
    [SerializeField]
    private Sprite fullPortraitSprite;

    [SerializeField]
    private Sprite saSprite;

    public Transform effectIconParent;
    public GameObject effectIconPrefab;

    public TurnDisplayer myTurnPlate;

    #region Action Region

    List<ActionItem> actionItems;
    [SerializeField]
    public List<ActionVariation> actionPool;
    List<int> actionWeightList;

    public List<StatusEffect> statusEffects;



    public AudioSource audioSource;
    public AudioClip[] hitSounds;
    public AudioClip[] attackSounds;
    public GameObject damagePrefab;
    public Vector3 rotation;

    #endregion

    public WorldSpaceHorizontalGroup worldSpaceHorizontalGroup;

    #region Scion Powers
    //private List<float> damageModifiers; //make sure this is not used either, then remove tbf
    private bool hasPurple = false; //tbf remove this shit please
    public GameObject purpleTgt;
    #endregion
    

    public Pawn myPrefab;

    public bool TurnDone { get => turnDone; set => turnDone = value; } //on a turntakers turn, turn order system waitsUntil TurnDone


    public int Initiative { get => initiative; set => initiative = value + initiativeBonus; }
    //public int SA_CurrentCooldown { get => _currentCooldown; set => _currentCooldown = value; }
    public bool ActionDone { get => actionDone; set => actionDone = value; } //Item has performed its action and is reporting "done"
    //public bool DoDoubleTurn { get => doDoubleTurn; set => doDoubleTurn = value; }
    //public bool DoSkipTurn { get => doSkipTurn; set => doSkipTurn = value; }
    
    
  

    public bool HasShield;
    public bool HasRoot;

    //public bool MovementDone { get => movementDone; set => movementDone = value; } // OPTIONAL - to have walk and attack actions

    public string Name { get => pawnName; } //TBF!!! this name might be a problem later. Should we always .ToString() the enum when needed? or find better solution?
    public Sprite PortraitSprite { get => portraitSprite; set => portraitSprite = value; }
    public Sprite FullPortraitSprite { get => fullPortraitSprite; set => fullPortraitSprite = value; }
    public Sprite SASprite { get => saSprite; set => saSprite = value; }


    public List<ActionItem> ActionItems { get => actionItems; }

    public GameObject asPurpleTgtGameObject => gameObject;

    public TurnInfo TurnInfo { get => _turnInfo; set => _turnInfo = value; }

    //GameObject PurpleTarget.gameObject { get => gameObject;}

    public static int totalPawns = 0;


    public System.Action OnTakeDamage;


    //TEMP AF  TBF
    [SerializeField]
    public string SA_Title;
    [TextArea]
    [SerializeField]
    public string SA_Description;

    //Also temp AF
    ShieldAttacher cachedShield = null; //TEMP TBF
    RootDownAttacher cachedRootDownAttacher = null; //temp TBF

    // end TEMP AF
    public int enemyLevel = -1; //should stay that way if not enemy

    

    public override void Init()
    {
        //base.Init(); // HP init
        anim = GetComponent<Animator>();
        tileWalker = GetComponent<TileWalker>();
        tileWalker.Init();
        actionItems = GetComponents<ActionItem>().ToList();
        audioSource = GetComponent<AudioSource>();
        worldSpaceHorizontalGroup = GetComponentInChildren<WorldSpaceHorizontalGroup>();
        saItems = GetComponents<SA_Item>();

        statusEffects = new List<StatusEffect>();

        hasSAs = (saItems.Length != 0);

        if (isEnemy)
        {
            targets = RefMaster.Instance.mercs;
            name.Replace("(Clone)", ""); //can be removed from build - may pose problem for name searching, if any exist
            EnemySheetAddon sheetAddon = gameObject.AddComponent<EnemySheetAddon>(); //why not just add this to the prefab? TBF
        }
        else
        {
            if ((mercSheetInPlayerData) == null)
            {
                Debug.LogError("No sheet with merc name of: " + mercName.ToString());
            }

            //mercSheetInPlayerData.baseSheetSO = _mercSheet.baseSheetSO; //pass baseSO via prefab - why not pass the base stats aswell?

            ApplyCharacterSheet();
        }
        base.Init(); // HP init

        totalPawns++; //static counter
        //grab art?
    }

    void ApplyCharacterSheet()
    {
        mercSheetInPlayerData.baseStatBlock = _mercSheet.baseStatBlock; //Copy the baseStatBlock straight off the prefab
        mercSheetInPlayerData.mercClass = _mercSheet.mercClass; //also grab the merc class - or should this be added to the statblock? maybe dont?
        mercSheetInPlayerData.basicPrefs = _mercSheet.basicPrefs;
        _mercSheet = mercSheetInPlayerData; //beomce one with the sheet in data

        //APPLY MERC SHEET
        //minmax dmg to weapon
        if (_mercSheet.characterName != mercName)
        {
            Debug.LogError("probably, no mercsheet exists for this pawn " + mercName.ToString());
        }

        List<IBenefit> statBenefits = mercSheetInPlayerData.gear.GetAllBenefits();

        int minDamgeBenefit = 0;
        int maxDamgeBenefit = 0;
        int maxHPBenefit = 0;
        if (statBenefits != null && statBenefits.Count >0)
        {
            foreach (var stats in statBenefits)
            {
                switch (((StatBenefit)stats).statToBenefit)
                {
                    case StatToBenefit.MaxHP:
                        maxHPBenefit = stats.Value();
                        break;
                    case StatToBenefit.FlatDamage:
                        minDamgeBenefit = stats.Value();
                        maxDamgeBenefit = stats.Value();
                        break;
                    default:
                        break;
                }
            }
        }

        int finalMinDamage = _mercSheet._minDamage + minDamgeBenefit;
        int finalMaxDamage = _mercSheet._maxDamage + maxDamgeBenefit;
        finalMinDamage = Mathf.Clamp(finalMinDamage, 0, finalMaxDamage-1); //maintain range of at least 1
        //finalMinDamage = Mathf.Clamp(finalMinDamage, 0, finalMaxDamage); //allows min == max damage


        GetComponent<WeaponItem>().SetDamage(_mercSheet._minDamage+ minDamgeBenefit, _mercSheet._maxDamage+ maxDamgeBenefit);
        //max hp bonus
        maxHP = _mercSheet._maxHp + maxHPBenefit;

        targets = RefMaster.Instance.enemyInstances;

        name.Replace("(Clone)", ""); //can be removed from build - may pose problem for name searching, if any exist


        //Load MercSheet on-to prefab - prefabs are always inited like this so they would always have relevant data
    }

    public void TakeTurn()
    {
        TurnDone = false;

        if (statusEffects != null && statusEffects.Count > 0)
        {
            StatusEffect[] startTurnEffects = statusEffects.Where(x => x is I_StatusEffect_TurnStart).ToArray();

            if(startTurnEffects.Length >0)
            {
                foreach (var item in startTurnEffects)
                {
                    item.Perform();
                }

                if(turnDone) //if skipped or whatever
                    return;
            }
        }

        CalculateActionList();
        if (actionWeightList.Count == 0)
        {
            FinishAnimation();

            return; //!!!!!!!!!!
        }

        int roll = UnityEngine.Random.Range(1, actionWeightList[actionWeightList.Count - 1]); //make sure this random IS the int random and we don't have a "rounded-float" situation
        int actionIndex = -1; //just so it will fuck up the actionPoll[actionIndex] in case it doesn't work properly

        for (int i = 0; i < actionWeightList.Count; i++)
        {
            if (roll < actionWeightList[i])
            {
                actionIndex = i;
                break;
            }
        }
        if (actionIndex == -1)
        {
            //skip rope
            Debug.LogError($"{Name} SKIPPED rope");
            FinishAnimation();
            return;
        }

        VariationConsole.Instance.Set(this, actionIndex);

        actionPool[actionIndex].PerformActionOnTarget();
    }

    public void CalculateActionList()
    {

        //SAFETY HEALTH CHECK

        if(currentHP <= 0)
        {
            turnDone = true;
            return;
        }


        actionPool = new List<ActionVariation>();
        actionWeightList = new List<int>();

        //Call action "before calcActionList"

        foreach (ActionItem ai in actionItems)
        {
            ai.CalculateVariations(); //Maybe not always, could "if" that out in some cases - SADLY NOT TRUE

            actionPool.AddRange(ai.actionVariations);
        }

        //This ALSO needs to be a suggestive effect!
        //if (hasPurple && purpleTgt != null)
        //{
        //    ActionVariation[] possibleActions = actionPool.Where(x => (x.target && x.target.Equals(purpleTgt))).ToArray();// tamir purple bug fixed with target null check, new action variations had no targets (shield for hadas has no "target" to pass on variation - it cannot be suggested upon, but it also jamed the purple buff)
        //    if (possibleActions != null && possibleActions.Length > 0)
        //    {
        //        foreach (var possibleAction in possibleActions)
        //        {
        //            possibleAction.weight *= purpleMultiplier;
        //        }
        //        hasPurple = false;
        //        purpleTgt = null;
        //    }
        //}
        //This ALSO needs to be a suggestive effect!
        //Joinning the list below
        HandleSuggestiveStatusEffects();

        int runningTotal = 0;
        foreach (ActionVariation av in actionPool)
        {
            runningTotal += av.weight;
            actionWeightList.Add(runningTotal);
        }
    }
    /// <summary>
    /// For the Insight status effect
    /// </summary>
    public void SpeculateActionList()
    {

        actionPool = new List<ActionVariation>();
        actionWeightList = new List<int>();

        //Call action "before calcActionList"

        foreach (ActionItem ai in actionItems)
        {
            ai.CalculateVariations(); //Maybe not always, could "if" that out in some cases - SADLY NOT TRUE

            actionPool.AddRange(ai.actionVariations);
        }

        // TBF TBD
        //This ALSO needs to be a suggestive effect!
        if (hasPurple && purpleTgt != null)
        {
            ActionVariation[] possibleActions = actionPool.Where(x => (x.target && x.target.Equals(purpleTgt))).ToArray();// tamir purple bug fixed with target null check, new action variations had no targets (shield for hadas has no "target" to pass on variation - it cannot be suggested upon, but it also jamed the purple buff)
            if (possibleActions != null && possibleActions.Length > 0)
            {
                foreach (var possibleAction in possibleActions)
                {
                    possibleAction.weight *= purpleMultiplier;
                }
                hasPurple = false;
                purpleTgt = null;
            }
        }
        //THIS EXCLUDES INSIGHT!
        HandleSuggestiveStatusEffectsPreview();

        int runningTotal = 0;
        foreach (ActionVariation av in actionPool)
        {
            runningTotal += av.weight;
            actionWeightList.Add(runningTotal);
        }
    }

    /// <summary>
    /// cycles through status effects and performs all suggestive effects.
    /// Suggestive effects are all effects applied after all action variations are gathered, before weight distribution and rolling
    /// It is the ideal place to change weights, preferences and avoiding/focusing VIABLE targets 
    /// Changes to targetting would need to happen BEFORE action variations are gathered.
    /// </summary>
    private void HandleSuggestiveStatusEffects()
    {
        if (statusEffects != null)
        {
            var suggestiveEffects = statusEffects.Where(x => x is I_StatusEffect_ActionWeightManipulator).ToList();
            if (suggestiveEffects != null && suggestiveEffects.Count > 0)
            {
                foreach (var effect in suggestiveEffects)
                {
                    effect.Perform();
                }
            }
        }
    }
    /// <summary>
    /// For the Insight status effect purposes only
    /// </summary>
    public void HandleSuggestiveStatusEffectsPreview()
    {
        if (statusEffects != null)
        {
            var suggestiveEffects = statusEffects.Where(x => x is AfterActionWeightsEffect).ToList();
            if (suggestiveEffects != null && suggestiveEffects.Count > 0)
            {
                foreach (var effect in suggestiveEffects)
                {
                    if (effect is InsightEffect)
                        continue;
                    (effect as AfterActionWeightsEffect).current++;
                    effect.Perform();
                }
            }
        }
    }


    public override int TakeDamage(int damage) //ADD DamageType and derrive text colour from that
    {

        //Sound prompt
        //audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length - 1)];
        //audioSource.Play();
        anim.SetTrigger("Hit");
        //Spawn damage text (numbers)
        GameObject go = Instantiate(damagePrefab, transform.position, damagePrefab.transform.rotation);


        if (damage != 0)
        {
            OnTakeDamage?.Invoke(); //relevant only if actual damage happens //should also be the way to override taking damage when pawn has shield

            
            //EXTRACT METHOD: DamageCalculation() TBF
            //if (DoYellowDebuff)
            //{
            //    damage = (int)(damage * damageModifier);

            //    DamageModifier = 1; // also TBF as buffs should all be components that set and unset things on their own 
            //                        //these add-on components can only subscribe to relevant pawn Actions if needed, Pawn should neither check for them not un/set them TBF
            //    DoYellowDebuff = false;
            //    RemoveIconByName("yellowDeBuff"); //TBF!!
            //}

            StatusEffect[] incomingDamageEffects = GetStatusEffectsByPredicate(x => x is I_StatusEffect_IncomingDamageMod);
            if (incomingDamageEffects != null && incomingDamageEffects.Length != 0)
            {
                foreach (var item in incomingDamageEffects)
                {
                    damage = (int)(item as I_StatusEffect_IncomingDamageMod).OperateOnDamage(damage);
                }
            }

            if (HasShield) //TEMP TBF
            {
                if (!cachedShield && !(cachedShield = GetComponent<ShieldAttacher>()))
                    Debug.LogError("Has shield is true but no ShieldAttacher found on gameObject");
                else
                {
                    int carryOver = cachedShield.TakeDamage(damage); //this 
                    if(carryOver < 0)
                    {
                        damage = carryOver;
                    }
                    damage = carryOver >= 0 ? 0 : carryOver * -1; //damage should be positive, or it heals
                }
            }
            //END EXTRACT METHOD: DamageCalculation() TBF
            if (HasRoot) //TEMP TBF
            {
                if (!cachedRootDownAttacher && !(cachedRootDownAttacher = GetComponent<RootDownAttacher>()))
                    Debug.LogError("Has shield is true but no ShieldAttacher found on gameObject");
                else
                {
                    int carryOver = cachedRootDownAttacher.TakeDamage(damage); //this 
                    if (carryOver < 0)
                    {
                        damage = carryOver;
                    }
                    damage = carryOver >= 0 ? 0 : carryOver * -1; //damage should be positive, or it heals
                }
            }
            //END EXTRACT METHOD: DamageCalculation() TBF

        }


        go.GetComponent<DamageText>().SetDamageText(damage);

        //DAMAGE SHOULD NOT BE REPORTED BY THE ATTACKER, BUT BY THE VICTIM!
        //Attack should be reported (numberless) to the log by the attacker, and the damaged should add the damage to previous log entry using appropriate methods
        //ADD TO PREVIOUS TBF

        //currentHP -= damage;
        //if (currentHP <= 0)
        //{
        //    Die();
        //}
        return base.TakeDamage(damage);
    }

    public int TakeDirectDamage(int damage) //ADD DamageType and derrive text colour from that
    {

        //Sound prompt
        //audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length - 1)];
        //audioSource.Play();
        //anim.SetTrigger("Hit");
        //Spawn damage text (numbers)
        GameObject go = Instantiate(damagePrefab, transform.position, damagePrefab.transform.rotation);

        go.GetComponent<DamageText>().SetDamageText(damage);

        //DAMAGE SHOULD NOT BE REPORTED BY THE ATTACKER, BUT BY THE VICTIM!
        //Attack should be reported (numberless) to the log by the attacker, and the damaged should add the damage to previous log entry using appropriate methods
        //ADD TO PREVIOUS TBF

        //currentHP -= damage;
        //if (currentHP <= 0)
        //{
        //    Die();
        //}
        return base.TakeDamage(damage);
    }

    //TBF! this needs to be implemented into just normal TakeDamage (and god help me use the TakeDamage method)
    public void TakeElementalDamage(int damage, Color colour) //ADD DamageType and derrive text colour from that
    {
        //add effect sound
        //audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length - 1)];
        //audioSource.Play();

        //Spawn damage dmgtext with in a different color, with offset
        GameObject go = Instantiate(damagePrefab, transform.position, damagePrefab.transform.rotation);

        //GameObject go = Instantiate(damagePrefab, transform.position + Vector3.right*damagePrefab.transform.lossyScale.x, damagePrefab.transform.rotation);
        TMP_Text t = go.GetComponentInChildren<TMP_Text>();
        t.text = damage.ToString();
        t.color = colour;

        //Consider vaulnerabillities and weaknesses and such
        //call DAMAGE CALC! TBF
        if (HasShield) //TEMP TBF
        {
            if (!cachedShield && !(cachedShield = GetComponent<ShieldAttacher>()))
                Debug.LogError("Has shield is true but no ShieldAttacher found on gameObject");
            else
            {
                int carryOver = cachedShield.ReduceTtlBy(damage); //this 
                if (carryOver < 0)
                {
                    damage = carryOver;
                }
                damage = carryOver >= 0 ? 0 : carryOver;
            }
        }


        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }
    public override void Die()
    {
        StartCoroutine(nameof(DelayedDeath));
    }
    
    public void Escape()
    {
        StartCoroutine(DelayedEscape());

    }

    private IEnumerator DelayedEscape()
    {
        //walk to the escape square - by tilePos(?)
        //Removes self from squad (permanently, for now)
        if (PartyMaster.Instance.currentSquad.pawns.Remove(this))
        {
            Debug.LogWarning("Escaped and destroyed!");
        }
        //else
        //{
        //    Debug.LogWarning("Escaped and destroyed!");
        //}

        //PlayerDataMaster.Instance.currentPlayerData.cowardMercs++; //MOVED To EscapeItem's Action()

        //Add to the list of the Cowardly (TurnMaster)
        RefMaster.Instance.mercs.Remove(this); //not ideal // *******************************************************
        //PartyMaster.Instance.availableMercPrefabs.Remove(this);
        RefMaster.Instance.AddCoward(mercName);
        //TurnMaster.Instance.RemoveTurnTaker(this);
        //TurnMachine.Instance.RemoveTurnInfoByTaker(this);
        TurnMachine.Instance.RemoveALLInfosForTaker(this);


        //TurnMaster.Instance.turnTakers.Remove(this);
        //TurnMaster.Instance.turnPlates.Remove(myTurnPlate); //replace with clear method
        //Destroy(myTurnPlate.gameObject);
        yield return new WaitForEndOfFrame();
        // THIS MIGHT BE WHERE ESCAPED mercs ARE CONSIDERED DEAD
        //by being destroyed and then -> counded as missing from the list
        //and somehow still getting into the "Cowardly list"
        //Destroy(gameObject);
        TurnDone = true;
        tileWalker.currentNode.RemoveOccupant(true);
        //Destroy(gameObject, .5f); //just for now, nukes it
    }

    IEnumerator DelayedDeath()
    {
        //simplify Death, YOU IDIOT!

        //if (statusEffects!=null)
        //{
        //    foreach (var item in statusEffects)
        //    {
        //        item.EndEffect();
        //    }
        //}

        //yield return new WaitForSeconds(.1f);
        //yield return new WaitForEndOfFrame();
        //BattleLogVerticalGroup.Instance.AddEntry(TurnMaster.Instance.currentTurnTaker.Name , ActionSymbol.Death, pawnName); //Maybe not announced by the killer, attempt 1#
        BattleLogVerticalGroup.Instance.AddEntry(TurnMachine.Instance.GetCurrentTurnTaker.Name , ActionSymbol.Death, pawnName); //Maybe not announced by the killer, attempt 1#
        if (!isEnemy)
        {
            //RefMaster.Instance.mercs.Remove(RefMaster.Instance.mercs.Where(x => x.name == name).SingleOrDefault()); //not ideal
            RefMaster.Instance.mercs.Remove(this); //not ideal
            PartyMaster.Instance.availableMercPrefabs.Remove(this);
            //TurnMaster.Instance.theDead.Add(mercName);
            RefMaster.Instance.AddDead(mercName);
            PlayerDataMaster.Instance.currentPlayerData.deadMercs++;
        }
        else
        {
            //RefMaster.Instance.enemies.Remove(RefMaster.Instance.enemies.Where(x => x.name == name).SingleOrDefault()); //not ideal
            RefMaster.Instance.enemyInstances.Remove(this); //not ideal
        }
        //TurnMaster.Instance.RemoveTurnTaker(this);
        TurnMachine.Instance.RemoveALLInfosForTaker(this);


        tileWalker.currentNode.RemoveOccupant(true); //true also destorys the gameObject
        yield return new WaitForEndOfFrame();
        //Destroy(gameObject);
    }

    public void SetupPurpleBuff(GameObject tgt)
    {
        hasPurple = true;
        purpleTgt = tgt;
    }

    List<GameObject> effectIcons = new List<GameObject>();
    public int purpleMultiplier;
    Dictionary<string, GameObject> nameToIconObject = new Dictionary<string, GameObject>();
    public void AddEffectIcon(Sprite newEffectIcon, string ID) //TBF - this needs to return a GameObject so effects could easily erase their icons without fucking googling them first
    {
        //Recently removed 07/02/23
        //if(effectIcons.Where(x => x.name == ID).Count() > 0)
        //{
        //    return;
        //}

        GameObject go = Instantiate(effectIconPrefab, effectIconParent); //return this!
        go.GetComponentInChildren<SpriteRenderer>().sprite = newEffectIcon; //have a better setter! TBF!

        nameToIconObject.Add(ID, go);
        go.name = ID;
        effectIcons.Add(go);
        worldSpaceHorizontalGroup.UpdateGroup();
    }

    public void RemoveIconByName(string iconName)
    {
        //List<GameObject> relevantList = effectIcons.Where(x => x.name == iconName).ToList(); //tbf - all icons should be addon-based componenets like blinded and charmed!
        GameObject go;
        if(nameToIconObject.TryGetValue(iconName, out go))
        {
            effectIcons.Remove(go);
            nameToIconObject.Remove(iconName);
            Destroy(go);
        }
        else
        {
            //Debug.LogError("Couldnt find icon of that ID");
            return; //no need to update
        }

       

        worldSpaceHorizontalGroup.UpdateGroup();
    }
    public Vector2Int GetGridPos()
    {
        return tileWalker.gridPos;
    }
    public void SetGridPos(Vector2Int newPos) //ignores these values (just in pawn's/tilewalker's case)
    {
        //tileWalker.Init();
        tileWalker.SetPos(newPos);
    }

    public string GetName()
    {
        return name;
    }
    /// <summary>
    /// Currently the main, if not only, way to end a turn.
    /// </summary>
    public void FinishAnimation()
    {
        TurnDone = true;
        if (statusEffects != null)
        {
            //StatusEffect[] basicStatusEffects = statusEffects.Where(x => x is EndOfTurnStatusEffect).ToArray();
            StatusEffect[] basicStatusEffects = statusEffects.Where(x => x is I_StatusEffect_TurnEnd).ToArray();
            foreach (var item in basicStatusEffects)
            {
                item.Perform();
            }
        }
    }

 
    //this needs to become AddStatusEffect - which recieves ALL status effects
    public void AddStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffects == null)
        {
            statusEffects = new List<StatusEffect>();
        }
        statusEffects.Add(statusEffect);
        if(statusEffect.iconSprite)
        AddEffectIcon(statusEffect.iconSprite, statusEffect.GetType().ToString());
    }
    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffects.Remove(statusEffect);
        RemoveIconByName(statusEffect.GetType().ToString());
    }

    IEnumerator RemoveSuggestiveEffectWithDelay(StatusEffect statusEffect)
    {
        //yield return new WaitForEndOfFrame();
        statusEffects.Remove(statusEffect);
        RemoveIconByName(statusEffect.GetType().ToString());
        yield return new WaitForEndOfFrame();

    }

    #region Predicate Getters 
    public StatusEffect[] GetStatusEffectsByPredicate(System.Func<StatusEffect, bool> pred)
    {
        if (statusEffects != null && statusEffects.Count != 0)
        {
            return statusEffects.Where(pred).ToArray();
        }
        return null;
    }
    public StatusEffect GetFirstStatusEffectByPredicate(System.Func<StatusEffect, bool> pred)
    {
        if (statusEffects != null && statusEffects.Count != 0)
        {
            return statusEffects.Where(pred).FirstOrDefault();
        }
        return null;
    }
    public StatusEffect GetSingleStatusEffectByPredicate(System.Func<StatusEffect, bool> pred)
    {
        if (statusEffects != null && statusEffects.Count != 0)
        {
            return statusEffects.Where(pred).SingleOrDefault();
        }
        return null;
    }
    public bool HasStatusEffect(System.Func<StatusEffect, bool> pred)
    {
        if (statusEffects != null && statusEffects.Count != 0)
        {
            return statusEffects.Where(pred).Any();
        }
        return false;
    }
    #endregion
    public void SetGFXScale(Vector3 scale)
    {
        GFX.transform.localScale = scale;
    }

    public ActionVariation GetIntention()
    {
        //CalculateActionList();
        SpeculateActionList();

        int roll = UnityEngine.Random.Range(1, actionWeightList[actionWeightList.Count - 1]); //make sure this random IS the int random and we don't have a "rounded-float" situation
        int actionIndex = -1; //just so it will fuck up the actionPoll[actionIndex] in case it doesn't work properly

        for (int i = 0; i < actionWeightList.Count; i++)
        {
            if (roll < actionWeightList[i])
            {
                actionIndex = i;
                break;
            }
        }
        if (actionIndex == -1)
        {
            //skip rope
            Debug.LogError($"{Name} SKIPPED rope");
            FinishAnimation();
            return null;
        }

        return actionPool[actionIndex];
    }

    public bool ActionPoolContainsVariation(ActionVariation av)
    {
        return actionPool.Any(x => x.target == av.target && x.relevantItem == av.relevantItem);
    }
    public ActionVariation[] GetActionVariationsByPredicate(System.Func<ActionVariation, bool> pred)
    {
        return actionPool.Where(pred).ToArray();
    }

}