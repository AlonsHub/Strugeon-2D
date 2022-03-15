using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomBiteItem : ActionItem, SA_Item
{
    [SerializeField]
    Sprite venomBiteSprite;
    [SerializeField]
    string venomBiteSpriteName;

    [SerializeField]
    int minDamage, maxDamage, minPoisonDamage, maxPoisonDamage; //this sets them all

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
        la = GetComponent<LookAtter>();
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
   

    public override void Action(GameObject tgt)
    {
        StartCooldown();

        toHit = tgt.GetComponent<Pawn>();

        if (!toHit)
        {
            Debug.Log("no tgt to hit");
            return;
        }


        int dist = pawn.tileWalker.currentNode.GetDistanceToTarget(toHit.tileWalker.currentNode);

        if (dist > 14)
        {
            StartCoroutine(WalkThenAttack(toHit));
            return;
        }

        if (tgt && la)
            la.tgt = tgt.transform;



        attackAction?.Invoke();
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
                la.tgt = tgt.transform;

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
            int weight = baseCost;

            int currentDistance = pawn.tileWalker.currentNode.GetDistanceToTarget(p.tileWalker.currentNode);

            if (p.currentHP <= 0)
                continue;
            //if(p.currentHP > 0)
            //{
            //    weight *= 5;

            //}

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
        toHit.TakeDamage(Random.Range(minDamage, maxDamage + 1));
        PoisonAttacher pa = toHit.gameObject.AddComponent<PoisonAttacher>();
        pa.SetMeFull(toHit, "Poison", posionDuration, minPoisonDamage, maxPoisonDamage);
    }


}
