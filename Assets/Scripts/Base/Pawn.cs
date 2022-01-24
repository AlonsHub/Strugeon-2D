using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Pawn : LiveBody, TurnTaker, GridPoser
{
    //public PawnStats _stats;
    //public CharacterSheet characterSheet; //either enemy or merc
    [SerializeField]
    private string pawnName; //character name (not GameObject name)!

    public MercName mercName;
    [SerializeField]
    //MercSheet _characterSheet;
    public MercSheet _mercSheet;
    public MercSheet GetMercSheet { get => PlayerDataMaster.Instance.SheetByName(mercName);} //these are created and constructed as level 1 with 0 exp when they are created.
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

    public ActionItem forDisplayPurposesOnly;
    
    //public int _currentCooldown;
    
    //public int saCooldown;

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

    public bool isEnemy = false;


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
    private bool doModifyDamage = false;
    private float damageModifier = 1;
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
    public bool DoModifyDamage { get => doModifyDamage; set => doModifyDamage = value; }
    public float DamageModifier { get => damageModifier; set => damageModifier = value; }

    //public bool MovementDone { get => movementDone; set => movementDone = value; } // OPTIONAL - to have walk and attack actions
    
    public string Name { get => pawnName; } 
    public Sprite PortraitSprite { get => portraitSprite; set => portraitSprite = value; }
    public Sprite FullPortraitSprite { get => fullPortraitSprite; set => fullPortraitSprite = value; }
    public Sprite SASprite { get => saSprite; set => saSprite = value; }


    public List<ActionItem> ActionItems { get => actionItems; }

    public static int totalPawns = 0;


    //TEMP AF
    [SerializeField]
    public string SA_Title;
    [TextArea]
    [SerializeField]
    public string SA_Description;

    // end TEMP AF

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

        hasSAs = (saItems.Length != 0);
        

        if (isEnemy)
        {
            targets = RefMaster.Instance.mercs;
            name.Replace("(Clone)", ""); //can be removed from build - may pose problem for name searching, if any exist
        }
        else
        {
            if ((_mercSheet = GetMercSheet) == null)
            {
                Debug.LogError("No sheet with merc name of: " + mercName.ToString());
            }
            //APPLY MERC SHEET
            //minmax dmg to weapon
            if(_mercSheet.characterName != mercName)
            {
                Debug.LogError("probably, no mercsheet exists for this pawn " + mercName.ToString());
            }


            GetComponent<WeaponItem>().ApplySheet(_mercSheet); //SHOULD crash and burn if fails, because this should never fail!

            //max hp bonus
            maxHP = currentHP += _mercSheet._maxHpBonus;

            targets = RefMaster.Instance.enemies;
            
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
            //TurnOrder.Instance.NextTurn(); //just be done
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

        foreach (ActionItem ai in actionItems)
        {
            ai.CalculateVariations(); //Maybe not always, could "if" that out in some cases - SADLY NOT TRUE

            actionPool.AddRange(ai.actionVariations);

        }
        int runningTotal = 0;

        if(hasPurple)
        {
            ActionVariation possibleAction = actionPool.Where(x => x.target == purpleTgt).FirstOrDefault();
            if (possibleAction != null)
            {
                possibleAction.weight *= purpleMultiplier;
                hasPurple = false;
            }
        }

        foreach (ActionVariation av in actionPool)
        {
            runningTotal += av.weight;
            actionWeightList.Add(runningTotal);
        }
    }

    public override void TakeDamage(int damage) //ADD DamageType and derrive text colour from that
    {
        //Sound prompt
        //audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length - 1)];
        //audioSource.Play();
        anim.SetTrigger("Hit");
        //Spawn damage text (numbers)
        GameObject go = Instantiate(damagePrefab, transform.position, damagePrefab.transform.rotation);
        go.GetComponent<DamageText>().SetDamageText(damage);

        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }
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

        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }
    public override void Die()
    {
        StartCoroutine("DelayedDeath");
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
        PartyMaster.Instance.availableMercPrefabs.Remove(this);
        TurnMaster.Instance.theCowardly.Add(mercName);
        TurnMaster.Instance.RemoveTurnTaker(this);

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



        yield return new WaitForSeconds(.1f);
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
            RefMaster.Instance.enemies.Remove(this); //not ideal
        }

        TurnMaster.Instance.RemoveTurnTaker(this);

        //TurnMaster.Instance.turnTakers.Remove(this);
        //TurnMaster.Instance.turnPlates.Remove(myTurnPlate);

        //Destroy(myTurnPlate.gameObject);
        Destroy(gameObject);
    }

    public Vector2Int GetGridPos()
    {
        return tileWalker.gridPos;
    }

    public void SetupPurpleBuff(GameObject tgt)
    {
        hasPurple = true;
        purpleTgt = tgt;
    }

    List<GameObject> effectIcons = new List<GameObject>();
    public int purpleMultiplier;

    public void AddEffectIcon(Sprite newEffectIcon, string ID)
    {
        GameObject go = Instantiate(effectIconPrefab, effectIconParent);
        go.GetComponentInChildren<SpriteRenderer>().sprite = newEffectIcon;
        go.name = ID;
        effectIcons.Add(go);
        worldSpaceHorizontalGroup.UpdateGroup();
    }

    public void RemoveIconByColor(string colorName)
    {
        if (effectIcons.Count > 0)
        {
            GameObject toRemove = effectIcons.Where(x => x.name == colorName).SingleOrDefault();
            effectIcons.Remove(toRemove);
            Destroy(toRemove);
            //GameObject toRemove;// = new GameObject(); //shouldn't and dont need to
            //foreach (GameObject icon in effectIcons)
            //{
            //    if (icon.name == colorName)
            //    {
            //        toRemove = icon;
            //    }
            //}
            //if (toRemove != null)
            //{
            //    effectIcons.Remove(toRemove);
            //    Destroy(toRemove);
            //}
        }
        worldSpaceHorizontalGroup.UpdateGroup();

        //Destroy(effectIcons.Where(x => x.name == colorName).FirstOrDefault());
        //effectIcons.Remove(effectIcons.Where(x => x.name == colorName).FirstOrDefault());
    }

    public void SetGridPos(Vector2Int newPos)
    {
        Debug.LogWarning("something is trying to change + " + name + "'s gridpos");
    }

    public string GetName()
    {
        return name;
    }
}


