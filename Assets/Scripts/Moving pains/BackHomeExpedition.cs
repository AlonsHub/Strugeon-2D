using PathCreation;
using UnityEngine;

//MODIFIED FROM ORIGINAL CODE

// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
public class BackHomeExpedition : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    float distanceTravelled;
    public Expedition expedition;
    //public SiteButton destinationSiteButton;

    //public GameObject prefabStartMissionButton; //fuck me...
    //[SerializeField]
    //private Vector3 missionButtonOffset;

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
        if (pathCreator != null)
        {
            //distanceTravelled += speed * Time.deltaTime;
            distanceTravelled += expedition.speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            // transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            //mod 1: dont rotate. bad for UI objects. thanks
            //if (Vector3.Distance(transform.position, pathCreator.path.GetPoint(1)) <= 1f)
            //{
            //    Debug.Log("End journey");
            //}

            if (Vector3.Distance(transform.position, pathCreator.path.GetPointAtTime(1f, endOfPathInstruction)) <= .1f)
            {
                Debug.Log("End journey");
                //spawn the START MISSION BUTTON

                //Canvas must be parent
                //GameObject go = Instantiate(prefabStartMissionButton, GetComponentInParent<Canvas>().transform); //consider changing this...
                //go.transform.position = transform.position + missionButtonOffset; //the end of the path + some offset
                //last KWA...
                // go.GetComponent<ExpeditionButton>().levelSO = levelSO; //THIS IS WHERE IT ALL BEGINS!
                // destinationSiteButton.OpenArrivedPartyPanel(expedition.expeditionPawns);
                //go.GetComponent<ExpeditionButton>().partyPawns = PartyMaster.Instance.currentMercParty; //THIS IS WHERE IT ALL BEGINS!
                Destroy(gameObject);
            }
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
