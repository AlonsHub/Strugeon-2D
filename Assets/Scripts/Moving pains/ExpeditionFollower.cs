using PathCreation;
using System.Linq;
using UnityEngine;

//MODIFIED FROM ORIGINAL CODE

// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
public class ExpeditionFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    float distanceTravelled;
    public Expedition expedition;
    public SiteButton destinationSiteButton;


    //public LevelSO levelSO; //KWA KWA KWA KWA KWA KWA
    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }
       
    }

    void Update()
    {
        if (pathCreator != null )
        {
            
            distanceTravelled += expedition.speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            

        }
        if (Vector3.Distance(transform.position, pathCreator.path.GetPointAtTime(1f, endOfPathInstruction)) <= .1f)
        {
            Debug.Log("End journey");
            
            //in comment since arrival panel changed and this whole script is depricated anyways
            //destinationSiteButton.OpenArrivedPartyPanel(expedition.expeditionPawns);
            
            Destroy(gameObject);
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
