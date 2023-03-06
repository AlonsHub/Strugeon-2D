using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MathOperator {Add,Subtract,Multiply,Divide,Power,Root};
/// <summary>
/// Damage modifiers are the same for incoming and outgoing damage, the difference being the lists to which they are added
/// The "AfterDamageCalc list of modifiers will be scanned
/// 
/// 
/// PROBLEM IS - Any use of this would need to correspond with reducing the status effect duration...
/// maybe status effects will hold the modifiers - and subscribe?
/// </summary>
[System.Serializable]
public class DamageModifier
{
    /// <summary>
    /// the value with which modification will be made.
    /// </summary>
    public float mod;

    /// <summary>
    /// the math "sign" for the operation (+, -, *, /,^...)
    /// </summary>
    public MathOperator mathOperator;

    /// <summary>
    /// set to 1 if just on
    /// </summary>
    public int totalDuration;

    public int currentDuration;

    

    /// <summary>
    /// use this to modify values in a way that is oblivious to the type of opertaion
    /// </summary>
    /// <param name="inValue"></param>
    /// <returns></returns>
    public float Operate(float inValue)
    {
        switch (mathOperator)
        {
            case MathOperator.Add:
                return inValue + mod;
                break;
            case MathOperator.Subtract:
                return inValue - mod;
                break;
            case MathOperator.Multiply:
                return inValue * mod;
                break;
            case MathOperator.Divide:
                return inValue / mod;
                break;
            case MathOperator.Power:
                return Mathf.Pow(inValue,mod);
                break;
            case MathOperator.Root:
                float outValue = inValue;
                for (int i = 0; i < mod; i++)
                {
                    outValue = Mathf.Sqrt(outValue);
                }
                return outValue;
                break;
            default:
                return -1;
                break;
        }
    }
}
