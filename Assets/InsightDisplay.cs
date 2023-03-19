using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsightDisplay : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer portrait;
    [SerializeField]
    SpriteRenderer actionSymbol;

    public void SetMe(ActionVariation av)
    {
        Pawn tgtPawn = av.target.GetComponent<Pawn>();
        if (tgtPawn)
            portrait.sprite = tgtPawn.PortraitSprite;
        else
            portrait.sprite = null;

        if (av.relevantItem is WeaponItem)
        {
            actionSymbol.sprite = BattleLogVerticalGroup.Instance.GetActionSymbol(ActionSymbol.Attack);
        }
        else if (av.relevantItem is SA_Item)
        {
            actionSymbol.sprite = (av.relevantItem as SA_Item).SA_Sprite();
        }

    }

    public void KillMe()
    {
        Destroy(gameObject);
    }

}
