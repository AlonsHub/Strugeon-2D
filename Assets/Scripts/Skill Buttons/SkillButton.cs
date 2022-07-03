using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public enum ButtonColor {Red, Blue, Yellow, Purple};
public class SkillButton : Hoverable
{
    public ButtonColor buttonColor;
    public NulColour nulColour;
   // public int maxValue; //full mana
    public float skillCost; //the cost for using this skill once //TBF make ScriptableObjects which hold skills
    //public int _currentValue; //current amount of relevant mana... not sure if needed.

    //public Bar relevantBar; //relevant mana bar

    [SerializeField]
    protected Sprite effectIcon;
    Button button;

    public Pawn pawnTgt;

    PsionSpectrumProfile psionProfile => PlayerDataMaster.Instance.currentPlayerData.psionSpectrum;

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
        //SHOULD FIX to have Energy values held by the player, and the UI_Energy_Bars should refresh completely OnValueChanged() //ON IT! NOW!
        //TBFing this atm

        //Interactablity is called by MouseBehaviour - so it can't ever be clicked when irrelevant.
        //maybe I should move this here to onenable... 
        //it could have been a problem to hook into the bars OnEnable since they may not be there if they haven't enabled yet...
        //but now, pulling from the PsionProfile - we should face no such issue

        psionProfile.ModifyCurrentValue(nulColour, skillCost * -1f); //Skill cost is a postive value to reduce from current.


        //shouldnt really test for value since it should be inactive if at too low a value
        //relevantBar.ReduceValue(skillCost);
        //Debug.Log(name + " Button pushed");
        InteractableCheck();
    }

    public void InteractableCheck()
    {
        //button.interactable = skillCost <= relevantBar.currentValue;
        button.interactable = skillCost <= psionProfile.GetValueByName(nulColour);
    }
    
}
