using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SacntumItemHover : HoverableWithBox
{
    [SerializeField]
    float leftX, rightX, topY, bottomY;

    [SerializeField]
    SanctumItemDisplayer sanctumItemDisplayer;

    [SerializeField]
    RectTransform rectTrans;

    private void Start()
    {
        MagicItem mi = sanctumItemDisplayer.magicItem;
        SetMyDisplayer(new List<string> { mi.magicItemName, mi.fittingSlotType.ToString(),  mi._Benefit().BenefitStatName(), mi._Benefit().Value().ToString(), mi.ItemDescription(), mi.goldValue.ToString() }, new List<Sprite> { mi.itemSprite});
        //FixLocalPos();
    }

    private void FixLocalPos()
    {
        //Debug.LogError($"Rect x: {rectTrans.rect.position.x} y:{rectTrans.rect.position.y}");
        //Debug.LogError($"Transform x: {transform.position.x} y:{transform.position.y}");
        if (transform.position.x > 1200f)
        {
            basicDisplayer.transform.localPosition = new Vector3(leftX, basicDisplayer.transform.localPosition.y, basicDisplayer.transform.localPosition.z);
        }
        else
        {
            basicDisplayer.transform.localPosition = new Vector3(rightX, basicDisplayer.transform.localPosition.y, basicDisplayer.transform.localPosition.z);
        }

        

        basicDisplayer.transform.localPosition = new Vector3(basicDisplayer.transform.localPosition.x, topY - transform.position.y/170, basicDisplayer.transform.localPosition.z);

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        FixLocalPos();
        basicDisplayer.transform.SetParent(All_Canvases.FrontestCanvas.transform);
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        basicDisplayer.transform.SetParent(transform);
        base.OnPointerExit(eventData);
    }
}
