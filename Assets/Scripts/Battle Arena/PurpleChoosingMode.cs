using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PurpleChoosingMode : MonoBehaviour
{
    public static PurpleChoosingMode Instance;

    public Pawn actor;

    public List<GridPoser> purpleTargets;
    private bool doUpdate;

    public LayerMask layerMask;

    public Camera cam;

    [ColorUsage(true)]
    public Color colorOn;
    [ColorUsage(true)]
    public Color colorOff;

    public bool isOn = false;
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
        this.isOn = isOn;
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

        foreach (Pawn p in RefMaster.Instance.enemies)
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
        foreach (Pawn p in RefMaster.Instance.enemies)
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

    private void Update()
    {
        if(doUpdate && Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
            {
                //ActionVariation actionVariation = actor.actionPool.Where(x => x.target == hit.collider.gameObject).Single();
                //actor.actionPool.Where(x => x.target == hit.collider.gameObject).Single().weight *= 10;
                actor.SetupPurpleBuff(hit.collider.gameObject);
                doUpdate = false;
                //actor.TurnDone = true;
                BattleLogVerticalGroup.Instance.AddPsionEntry(actor.Name, PsionActionSymbol.Purple, Color.magenta);

                Renderer rend = MouseBehaviour.hitTarget.GetComponentInChildren<Renderer>();
                //Color emissionOff = rend.material.GetColor("_EmissionColor");
                //emissionOff *= 0.025f;
                //rend.material.SetColor("_EmissionColor", emissionOff);



                //MouseBehaviour.Instance.CloseMenus();
                //rend.material.SetFloat("_EmissionAmount", 0f);
                //rend.material.SetColor("_Tint", colorOff);

                //Renderer ren;
                //foreach (Pawn p in RefMaster.Instance.mercs)
                //{
                //    ren = p.GetComponentInChildren<Renderer>();
                //    //ren.material.SetFloat("_EmissionAmount", 0f);
                //    ren.material.SetColor("_Tint", colorOff);
                //    purpleTargets.Add(p);

                //}
                //foreach (Pawn p in RefMaster.Instance.enemies)
                //{
                //    ren = p.GetComponentInChildren<Renderer>();
                //    //ren.material.SetFloat("_EmissionAmount", 0f);
                //    ren.material.SetColor("_Tint", colorOff);
                //    purpleTargets.Add(p);

                //}
                ToggleTint(false);
                MouseBehaviour.Instance.CloseMenus();
            }

        }
    }
}
