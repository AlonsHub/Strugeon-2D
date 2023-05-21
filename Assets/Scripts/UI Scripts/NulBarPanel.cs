using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NulBarPanel : MonoBehaviour
{
    [SerializeField]
    GameObject nulBarPrefab; //Going with this for now //NO! BAD! don't instantiate! the NulBarPanel prefa should come with all bars, and just turn off those you don't need
    [SerializeField]
    Transform nulBarParent; //should some sort of Layout group?

    [SerializeField]
    List<NoolColour> coloursToShow;

    [SerializeField]
    List<NulBar> nulBars;

    [SerializeField, Tooltip("Automatically sets to PsionSpectrumProfile on Awake()")]
    bool setOnAwake;

    [SerializeField, Tooltip("OPTIONAL! - only relevant for BattleBarPanel")]
    bool doHookToRegen;

    private void Awake()
    {
        //if (setOnAwake)
        //    SetMe();

            
        SetMe();
        PlayerDataMaster.Instance.currentPlayerData.noolProfile.OnAnyValueChanged += SetMe;
    }

    //private void Start()
    //{


    //}

    //private void OnEnable()
    //{
    //    if (doHookToRegen)
    //    {
    //        if (TurnMaster.Instance) 
    //        {
    //            //TurnMaster.Instance.OnTurnOver += SetMe;
    //            TurnMaster.Instance.OnTurnOrderRestart += SetMe;
    //            PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.OnAnyValueChanged += SetMe;

    //        }
    //        else
    //            StartCoroutine(nameof(WaitAfterFirstOnEnable));

    //    }
    //}
    //private void OnEnable()
    //{
    //    if (doHookToRegen)
    //    {
    //        if (TurnMachine.Instance) 
    //        {
    //            //TurnMaster.Instance.OnTurnOver += SetMe;
    //            TurnMachine.Instance.OnStartNewRound += SetMe;
    //            //PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.OnAnyValueChanged += SetMe;
    //            PlayerDataMaster.Instance.currentPlayerData.noolProfile.OnAnyValueChanged += SetMe;
    //        }
    //        else
    //            StartCoroutine(nameof(WaitAfterFirstOnEnable));
    //    }
    //}

    //IEnumerator WaitAfterFirstOnEnable()
    //{
    //    yield return new WaitUntil(() => (TurnMachine.Instance || Time.timeSinceLevelLoad > 2f));

    //    if (!TurnMachine.Instance)
    //        Debug.LogError("NO TURN MACHINE!");
    //    else
    //    {
    //        //TurnMaster.Instance.OnTurnOver += SetMe;
    //        TurnMachine.Instance.OnStartNewRound += SetMe;
    //        PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.OnAnyValueChanged += SetMe;
    //    }

    //}
    private void OnDisable()
    {
        //if (doHookToRegen && TurnMaster.Instance)
        //{
        //TurnMaster.Instance.OnTurnOver -= SetMe;
        //PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.OnAnyValueChanged -= SetMe;
        PlayerDataMaster.Instance.currentPlayerData.noolProfile.OnAnyValueChanged -= SetMe;


        if (TurnMachine.Instance)
            TurnMachine.Instance.OnStartNewRound -= SetMe;
        //}
    }


    /// <summary>
    /// No argument -> uses the player's stats
    /// </summary>
    public void SetMe() 
    {
        //if (nulBars == null)
        //{
        //    InitBars();
        //}

        foreach (var nulElement in PlayerDataMaster.Instance.currentPlayerData.noolProfile.nools)
        {
            if (nulElement.colour == NoolColour.White)
                continue;
            if (!coloursToShow.Contains(nulElement.colour))
            {
                nulBars[(int)nulElement.colour].gameObject.SetActive(false);
                continue;
            }

            nulBars[(int)nulElement.colour].SetMe(nulElement);
        }
            //foreach (var nulElement in PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.psionElements)
            //{
            //    if (!coloursToShow.Contains(nulElement.GetNulColour))
            //    {
            //        nulBars[(int)nulElement.GetNulColour].gameObject.SetActive(false);
            //        continue;
            //    }
                
            //    nulBars[(int)nulElement.GetNulColour].SetMe(nulElement); //Also sets MaxValues!
            //}

        //}
        //else
        //{
        //    foreach (var nulElement in PlayerDataMaster.Instance.currentPlayerData.noolProfile.nools)
        //    {
        //        //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
        //        if (!coloursToShow.Contains(nulElement.colour))
        //            continue;
        //        //NulBar temp = nulBars[(int)nulElement.colour];

        //        ////if(temp.maxValue != nulElement.maxValue)
        //        ////temp.AnimatedSetMaxValue(nulElement.maxValue, 1f); //TBF replace with a better duration, and set the value immediatly - only displaying the way to target (don't gradually increase value!)
        //        ////if(temp.currentValue != nulElement.currentValue)
        //        //temp.AnimatedSetValue(nulElement.currentValue, .5f);
        //        nulBars[(int)nulElement.colour].SetMe(nulElement); //Also sets MaxValues!

        //        //nulBar.SetMe(nulElement);
        //        //nulBars.Add(nulBar);
        //    }
        //    //foreach (var nulElement in PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.psionElements)
        //    //{
        //    //    //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
        //    //    if (!coloursToShow.Contains(nulElement.GetNulColour))
        //    //        continue;
        //    //    NulBar temp = nulBars[(int)nulElement.GetNulColour];

        //    //    //if(temp.maxValue != nulElement.maxValue)
        //    //    //temp.AnimatedSetMaxValue(nulElement.maxValue, 1f); //TBF replace with a better duration, and set the value immediatly - only displaying the way to target (don't gradually increase value!)
        //    //    if(temp.currentValue != nulElement.value)
        //    //    temp.AnimatedSetValue(nulElement.value, .5f);
        //    //    //nulBar.SetMe(nulElement);
        //    //    //nulBars.Add(nulBar);
        //    //}

        //}

        //TBF! this will have more complex logic if not all bars are to be displayed

        //turn off any bars that irrelvant

    }
    //public void SetMe(float values) 
    //{
    //    if(nulBars == null)
    //    {
    //        InitBars();
    //    }


    //    for (int i = 0; i < nulBars.Count; i++)
    //    {
    //        nulBars[i].SetColour((NoolColour)i);
    //        //nulBars[i].SetValue(values);
    //        nulBars[i].AnimatedSetValue(values, 1f);
    //    }

    //    ////TBF! this will have more complex logic if not all bars are to be displayed
    //    //foreach (var nulElement in PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.psionElements) 
    //    //{
    //    //    //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
    //    //    nulBars[(int)nulElement.nulColour].SetMe(nulElement);
    //    //    //nulBar.SetMe(nulElement);
    //    //    //nulBars.Add(nulBar);
    //    //}

    //    //turn off any bars that irrelvant

    //}

    /// <summary>
    /// Uses item stats, Max values should still matter? do we still use the player's stats for that?
    /// </summary>
    //public void SetMe(ItemSpectrumProfile itemSpectrumProfile)
    //{
    //    if (nulBars == null)
    //    {
    //        InitBars();
    //    }

    //    foreach (var nulElement in itemSpectrumProfile.elements)
    //    {
    //        //Debug.LogError($"{nulElement.nulColour} of {nulElement.value}");
    //        nulBars[(int)nulElement.nulColour].SetMe(nulElement);
    //        //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();

    //        //nulBar.SetMe(nulElement);
    //        //nulBars.Add(nulBar);
    //    }
    //    //turn off any bars that irrelvant
    //}
    
    public void TurnOffAllBarTexts()
    {
        foreach (var item in nulBars)
        {
            item.TextOnOff(false);
        }
    }
   

    public void SetBarText(int index, string text)
    {
        nulBars[index].SetText(text);
    }
}
