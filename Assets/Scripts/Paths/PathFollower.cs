using UnityEngine;
using PathCreation;

//namespace PathCreation.Examples
//{
// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
public class PathFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    [SerializeField]
    float distanceTravelled;
    [SerializeField]
    float totalTimeToTravel; //temp

    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            //speed = pathCreator.path.cumulativeLengthAtEachVertex[pathCreator.path.cumulativeLengthAtEachVertex.Length - 1] / totalTimeToTravel;
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    void Update()
    {
        if (pathCreator != null)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        }
    }
    public void SetDistanceTravelled(System.DateTime deptTime, float totalTravelTime)
    {
        float deltaSeconds = (float)(System.DateTime.Now - deptTime).TotalSeconds;
        distanceTravelled = deltaSeconds * speed;
    }


    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
