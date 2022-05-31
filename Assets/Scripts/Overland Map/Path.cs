using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Waypoint> waypoints;
    List<Vector3> verts;

    public float CalculateLinearDistance()
    {
        float distance = 0f;

        // Transalte from transform-data of Waypoints - to vector3 points on the canvas
        //here is where different types of interpolation are applicable
        SetVertsLinearPath(1);

        if (verts != null && verts.Count >= 2) //origin and destination are Always waypoints, so minimum count 2
        {
            for (int i = 0; i < verts.Count - 1; i++)
            {
                distance += Vector3.Distance(verts[i], verts[i + 1]);
            }
        }
        return distance;
    }

    private void SetVertsLinearPath(int resolution)
    {
        if(verts == null)
        {
            verts= new List<Vector3>();
        }
        for (int i = 0; i < waypoints.Count-1; i++)
        {
            //int vertsByDistance = Vector3.Distance
            for (int k = 0; k < resolution; k++)
            {
                //verts.Add(Vector3.Lerp(waypoints[i], waypoints[i+1], ));
            }
        }
        
        
    }
}
