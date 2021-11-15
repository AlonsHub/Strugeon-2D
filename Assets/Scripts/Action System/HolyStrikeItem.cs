using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyStrikeItem : MonoBehaviour
{
    [SerializeField]
    WeaponItem weaponItem;

    [SerializeField]
    int maxDmg;
    [SerializeField]
    int minDmg;

    [Tooltip("As % chance to occur")]
    [SerializeField]
    int chanceToProc; //out of a hundred

    [SerializeField]
    GameObject addonPrefab;
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
        if(roll < chanceToProc)
        {
            Debug.Log("Yes strike");
            //add extra VFX
            //add extra damage (perhaps with its own dmg txt)
            weaponItem.ExtraDamageTarget(minDmg, maxDmg);
            BattleLogVerticalGroup.Instance.AddToNextEntry(addonPrefab);
        }
        else
        {
            Debug.Log("No strike");
        }
    }
}
