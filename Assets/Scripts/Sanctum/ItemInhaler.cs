using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemInhaler : MonoBehaviour
{
    //public stat 

    //TEMP AF TBF - this needs to be somewhere where stats and numbers are more manageable for game designers TBF
    [SerializeField, Tooltip("Chance-to-hit = % (psion value for that colour) * (item value for same colour) * chanceToHitFactor. \n Default: 1f")]
    float chanceToHitFactor = 1f;
    //[SerializeField, Tooltip("Amount = 1d[psion value for that colour] * (item value for same colour) * amountFactor. \n Default: .01f")]
    //float amountFactor= .1f;

    public static System.Action OnInhaleStart;
    public static System.Action OnInhaleEnd;
    //TEMP AF TBF - this needs to be somewhere where stats and numbers are more manageable for game designers TBF

    //PsionSpectrumProfile _psionSpectrumProfile => PlayerDataMaster.Instance.currentPlayerData.psionSpectrum;
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

    public static bool inhaling;


    //[SerializeField]
    //NulBarPanel itemNulBarPanel;
    //NulBarPanel psionNulBarPanel;
    [SerializeField]
    PillPanel pillPanel;

    [SerializeField]
    StarGraph startGraph;

    [SerializeField]
    NoolChartHider noolChartHider;
    Coroutine coro;

    [SerializeField]
    NoolColour[] colourInhaleOrder;

    [SerializeField]
    List<NoolColour> coloursToAvoid;

    float[] _tempValues;

    public All_PsionSpells all_PsionSpells;
    [SerializeField] GameObject prefabMessage;

    bool doBumpIdleLog = false;
    private void Awake()
    {
        if (!button)
        button = GetComponent<UnityEngine.UI.Button>();

        button.onClick.AddListener(InhaleAndLogSelectedItem);
        inhaling = false;
        button.interactable = false;

        all_PsionSpells.GetSpellThresholds();
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
        if(_item == null) //should't be possible
        {
            print("no item to inhale - should't be possible");
            //become unclickable
            return "no item!";
            //SelectFirstInvItem();
        }
        inhaling = true;
        button.interactable = false;

        float[] values = new float[noolProfile.nools.Length];

        string s = $"{_item.magicItemName} was Inhaled.\n";

        //start sequence:
        //chance to hit:
        int hits = 0;

        //One-shot roll
        int[] toHitChances = new int[7]; //7 -number of nool colours (sans white). Arry of % chance to Hit
        //int hitChanceTotal = 0;
        //int activeBarCount = 0;

        int attemptMaxTTL = 100;

        while (hits == 0) //DANGER! CAN GO ON FOREVER!
        {
            foreach (var pill in _item.pillProfile.pills)
            {
                if (coloursToAvoid.Contains(pill.colour))
                    continue;

                float psionPotential = psionPillProfile.pills[(int)pill.colour].potential;
                float chanceToHit = pill.potential * psionPotential * chanceToHitFactor; //if items value = 5, psion potential = 3 -> 15% flat odd "to hit" on this colour

                values[(int)pill.colour] = 0;
                if (RollChance((int)chanceToHit, 100))
                {
                    //HIT!
                    hits++;

                    float amount = Random.Range(1, (int)psionPotential + 1) * pill.potential * GameStats.inhaleAmountFactor;
                    if (amount < 1)
                        amount = 1;
                    s += $"hit! on {pill.colour}. Amount received {amount} \n";
                    values[(int)pill.colour] = amount;

                    CheckForNewSpells(pill, amount);

                    //Add value to that nulcolours max value (psion profile)
                    noolProfile.nools[(int)pill.colour].capacity += amount;
                }
            }
            attemptCount++;
            if(attemptCount >= attemptMaxTTL)
            {
                Debug.LogError($"{attemptMaxTTL} attempts failed. Item is broken.");
                break;
            }
        }

        s += $"at {attemptCount} attempt/s \n";


        attemptCount = 0;
        //SAFTY WAIT!
        //Invoke(nameof(RemoveMagicItemFromInventory), .1f);
        //Inventory.Instance.RemoveMagicItem(_item);
        RemoveMagicItemFromInventory();
        //processingColourIndicator.SetFloat("speed", 1f / timePerBar);


        OnInhaleStart?.Invoke();
        coro = StartCoroutine(SuccessfulInhaleSequence(values));
        return s;
    }

    private void CheckForNewSpells(Pill pill, float amount)
    {
        BasicSpellData bsd = all_PsionSpells.CheckIfThresholdWillBePassed(pill.colour, noolProfile.nools[(int)pill.colour].capacity, amount);

        if (bsd != null)
        {
            //PROMPT THE THING TO SHOW WE GOT THIS NEW SPELL
            PlayerDataMaster.Instance.currentPlayerData.openedSpellsAsString += $" {bsd.spellName},";
            IdleLog.Instance.RecieveGenericMessage(prefabMessage, new List<string> { bsd.spellName }, new List<Sprite> { bsd.icon });
            doBumpIdleLog = true;
        }
    }

    void RemoveMagicItemFromInventory()
    {
        if (_item != null)
        {

            Inventory.Instance.RemoveMagicItem(_item);
            //SanctumSelectedPanel.Instance.magicItem = null;
            //SanctumSelectedPanel.Instance.SetMeFull();
        }
        else
        {
            Debug.LogError("item was already null!");
        }
        //_item = null;
    }

    IEnumerator SuccessfulInhaleSequence(float[] s)
    {
        _tempValues = s;
        SanctumSelectedPanel.Instance.SetMeFull();


        startGraph.SetToValues(_tempValues);

        skipButtonObj.SetActive(true);
        resultText.text = "";
        resultText.gameObject.SetActive(true);

        noolChartHider.SetAllHidersToValue(1f);

        //for (int i = 0; i < _psionSpectrumProfile.psionElements.Count; i++)
        for (int i = 0; i < psionPillProfile.pills.Length; i++)
        {
            NoolColour currentColour = colourInhaleOrder[i];
            if(coloursToAvoid.Contains(currentColour))
            {
                continue;
            }
            
            float _timer = timePerBar;
            while (_timer >= 0)
            {
                _timer -= Time.deltaTime;
                noolChartHider.SetHiderToValue((int)currentColour, _timer/timePerBar);
                yield return new WaitForEndOfFrame();
            }

            //yield return new WaitForSeconds(timePerBar);
            //processingColourIndicator.gameObject.SetActive(false);
            resultText.gameObject.SetActive(true);

            if (_tempValues[(int)currentColour] != 0f)
            {
                //resultText.text = $"Inhaled {psionPillProfile.pills[i].colour} colour by {s[i]}.";
                resultText.text = $"Inhaled {currentColour} colour by {_tempValues[(int)currentColour]}.";
                pillPanel.PromptReward((int)currentColour, (int)s[(int)currentColour]);
            }
            else
            {
                //resultText.text = $"Failed to gain {psionPillProfile.pills[i].colour} energy.";
                resultText.text = $"Failed to gain {currentColour} energy.";
                //itemNulBarPanel.SetBarText(i, "X");
            }
            yield return new WaitForSeconds(timePerResult);
            //yield return new WaitUntil(() => Input.anyKey);
        }

        FinalizeInhaleSequence();


        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        resultText.gameObject.SetActive(false);
        pillPanel.SetToPsion();
        startGraph.SetAllToValue(0f);
    }

    //IEnumerator 



    private void FinalizeInhaleSequence()
    {
        OnInhaleEnd?.Invoke();
        noolChartHider.SetAllHidersToValue(0f);

        if(doBumpIdleLog)
        {
            IdleLog.Instance.BumpOpen();
            doBumpIdleLog = false;
        }

        inhaling = false;

        //check if new spells are available

        skipButtonObj.SetActive(false);
    }

    bool RollChance(int x, int outOf)
    {
        return Random.Range(1,outOf+1) <= x;
    }

    public void SkipInhale()
    {
        StartCoroutine(nameof(SkipInhaleCoro));
    }
    public IEnumerator SkipInhaleCoro()
    {
        if (coro != null)
        {
            StopCoroutine(coro);
            coro = null;
        }
        for (int i = 0; i < _tempValues.Length; i++)
        {
            if (_tempValues[i] != 0)
            {
                pillPanel.PromptReward(i, (int)_tempValues[i]);
            }
        }

        FinalizeInhaleSequence();
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        resultText.gameObject.SetActive(false);
        pillPanel.SetToPsion();
        startGraph.SetAllToValue(0f);
    }
}
