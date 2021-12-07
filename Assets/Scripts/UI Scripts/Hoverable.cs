using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string myData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverTextBoard.Instance.SetMe(myData, transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverTextBoard.Instance.UnSetMe();
    }
}
