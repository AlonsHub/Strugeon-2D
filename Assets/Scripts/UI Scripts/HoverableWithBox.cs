using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using UnityEngine;

public class HoverableWithBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    protected BasicDisplayer basicDisplayer;

    public virtual void SetMyDisplayer(List<string> strings, List<Sprite> sprites)
    {
        basicDisplayer.SetMe(strings, sprites);
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (basicDisplayer)
        {
            basicDisplayer.gameObject.SetActive(true);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (basicDisplayer)
        {
            basicDisplayer.gameObject.SetActive(false);

        }
    }


}
