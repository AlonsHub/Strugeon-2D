using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinded : MonoBehaviour
{
    [SerializeField]
    WeaponItem weaponItem;
    [SerializeField]
    int lifetime; //current number of turns left

    int oldDamage;

    public void SetMe(WeaponItem wi, int totalLifetime)
    {
        weaponItem = wi;
        lifetime = totalLifetime;
        //weaponItem.targets = RefMaster.Instance.mercs.Where(x => x.Name != wi.pawn.Name).ToList(); // <- "Why would you hit yourself?!"
        //weaponItem.targets = RefMaster.Instance.mercs; // <- "Why not?"
        oldDamage = weaponItem.damage;
        weaponItem.damage = -20; //makes sure the damage is less than 0 beacuse damage that is less than 0 becomes 0.
        StartCoroutine(TurnCounter());
    }

    IEnumerator TurnCounter()
    {
        while (lifetime > 0)
        {
            yield return new WaitUntil(() => weaponItem.pawn.TurnDone);
            lifetime--;
            yield return new WaitUntil(() => !weaponItem.pawn.TurnDone);
        }
        // change pawn back
        //weaponItem.targets = RefMaster.Instance.enemies;
        weaponItem.damage = oldDamage;
        Destroy(this); //just this Charmed component! 
    }
}
