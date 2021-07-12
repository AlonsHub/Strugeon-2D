using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiveBody : MonoBehaviour
{
    public int currentHP;
    public int maxHP;

    public virtual void Init()
    {
        currentHP = maxHP;
    }
    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            Die();
        }
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
