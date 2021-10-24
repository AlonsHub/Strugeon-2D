﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charmed : MonoBehaviour
{
    [SerializeField]
    WeaponItem weaponItem;
    [SerializeField]
    int lifetime; //current number of turns left
    
    public void SetMe(WeaponItem wi, int totalLifetime)
    {
        weaponItem = wi;
        lifetime = totalLifetime;
        weaponItem.targets = RefMaster.Instance.mercs;

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
        weaponItem.targets = RefMaster.Instance.enemies;
        Destroy(this); //just this Charmed component! 
    }

}
