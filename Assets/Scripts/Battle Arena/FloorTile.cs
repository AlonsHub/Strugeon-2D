using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TileColors {enemy, hero, blank };
[System.Serializable]
public class FloorTile : MonoBehaviour
{
    public Vector2Int gridIndex;

    public bool isEmpty;
    public GameObject myOccupant;

    public int gCost;
    public int hCost;

    public FloorTile previousNode;

    MeshRenderer mRenderer;
    public TileColors currentColorByType = TileColors.blank; //enemy, hero or blank
    [SerializeField]
    private LayerMask pawnLayer;



    private void Start()
    {
        isEmpty = true;
        mRenderer = GetComponentInChildren<MeshRenderer>();
    }
    public int fCost { 
        get { return gCost + hCost; } }


    public void CalculateCosts(FloorTile startNode, FloorTile targetNode)
    {
        gCost = GetDistanceToTarget(targetNode);
        hCost = GetDistanceToTarget(startNode);
    }

    public int GetDistanceToTarget(FloorTile targetNode)
    {
        int deltaX = Mathf.Abs(gridIndex.x - targetNode.gridIndex.x);
        int deltaY = Mathf.Abs(gridIndex.y - targetNode.gridIndex.y);
        
        if(deltaX > deltaY)
        {
            return 14 * deltaY + 10 * (deltaX - deltaY);
        }
        else
        {
            return 14 * deltaX + 10 * (deltaY - deltaX);
        }
    }
}
