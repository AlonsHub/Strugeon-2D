using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TileWalker : MonoBehaviour
{
    public FloorGrid floorGrid;
    public Vector2Int gridPos;
    public int elevation;
    public int moveSpeed; //number of tiles per move action
    [SerializeField]
    private Vector3 offsetFromGrid;

    public List<FloorTile> path;

    public FloorTile currentNode;

    public bool isHero; 

    public bool doLimitSteps = false; //BlueHostile skill for example
    public int stepLimit; //if doLimitSteps

    public Pawn pawn;

    LookAtter lookAtter;

    public void Init()
    {
        pawn = GetComponent<Pawn>();
        elevation = 0;

        if (floorGrid == null)
        {
            floorGrid = FloorGrid.Instance;
        }
        FindOwnGridPos();

        lookAtter = GetComponentInChildren<LookAtter>();
        doLimitSteps = false;
    }
    public void SetPos(Vector2Int pos)
    {
        gridPos = pos;
        currentNode = FloorGrid.Instance.floorTiles[gridPos.x, gridPos.y];
        currentNode.AcceptOccupant(gameObject);
        
        transform.position = currentNode.transform.position + offsetFromGrid;
    }


    public bool hasPath = false;
    //int currentPathIndex; //the index of the current node in the path list.
    public float stepTime;
    public void StartNewPathWithRange(Vector2Int gridPos)
    {
        
        //Pathfinder.Instance.SetTargetAndSeekr(tgt, this);
        //Debug.Log(name + " Targeting ->" + tgt.name);


        path = PathMaker.Instance.FindPath(this, FloorGrid.Instance.GetTileByIndex(gridPos)); 

        if (path.Count <= 0)
        {
            Debug.LogError(name + " No path found, passed a null path to tilewalker.");
            pawn.FinishAnimation();
        }
        else
        {
            hasPath = true;

            //currentPathIndex = 0;

            WalkWithRange(1);

            //here, after the walking coroutines have finished - 
            //TileWalkers's job is done and he should either tell the ownerCharacter directly or simply no longer hold a path and use WaitUntil()
        }
    }
    public void StartNewPathWithRange(TileWalker tgt, int range)
    {
        lookAtter.tgt = tgt.transform;
        //Pathfinder.Instance.SetTargetAndSeekr(tgt, this);
        //Debug.Log(name + " Targeting ->" + tgt.name);
        path = PathMaker.Instance.FindPath(this, tgt); // find path should be the method to take in Target and Seeker - they should never have been local properties

        if (path.Count <= 0 || pawn.HasRoot) //TBD casuses caharacters to sometime skip turns
        {
            //Debug.LogError(name + " No path found, passed a null path to tilewalker.");
            lookAtter.tgt = null; //stops rotating
            pawn.FinishAnimation();
        }
        else
        {
            hasPath = true;
           
            //currentPathIndex = 0;

            WalkWithRange(range);

            //here, after the walking coroutines have finished - 
            //TileWalkers's job is done and he should either tell the ownerCharacter directly or simply no longer hold a path and use WaitUntil()
        }
    }

    
    void WalkWithRange(int range) //rangeInTiles (1 = 10 || 14)
    {
        FindOwnGridPos();

        currentNode.RemoveOccupant(false);
        
        int stepsToTake = path.Count - range;
       
        int difference = path.Count - stepsToTake;

        for (int i = 0; i < difference; i++)
        {
            path.RemoveAt(path.Count - 1); // why not trim? 
        }
        StartCoroutine(TileByTileWalk(path));
    }

    IEnumerator TileByTileWalk(List<FloorTile> walkPath) //this has the last "step" removed from it already (meaning we stop just before the target)
    {
        int stepsToTake = walkPath.Count;
       

        for (int i = 0; i < stepsToTake; i++)
        {
            transform.position = walkPath[i].transform.position + offsetFromGrid;
            
            //Apply tile effects like fire - TBA TBF

            yield return new WaitForSeconds(stepTime);
        }

        FindOwnGridPos();
        currentNode.AcceptOccupant(gameObject);

        lookAtter.tgt = null; //stops rotating

        hasPath = false; //Finished!
        
    }

    public void Step(FloorTile stepDest) //takes one(?) step in a direction on the Array of the map.
    {
        currentNode.RemoveOccupant(false);
        
        transform.position = stepDest.transform.position;

        FindOwnGridPos(); //This REALLLLY needs to change 
        stepDest.AcceptOccupant(gameObject);
        //currentNode = stepDest;
        //currentNode.isEmpty = false;
    }

    public bool StepInDirection(Vector2Int dir)
    {
        FloorTile nextTile = floorGrid.floorTiles[currentNode.gridIndex.x + dir.x, currentNode.gridIndex.y + dir.y];
        //check if available...
        if(!nextTile)
        {
            return false;
        }
        currentNode.RemoveOccupant(false);

        nextTile.AcceptOccupant(gameObject);
            return true;
    }


    //Should really not use this as much... it doens't hurt, but there is no need to refind position so often TBF
    public void FindOwnGridPos() //just in case they get lost somehow
    {
        //approximate location accuratly by dividing the x and z pos, to bet the index - then access that tile and steal its position data

        int x = Mathf.RoundToInt((transform.position.x - floorGrid.startingPoint.position.x)/(floorGrid.gapSize.x + floorGrid.tileSize.x));
        int y = Mathf.RoundToInt((transform.position.z - floorGrid.startingPoint.position.z)/(floorGrid.gapSize.y + floorGrid.tileSize.y));

        if (currentNode == null || gridPos.x != x || gridPos.y != y)
        {
            //gridPos.x = Mathf.RoundToInt(x);
            //gridPos.y = Mathf.RoundToInt(y);
            //currentNode = floorGrid.floorTiles[gridPos.x, gridPos.y];
            //currentNode.AcceptOccupant(gameObject);
            ////currentNode.isEmpty = false;
            ////currentNode.myOccupant = gameObject;
            //transform.position = currentNode.transform.position + offsetFromGrid;
            SetPos(new Vector2Int(x, y));
        }
    }

    public int GetDistanceFromMeToYou(TileWalker you) //this may be kind of useless 
    {
        //return currentNode.GetDistanceToTarget(you.currentNode) + elevation + you.elevation;
        return currentNode.GetDistanceToTarget(you.currentNode) + Mathf.Abs(elevation - you.elevation);
    }

    public void SetElevation(int newElevation)
    {
        elevation = newElevation;
    }
}
