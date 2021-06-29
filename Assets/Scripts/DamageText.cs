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
    float ttl;

    public void SetDamageText(int dmg)
    {
        dmgTextDisplayer.text = dmg.ToString();
        Destroy(gameObject, ttl);
    }
    private void Update()
    {
        transform.position += Vector3.forward * riseSpeed * Time.deltaTime;
        dmgTextDisplayer.alpha -= riseSpeed * Time.deltaTime * alphaSpeed;  
    }

}
