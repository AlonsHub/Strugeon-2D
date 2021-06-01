using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class TileWalker : MonoBehaviour
{
    public FloorGrid floorGrid;
    public Vector2Int gridPos;
    public int moveSpeed; //number of tiles per move action
    [SerializeField]
    private Vector3 offsetFromGrid;

    public List<FloorTile> path;

    public FloorTile currentNode;

    public bool isHero; 

    public bool doLimitSteps = false; //BlueHostile skill for example
    public int stepLimit; //if doLimitSteps

    public Pawn pawn;
 
    public void Init()
    {
        pawn = GetComponent<Pawn>();

        if (floorGrid == null)
        {
            floorGrid = FloorGrid.Instance;
        }
        FindOwnGridPos();
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
            pawn.TurnDone = true;
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

        //Pathfinder.Instance.SetTargetAndSeekr(tgt, this);
        //Debug.Log(name + " Targeting ->" + tgt.name);
        path = PathMaker.Instance.FindPath(this, tgt); // find path should be the method to take in Target and Seeker - they should never have been local properties

        if (path.Count <= 0)
        {
            Debug.LogError(name + " No path found, passed a null path to tilewalker.");
            GetComponentInChildren<LookAtter>().tgt = null; //stops rotating
            pawn.TurnDone = true;
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

    //public void StartEspacePath(TileWalker tgt)
    //{
    //    EscapeFinder.Instance.SetTargetAndSeekr(tgt, this);
    //    path = EscapeFinder.Instance.FindPath();
    //    if (path.Count <= 0)
    //    {
    //        Debug.LogError("No path found, passed a null path to tilewalker.");
    //    }
    //    else
    //    {
    //        hasPath = true;
    //        // passedHalfPath = false;
    //        currentPathIndex = 0;
    //        //Walk(); //start walk sequence

    //        //SUPER TEMP! this needs to be StartWalking(int range)
           
          
    //            StartCoroutine("StartWalkingTo");
            
    //        //SUPER TEMP!

    //    }
    //}
    void WalkWithRange(int range) //rangeInTiles (1 = 10 || 14)
    {
        FindOwnGridPos();

        currentNode.isEmpty = true;

        FloorTile destNode = currentNode;

        int stepsToTake = path.Count - range;
        //if (doLimitSteps && stepsToTake > stepLimit) //currently, if the walker didn't try to move more than the stepLimit - the limit is still active
        //{
        //    doLimitSteps = false; //this kills the effect with a Coroutine called "EndWhen"
        //    //GetComponent<Character>().EndSpecialEffect();//only applies to the icon ATM || ALSO! TileWalker might need to know their OwnerCharacters
        //   // character.RemoveIconByTag("Blue");
        //    stepsToTake = stepLimit;
        //}


        //List<FloorTile> walkList = path.
        int difference = path.Count - stepsToTake;
        for (int i = 0; i < difference; i++)
        {
            path.RemoveAt(path.Count - 1);
        }
        StartCoroutine(TileByTileWalk(path));
        //NavMeshAgent agent = GetComponent<NavMeshAgent>();

        //agent.SetDestination(path[stepsToTake - 1].transform.position);

        //yield return new WaitUntil(() => (agent.remainingDistance < .001f || !agent.hasPath));

        //FindOwnGridPos();
        //destNode = path[stepsToTake-1];
        //currentNode = destNode;
        //destNode.isEmpty = false;
        
        //hasPath = false; //Finished!
        //pawn.TurnDone = true; //hmm
        ////END TURN
    }

    IEnumerator TileByTileWalk(List<FloorTile> walkPath) //this has the last "step" removed from it already (meaning we stop just before the target)
    {
        for (int i = 0; i < walkPath.Count; i++)
        {
            transform.position = walkPath[i].transform.position + offsetFromGrid;
            //transform.rotation = Quaternion.Euler(0, 0, );
            yield return new WaitForSeconds(stepTime);
        }
        FindOwnGridPos();
        currentNode.isEmpty = false;

        GetComponentInChildren<LookAtter>().tgt = null; //stops rotating

        hasPath = false; //Finished!
        pawn.TurnDone = true; //hmm
        //END TURN
    }

    public void Step(FloorTile stepDest) //takes one(?) step in a direction on the Array of the map.
    {
        currentNode.isEmpty = true;
       
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.Warp(stepDest.transform.position + offsetFromGrid);
       
        currentNode = stepDest;

        currentNode.isEmpty = false;
    }

    public void FindOwnGridPos() //just in case they get lost somehow
    {
        //approximate location accuratly by dividing the x and z pos, to bet the index - then access that tile and steal its position data
        float x = (transform.position.x - floorGrid.startingPoint.position.x)/(floorGrid.gapSize.x + floorGrid.tileSize.x);
        float y = (transform.position.z - floorGrid.startingPoint.position.z)/(floorGrid.gapSize.y + floorGrid.tileSize.y);
        
        gridPos.x = Mathf.RoundToInt(x);
        gridPos.y = Mathf.RoundToInt(y);
        currentNode = floorGrid.floorTiles[gridPos.x, gridPos.y];
        currentNode.isEmpty = false;
        transform.position = currentNode.transform.position + offsetFromGrid;
    }
        
}
