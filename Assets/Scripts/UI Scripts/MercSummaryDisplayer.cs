using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercSummaryDisplayer : MonoBehaviour
{
    [SerializeField]
    GameObject deathVeil;
    [SerializeField]
    Image portarit;

    public void SetMe(Sprite sprite, bool isDead)
    {
        portarit.sprite = sprite;
        deathVeil.SetActive(isDead);
        //ALSO CHECK FOR ifEscaped!

    }
    
}
