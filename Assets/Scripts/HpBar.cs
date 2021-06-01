using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;


public class HpBar : MonoBehaviour
{
    //public Image greenBar;

    public Transform spriteMaskTrans;
    public Transform rotatingParent;
    public Transform myParent;

    public Pawn pawn;
    // Start is called before the first frame update
    //void Start()
    //{
    //    //pawn = GetComponent<Pawn>();
    //    //greenBar = GetComponent<Image>();
    //}
    Quaternion startRot;
    Quaternion localRot;
    private void Start()
    {
        //transform.GetChild(0).LookAt(Camera.main.transform);
     //    localRot = transform.localRotation = Quaternion.Euler(Vector3.zero);
        //Quaternion localRot = transform.localRotation;
        //transform.rotation = Camera.main.transform.rotation;
        startRot = transform.rotation;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //should change only onvaluechanged?
        //greenBar.fillAmount = (float) pawn.currentHP / (float)pawn.maxHP;
        //transform.localRotation = Quaternion.Euler(Vector3.zero);
        //transform.rotation = startRot;

        //Should be made into a "damage delt" event

        spriteMaskTrans.localScale = new Vector3 (2f * ((float)pawn.currentHP / (float)pawn.maxHP), 1, 1); //full hp is (2,1,1)



    }

    private void LateUpdate()
    {
        Vector3 newRot = rotatingParent.rotation.eulerAngles;
        //float div = newRot.y / 45f;
        newRot = (int)(newRot.y / 90) * 90 * Vector3.up;

        myParent.rotation = Quaternion.Euler(newRot); 
        
    }
}
