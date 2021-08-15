﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform tgt;
    public Vector3 tgtPos;
    [SerializeField]
    float arrowSpeed;
    [SerializeField]
    float ttl;

    public GameObject hitEffect; //hit GFX
    //public Arrow(Transform newTgt)
    //{
    //    tgt = newTgt;
    //}
    private void Start()
    {
        //Debug.LogError("arrow shot");
      //  tgt = Pathfinder.Instance.target.transform;
        //Destroy(gameObject, ttl);
        //tgtPos = tgt.position;// + new Vector3(0, tgt.lossyScale.y/2, 0);
        if(tgt)
        {
            tgtPos = tgt.position;
        }
        else
        {
            Debug.LogError("Rock spawns but no set target");
            //Destroy
        }
    }
    private void Update()
    {
        //if (tgt)
        //{
            transform.LookAt(tgtPos);

            transform.Translate(Vector3.forward * arrowSpeed * Time.deltaTime);

            arrowSpeed -= arrowSpeed / 2f * Time.deltaTime;

            if (Vector3.Distance(transform.position, tgtPos) <= 1f)
            {
                //Instantiate(hitEffect, tgtPos + hitEffect.transform.position, hitEffect.transform.rotation);
                Instantiate(hitEffect, tgtPos, hitEffect.transform.rotation);
                // Stop projectile from moving, and play hit animation... also do the DamageGlow (red)
                Destroy(gameObject);
                // Destroy(this);
            }
        }
    //}
}