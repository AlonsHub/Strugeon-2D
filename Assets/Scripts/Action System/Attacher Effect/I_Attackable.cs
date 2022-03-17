using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Attackable //currently includes: pawns(as livebody is I_Attackable), and the attachers (both targetable and not) (need to be renamed to attackble)
{
    //provide access to the damage damage method?
    int TakeDamage(int damage); //each user of this interface will implement its own way to handle death-checks and the like
}
