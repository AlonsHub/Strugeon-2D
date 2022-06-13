using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInhaler : MonoBehaviour
{
    //TEMP AF TBF - this needs to be somewhere where stats and numbers are more manageable for game designers TBF
    [SerializeField, Tooltip("Chance-to-hit = % (psion value for that colour) * (item value for same colour) * chanceToHitFactor. \n Default: 1f")]
    float chanceToHitFactor = 1f;
    [SerializeField, Tooltip("Amount = 1d[psion value for that colour] * (item value for same colour) * amountFactor. \n Default: .01f")]
    float amountFactor= .01f;

    //TEMP AF TBF - this needs to be somewhere where stats and numbers are more manageable for game designers TBF

    PsionSpectrumProfile _psionSpectrumProfile => PlayerDataMaster.Instance.currentPlayerData.psionSpectrum;

    MagicItem _item;

    //SAFETY MEASURES:
    int maxAttempts = 20;
    int attemptCount = 0;
    [ContextMenu("test")]
    public void SelectFirstInvItem()
    {
        SelectItem(Inventory.Instance.inventoryItems[0]); //BAD! EVEN FOR A TEST THIS SHOWS we need better access to lists. TBF
    }

    public void SelectItem(MagicItem selectedItem)
    {
        _item = selectedItem;
    }
    [ContextMenu("InhaleSelceted")]
    public string InhaleSelectedItem()
    {
        string s = "";
        if(_item == null)
        {
            SelectFirstInvItem();
        }
        //start sequence:
        //chance to hit:
        int hits = 0;

        foreach (var item in _item.spectrumProfile.elements)
        {
            float psionPotential = _psionSpectrumProfile.GetValueByName(item.nulColour);
            float chanceToHit = item.value * psionPotential * chanceToHitFactor; //if items value = 5, psion potential = 3 -> 15% flat odd "to hit" on this colour

            if(RollChance((int)chanceToHit, 100))
            {
                //HIT!
                hits++;
                //Roll Amount:
                float amount = Random.Range(1, (int)psionPotential + 1) * item.value * amountFactor;
                s += $"hit! on {item.nulColour}. Amount received {amount} \n";
                print($"hit! on {item.nulColour}. Amount received {amount}");
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

        print($"at {attemptCount+1} attempt/s");

        attemptCount = 0;
        return s;
    }

    bool RollChance(int x, int outOf)
    {
        return Random.Range(1,outOf+1) <= x;
    }
}
