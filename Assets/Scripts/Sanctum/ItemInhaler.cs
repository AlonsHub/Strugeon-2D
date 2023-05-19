using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemInhaler : MonoBehaviour
{
    //TEMP AF TBF - this needs to be somewhere where stats and numbers are more manageable for game designers TBF
    [SerializeField, Tooltip("Chance-to-hit = % (psion value for that colour) * (item value for same colour) * chanceToHitFactor. \n Default: 1f")]
    float chanceToHitFactor = 1f;
    //[SerializeField, Tooltip("Amount = 1d[psion value for that colour] * (item value for same colour) * amountFactor. \n Default: .01f")]
    //float amountFactor= .1f;

    public System.Action OnInhale;
    //TEMP AF TBF - this needs to be somewhere where stats and numbers are more manageable for game designers TBF

    PsionSpectrumProfile _psionSpectrumProfile => PlayerDataMaster.Instance.currentPlayerData.psionSpectrum;
    NoolProfile noolProfile => PlayerDataMaster.Instance.currentPlayerData.noolProfile;
    PillProfile psionPillProfile => PlayerDataMaster.Instance.currentPlayerData.pillProfile;

    MagicItem _item => SanctumSelectedPanel.Instance.magicItem;
    //MagicItem emptyItem;

    //SAFETY MEASURES:
    int maxAttempts = 100;
    int attemptCount = 0;

    [SerializeField]
    float timePerBar;
    [SerializeField]
    float timePerResult;

    [SerializeField]
    //GameObject processingColourIndicator; //placeholder TBF - currently just activates a sprite with a default animation
    Animator processingColourIndicator; //placeholder TBF - currently just activates a sprite with a default animation
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

    [SerializeField]
    GameObject skipButtonObj; 

    public bool inhaling;


    //[SerializeField]
    //NulBarPanel itemNulBarPanel;
    //NulBarPanel psionNulBarPanel;
    [SerializeField]
    PillPanel pillPanel;

    Coroutine coro;
    

    private void Awake()
    {
        if (!button)
        button = GetComponent<UnityEngine.UI.Button>();

        button.onClick.AddListener(InhaleAndLogSelectedItem);
        inhaling = false;
    }

    void InhaleAndLogSelectedItem()
    {
      
            string s = InhaleSelectedItem();
            print(s);
       
    }
    //public void SelectItem(MagicItem selectedItem)
    //{
    //    //_item = selectedItem;
    //    button.interactable = selectedItem != null; //in case of the below error, set the button.interactable to false
    //}
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

        float[] values = new float[noolProfile.nools.Length];

        string s = $"{_item.magicItemName} was Inhaled.\n";

        //start sequence:
        //chance to hit:
        int hits = 0;

        //One-shot roll
        int[] toHitChances = new int[7]; //7 -number of nool colours (sans white). Arry of % chance to Hit
        int hitChanceTotal = 0;
        int activeBarCount = 0;
        foreach (var pill in _item.pillProfile.pills)
        {
            if (pill.potential <= 0)
                continue;
            //float psionPotential = _psionSpectrumProfile.GetValueByName(el.nulColour);
            float psionPotential = psionPillProfile.pills[(int)pill.colour].potential;
            float chanceToHit = pill.potential * psionPotential * chanceToHitFactor; //if items value = 5, psion potential = 3 -> 15% flat odd "to hit" on this colour
            toHitChances[(int)pill.colour] = (int)chanceToHit;
            hitChanceTotal += (int)chanceToHit;
            activeBarCount++;
        }
        //int barHits = 0;
        
        float chanceForSingleBarHit = hitChanceTotal/activeBarCount; //total is chance out of 700, divide by 7 for %

        
        //foreach (var el in _item.spectrumProfile.elements)
        foreach (var pill in _item.pillProfile.pills)
        {
            float psionPotential = psionPillProfile.pills[(int)pill.colour].potential;
            float chanceToHit = pill.potential * psionPotential * chanceToHitFactor; //if items value = 5, psion potential = 3 -> 15% flat odd "to hit" on this colour

            values[(int)pill.colour] =0;
            if (RollChance((int)chanceToHit, 100))
            {
                //HIT!
                hits++;
                //Roll Amount:
                //float amountFactor = PlayerDataMaster.Instance.currentPlayerData.psionProgressionLevel >= 5 ? .5f : (0.2f * (1 - (noolProfile.nools[(int)pill.colour].capacity - 1) / 20)); //this should probably be 700 not 20, and reconsidered now that potential is used
                //float amountFactor = PlayerDataMaster.Instance.currentPlayerData.psionProgressionLevel >= 5 ? .5f : .2f;
                float amountFactor = .5f;

                float amount = Random.Range(1, (int)psionPotential + 1) * pill.potential * amountFactor;
                s += $"hit! on {pill.colour}. Amount received {amount} \n";
                values[(int)pill.colour] = amount;

                noolProfile.nools[(int)pill.colour].capacity += amount;

                //Add value to that nulcolours max value (psion profile)
                //PlayerDataMaster.Instance.AddToPsionNulMax(pill.colour, amount);

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

        processingColourIndicator.SetFloat("speed", 1f / timePerBar);


        coro = StartCoroutine(SuccessfulInhaleSequence(values));
        OnInhale?.Invoke();
        return s;
    }
    void RemoveMagicItemFromInventory()
    {
        if (_item != null)
        {

            Inventory.Instance.RemoveMagicItem(_item);
            SanctumSelectedPanel.Instance.magicItem = null;
        }
        else
        {
            Debug.LogError("item was already null!");
        }
        //_item = null;
    }

    IEnumerator SuccessfulInhaleSequence(float[] s)
    {
        inhaling = true;
        button.interactable = false;
        skipButtonObj.SetActive(true);
        resultText.gameObject.SetActive(true);
        //for (int i = 0; i < _psionSpectrumProfile.psionElements.Count; i++)
        for (int i = 0; i < psionPillProfile.pills.Length-1; i++)
        {
            processingColourIndicator.gameObject.SetActive(true);
            //resultText.gameObject.SetActive(false);
            yield return new WaitForSeconds(timePerBar);
            processingColourIndicator.gameObject.SetActive(false);
            //resultText.gameObject.SetActive(true);

            if (s[i] != 0f)
            {
                resultText.text = $"Inhaled {psionPillProfile.pills[i].colour} colour by {s[i]}.";
                //itemNulBarPanel.SetBarText(i, $"+{s[i]}");
            }
            else
            {
                resultText.text = $"Failed to gain {psionPillProfile.pills[i].colour} energy.";
                //itemNulBarPanel.SetBarText(i, "X");
            }
            yield return new WaitForSeconds(timePerResult);
            //yield return new WaitUntil(() => Input.anyKey);
        }

        //yield return new WaitUntil(() => Input.anyKey);
        //itemNulBarPanel.TurnOffAllBarTexts();
        //nulBarPanel.SetMe(); //resets to accomedate new max values
        //psionNulBarPanel.SetMe();
        FinalizeInhaleSequence();
    }

    private void FinalizeInhaleSequence()
    {
        processingColourIndicator.gameObject.SetActive(false);

        pillPanel.SetToPsion();
        SanctumSelectedPanel.Instance.SetMeFull();

        resultText.gameObject.SetActive(false);
        inhaling = false;
        button.interactable = true;
        skipButtonObj.SetActive(false);
    }

    bool RollChance(int x, int outOf)
    {
        return Random.Range(1,outOf+1) <= x;
    }

    public void SkipInhale()
    {
        StopCoroutine(coro);
        FinalizeInhaleSequence();
    }
}
