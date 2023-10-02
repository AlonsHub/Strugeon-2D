using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderForHP_Hover : MonoBehaviour
{
    [SerializeField]
    Pawn pawn;


    private void Awake()
    {
        if (!pawn)
            pawn = GetComponentInParent<Pawn>();
    }
    private void OnMouseEnter()
    {
        Debug.Log($"Mouse entered: {pawn.Name} aka {pawn.name}");

        Hover_HpBar.Instance.SetMe(pawn);
    }
    private void OnMouseExit()
    {
        Debug.Log($"Mouse exited: {pawn.Name} aka {pawn.name}");

        Hover_HpBar.Instance.SetOff();
    }
}
