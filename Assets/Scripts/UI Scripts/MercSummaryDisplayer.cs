using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercSummaryDisplayer : MonoBehaviour
{
    [SerializeField]
    GameObject deathVeil;
    [SerializeField]
    GameObject fleeVeil;

    [SerializeField]
    Image portarit;

    public void SetMe(Sprite sprite, bool isDead, bool isEscaped)
    {
        portarit.sprite = sprite;
        deathVeil.SetActive(isDead);
        fleeVeil.SetActive(isEscaped);

        if (isEscaped && isDead)
            Debug.LogError("escaped and dead, how?");
        //ALSO CHECK FOR ifEscaped!

    }
    
}
