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

    public void SetDamageText(int dmg)
    {
        dmgTextDisplayer.text = dmg.ToString();
        Destroy(gameObject, 2);
    }
    private void Update()
    {
        transform.position += Vector3.forward * riseSpeed * Time.deltaTime;
        dmgTextDisplayer.alpha -= riseSpeed * Time.deltaTime * .5f;  
    }

}
