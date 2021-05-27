using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTile : MonoBehaviour
{
    TileWalker tw;
    void Start()
    {
        tw = GetComponent<TileWalker>();
        StartCoroutine("GetDoorTile");
    }

   IEnumerator GetDoorTile()
    {
        yield return new WaitUntil(() => tw.currentNode);
        FloorGrid.Instance.doorWalker = tw;
        
    }
}
