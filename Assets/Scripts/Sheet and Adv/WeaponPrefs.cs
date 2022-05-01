using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeaponPrefs
{
    public int baseAttackValue;
    public int attackFoesAtRange0_modifier; //e.g. Root sits on a Pawn - for that pawn, the root is at range 0
    public int attackFoesAtRange1_modifier; // Melee range (1 square away)
    [Tooltip("RANGED-Attackers ONLY")]
    public int attackFoesWithinAttackRange_modifier; // Ranged-attacker only?
    public int approachFoesOutOfAttackRange_modifier; // Both ranged and melee
    //public float attackFoesAtRange1_modifier; // Melee range (1 square away)
}
