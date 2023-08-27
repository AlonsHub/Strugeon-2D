using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    MagicItem magicItem;

    [SerializeField]
    Image icon;

    [SerializeField]
    BasicDisplayer hoverBd;

    Transform _oldParent;
    Vector3 _ogLocalPos;
    private void Awake()
    {
        _ogLocalPos = hoverBd.transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Input.mousePosition.x > Screen.currentResolution.width/2)
        {
            hoverBd.transform.localPosition = new Vector3(_ogLocalPos.x *-1f, _ogLocalPos.y, _ogLocalPos.z);
        }
        else
        {
            hoverBd.transform.localPosition = _ogLocalPos;
        }
        hoverBd.gameObject.SetActive(true);
        _oldParent = hoverBd.transform.parent;
        hoverBd.transform.SetParent(All_Canvases.FrontestCanvas.transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverBd.gameObject.SetActive(false);
        hoverBd.transform.SetParent(_oldParent);
    }

   public void SetMe(MagicItem mi)
    {
        magicItem = mi;
        icon.sprite = mi.itemSprite;


        hoverBd.SetMe(new List<string> { magicItem.magicItemName, magicItem.ItemDescription(), magicItem.fittingSlotType.ToString(), magicItem._BenefitsStat(), magicItem._Benefit().Value().ToString(), magicItem.goldValue.ToString() }, new List<Sprite> { magicItem.itemSprite });
    }
    public void SetMeEmpty()
    {
        magicItem = null;
        icon.sprite = null;
        hoverBd.gameObject.SetActive(false);
        //hoverBd.SetMe(new List<string> { "", "", "", "", "", "" }, new List<Sprite> {null});
    }

}
