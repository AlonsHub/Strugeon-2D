using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAddonItem : ActionItem
{
    Censer censer;
    WeaponItem attachedWeapon;
    public override void Awake()
    {
        base.Awake();
        attachedWeapon = GetComponent<WeaponItem>();
        if(!attachedWeapon)
        {
            Debug.LogError("Weapon add on could not find a weaponItem on it's gameObject. Name: " + name);
        }
    }
    public override void Action(GameObject tgt) //getting an add-on
    {
        //tgt is a gameobject with a <censor> like object
         censer = tgt.GetComponent<Censer>();
        //BatllelogVerticalGroup.Instance.AddEntry(pawn.name, ActionIcon.Censer, pawn.name);

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Censer, censer.Name);

        if (censer.currentNode.GetDistanceToTarget(pawn.tileWalker.currentNode) > 14)
        {
            //walk to censer first
            attachedWeapon.feetItem.Action(censer.gameObject);
        }
        //StartCoroutine("AddFireEffectIcon"); //walk AND use, or just use - may want to add another waitforseconds between them
        StartCoroutine("AddFireEffect"); //walk AND use, or just use - may want to add another waitforseconds between them


    }

    IEnumerator AddFireEffect()
    {
        yield return new WaitUntil(() => pawn.tileWalker.currentNode.GetDistanceToTarget(censer.currentNode) <= 14);
        attachedWeapon.AddEffect(censer.effectAddon);
        attachedWeapon.effectColour = RefMaster.Instance.censer.effectColour; // Get this from setting the effect please and not like an asshole, thank you <3
        pawn.AddEffectIcon(censer.effectIcon, "fireBuff");
    }

    void AddFireEffectIcon()
    {
        attachedWeapon.AddEffect(censer.effectAddon);
        attachedWeapon.effectColour = RefMaster.Instance.censer.effectColour; // Get this from setting the effect please and not like an asshole, thank you <3
        pawn.AddEffectIcon(censer.effectIcon, "fireBuff");
    }

    public override void CalculateVariations()
    {
            actionVariations.Clear(); // INPORTANT and Better!
        if (attachedWeapon.hasEffect)
        {
            return;
        }

        int currentDistance = attachedWeapon.pawn.tileWalker.currentNode.GetDistanceToTarget(RefMaster.Instance.censer.currentNode);
        int weight = baseCost;

        if (currentDistance <= 14)
        {
            weight *= 10;
        }

        if (pawn.targets.Count > 0)
        {
            bool areEnemiesCloser = false;
            foreach (Pawn potentionalTgt in pawn.targets)
            {
                if (pawn.tileWalker.currentNode.GetDistanceToTarget(potentionalTgt.tileWalker.currentNode) <= currentDistance)
                {
                    areEnemiesCloser = true;
                    break;
                }
            }

            if (!areEnemiesCloser)
            {
                weight *= 5;
            }
        }

        actionVariations.Add(new ActionVariation(this, RefMaster.Instance.censer.gameObject, weight));
    }
}
