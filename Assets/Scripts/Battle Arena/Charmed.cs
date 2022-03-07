using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class Charmed : MonoBehaviour
{
    [SerializeField]
    WeaponItem weaponItem;
    [SerializeField]
    int lifetime; //current number of turns left

    [SerializeField]
    Sprite iconToAddOn;
    private void Awake()
    {
        iconToAddOn = Resources.Load<Sprite>("Icons/Charm");
    }

    public void SetMe(WeaponItem wi, int totalLifetime)
    {
        weaponItem = wi;
        lifetime = totalLifetime;
        //weaponItem.targets = RefMaster.Instance.mercs.Where(x => x.Name != wi.pawn.Name).ToList(); // <- "Why would you hit yourself?!"
        weaponItem.targets = RefMaster.Instance.mercs; // <- "Why not?"

        Charmed[] existingBlindedComponent = GetComponents<Charmed>();

        if (existingBlindedComponent.Length > 1)
        {
            existingBlindedComponent[0].lifetime = totalLifetime;
            Destroy(this);
        }
        else
        {
            StartCoroutine(TurnCounter());
        }
    }

    IEnumerator TurnCounter()
    {
        weaponItem.pawn.AddEffectIcon(iconToAddOn, "charmed"); //the string here should be a varibale and ALL these STATUS classes, should inherit 99% of just one neat base class

            //yield return new WaitUntil(() => !weaponItem.pawn.TurnDone);            //Waits for the first time of it being the charmed-pawn's turn, then lowers lifetime after each turnDone, also checking right after

        while (lifetime > 0)
        {
            //yield return new WaitUntil(() => weaponItem.pawn.TurnDone);
            //lifetime--;
            yield return new WaitUntil(() => !weaponItem.pawn.TurnDone);
            lifetime--;
            yield return new WaitUntil(() => weaponItem.pawn.TurnDone);
            


        }
        weaponItem.pawn.RemoveIconByName("charmed");

        // change pawn back
        weaponItem.targets = RefMaster.Instance.enemyInstances;
        Destroy(this); //just this Charmed component! 
    }

}
