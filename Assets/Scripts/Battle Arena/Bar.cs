﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    //public Slider slider;

    
    public float maxValue;
    public float currentValue;
    public Image img;
    private void Start()
    {
        currentValue = maxValue;
        img = GetComponent<Image>();
    }
    public void SetMaxValue(float value)
    {
        maxValue = currentValue = value;
        //currentValue = value;
    }
    public void SetValue(float value)
    {
        //slider.value = value;
        img.fillAmount = currentValue / maxValue;
    }
    public void AddValue(float value)//redundant? one of them is
    {
        currentValue += value;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        img.fillAmount = currentValue / maxValue;

    }
    public void ReduceValue(float value) //redundant? one of them is
    {
        currentValue -= value;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        img.fillAmount = currentValue / maxValue;

    }

    void ClampInRange()
    {

    }
}