﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseBehaviour : MonoBehaviour
{
    public static MouseBehaviour Instance;
    public Camera cam;
    public GameObject heroButtonCircle;
    public GameObject enemyButtonCircle; 
    //rects:
    public RectTransform heroButtonRect;
    public RectTransform enemyButtonRect;
    public bool areButtonsOpen;

    public LayerMask layerMask;

    public static Pawn hitTarget;
    
    public Vector3 offset;
    
    //TEMP
    //this and all values stored in "Bar"s and "UI"s and so on, should all move to a dedicated Player class.
    // - moved.
    //TEMP
    List<SpellButton> skillButtons;

    [SerializeField]
    float outlineThickness;

    bool gameRunning = true;

    void Start()
    {
        Instance = this;


        //cam = Camera.main;
        areButtonsOpen = false;
        skillButtons = heroButtonCircle.GetComponentsInChildren<SpellButton>().ToList();
        skillButtons.AddRange(enemyButtonCircle.GetComponentsInChildren<SpellButton>());

        heroButtonCircle.SetActive(false);
        enemyButtonCircle.SetActive(false);
        //heroButtonRect = heroButtonCircle.transform.GetChild().GetComponent<RectTransform>(); //Get Rect
        //enemyButtonRect = enemyButtonCircle.GetComponent<RectTransform>(); //Get Rect
       // timeScaleSlider.onValueChanged += SliderValueChanged;
    }


    //SHOULD NOT BE HERE!
    public void SliderValueChanged()
    {
        preferedTimeScale = timeScaleSlider.value;
        Time.timeScale = preferedTimeScale;
        timeScaleDisplayer.text = (preferedTimeScale - (preferedTimeScale % 0.01f)).ToString();
    }
    public void SlowSliderValueChanged()
    {
        menuSlowTime = slowTimeScaleSlider.value;
        //Time.timeScale = preferedTimeScale;
        slowTimeScaleDisplayer.text = (menuSlowTime - (menuSlowTime % 0.01f)).ToString();
    }

    Ray ray;
    [SerializeField]
    private float menuSlowTime;
    public float preferedTimeScale;

    public Slider timeScaleSlider;
    public TMP_Text timeScaleDisplayer;
    public float buttonCircleRadius;
    public float buttonInnerCircleRadius;

    bool overCircle = false;
    public Slider slowTimeScaleSlider;
    public TMP_Text slowTimeScaleDisplayer;

    void Update()
    {
        if (!gameRunning)
            return;

        if (areButtonsOpen)
        {
            if(hitTarget == null) // disables the menus in cases of Death and Escape(Flee)
            {
                CloseMenus();
                return;
            }

            //I dont think im using this
            float distanceFromButtonCircle;
            //Vector3 circlePos = cam.WorldToScreenPoint(hitTarget.transform.position);
            distanceFromButtonCircle = heroButtonRect.gameObject.activeInHierarchy ? Vector3.Distance(Input.mousePosition, heroButtonRect.position) : Vector3.Distance(Input.mousePosition, enemyButtonRect.position);
           
            //overCircle = distanceFromButtonCircle <= buttonCircleRadius && distanceFromButtonCircle >= buttonInnerCircleRadius;
            //I dont think im using this[end]

            
            //if(!PurpleChoosingMode.Instance.IsOn &&( Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)))
            if(!PurpleChoosingMode.Instance.IsOn && Input.GetMouseButtonDown(0) && distanceFromButtonCircle >= buttonCircleRadius)
            {
                Renderer rend = hitTarget.GetComponentInChildren<Renderer>();
                //Color emissionOff = rend.material.GetColor("_EmissionColor");
                //emissionOff *= 0.025f;
                rend.material.SetFloat("_Thickness", 0f);

                CloseMenus();
                //PurpleChoosingMode.Instance.ToggleTint(false);
            }
        }
        
        else if (!PurpleChoosingMode.Instance.IsOn && Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 50f, layerMask)) //not sure about max length yet, but we should add a LayerMask
            {
                //Time.timeScale = menuSlowTime;
                TimeChanger.Instance.ToggleTimePause(true);
                hitTarget = hit.collider.gameObject.GetComponentInParent<Pawn>();

                //Debug.Log(hitTarget.name);
                if(hitTarget.CompareTag("Merc"))
                {
                    heroButtonCircle.SetActive(true); //heroButtonRect.GameObject.SetActive(true); //maybe better
                    
                    Vector2 newPos = cam.WorldToScreenPoint(hitTarget.transform.position) + offset;
                    heroButtonRect.position = newPos;
                    areButtonsOpen = true;
                }
                else if (hitTarget.CompareTag("Enemy"))
                {
                    enemyButtonCircle.SetActive(true); //enemyButtonCircle.GameObject.SetActive(true); //maybe better
                    enemyButtonRect.position = cam.WorldToScreenPoint(hitTarget.transform.position) + new Vector3(-offset.x, offset.y);
                    areButtonsOpen = true;
                }
                

                if(areButtonsOpen)
                {
                    //FIRST InteractableCheck() for all buttons
                    foreach (SpellButton sb in skillButtons)
                    {
                        sb.InteractableCheck();
                    }
                    Renderer hitRenderer = hitTarget.GetComponentInChildren<Renderer>();
                   // Color emissionOn = hitRenderer.material.GetColor("_EmissionColor");
                    //emissionOn*= 40f; //does this not accumlate tons of emission over time?

                    hitRenderer.material.SetFloat("_Thickness", outlineThickness);
                }
            }
        }
    }
    public void ShutDown()
    {
        CloseMenus();
        gameRunning = false;
    }
    public void CloseMenus()
    {
        //if (PurpleChoosingMode.Instance.IsOn)
        //{
        //    PurpleChoosingMode.Instance.ToggleTint(false);
        //    Renderer r =
        //    PurpleChoosingMode.Instance.actor.GetComponentInChildren<Renderer>();
        //    r.material.SetFloat("_Thickness", 0f);
        //}

        //shut down the text box as well
        HoverTextBoard.Instance.UnSetMe();

        heroButtonCircle.SetActive(false);
        enemyButtonCircle.SetActive(false);
        areButtonsOpen = false;
        //Time.timeScale = preferedTimeScale;
        TimeChanger.Instance.ToggleTimePause(false);
    }
    public void HideMenus()
    {
        heroButtonCircle.SetActive(false);
        enemyButtonCircle.SetActive(false);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 50f);
    //}
}
