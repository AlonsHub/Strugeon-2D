using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAddonItem : ActionItem
{
    Censer censer;
    WeaponItem attachedWeapon;
    LookAtter la;

    public override void Awake()
    {
        base.Awake();
        attachedWeapon = GetComponent<WeaponItem>();
        la = GetComponentInChildren<LookAtter>();
        if (!attachedWeapon)
        {
            Debug.LogError("Weapon add on could not find a weaponItem on it's gameObject. Name: " + name);
        }
    }
    public override void Action(ActionVariation av) //getting an add-on
    {
        //tgt is a gameobject with a <censor> like object
         censer = av.target.GetComponent<Censer>();
        //BatllelogVerticalGroup.Instance.AddEntry(pawn.name, ActionIcon.Censer, pawn.name);

        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Censer, censer.Name);

        if (censer.currentNode.GetDistanceToTarget(pawn.tileWalker.currentNode) > 14)
        {
            //walk to censer first
            //attachedWeapon.feetItem.Action(censer.gameObject);
            attachedWeapon.feetItem.Action(av);
        }
        StartCoroutine(nameof(AddFireEffect)); //walk AND use, or just use - may want to add another waitforseconds between them
    }

    IEnumerator AddFireEffect()
    {
        yield return new WaitUntil(() => pawn.tileWalker.currentNode.GetDistanceToTarget(censer.currentNode) <= 14);

        if (la && censer)
            la.LookOnce(censer.transform);

        attachedWeapon.AddEffect(censer.effectAddon);
        attachedWeapon.effectColour = RefMaster.Instance.censer.effectColour; // Get this from setting the effect please and not like an asshole, thank you <3
        pawn.AddEffectIcon(censer.effectIcon, "fireBuff");
    }

    //void AddFireEffectIcon()
    //{
    //    attachedWeapon.AddEffect(censer.effectAddon);
    //    attachedWeapon.effectColour = RefMaster.Instance.censer.effectColour; // Get this from setting the effect please and not like an asshole, thank you <3
    //    pawn.AddEffectIcon(censer.effectIcon, "fireBuff");
    //}

    public override void CalculateVariations()
    {
            actionVariations.Clear(); // INPORTANT and Better!
        if (attachedWeapon.hasEffect)
        {
            return;
        }

        int currentDistance = attachedWeapon.pawn.tileWalker.currentNode.GetDistanceToTarget(RefMaster.Instance.censer.currentNode);
        int weight = baseweight;

        if (currentDistance <= 14) //distance of 1 tile
        {
            weight *= 5;// changed from 10 - 08/02/22
        }
        else if (pawn.HasRoot)
        {
            return;
        }
        //else //add 23/02/22 - this separates the two *5
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
                weight *= 5; //changed from 5
            }
        }

        actionVariations.Add(new ActionVariation(this, RefMaster.Instance.censer.gameObject, weight));
    }
}
