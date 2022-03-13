using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStarter : MonoBehaviour
{
    [SerializeField]
    GameObject tileFireEffect;

    [SerializeField]
    LayerMask floortileMask;

    
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(r, out hit, 100f, floortileMask))
            {
                FloorTile ft = hit.collider.GetComponentInParent<FloorTile>();

                BurningTileEffect tileEffect = Instantiate(tileFireEffect).GetComponent<BurningTileEffect>();

                tileEffect.SetEffect(ft, 5, 10);
            }
        }
    }
}
