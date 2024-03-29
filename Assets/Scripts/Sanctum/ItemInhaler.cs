﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemInhaler : MonoBehaviour
{
    //TEMP AF TBF - this needs to be somewhere where stats and numbers are more manageable for game designers TBF
    [SerializeField, Tooltip("Chance-to-hit = % (psion value for that colour) * (item value for same colour) * chanceToHitFactor. \n Default: 1f")]
    float chanceToHitFactor = 1f;
    [SerializeField, Tooltip("Amount = 1d[psion value for that colour] * (item value for same colour) * amountFactor. \n Default: .01f")]
    float amountFactor= .01f;

    public System.Action OnInhale;
    //TEMP AF TBF - this needs to be somewhere where stats and numbers are more manageable for game designers TBF

    PsionSpectrumProfile _psionSpectrumProfile => PlayerDataMaster.Instance.currentPlayerData.psionSpectrum;

    MagicItem _item;
    //MagicItem emptyItem;

    //SAFETY MEASURES:
    int maxAttempts = 100;
    int attemptCount = 0;

    [SerializeField]
    float timePerBar;
    [SerializeField]
    float timePerResult;

    [SerializeField]
    GameObject processingColourIndicator; //placeholder TBF - currently just activates a sprite with a default animation
    [SerializeField]
    TMP_Text resultText;
    //public void SelectFirstInvItem()
    //{
    //    //if (Inventory.Instance.inventoryItems.Count > 0 && Inventory.Instance.inventoryItems[0] != null)
    //    //    SelectItem(Inventory.Instance.inventoryItems[0]); //BAD! EVEN FOR A TEST THIS SHOWS we need better access to lists. TBF
    //    //else
    //        SelectItem(emptyItem);
    //}
    [SerializeField]
    UnityEngine.UI.Button button;

    public bool inhaling;

    [SerializeField]
    NulBarPanel itemNulBarPanel;
    [SerializeField]
    NulBarPanel psionNulBarPanel;

    private void Awake()
    {
        if(!button)
        button = GetComponent<UnityEngine.UI.Button>();

        button.onClick.AddListener(InhaleAndLogSelectedItem);
        inhaling = false;
    }

    void InhaleAndLogSelectedItem()
    {
        //if (_item != null)
        //{
            string s = InhaleSelectedItem();
            print(s);
        //}
        //else
        //{
        //    //SelectFirstInvItem();
        //    //string s = InhaleSelectedItem();
        //    //print(s);
        //    print("no item to in");
        //}

        //SelectFirstInvItem();

        //print(s);
    }
    public void SelectItem(MagicItem selectedItem)
    {
        _item = selectedItem;
        button.interactable = selectedItem != null; //in case of the below error, set the button.interactable to false
    }
    [ContextMenu("InhaleSelceted")]
    public string InhaleSelectedItem()
    {
        if(_item == null)
        {
            print("no item to inhale");
            //become unclickable
            return "no item!";
            //SelectFirstInvItem();
        }

        float[] values = new float[_psionSpectrumProfile.psionElements.Count];

        string s = $"{_item.magicItemName} was Inhaled.\n";

        //start sequence:
        //chance to hit:
        int hits = 0;

        //One-shot roll
        int[] toHitChances = new int[7]; //7 -number of nul colours. Arry of % chance to Hit
        int hitChanceTotal = 0;
        int activeBarCount = 0;
        foreach (var el in _item.spectrumProfile.elements)
        {
            float psionPotential = _psionSpectrumProfile.GetValueByName(el.nulColour);
            float chanceToHit = el.value * psionPotential * chanceToHitFactor; //if items value = 5, psion potential = 3 -> 15% flat odd "to hit" on this colour
            toHitChances[(int)el.nulColour] = (int)chanceToHit;
            hitChanceTotal += (int)chanceToHit;
            activeBarCount++;
        }
        int barHits = 0;
        //roll from 1-3 how many 
        float chanceForSingleBarHit = hitChanceTotal/activeBarCount; //total is chance out of 700, divide by 7 for %

        //One-shot roll


        foreach (var el in _item.spectrumProfile.elements)
        {
            float psionPotential = _psionSpectrumProfile.GetValueByName(el.nulColour);
            float chanceToHit = el.value * psionPotential * chanceToHitFactor; //if items value = 5, psion potential = 3 -> 15% flat odd "to hit" on this colour

            values[(int)el.nulColour] =0;
            if (RollChance((int)chanceToHit, 100))
            {
                //HIT!
                hits++;
                //Roll Amount:
                float amount = Random.Range(1, (int)psionPotential + 1) * el.value * amountFactor;
                s += $"hit! on {el.nulColour}. Amount received {amount} \n";
                values[(int)el.nulColour] = amount;

                //Add value to that nulcolours max value (psion profile)
                PlayerDataMaster.Instance.AddToPsionNulMax(el.nulColour, amount);

                //This was the problem I think!
                //Inventory.Instance.RemoveMagicItem(_item);
                //_item = null;
            }
        }
        if(hits == 0)
        {
            attemptCount++;
            if(attemptCount>=maxAttempts)
            {
                attemptCount = 0;
                return "nothing. attempt count exceeded limit";
            }
           return InhaleSelectedItem();
        }
        s += $"at {attemptCount + 1} attempt/s \n";


        attemptCount = 0;
        //SAFTY WAIT!
        Invoke(nameof(RemoveMagicItemFromInventory), .1f);
        //Inventory.Instance.RemoveMagicItem(_item);

        StartCoroutine(SuccessfulInhaleSequence(values));
        OnInhale?.Invoke();
        return s;
    }
    void RemoveMagicItemFromInventory()
    {
        Inventory.Instance.RemoveMagicItem(_item);
        _item = null;
    }

    IEnumerator SuccessfulInhaleSequence(float[] s)
    {
        inhaling = true;
        button.interactable = false;
        for (int i = 0; i < _psionSpectrumProfile.psionElements.Count; i++)
        {
            processingColourIndicator.SetActive(true);
            resultText.transform.parent.gameObject.SetActive(false);
            yield return new WaitForSeconds(timePerBar);
            processingColourIndicator.SetActive(false);
            resultText.transform.parent.gameObject.SetActive(true);

            if (s[i] != 0f)
            {
                resultText.text = $"Inhaled {_psionSpectrumProfile.psionElements[i].GetNulColour} colour by {s[i]}.";
                itemNulBarPanel.SetBarText(i, $"+{s[i]}");
            }
            else
            {
                resultText.text = $"Failed to gain {_psionSpectrumProfile.psionElements[i].GetNulColour} energy.";
                itemNulBarPanel.SetBarText(i, "X");
            }
            yield return new WaitForSeconds(timePerResult);
            //yield return new WaitUntil(() => Input.anyKey);
        }

        yield return new WaitUntil(() => Input.anyKey);
        itemNulBarPanel.TurnOffAllBarTexts();
        //nulBarPanel.SetMe(); //resets to accomedate new max values
        psionNulBarPanel.SetMe();
        resultText.transform.parent.gameObject.SetActive(false);
        inhaling = false;
        button.interactable = true;
    }

    bool RollChance(int x, int outOf)
    {
        return Random.Range(1,outOf+1) <= x;
    }
}
