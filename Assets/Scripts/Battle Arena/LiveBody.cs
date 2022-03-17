using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiveBody : MonoBehaviour, I_Attackable
{
    public int currentHP;
    public int maxHP;

    public virtual void Init()
    {
        currentHP = maxHP;
    }
    public virtual int TakeDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            Die();
        }
        return currentHP;
    }
    public virtual void Heal(int amount)
    {
        currentHP += amount;
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
