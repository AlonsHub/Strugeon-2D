using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBenefit
{
    string BenefitProperNoun();
    string BenefitStatName();

    int Value();

    //something which implies/directs the Order in which benfits are applied (like a math-order or yu-gi-oh stages)

}
