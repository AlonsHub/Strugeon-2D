using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWalker : MonoBehaviour
{
    // start - end
    public Transform origin; //might not need, if they all spawn at tavern
    public Transform destination;

    // speed and time
    public float travelDuration;
    float walkSpeed; //derrived from travelTime
    public float currentDistanceToTarget; //calculated locally, might not NEED to be public
    float currentTime;
    float stoppingDistance = .1f; //temp

    [Tooltip("The time to wait between steps. From this, the path's Vertex Resolution derrives from this")]
    [SerializeField]
    float stepDelay;

    //Ref to THIS expeditions part?

    //[must haves: origin, destination,]
    public void Init(Transform og, Transform dest, float _stepDelay, float duration)
    {
        origin = og;
        destination = dest;

        transform.position = origin.position;

        stepDelay = _stepDelay;
        travelDuration = duration;

        CallWalk(); //temp
    }

    public void CallWalk()
    {
        StartCoroutine("WalkCorou");
    }

    IEnumerator WalkCorou()
    {
        currentDistanceToTarget = Vector3.Distance(origin.position, destination.position);
        currentTime = 0;

        while (currentDistanceToTarget > stoppingDistance)
        {
            transform.position = Vector3.Lerp(origin.position, destination.position, currentTime / travelDuration);

            yield return new WaitForSeconds(stepDelay);
            currentTime += stepDelay;
            currentDistanceToTarget = Vector3.Distance(origin.position, destination.position);
        }
        Debug.LogWarning("Arrived!");

        // ArrivedMethod();
    }

    //public float DistanceToTarget()
    //{
    //    if (origin == null || destination == null)
    //        return float.NegativeInfinity;

    //    return Vector3.Distance(origin.position, destination.position);
    //}
    // send me to destination

    // check if reached
}
