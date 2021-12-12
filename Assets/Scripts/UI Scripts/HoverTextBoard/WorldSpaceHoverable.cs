using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldSpaceHoverable : MonoBehaviour
{
    [SerializeField] string myData;

    public string SetMyData { set => myData = value; }

    public virtual string GetMyData { get => myData; }
    private void OnMouseEnter()
    {
        HoverTextBoard.Instance.SetMe(myData);
    }

    public void OnMouseExit()
    {
        HoverTextBoard.Instance.UnSetMe();
    }
}
