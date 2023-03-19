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
        portrait.sprite = av.target.GetComponent<Pawn>().PortraitSprite;
        actionSymbol.sprite = BattleLogVerticalGroup.Instance.GetActionSymbol(ActionSymbol.Attack);
    }

    public void KillMe()
    {
        Destroy(gameObject);
    }

}
