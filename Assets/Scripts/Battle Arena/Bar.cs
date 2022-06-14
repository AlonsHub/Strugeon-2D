using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    //public Slider slider;

    
    public float maxValue;
    public float currentValue;
    public float regenRate;
    [SerializeField]
    Image img;
    protected virtual void OnEnable()
    {
        if(TurnMaster.Instance)
        TurnMaster.Instance.OnTurnOrderRestart += Regen; 
    }
    private void Start()
    {
        TurnMaster.Instance.OnTurnOrderRestart += Regen; 
        
    }

    protected virtual void OnDisable()
    {
        TurnMaster.Instance.OnTurnOrderRestart -= Regen;    
    }
    //public virtual void SetMaxValue(float value)
    //{
    //    maxValue = currentValue = value;
    //    //currentValue = value;
    //}
    public virtual void ShowValue()
    {
        //slider.value = value;
        img.fillAmount = currentValue / maxValue;
    }
    public virtual void AddValue(float value)//redundant? one of them is
    {
        currentValue += value;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        img.fillAmount = currentValue / maxValue;

    }
    public virtual void Regen()//redundant? one of them is
    {
        currentValue += regenRate;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        img.fillAmount = currentValue / maxValue;

    }
    public virtual void ReduceValue(float value) //redundant? one of them is
    {
        currentValue -= value;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        img.fillAmount = currentValue / maxValue;

    }
}
