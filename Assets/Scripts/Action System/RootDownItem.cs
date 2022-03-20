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
    int rootDuration;

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
        la = GetComponent<LookAtter>();
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
        RootDownAttacher rda = tgt.AddComponent<RootDownAttacher>();
        rda.SetMeFull(toRoot, rootDownSpriteName, rootDuration, rootHP, minDamage, maxDamage, spikeDamage);

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

            actionVariations.Add(new ActionVariation(this, p.gameObject, baseCost)); //base cost should be high since it affects all allies.

        }
    }
}
