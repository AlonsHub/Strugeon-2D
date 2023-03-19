using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VenomBiteItem : ActionItem, SA_Item
{
    [SerializeField]
    Sprite venomBiteSprite;
    [SerializeField]
    string venomBiteSpriteName;

    [SerializeField]
    int minDamage, maxDamage, minPoisonDamage, maxPoisonDamage; //this sets them all
    [SerializeField]
    int minDamagePerLevel, maxDamagePerLevel, minPoisonDamagePerLevel, maxPoisonDamagePerLevel; //this sets them all

    int _currentCooldown;

    [SerializeField]
    int fullCooldown;

    [SerializeField]
    int posionDuration;

    List<Pawn> targets;

    Pawn toHit;

    System.Action attackAction;
    LookAtter la;
    public bool SA_Available()
    {
        return !(_currentCooldown > 0);
    }

    public string SA_Description()
    {
        return "poisons stuff [I don't think this ever appears anyhere]";
    }

    public string SA_Name()
    {
        return "Venom Bite";
    }

    public Sprite SA_Sprite()
    {
        return venomBiteSprite;
    }

    public void StartCooldown()
    {
        _currentCooldown = fullCooldown;
    }
    public override void Awake()
    {
        actionVariations = new List<ActionVariation>();
        if(!la)
        la = GetComponentInChildren<LookAtter>();

        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentCooldown = 0;

        if (pawn.isEnemy)
            targets = RefMaster.Instance.mercs;
        else
            targets = RefMaster.Instance.enemyInstances;
    }
   

    public override void Action(ActionVariation av)
    {
        StartCooldown();

        toHit = av.target.GetComponent<Pawn>();

        if (!toHit)
        {
            Debug.Log("no tgt to hit");
            pawn.FinishAnimation();
            return;
        }


        //int dist = pawn.tileWalker.currentNode.GetDistanceToTarget(toHit.tileWalker.currentNode);
        int dist = pawn.tileWalker.GetDistanceFromMeToYou(toHit.tileWalker);

        if (dist > 14)
        {
            StartCoroutine(WalkThenAttack(toHit));
            return;
        }

        if (av.target && la)
            la.LookOnce(av.target.transform);
            //la.tgt = tgt.transform;



        //attackAction?.Invoke();
        //pawn.transform.LookAt(tgt.transform);
        //pawn.transform.rotation = Quaternion.Euler(0, pawn.transform.eulerAngles.y, 0);
        pawn.anim.SetTrigger("VenomBite"); // sets TurnDone via animation behaviour
    }

    IEnumerator WalkThenAttack(Pawn tgt)
    {
        pawn.tileWalker.StartNewPathWithRange(tgt.tileWalker, 1);

        yield return new WaitUntil(() => !pawn.tileWalker.hasPath || pawn.TurnDone); // 

        if (!pawn.TurnDone) //in case of step limiters
        {
            if (tgt && la)
            la.LookOnce(tgt.transform);

            attackAction?.Invoke();
            pawn.anim.SetTrigger("VenomBite"); // sets TurnDone via animation behaviour
        }
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear();

        if(_currentCooldown>0)
        {
            _currentCooldown--;
            return;
        }    

        if (targets.Count == 0)
        {
            Debug.Log(name + " Found no enemies, no weapon action variations added");
            return;// end match
        }


        //if (feetItem)
        //    feetItem.currentRangeInTiles = range; // maybe do this at Start()?

        foreach (Pawn p in targets)
        {
            int weight = baseweight;

            //int currentDistance = pawn.tileWalker.currentNode.GetDistanceToTarget(p.tileWalker.currentNode);
            int currentDistance = pawn.tileWalker.GetDistanceFromMeToYou(p.tileWalker);

            if (p.currentHP <= 0)
                continue;
            if (pawn.tileWalker.elevation != p.tileWalker.elevation)
                continue;
            

            if (currentDistance <= 1 * 14) //melee=14 ranged is more
            {
                //melee attacker only have 1 range, so this means adjacent
                weight *= 20; // changed from 20 to 2 //changed back to 20
            }
            if (p.currentHP <= p.maxHP / 2.5f) //40%
            {
                weight *= 10;
            }

            if (weight != 0)
            {
                actionVariations.Add(new ActionVariation(this, p.gameObject, weight));
            }
        }

    }

    public void VenomBiteAnimEvent() //called by animation event trigger 
    {
        int dmg = Random.Range(minDamage, maxDamage + 1);
        toHit.TakeDamage(dmg);
        //PoisonAttacher pa = toHit.gameObject.AddComponent<PoisonAttacher>();
        //pa.SetMeFull(toHit, venomBiteSpriteName, posionDuration, minPoisonDamage, maxPoisonDamage);

        new VenomEffect(toHit, venomBiteSprite,pawn, minPoisonDamage, maxPoisonDamage);

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Attack, toHit.Name, dmg, Color.red);



        pawn.FinishAnimation();
    }

    public void SetToLevel(int level)
    {
        minDamage += minDamagePerLevel * (level - 1);
        maxDamage += maxDamagePerLevel * (level - 1);
        minPoisonDamage += minPoisonDamage * (level - 1);
        maxPoisonDamage += maxPoisonDamagePerLevel * (level - 1);
    }
}
