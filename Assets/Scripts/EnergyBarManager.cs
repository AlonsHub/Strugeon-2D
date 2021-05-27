using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarManager : MonoBehaviour
{
    [SerializeField]
    Bar[] bars;
    public SkillButton[] skillButtons; 

    public int regenAmount;
    public static EnergyBarManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void Regen()
    {
        foreach(Bar b in bars)
        {
            b.AddValue(regenAmount);
        }
    }
    public void ButtonsInteractableCheck()
    {
        foreach(SkillButton sb in skillButtons)
        {
            if(sb)
            sb.InteractableCheck();
        }
    }
}
