using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySummary : MonoBehaviour
{
    [SerializeField]
    Image img;

    public void SetMe(Sprite s)
    {
        img.sprite = s;
    }
}
