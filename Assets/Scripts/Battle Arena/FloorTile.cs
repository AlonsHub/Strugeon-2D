using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TileColors {enemy, hero, blank };
public enum TileElement {None, Fire, Water, Poison};
[System.Serializable]
public class FloorTile : MonoBehaviour
{
    public Vector2Int gridIndex;

    public bool isEmpty;
    public GameObject myOccupant; //all direct access to myOccupant is absolute bullshit that needs fixing

    public TileElement tileElement = TileElement.None; // may need to be more robust - current access is only through ChangeElement (should be tryChangeElement)



    public int gCost;
    public int hCost;

    public FloorTile previousNode;

    MeshRenderer mRenderer;
    public TileColors currentColorByType = TileColors.blank; //enemy, hero or blank
    [SerializeField]
    private LayerMask pawnLayer;

    public System.Action OnOccupantEnter;

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

    public void AcceptOccupant(GameObject newOccupant)
    {
        isEmpty = false;
        myOccupant = newOccupant;
        myOccupant.transform.position = transform.position; // NO OFFSET?
        OnOccupantEnter?.Invoke();
    }
    public void RemoveOccupant(bool doAlsoDestroy)
    {
        isEmpty = true;

        if (doAlsoDestroy)
            Destroy(myOccupant);
        myOccupant = null;
    }
    ///// <summary>
    ///// Returns true if the element changed successfully. 
    ///// </summary>
    ///// <param name="newElement"></param>
    ///// <param name="doOverwrite">True if element should overwrite the existing element on that tile</param>
    ///// <returns></returns>
    //public bool TryChangeElement(TileElement newElement, bool doOverwrite) 
    //{
    //    if (doOverwrite || tileElement == TileElement.None)
    //    {
    //        tileElement = newElement;
    //        return true;
    //    }
    //    return false;
    //}

    //public void AddElementalEffect()
    //{

    //}
}
