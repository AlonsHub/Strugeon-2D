using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField]
    private float deathTime;
    //Animator anim;
    private void Start()
    {
        //anim = GetComponent<Animator>();
        Destroy(gameObject, deathTime);
    }
}
