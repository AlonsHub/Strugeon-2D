using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    TMP_Text dmgTextDisplayer;
    [SerializeField]
    float riseSpeed;

    [SerializeField]
    float alphaSpeed;
    [SerializeField]
    float alphaDelay;

    bool _doAlpha;

    [SerializeField]
    float ttl;

    public void SetDamageText(int dmg)
    {
        dmgTextDisplayer.text = dmg.ToString();

        StartCoroutine(AlphaDelayed());


        Destroy(gameObject, ttl);
    }
    public void SetDamageText(int dmg, Color col)
    {
        dmgTextDisplayer.color = col;
        dmgTextDisplayer.text = dmg.ToString();

        StartCoroutine(AlphaDelayed());

        Destroy(gameObject, ttl);
    }


    private void Update()
    {
        transform.position += Vector3.forward * riseSpeed * Time.deltaTime;
        if(_doAlpha)
        dmgTextDisplayer.alpha -= Time.deltaTime * alphaSpeed;  
    }

    IEnumerator AlphaDelayed()
    {
        yield return new WaitForSeconds(alphaDelay);
        _doAlpha = true;
    }
}
