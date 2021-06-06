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

    int initiative;
    int initiativeBonus;

    bool turnDone;
    bool actionDone;

    public List<Pawn> targets;

    public TileWalker tileWalker;

    public Animator anim;

    [SerializeField]
    private Sprite portraitSprite;

    public Transform effectIconParent;
    public GameObject effectIconPrefab;

    public GameObject myTurnPlate;

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
    public bool TurnDone { get => turnDone; set => turnDone = value; } //on a turntakers turn, turn order system waitsUntil TurnDone


    public int Initiative { get => initiative; set => initiative = value + initiativeBonus; }
    public bool ActionDone { get => actionDone; set => actionDone = value; } //Item has performed its action and is reporting "done"
    public bool DoDoubleTurn { get => doDoubleTurn; set => doDoubleTurn = value; }
    public bool DoSkipTurn { get => doSkipTurn; set => doSkipTurn = value; }
    public bool DoModifyDamage { get => doModifyDamage; set => doModifyDamage = value; }
    public float DamageModifier { get => damageModifier; set => damageModifier = value; }

    //public bool MovementDone { get => movementDone; set => movementDone = value; } // OPTIONAL - to have walk and attack actions
    
    public string Name { get => pawnName; } 
    public Sprite PortraitSprite { get => portraitSprite; set => portraitSprite = value; }

    public static int totalPawns = 0;

    public override void Init()
    {
        base.Init(); // HP init
        anim = GetComponent<Animator>();
        tileWalker = GetComponent<TileWalker>();
        tileWalker.Init();
        actionItems = GetComponents<ActionItem>().ToList();
        audioSource = GetComponent<AudioSource>();
        worldSpaceHorizontalGroup = GetComponentInChildren<WorldSpaceHorizontalGroup>();


        if (isEnemy)
        {
            targets = RefMaster.Instance.mercs;
        }
        else
        {
            targets = RefMaster.Instance.enemies;
          //  RefMaster.Instance.mercs.Add(this);
        }

        if (isEnemy)
        {
            name = totalPawns.ToString() + " Monster";
        }
        else
        {
            name.Replace("(Clone)", "");
        }

        totalPawns++;
        //grab art?
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
        //In previous version: I checked for double turn here, to subtract turn order by 1 to make the next turn mine again
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

        //  TurnMaster.Instance.RemoveTurnTaker(this);
        //if (!isEnemy)
        //{
        //    //RefMaster.Instance.mercs.Remove(RefMaster.Instance.mercs.Where(x => x.name == name).SingleOrDefault()); //not ideal
        //    RefMaster.Instance.mercs.Remove(this); //not ideal
        //}
        //else
        //{
        //    //RefMaster.Instance.enemies.Remove(RefMaster.Instance.enemies.Where(x => x.name == name).SingleOrDefault()); //not ideal
        //    RefMaster.Instance.enemies.Remove(this); //not ideal
        //}

        //TurnMaster.Instance.turnTakers.Remove(this);
        ////PartyMaster.Instance.currentMercParty.Remove(PartyMaster.Instance.currentMercParty.Where(x => x.name == name).SingleOrDefault());
        //StopAllCoroutines();
        //Destroy(gameObject, .01f);
    }

    IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(.1f);
        if (!isEnemy)
        {
            //RefMaster.Instance.mercs.Remove(RefMaster.Instance.mercs.Where(x => x.name == name).SingleOrDefault()); //not ideal
            RefMaster.Instance.mercs.Remove(this); //not ideal
        }
        else
        {
            //RefMaster.Instance.enemies.Remove(RefMaster.Instance.enemies.Where(x => x.name == name).SingleOrDefault()); //not ideal
            RefMaster.Instance.enemies.Remove(this); //not ideal
        }

        TurnMaster.Instance.turnTakers.Remove(this);
        TurnMaster.Instance.turnPlates.Remove(myTurnPlate.transform);
        Destroy(myTurnPlate);
        //TurnMaster.Instance.turnPlates.r
        //PartyMaster.Instance.currentMercParty.Remove(PartyMaster.Instance.currentMercParty.Where(x => x.name == name).SingleOrDefault());
        StopAllCoroutines();
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
            GameObject toRemove = new GameObject(); //shouldn't and dont need to
            foreach (GameObject icon in effectIcons)
            {
                if (icon.name == colorName)
                {
                    toRemove = icon;
                }
            }
            if (toRemove != null)
            {
                effectIcons.Remove(toRemove);
                Destroy(toRemove);
            }
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


