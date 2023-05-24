using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomItemGetter : MonoBehaviour
{
    //Button button;
    //private void Awake()
    //{
    //    button = GetComponent<Button>();
    //}
    //private void OnEnable()
    //{
    //    button.onClick.AddListener(OnClick);
    //}
    //private void OnDisable()
    //{
    //    button.onClick.RemoveListener(OnClick);
    //}

    public void OnClick()
    {
        Inventory.Instance.AddMagicItem(DifficultyTranslator.Instance.DifficultyToSingleReward((LairDifficulty)Random.Range(0, System.Enum.GetValues(typeof(LairDifficulty)).Length)));
    }
}
