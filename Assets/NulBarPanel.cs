using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NulBarPanel : MonoBehaviour
{
    [SerializeField]
    GameObject nulBarPrefab; //Going with this for now //NO! BAD! don't instantiate! the NulBarPanel prefa should come with all bars, and just turn off those you don't need
    [SerializeField]
    Transform nulBarParent; //should some sort of Layout group?

    List<NulBar> nulBars;

    [SerializeField, Tooltip("Automatically sets to PsionSpectrumProfile on Awake()")]
    bool setOnAwake;


    private void Awake()
    {
        if (setOnAwake)
            SetMe();
    }

    //private void OnEnable()
    //{
    //    if (nulBars == null)
    //        nulBars = new List<NulBar>();
    //}

    /// <summary>
    /// No argument -> uses the player's stats
    /// </summary>
    public void SetMe() 
    {
        if(nulBars == null)
        {
            InitBars();
        }

        //TBF! this will have more complex logic if not all bars are to be displayed
        foreach (var nulElement in PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.psionElements) 
        {
            //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
            nulBars[(int)nulElement.nulColour].SetMe(nulElement);
            //nulBar.SetMe(nulElement);
            //nulBars.Add(nulBar);
        }

        //turn off any bars that irrelvant

    }
    public void SetMe(float values) 
    {
        if(nulBars == null)
        {
            InitBars();
        }


        for (int i = 0; i < nulBars.Count; i++)
        {
            nulBars[i].SetColour((NulColour)i);
            //nulBars[i].SetValue(values);
            nulBars[i].AnimatedSetValue(values, 1f);
        }

        ////TBF! this will have more complex logic if not all bars are to be displayed
        //foreach (var nulElement in PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.psionElements) 
        //{
        //    //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
        //    nulBars[(int)nulElement.nulColour].SetMe(nulElement);
        //    //nulBar.SetMe(nulElement);
        //    //nulBars.Add(nulBar);
        //}

        //turn off any bars that irrelvant

    }

    /// <summary>
    /// Uses item stats, Max values should still matter? do we still use the player's stats for that?
    /// </summary>
    public void SetMe(ItemSpectrumProfile itemSpectrumProfile)
    {
        if (nulBars == null)
        {
            InitBars();
        }

        foreach (var nulElement in itemSpectrumProfile.elements)
        {
            nulBars[(int)nulElement.nulColour].SetMe(nulElement);
            //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();

            //nulBar.SetMe(nulElement);
            //nulBars.Add(nulBar);
        }
        //turn off any bars that irrelvant
    }
    
    public void TurnOffAllBarTexts()
    {
        foreach (var item in nulBars)
        {
            item.TextOnOff(false);
        }
    }
    void InitBars()
    {
        //if(nulBars!=null || nulBars.Count > 0)
        //{
        //    print("Nul bars already exist");
        //    return;
        //}
        nulBars = new List<NulBar>();

        for (int i = 0; i < System.Enum.GetValues(typeof(NulColour)).Length; i++)
        {
            NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
            nulBars.Add(nulBar);
        }
    }

    public void SetBarText(int index, string text)
    {
        nulBars[index].SetText(text);
    }
}
