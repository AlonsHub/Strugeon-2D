using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTester : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
            {
                Debug.LogError(hit.collider.name);
            }
        }
    }
}
