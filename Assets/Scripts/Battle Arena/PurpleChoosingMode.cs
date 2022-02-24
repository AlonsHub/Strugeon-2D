using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PurpleChoosingMode : MonoBehaviour
{
    public static PurpleChoosingMode Instance;

    public Pawn actor;

    [SerializeField]
    GameObject chosenByPurpleVFX; //prefab

    public List<GridPoser> purpleTargets;
    private bool doUpdate;
    public bool IsOn { get => doUpdate; }
    public LayerMask layerMask;

    public Camera cam;

    [ColorUsage(true)]
    public Color colorOn;
    [ColorUsage(true)]
    public Color colorOff;

    //public bool isOn = false;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    public void ToggleTint(bool isOn)
    {
        //this.isOn = isOn;
        Renderer ren;
        Color newColor;
        if (isOn)
        {
            newColor = colorOn;
        }
        else
        {
            newColor = colorOff;
            ren = actor.GetComponentInChildren<Renderer>();
            ren.material.SetFloat("_Thickness", 0f); // the selection for psion powers in general
        }

        foreach (Pawn p in RefMaster.Instance.enemyInstances)
        {
            ren = p.GetComponentInChildren<Renderer>();
            ren.material.SetColor("_Tint", newColor);
        }

        if (actor.ActionItems.Any(x => x.targetAllies))
        {
            foreach (Pawn p in RefMaster.Instance.mercs)
            {
                ren = p.GetComponentInChildren<Renderer>();
                ren.material.SetColor("_Tint", newColor);
            }
        }
        //ren = actor.GetComponentInChildren<Renderer>();
        //ren.material.SetColor("_Tint", colorOff); //the actor always has their color off

        //ADD CENSER
        //ADD ESCAPE OPTION

    }

    public void Setup(Pawn newActor)
    {
        actor = newActor;

        //bring all pawns
        purpleTargets = new List<GridPoser>();

        //Very similar to "ToggleTint()" but this is on Setup and we're looping through them anyways

        Renderer ren;
        foreach (Pawn p in RefMaster.Instance.enemyInstances)
        {
            ren = p.GetComponentInChildren<Renderer>();
            ren.material.SetColor("_Tint", colorOn);
            //ren.material.SetFloat("_EmissionAmount", 2f);
            purpleTargets.Add(p);

        }
        if (actor.ActionItems.Any(x => x.targetAllies))
        {
            foreach (Pawn p in RefMaster.Instance.mercs)
            {
                ren = p.GetComponentInChildren<Renderer>();
                ren.material.SetColor("_Tint", colorOn);
                purpleTargets.Add(p);
            }
        }
        //ren = actor.GetComponentInChildren<Renderer>();
        //ren.material.SetColor("_Tint", colorOff);

        //purpleTargets.AddRange(RefMaster.Instance.mercs);
        //purpleTargets.AddRange(RefMaster.Instance.enemies);
        //p.GetComponent<Renderer>().material.SetFloat("_EmissionAmount", 2f);

        purpleTargets.Add(RefMaster.Instance.censer);
        ren = RefMaster.Instance.censer.GetComponentInChildren<Renderer>();
        ren.material.SetColor("_Tint", colorOn);
        //purpleTargets.Add(RefMaster.Instance.censer); // ADD EXIT/escape OPTION
        doUpdate = true;


    }

    private void LateUpdate()
    {
        if(doUpdate && Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
            {
                PurpleTarget justAnInterface = hit.collider.GetComponentInParent<PurpleTarget>(); //could be Pawn or Censer at the moment 07/02/22 
                
                actor.SetupPurpleBuff(justAnInterface.asPurpleTgtGameObject); //pass on as the gameobject on which there is an empty interface of PurpleTarget, tagging only by Pawn and Censer 

                //Play Chosen-By-Purple Effect
                GameObject vfx = Instantiate(chosenByPurpleVFX, justAnInterface.asPurpleTgtGameObject.transform);

                doUpdate = false;
                //actor.TurnDone = true;
                BattleLogVerticalGroup.Instance.AddPsionEntry(actor.Name, PsionActionSymbol.Purple, Color.magenta);

                Renderer rend = MouseBehaviour.hitTarget.GetComponentInChildren<Renderer>();
                
                ToggleTint(false);
                MouseBehaviour.Instance.CloseMenus();
            }

        }
    }
}
