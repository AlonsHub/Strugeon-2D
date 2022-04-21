using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindingItem : ActionItem, SA_Item //, SA_Item
{
    [SerializeField]
    int duration;
    //int currentCooldown = 0;
    //[SerializeField]
    //int maxCooldown;
    WeaponItem weaponItem;
    public List<Pawn> targets;

    [Tooltip("As % chance to occur")]
    [SerializeField]
    int chanceToProc; //out of a hundred
    [SerializeField]
    int chanceToProcPerLevel;
    //int chanceToProcPerLevels;
    //[SerializeField]
    //int levelsPerProc;

    [SerializeField]
    GameObject addonPrefab;

    int currentCooldown = 0;
    [SerializeField]
    int maxCooldown; //set in inspector

    public int saCooldown;
    [SerializeField]
    Sprite blindSprite;

    private void Start()
    {
        weaponItem = GetComponent<WeaponItem>();
        weaponItem.attackAction += OnAttack;
    }
    public void OnAttack()
    {
        //roll chance (15% as GDD)
        int roll = Random.Range(1, 101);

        Debug.Log("Something!");
        if (roll < chanceToProc)
        {
            Debug.Log("Blind!");
            //add extra VFX
            //add extra damage (perhaps with its own dmg txt)
            //weaponItem.ExtraDamageTarget(minDmg, maxDmg);
            Blinded blinded = weaponItem.toHit.gameObject.AddComponent<Blinded>();
            blinded.SetMe(weaponItem.toHit.GetComponent<WeaponItem>(), duration);
            BattleLogVerticalGroup.Instance.AddToNextEntry(addonPrefab);
        }
        else
        {
            Debug.Log("No Blind");
        }
    }

    public bool SA_Available()
    {
        return !(currentCooldown > 0);
    }

    public int CurrentCooldown()
    {
        return currentCooldown;
    }

    public void StartCooldown()
    {
        currentCooldown = saCooldown;
    }

    public Sprite SA_Sprite()
    {
        return blindSprite;
    }
    public string SA_Name()
    {
        return "Blinding Dart";
    }

    public string SA_Description()
    {
        return "Cheeto flings a Blinding Dart at an enemy, dealing regular damage and Blinding that enemy for 2 turns. Blinded enemie's attacks deal 0 damage."; //we'll see...
    }

    public void SetToLevel(int level)
    {
        chanceToProc += chanceToProcPerLevel* (level-1);
    }

    //private void Start()
    //{
    //    targets = pawn.targets;
    //}

    //// Start is called before the first frame update
    //public override void Action(GameObject tgt)
    //{
    //    Blinded blinded = tgt.gameObject.AddComponent<Blinded>();
    //    blinded.SetMe(tgt.GetComponent<WeaponItem>(), duration);

    //    //pawn.anim.SetTrigger(""); //normal attack? have some vfx?
    //    currentCooldown = maxCooldown;
    //    StartCoroutine(CountDownCool());
    //    pawn.TurnDone = true;
    //}

    //IEnumerator CountDownCool()
    //{
    //    while (currentCooldown > 0)
    //    {
    //        yield return new WaitUntil(() => !pawn.TurnDone);
    //        currentCooldown--;
    //        yield return new WaitUntil(() => pawn.TurnDone);
    //    }
    //}

    //public override void CalculateVariations()
    //{
    //    actionVariations.Clear(); //Most important to do this! when a merc looks for actionVariations - he may find stale ones here (even if returned on cooldown > 0)

    //    if (currentCooldown > 0)
    //    {
    //        return;
    //    }

    //    if (targets.Count <= 0)
    //    {
    //        Debug.Log(name + " Found no enemies, no weapon action variations added");
    //        return;// end match
    //    }

    //    int weight = baseCost;

    //    foreach (Pawn p in targets)
    //    {
    //        actionVariations.Add(new ActionVariation(this, p.gameObject, weight));
    //    }
    //}
}
