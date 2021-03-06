using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootDownItem : ActionItem, SA_Item
{
    [SerializeField]
    Sprite rootDownSprite;
    [SerializeField]
    string rootDownSpriteName;


    int _currentCooldown;

    [SerializeField]
    int fullCooldown;

    LookAtter la;
    private List<Pawn> targets;

    [SerializeField]
    int rootHP;
    [SerializeField]
    int minDamage, maxDamage, spikeDamage;
    [SerializeField]
    int minDamagePerLevel, maxDamagePerLevel, spikeDamagePerLevel;

    [SerializeField]
    int rootDuration;

    [SerializeField]
    GameObject rootVisual;

   

    public bool SA_Available()
    {
        return _currentCooldown <= 0;
    }

    public string SA_Description()
    {
        return "Root will create tangling vines around a hero, preventing him from moving for 2 turns.";
    }

    public string SA_Name()
    {
        return "Root";
    }

    public Sprite SA_Sprite()
    {
        return rootDownSprite;
    }

    public void StartCooldown()
    {
        _currentCooldown = fullCooldown;
    }

    public override void Awake()
    {
        actionVariations = new List<ActionVariation>();
        la = GetComponentInChildren<LookAtter>();
        base.Awake();
    }
    void Start()
    {
        _currentCooldown = 0; //TBF current cooldowns should be fixed logic for the SA which should inherit the weapon item

        if (pawn.isEnemy)
            targets = RefMaster.Instance.mercs;
        else
            targets = RefMaster.Instance.enemyInstances;
    }

    public override void Action(GameObject tgt)
    {
        StartCooldown();
        
        //start animation


        Pawn toRoot = tgt.GetComponent<Pawn>(); //could go by TileWalker as well

        if (!toRoot)
        {
            Debug.LogError("no pawn to root!");
            pawn.TurnDone = true;
            return;
        }

        la.LookOnce(tgt.transform);

        RootDownAttacher rda = tgt.AddComponent<RootDownAttacher>();

        //Scaling up root with mavka level
        rda.SetMeFull(toRoot, rootDownSpriteName, rootDuration, rootVisual, (rootHP + (pawn.enemyLevel-1)*5 ), minDamage + (pawn.enemyLevel - 1) * 2, maxDamage + (pawn.enemyLevel - 1) * 2, spikeDamage + (pawn.enemyLevel - 1) * 3);

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Walk, toRoot.Name);

        //attach a RootDown component to that pawn
            pawn.TurnDone = true;
    }

    public override void CalculateVariations()
    {
        actionVariations.Clear();

        if (_currentCooldown > 0)
        {
            _currentCooldown--;
            Debug.Log($"Cooldown is:{_currentCooldown}.");
            return;
        }

        foreach (var p in targets)
        {
            if (p.HasRoot)
                continue;

            actionVariations.Add(new ActionVariation(this, p.gameObject, baseweight)); //base cost should be high since it affects all allies.

        }
    }

    public void SetToLevel(int level)
    {
        minDamage += minDamagePerLevel * (level - 1);
        maxDamage += maxDamagePerLevel * (level - 1);
        spikeDamage += spikeDamagePerLevel * (level - 1);
    }
}
