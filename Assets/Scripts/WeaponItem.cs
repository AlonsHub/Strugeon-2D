using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamageType {Slashing, Bludgening, Piercing, Pure}; // Pure? True? 

public class WeaponItem : ActionItem
{
    public FeetItem feetItem; //does the walking as an actionvariation added by weaponitem, but holding the feetItem in the List entry of actionsvariations 

    //added params:Range, Damage, ?
    public int range;
    public int damage;

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
    EffectAddonDataType effectData;
    public DamageType damageType;

    public List<Pawn> targets;
    public bool hasRedBuff;
    public Pawn toHit;
    public override void Awake()
    {
        actionVariations = new List<ActionVariation>();
        isWeapon = true;
        hasEffect = false;
        hasRedBuff = false;
        feetItem = GetComponent<FeetItem>();
        base.Awake();
    }

    private void Start()
    {
        if (pawn.isEnemy)
            targets = RefMaster.Instance.mercs;
        else
            targets = RefMaster.Instance.enemies;

       // arrowSpawn = pawn.arrow
    }
    public override void Action(GameObject tgt)
    {

        toHit = tgt.GetComponent<Pawn>();

        if (!toHit)
        {
            Debug.Log("word");
            return;
        }

        LookAtter la = GetComponentInChildren<LookAtter>();
        if(tgt && la)
        la.tgt = tgt.transform;


       
        //pawn.transform.LookAt(tgt.transform);
        //pawn.transform.rotation = Quaternion.Euler(0, pawn.transform.eulerAngles.y, 0);
        pawn.anim.SetTrigger("Attack"); // sets TurnDone via animation behaviour

    }

    public void ShootProjectile()
    {
        GameObject effectGo = Instantiate(arrowGfx, arrowSpawn.position, arrowSpawn.rotation);
        effectGo.GetComponent<Arrow>().tgt = toHit.transform;
        StartCoroutine(WaitForArrowToHit(effectGo));
    }

    IEnumerator WaitForArrowToHit(GameObject arrow) //or die, currently always hits
    {
        yield return new WaitUntil(() => (arrow == null));
        Debug.Log("Arrow hit");
        DamageTarget();
    }
    public void DamageTarget()
    {
        float rolledDamage = damage + Random.Range(-5, 6);
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
        }
        if (hasRedBuff)
        {
            rolledDamage *= 1.5f;
            hasRedBuff = false;
            pawn.RemoveIconByColor("redBuff");
        }

        toHit.TakeDamage((int)rolledDamage); // add time delay to reduce HP only after hit (atm this is done in TakeDamage and ReduceHP methods in character)
        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Attack, toHit.Name, (int)rolledDamage ,Color.red);

        pawn.TurnDone = true;
        //go.transform.LookAt(tgt.transform);
        //GetComponent<LookAt>().lookAtTargetPosition = tgt.transform.position;
        GetComponentInChildren<LookAtter>().tgt = null;
    }
    public void AddEffect(WeaponEffectAddon effectAddon)
    {
        hasEffect = true;
        effectData = effectAddon.data;

        pawn.TurnDone = true;
    }

    public override void CalculateVariations()
    {
        actionVariations = new List<ActionVariation>();

        if(targets.Count <= 0)
        {
            Debug.Log(name + " Found no enemies, no weapon action variations added");
            return;// end match
        }

        int weight = 0;
        feetItem.currentRangeInTiles = range;
        foreach(Pawn p in targets)
        {
            //int currentDistance = tileWalker.currentNode.GetDistanceToTarget(p.tileWalker.currentNode);
            int currentDistance = pawn.tileWalker.currentNode.GetDistanceToTarget(p.tileWalker.currentNode);
            
            if (currentDistance <= range*14)
            {
                if (isRanged && currentDistance <= 14)
                {
                    continue;
                }
                weight = 15;
                actionVariations.Add(new ActionVariation(this, p.gameObject, weight));
            }
            else
            {
                weight = 5;
                actionVariations.Add(new ActionVariation(feetItem, p.gameObject, weight));
            }
        }

        //Emergency step, to make sure you have SOME actionvariation to use
        //if(actionVariations.Count == 0)
        //{
        //    List<FloorTile> surroundingTiles = FloorGrid.Instance.GetNeighbours(tileWalker.currentNode);

        //    foreach (FloorTile ft in surroundingTiles)
        //    {
        //        if (!ft.hasObstacle && !ft.hasEnemy)
        //        {
        //            actionVariations.Add(new ActionVariation(this, ft.gameObject, baseCost));
        //        }
        //    }
        //}
    }
}
