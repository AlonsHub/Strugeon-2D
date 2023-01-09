using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour 
{
    public float maxValue;
    protected float target_maxValue;
    public float currentValue;
    protected float target_currentValue;
    public float regenRate;
    [SerializeField]
    protected Image img;
    //protected virtual void OnEnable()
    //{
    //    if(TurnMaster.Instance)
    //    //relevant for 2 reasons. 
    //    //in arena - TurnMaster may/should not be accessible before Start(). 
    //        TurnMaster.Instance.OnTurnOrderRestart += Regen; 

    //}

    //This is BattleBar - maybe it should inherit from nul bar?


    private void Start()
    {
        if (TurnMachine.Instance)
            TurnMachine.Instance.OnStartNewRound += Regen;


    }
    

    protected virtual void OnDisable()
    {
        if (TurnMachine.Instance)
            TurnMachine.Instance.OnStartNewRound -= Regen;
    }
  
    public virtual void ShowValue()
    {
        //slider.value = value;
        img.fillAmount = currentValue / maxValue;
    }
    public virtual void AddValue(float value)//redundant? one of them is
    {
        currentValue += value;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        
        ShowValue();

    }
    public virtual void Regen()//redundant? one of them is
    {
        currentValue += regenRate;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        
        ShowValue();
    }
    public virtual void ReduceValue(float value) //redundant? one of them is
    {
        currentValue -= value;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        
        ShowValue();
    }
}
