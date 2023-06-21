using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCollider : MonoBehaviour
{
    List<Pawn> _healedThisRound;
    List<Pawn> _inRange;

    public List<Pawn> GetHealedThisRound => _healedThisRound;
    public List<Pawn> GetInRangeTargets => _inRange;

    Pawn poorSoul;

    int _amount;

    public void Init(int amount, Pawn target)
    {
        _amount = amount;
        poorSoul = target;
    }
    private void Awake()
    {
        _healedThisRound = new List<Pawn>();
        _inRange = new List<Pawn>();
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if(other.CompareTag("Merc"))
        {
            //CAN BE SIMPLIFIED if we check the contains against a list of GameObject Pawns, before the GetCOmponent.
            Pawn p = other.GetComponentInParent<Pawn>();
        print(p.mercName);
            _inRange.Add(p);
            if (_healedThisRound.Contains(p))
                return;


            BattleLogVerticalGroup.Instance.AddEntry("Vitality Theft", ActionSymbol.Heal, p.Name, _amount, Color.green); //make vitality theft symbol //TBF fix to be a sturgeon green
            poorSoul.TakeDamage(_amount);
            p.Heal(_amount);

            _healedThisRound.Add(p);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Merc"))
        {
            //CAN BE SIMPLIFIED if we check the contains against a list of GameObject Pawns, before the GetCOmponent.
            Pawn p = other.GetComponentInParent<Pawn>();
            //if (!_healedThisRound.Contains(p))
            //    return;
            _inRange.Remove(p);
        }
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
