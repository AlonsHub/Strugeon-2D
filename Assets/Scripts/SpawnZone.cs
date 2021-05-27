using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnZone : MonoBehaviour
{
    Collider col;
    [SerializeField]
    LayerMask layerMask;
    
    public List<Vector2Int> toReturn;

    public bool isMerc;

    private void Start()
    {
        col = GetComponent<Collider>();
        Invoke("GetZoneTiles", 1);
    }
    [ContextMenu("GetZoneTiles")]
    //public List<Vector2Int> GetZoneTiles()
    public void GetZoneTiles()
    {
       toReturn = new List<Vector2Int>();

        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, transform.rotation , layerMask);

        foreach(Collider c in cols)
        {
            toReturn.Add(c.GetComponentInParent<FloorTile>().gridIndex);
        }

        toReturn.Sort(SortSpawnTiles);

        if (!ArenaMaster.Instance)
        {
            Debug.LogError("No ArenaMaster.Instance");
        }

        if(isMerc)
        {
            ArenaMaster.Instance.mercSpawnTiles = new List<Vector2Int>(toReturn);
        }
        else
        {
            ArenaMaster.Instance.enemySpawnTiles = new List<Vector2Int>(toReturn);
        }

        //return toReturn;
    }

    int SortSpawnTiles(Vector2Int posA, Vector2Int posB)
    {
        if (posA.x > posB.x)
        {
            return -1;
        }
        if (posA.x < posB.x)
        {
            return 1;
        }

        if(posA.y > posB.y)
        {
            return -1;
        }
        if (posA.y < posB.y)
        {
            return 1;
        }

        //will never reach this, but just in-case
        Debug.LogError("double tiles!");
        return 0;
    }
}
