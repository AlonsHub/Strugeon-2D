using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttacher : Attacher
{
    int minDamage, maxDamage;

    public void SetMeFull(Pawn target, string buffIconName, int newMaxTTL, int minDmg, int maxDmg)
    {
        minDamage = minDmg;
        maxDamage = maxDmg;
        PoisonAttacher[] existingPoisonComponents = GetComponents<PoisonAttacher>();

        if (existingPoisonComponents.Length > 1)
        {
            existingPoisonComponents[0].StartCooldown();
            Destroy(this);
        }
        else
        {
            base.SetMe(target, buffIconName, newMaxTTL);
            ApplyEffect();
        }

    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        StartCoroutine(nameof(DamagePerTurn));
    }
    
    IEnumerator DamagePerTurn()
    {
        while (ttl > 0)
        {
            yield return new WaitUntil(() => tgtPawn.TurnDone != true);
            int dmg = Random.Range(minDamage, maxDamage+1);
            tgtPawn.TakeElementalDamage(dmg, Color.green);

            BattleLogVerticalGroup.Instance.AddEntry("Venom Bite", ActionSymbol.Poison, tgtPawn.Name,dmg, Color.green);

            ReduceTtlByOne();
            yield return new WaitUntil(() => tgtPawn.TurnDone == true);
        }
        RemoveEffect();
    }

}
