using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PillGraphSlice : StarGraphSlice
{
    [SerializeField, Tooltip("Basically, the value if the pill was as full as can show")]
    float topMax;

    [SerializeField]
    Image negativeFill;

    [SerializeField]
    BasicDisplayer rewardDisplayer;
    [SerializeField]
    float timeTillFade_RewardDisplayer;
    
    public void SetReward(int rewardAmount) //to simplify the printed number
    {
        rewardDisplayer.SetMe(new List<string> { rewardAmount.ToString() }, new List<Sprite> { });
        StartCoroutine(nameof(OnAndOffSequence));
    }
    IEnumerator OnAndOffSequence()
    {
        rewardDisplayer.gameObject.SetActive(true);

        yield return new WaitUntil(() => !ItemInhaler.inhaling);

        yield return new WaitForSeconds(timeTillFade_RewardDisplayer);

        //Fade Out logic here

        rewardDisplayer.gameObject.SetActive(false);

    }
    public void SetMeFull(Nool element)
    {
        topMax = (Mathf.Floor(element.capacity / 100) + 1)*100;
        maxValue = element.capacity;
        negativeFill.fillAmount = 1- maxValue/topMax;
        //positiveFill.fillAmount = maxValue / topMax;

        currentValue = element.currentValue;

        fillImg.fillAmount = currentValue / topMax; // the positive does not change size, hence the full 1f fillamount amounts to a full topMax (and not the current capacity)
        base.SetMyDisplayer(new List<string> { $"{currentValue} / {maxValue}" }, new List<Sprite> ());
    }

    

}
