using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public enum ButtonColor {Red, Blue, Yellow, Purple};
public class SkillButton : MonoBehaviour
{
    public ButtonColor buttonColor;

   // public int maxValue; //full mana
    public int skillCost; //the cost for using this skill once
    //public int _currentValue; //current amount of relevant mana... not sure if needed.

    public Bar relevantBar; //relevant mana bar

    [SerializeField]
    protected Sprite effectIcon;
    Button button;

    public Pawn pawnTgt;
    

    private void Awake()
    {
        button = GetComponent<Button>();
       // InteractableCheck();
    }

    //private void OnEnable()
    //{
    //  //  InteractableCheck();
    //}

    public virtual void OnButtonClick()
    {
        //SHOULD FIX to have Energy values held by the player, and the UI_Energy_Bars should refresh completely OnValueChanged()

        //shouldnt really test for value since it should be inactive if at too low a value
        relevantBar.ReduceValue(skillCost);
        Debug.Log(name + " Button pushed");
        InteractableCheck();
    }

    public void InteractableCheck()
    {
        button.interactable = skillCost <= relevantBar.currentValue;
    }
    
}
