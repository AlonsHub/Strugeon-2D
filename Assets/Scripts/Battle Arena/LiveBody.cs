using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiveBody : MonoBehaviour, I_Attackable
{
    public int currentHP;
    public int maxHP;

    //finalDamageModifier is the LAST multiplication done before applying damageto currentHP
    float finalDamageModifier = 1f; //mostly should stay at 1.
    public float FinalDamageMod { get => finalDamageModifier; set => finalDamageModifier = value; }//simple place holder, there should be a method to modifiying each modifer... TBF

    //finalHealModifier is the LAST multiplication done before applying heal to currentHP
    float finalHealModifier = 1f; //mostly should stay at 1.
    public float FinalHealModifier { get => FinalHealModifier; set => FinalHealModifier = value; } //simple place holder, there should be a method to modifiying each modifer... TBF


    public virtual void Init()
    {
        currentHP = maxHP;
    }
    public virtual int TakeDamage(int damage)
    {
        currentHP -= (int) (damage * finalDamageModifier);
        if(currentHP <= 0)
        {
            Die();
        }
        return currentHP;
    }
    public virtual int TakeDamage(int damage, bool? critOrGraze, Color col)
    {
        //do specific things for crit or graze? 

        //Some characters may get more/less damage from crits and grazes

        return TakeDamage(damage);
    }

    public virtual void Heal(int amount)
    {
        currentHP += (int)(amount * finalHealModifier);
        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
    }

    public virtual void Die()
    {
        Debug.Log(name.ToString() + " died at LiveBody level");
        Destroy(gameObject);
    }
}
