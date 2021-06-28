using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObastacle : MonoBehaviour
{
    //[SerializeField]
    // LayerMask layerMask;

    Vector2Int gridPos;

    public void SpawnMeOnGrid()
    {
        FindOwnGridPos();

        FloorGrid.Instance.SetTileToStaticObstacle(gridPos, gameObject);
    }
    public void FindOwnGridPos() //just in case they get lost somehow
    {
        
        float x = (transform.position.x - FloorGrid.Instance.startingPoint.position.x) / (FloorGrid.Instance.gapSize.x + FloorGrid.Instance.tileSize.x);
        float y = (transform.position.z - FloorGrid.Instance.startingPoint.position.z) / (FloorGrid.Instance.gapSize.y + FloorGrid.Instance.tileSize.y);

        gridPos.x = Mathf.RoundToInt(x);
        gridPos.y = Mathf.RoundToInt(y);
        
        
    }

    //public void BlockMyArea()
    //{
    //    Collider myCol = GetComponent<Collider>();

    //    Collider[] hits = Physics.OverlapBox(myCol.bounds.center, myCol.bounds.extents, transform.rotation, layerMask);

    //    if(hits.Length == 0)
    //    {
    //        Debug.LogWarning("Static Obstacle got 0 hits");
    //        return;
    //    }

    //    foreach (var h in hits)
    //    {
    //        //f(h.CompareTag("")) //only checking on FloorTiles layer so... yeah, they're all FloorTiles
    //        FloorTile ft = h.GetComponentInParent<FloorTile>();
    //        if(!ft)
    //        {
    //            Debug.LogWarning("not floor tile found on hit");
    //            continue;
    //        }    
    //    }
    //}
}
