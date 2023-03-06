using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_StatusEffect_Interfaces
{
    
}

public interface I_StatusEffect_TurnStart
{

}
public interface I_StatusEffect_TurnEnd
{

}
public interface I_StatusEffect_ActionWeightManipulator
{

}
public interface I_StatusEffect_OutgoingDamageMod
{
    float OperateOnDamage(float originalDamage);
}
public interface I_StatusEffect_IncomingDamageMod
{
    float OperateOnDamage(float originalDamage);
}



