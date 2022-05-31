using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour //interface instead? TBF - dont like it. no good classes to attach it to
{
    Path myPath;
    List<Waypoint> waypoints { get => myPath.waypoints; set => myPath.waypoints = value; } //this should get the points from the associated path

    public Vector3 position { get => transform.position; set => transform.position = value; }
}
