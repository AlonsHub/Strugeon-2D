using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrefs
{
    public int baseAttackValue;
    public float attackFoesAtRange0_modifier; //e.g. Root sits on a Pawn - for that pawn, the root is at range 0
    public float attackFoesAtRange1_modifier; // Melee range (1 square away)
    //public float attackFoesAtRange1_modifier; // Melee range (1 square away)
}
