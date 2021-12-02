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
    [SerializeField]
    Sprite iconToAddOn;
    private void Awake()
    {
        iconToAddOn = Resources.Load<Sprite>("Icons/BlindIcon");
    }

    public void SetMe(WeaponItem wi, int totalLifetime)
    {
        weaponItem = wi;
        lifetime = totalLifetime;
        //weaponItem.targets = RefMaster.Instance.mercs.Where(x => x.Name != wi.pawn.Name).ToList(); // <- "Why would you hit yourself?!"
        //weaponItem.targets = RefMaster.Instance.mercs; // <- "Why not?"
        oldDamage = weaponItem.damage;
        weaponItem.damage = -20; //makes sure the damage is less than 0 beacuse damage that is less than 0 becomes 0.

        //target gets the icon set on it.



        StartCoroutine(TurnCounter());
    }

    IEnumerator TurnCounter()
    {
        weaponItem.pawn.AddEffectIcon(iconToAddOn, "blinded");
        while (lifetime > 0)
        {
            yield return new WaitUntil(() => weaponItem.pawn.TurnDone);
            lifetime--;
            yield return new WaitUntil(() => !weaponItem.pawn.TurnDone);
        }
        weaponItem.pawn.RemoveIconByColor("blinded");
        // change pawn back
        //weaponItem.targets = RefMaster.Instance.enemies;
        weaponItem.damage = oldDamage;
        Destroy(this); //just this Charmed component! 
    }
}
