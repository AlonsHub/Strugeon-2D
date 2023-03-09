using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class PathMaker : MonoBehaviour
{
    public static PathMaker Instance;

    FloorGrid floorGrid;
    [SerializeField]
    List<FloorTile> openTiles = new List<FloorTile>();
    [SerializeField]
    HashSet<FloorTile> closedTiles = new HashSet<FloorTile>();

    public TileWalker seeker, target;
    void Awake()
    {
        if(Instance != null && Instance !=this)
        {
            Debug.LogError("more than one PathMaker in scene");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        floorGrid = FloorGrid.Instance;

        openTiles = new List<FloorTile>();
        closedTiles = new HashSet<FloorTile>();

    }
    //public void SetTargetAndSeekr(TileWalker tgt, TileWalker skr)
    //{
    //    target = tgt;
    //    seeker = skr;
    //}

    //[SerializeField]
    //bool isLooking = false;
    public List<FloorTile> FindPath(TileWalker skr, FloorTile tgtTile)
    {
        //seeker.FindOwnGridPos();
        FloorTile startNode = skr.currentNode;
        //target.FindOwnGridPos();
        FloorTile targetNode = tgtTile;
        openTiles = new List<FloorTile>();
        closedTiles = new HashSet<FloorTile>();

        /// DONT REDUCE PATH STEPS Displace one step in a logical/natural direction (e.g. y-1)

        openTiles.Add(startNode);
        while (openTiles.Count > 0)
        {
            FloorTile currentNode = openTiles[0];

            for (int i = 1; i < openTiles.Count; i++)
            {
                // openTiles[i].CalculateCosts(startNode, targetNode);
                if (openTiles[i].fCost < currentNode.fCost || (openTiles[i].fCost == currentNode.fCost && openTiles[i].hCost < currentNode.hCost))
                {
                    currentNode = openTiles[i]; //This makes sure we're always focusing on the lowest fCost node, or gCost of similar.
                }
            }

            openTiles.Remove(currentNode);
            closedTiles.Add(currentNode);

            if (currentNode == targetNode)
            {

                //isLooking = false;
                return RetracePath(startNode, targetNode); //DONE!

            }

            foreach (FloorTile neighbour in FloorGrid.Instance.GetNeighbours(currentNode))
            {
                if (closedTiles.Contains(neighbour)) continue;
                //if ((neighbour.hasEnemy || neighbour.hasObstacle || neighbour.hasHero || neighbour.hasItem) && neighbour != targetNode)
                if (!neighbour.isEmpty && neighbour != targetNode) continue;
                
                int newDistanceInPath = currentNode.gCost + currentNode.GetDistanceToTarget(neighbour);
                if (newDistanceInPath < neighbour.gCost || !openTiles.Contains(neighbour))
                {
                    neighbour.gCost = newDistanceInPath;
                    neighbour.hCost = neighbour.GetDistanceToTarget(targetNode);
                    neighbour.previousNode = currentNode;

                    if (!openTiles.Contains(neighbour)) //I think we need to reset cost even if it does exist in the list - but not add it
                    {
                        openTiles.Add(neighbour);
                    }
                }
            }
        }

        //Debug.LogError(seeker.name + "has failed to find path to " + target.name + ", return empty List<FloorTile>");
        return new List<FloorTile>();
    }
    public List<FloorTile> FindPath(TileWalker skr, TileWalker tgt)
    {
        FloorTile startNode = skr.currentNode;
        FloorTile targetNode = tgt.currentNode;
        openTiles.Clear();
        closedTiles.Clear();

        /// DONT REDUCE PATH STEPS Displace one step in a logical/natural direction (e.g. y-1)

        openTiles.Add(startNode);
        while (openTiles.Count >0)
        {
            FloorTile currentNode = openTiles[0];

            for (int i = 1; i < openTiles.Count; i++)
            {
                if(openTiles[i].fCost < currentNode.fCost || (openTiles[i].fCost == currentNode.fCost && openTiles[i].hCost < currentNode.hCost))
                {
                    currentNode = openTiles[i]; //This makes sure we're always focusing on the lowest fCost node, or gCost of similar.
                }
            }

            openTiles.Remove(currentNode);
            closedTiles.Add(currentNode);

            if(currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode); //DONE!
            }
            
            foreach(FloorTile neighbour in FloorGrid.Instance.GetNeighbours(currentNode))
            {
                if (closedTiles.Contains(neighbour)) continue;
                if (!neighbour.isEmpty && neighbour != targetNode) continue;
                
                int newDistanceInPath = currentNode.gCost + currentNode.GetDistanceToTarget(neighbour);
                if(newDistanceInPath < neighbour.gCost || !openTiles.Contains(neighbour))
                {
                    neighbour.gCost = newDistanceInPath;
                    neighbour.hCost = neighbour.GetDistanceToTarget(targetNode);
                    neighbour.previousNode = currentNode;

                    if(!openTiles.Contains(neighbour)) //I think we need to reset cost even if it does exist in the list - but not add it
                    {
                        openTiles.Add(neighbour);
                    }
                }
            }
        }
        
        //Debug.LogWarning(seeker.name + "has failed to find path to " + target.name + ", return empty List<FloorTile>");
        skr.pawn.FinishAnimation();
        return new List<FloorTile>();
    }


    public List<FloorTile> RetracePath(FloorTile startNode, FloorTile endNode)
    {
        List<FloorTile> path = new List<FloorTile>();
        FloorTile tracingNode = endNode;
        while(tracingNode != startNode)
        {
            path.Add(tracingNode);
            tracingNode = tracingNode.previousNode;
        }
        path.Reverse();
        //floorGrid.path = path;
        return path;

    }
}
