using System.Collections;
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

    public bool isEnemy;

    public MercName mercName;
    [SerializeField]
    //MercSheet _characterSheet;
    MercSheet _mercSheet;
    public MercSheet mercSheetInPlayerData { get => PlayerDataMaster.Instance.SheetByName(mercName);} //these are created and constructed as level 1 with 0 exp when they are created.
                                                                                                  //in any other case they are loaded as data and not constructed at all
    bool isSheetInit = false;
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



    public AudioSource audioSource;
    public AudioClip[] hitSounds;
    public AudioClip[] attackSounds;
    public GameObject damagePrefab;
    public Vector3 rotation;

    #endregion

    public WorldSpaceHorizontalGroup worldSpaceHorizontalGroup;

    #region Scion Powers
    private bool doDoubleTurn = false;
    private bool doSkipTurn = false;
    private bool doYellowDebuff = false;
    private float damageModifier = 1;
    private List<float> damageModifiers;
    private bool hasPurple = false;
    public GameObject purpleTgt;
    #endregion
    // Vector2Int GetGridPos { get => tileWalker.gridPos; }

    public Pawn myPrefab;

    public bool TurnDone { get => turnDone; set => turnDone = value; } //on a turntakers turn, turn order system waitsUntil TurnDone


    public int Initiative { get => initiative; set => initiative = value + initiativeBonus; }
    //public int SA_CurrentCooldown { get => _currentCooldown; set => _currentCooldown = value; }
    public bool ActionDone { get => actionDone; set => actionDone = value; } //Item has performed its action and is reporting "done"
    public bool DoDoubleTurn { get => doDoubleTurn; set => doDoubleTurn = value; }
    public bool DoSkipTurn { get => doSkipTurn; set => doSkipTurn = value; }
    public bool DoYellowDebuff { get => doYellowDebuff; set => doYellowDebuff = value; } //TBF - modify damage chain!
    public bool DoModifyDamage => DamageModifiers.Count>0; //new approach that will mod damage only if there are mods to add - mods will remove themselves?
    public float DamageModifier { get => damageModifier; set => damageModifier = value; }
    public List<float> DamageModifiers { get => damageModifiers; set => damageModifiers = value; }

    public bool HasShield;

    //public bool MovementDone { get => movementDone; set => movementDone = value; } // OPTIONAL - to have walk and attack actions

    public string Name { get => pawnName; } 
    public Sprite PortraitSprite { get => portraitSprite; set => portraitSprite = value; }
    public Sprite FullPortraitSprite { get => fullPortraitSprite; set => fullPortraitSprite = value; }
    public Sprite SASprite { get => saSprite; set => saSprite = value; }


    public List<ActionItem> ActionItems { get => actionItems; }

    public GameObject asPurpleTgtGameObject => gameObject;

    //GameObject PurpleTarget.gameObject { get => gameObject;}

    public static int totalPawns = 0;


    public System.Action OnTakeDamage;


    //TEMP AF
    [SerializeField]
    public string SA_Title;
    [TextArea]
    [SerializeField]
    public string SA_Description;

    // end TEMP AF
    public int enemyLevel = -1; //should stay that way if not enemy

    public override void Init()
    {
        base.Init(); // HP init
        anim = GetComponent<Animator>();
        tileWalker = GetComponent<TileWalker>();
        tileWalker.Init();
        actionItems = GetComponents<ActionItem>().ToList();
        audioSource = GetComponent<AudioSource>();
        worldSpaceHorizontalGroup = GetComponentInChildren<WorldSpaceHorizontalGroup>();
        saItems = GetComponents<SA_Item>();

        damageModifiers = new List<float>();

        hasSAs = (saItems.Length != 0);
        

        if (isEnemy)
        {
            targets = RefMaster.Instance.mercs;
            name.Replace("(Clone)", ""); //can be removed from build - may pose problem for name searching, if any exist
            EnemySheetAddon sheetAddon = gameObject.AddComponent<EnemySheetAddon>();


        }
        else
        {
            if ((_mercSheet = mercSheetInPlayerData) == null)
            {
                Debug.LogError("No sheet with merc name of: " + mercName.ToString());
            }
            //APPLY MERC SHEET
            //minmax dmg to weapon
            if(_mercSheet.characterName != mercName)
            {
                Debug.LogError("probably, no mercsheet exists for this pawn " + mercName.ToString());
            }


            GetComponent<WeaponItem>().ApplySheet(_mercSheet._minDamageBonus, _mercSheet._maxDamageBonus); //SHOULD crash and burn if fails, because this should never fail!

            //max hp bonus
            maxHP = currentHP += _mercSheet._maxHpBonus;

            targets = RefMaster.Instance.enemyInstances;
            
            name.Replace("(Clone)", ""); //can be removed from build - may pose problem for name searching, if any exist

            
            //Load MercSheet on-to prefab - prefabs are always inited like this so they would always have relevant data
        }

        totalPawns++; //static counter
        //grab art?
    }

    public void SetCharacterSheet()
    {

    }

    public void TakeTurn()
    {
        TurnDone = false;
        CalculateActionList();
        if (actionWeightList.Count == 0)
        {
            TurnDone = true;
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
        
        actionPool[actionIndex].PerformActionOnTarget();
    }

    void CalculateActionList()
    {
        actionPool = new List<ActionVariation>();
        actionWeightList = new List<int>();

        //Call action "before calcActionList"

        foreach (ActionItem ai in actionItems)
        {
            ai.CalculateVariations(); //Maybe not always, could "if" that out in some cases - SADLY NOT TRUE

            actionPool.AddRange(ai.actionVariations);
        }

        if(hasPurple && purpleTgt != null)
        {
            ActionVariation[] possibleActions = actionPool.Where(x => x.target.Equals(purpleTgt)).ToArray();
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

        int runningTotal = 0;
        foreach (ActionVariation av in actionPool)
        {
            runningTotal += av.weight;
            actionWeightList.Add(runningTotal);
        }

        //Call action "after calcActionList"
    }
    ShieldAttacher cachedShield = null; //TEMP TBF
    public override void TakeDamage(int damage) //ADD DamageType and derrive text colour from that
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
            if (DoYellowDebuff)
            {
                damage = (int)(damage * damageModifier);

                DamageModifier = 1; // also TBF as buffs should all be components that set and unset things on their own 
                                    //these add-on components can only subscribe to relevant pawn Actions if needed, Pawn should neither check for them not un/set them TBF
                DoYellowDebuff = false;
                RemoveIconByName("yellowDeBuff"); //TBF!!
            }

            if (HasShield) //TEMP TBF
            {
                if (!cachedShield && !(cachedShield = GetComponent<ShieldAttacher>()))
                    Debug.LogError("Has shield is true but no ShieldAttacher found on gameObject");
                else
                {
                    int carryOver = cachedShield.ReduceTtlBy(damage); //this 
                    if(carryOver < 0)
                    {
                        damage = carryOver;
                    }
                    damage = carryOver >= 0 ? 0 : carryOver;
                }
            }
            //END EXTRACT METHOD: DamageCalculation() TBF

        }


        go.GetComponent<DamageText>().SetDamageText(damage);

        //DAMAGE SHOULD NOT BE REPORTED BY THE ATTACKER, BUT BY THE VICTIM!
        //Attack should be reported (numberless) to the log by the attacker, and the damaged should add the damage to previous log entry using appropriate methods
        //ADD TO PREVIOUS TBF

        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    //TBF! this needs to be implemented into just normal TakeDamage (and god help me use the TakeDamage method)
    public void TakeElementalDamage(int damage, Color colour) //ADD DamageType and derrive text colour from that
    {
        //add effect sound
        //audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length - 1)];
        //audioSource.Play();

        //Spawn damage dmgtext with in a different color, with offset
        GameObject go = Instantiate(damagePrefab, transform.position + Vector3.right*damagePrefab.transform.lossyScale.x, damagePrefab.transform.rotation);
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
        TurnMaster.Instance.theCowardly.Add(mercName);
        TurnMaster.Instance.RemoveTurnTaker(this);

        tileWalker.currentNode.RemoveOccupant(false) ;

        //TurnMaster.Instance.turnTakers.Remove(this);
        //TurnMaster.Instance.turnPlates.Remove(myTurnPlate); //replace with clear method
        //Destroy(myTurnPlate.gameObject);
        
        // THIS MIGHT BE WHERE ESCAPED mercs ARE CONSIDERED DEAD
        //by being destroyed and then -> counded as missing from the list
        //and somehow still getting into the "Cowardly list"

        Destroy(gameObject, .5f); //just for now, nukes it
    }

    IEnumerator DelayedDeath()
    {
        //simplify Death, YOU IDIOT!

        //yield return new WaitForSeconds(.1f);
        yield return new WaitForEndOfFrame();
        BattleLogVerticalGroup.Instance.AddEntry(TurnMaster.Instance.currentTurnTaker.Name , ActionSymbol.Death, pawnName); //Maybe not announced by the killer, attempt 1#
        if (!isEnemy)
        {
            //RefMaster.Instance.mercs.Remove(RefMaster.Instance.mercs.Where(x => x.name == name).SingleOrDefault()); //not ideal
            RefMaster.Instance.mercs.Remove(this); //not ideal
            PartyMaster.Instance.availableMercPrefabs.Remove(this);
            TurnMaster.Instance.theDead.Add(mercName);
            PlayerDataMaster.Instance.currentPlayerData.deadMercs++;
        }
        else
        {
            //RefMaster.Instance.enemies.Remove(RefMaster.Instance.enemies.Where(x => x.name == name).SingleOrDefault()); //not ideal
            RefMaster.Instance.enemyInstances.Remove(this); //not ideal
        }
        TurnMaster.Instance.RemoveTurnTaker(this);
        tileWalker.currentNode.RemoveOccupant(true); //true also destorys the gameObject
        //Destroy(gameObject);
    }

    public void SetupPurpleBuff(GameObject tgt)
    {
        hasPurple = true;
        purpleTgt = tgt;
    }

    List<GameObject> effectIcons = new List<GameObject>();
    public int purpleMultiplier;

    public void AddEffectIcon(Sprite newEffectIcon, string ID) //TBF - this needs to return a GameObject so effects could easily erase their icons without fucking googling them first
    {
        if(effectIcons.Where(x => x.name == ID).Count() > 0)
        {
            return;
        }

        GameObject go = Instantiate(effectIconPrefab, effectIconParent); //return this!
        go.GetComponentInChildren<SpriteRenderer>().sprite = newEffectIcon; //have a better setter! TBF!
        go.name = ID;
        effectIcons.Add(go);
        worldSpaceHorizontalGroup.UpdateGroup();
    }

    public void RemoveIconByName(string iconName)
    {
        List<GameObject> relevantList = effectIcons.Where(x => x.name == iconName).ToList(); //tbf - all icons should be addon-based componenets like blinded and charmed!

        if (relevantList.Count > 0)
        {
            foreach (var item in relevantList)
            {
                effectIcons.Remove(item);
                Destroy(item);
            }
        }
        else
        {
            Debug.LogError($"No icon by this name: {iconName} was found");
        }
       
        worldSpaceHorizontalGroup.UpdateGroup();
    }
    public Vector2Int GetGridPos()
    {
        return tileWalker.gridPos;
    }
    public void SetGridPos(Vector2Int newPos) //ignores these values (just in pawn's/tilewalker's case)
    {
        tileWalker.Init();
    }

    public string GetName()
    {
        return name;
    }
}


