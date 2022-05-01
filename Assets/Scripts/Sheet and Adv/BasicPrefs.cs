using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a profile of ALL the specific ActionItem-preferences to generic types of action-conditions (e.g. the modifier for for attacking foes at range 1)
[System.Serializable]
public class BasicPrefs 
{
    [Header("Combat:")]
    public WeaponPrefs weaponPrefs;
    public WeaponAddOnPrefs weaponAddOnPrefs;
    public TargetHealthPrefs targetHealthPrefs;

    [Header("Composure:")]
    public FleePrefs fleePrefs;
    public SuggestivePrefs suggestivePrefs;

    //Positioning Prefs!!! TBD TBA(dded) TBF(ound... so I may find this one day)

    //

}
