using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public enum ButtonColor {Red, Blue, Yellow, Purple};
public class SpellButton : Hoverable
{
    public ButtonColor buttonColor;
    public NulColour nulColour;
   // public int maxValue; //full mana
    public float skillCost; //the cost for using this skill once //TBF make ScriptableObjects which hold skills
    //public int _currentValue; //current amount of relevant mana... not sure if needed.

    [SerializeField]
    GameObject vfxPrefab;

    [SerializeField]
    bool checkOnEnable;
    [SerializeField]
    protected Sprite effectIcon;
    Button button;
    [SerializeField]
    bool closeMenu;

    public Pawn pawnTgt;

    //Experimental!
    protected virtual System.Type GetStatusEffectType()
    {
        return null; //This is only relevant for spells which leave status effects
    }

    //Experimental!

    PsionSpectrumProfile psionProfile => PlayerDataMaster.Instance.currentPlayerData.psionSpectrum;

    private void Awake()
    {
        button = GetComponent<Button>();
        // InteractableCheck();
        InteractableCheck();
    }

    private void OnEnable()
    {
        if (checkOnEnable)
            InteractableCheck();
    }

    public virtual void OnButtonClick()
    {
        //SHOULD FIX to have Energy values held by the player, and the UI_Energy_Bars should refresh completely OnValueChanged() //ON IT! NOW!
        //TBFing this atm

        //Interactablity is called by MouseBehaviour - so it can't ever be clicked when irrelevant.
        //maybe I should move this here to onenable... 
        //it could have been a problem to hook into the bars OnEnable since they may not be there if they haven't enabled yet...
        //but now, pulling from the PsionProfile - we should face no such issue

        psionProfile.ModifyCurrentValue(nulColour, skillCost * -1f); //Skill cost is a postive value to reduce from current.

        if(vfxPrefab)
        {
            GameObject go = Instantiate(vfxPrefab, pawnTgt.transform);
            Renderer rend = go.GetComponent<Renderer>();
            Color col = Color.white;
            switch (nulColour)
            {
                case NulColour.Orange:
                    break;
                case NulColour.Yellow:
                    col = SturgeonColours.Instance.noolYellow;
                    break;
                case NulColour.Green:
                    break;
                case NulColour.Blue:
                    col = SturgeonColours.Instance.noolBlue;
                    break;
                case NulColour.Red:
                    col = SturgeonColours.Instance.noolRed;
                    break;
                case NulColour.Purple:
                    col = SturgeonColours.Instance.noolPurple;
                    break;
                case NulColour.Black:
                    break;
                default:
                    break;
            }

            rend.material.SetColor("_EmissionColor", col*3f);

            Destroy(go, 1f);
        }
        //shouldnt really test for value since it should be inactive if at too low a value
        //relevantBar.ReduceValue(skillCost);
        //Debug.Log(name + " Button pushed");
        InteractableCheck();

        if(closeMenu)
        {
            MouseBehaviour.Instance.CloseMenus();

            //TEMP - move to mousebehaviour?
            Renderer hitRenderer = MouseBehaviour.hitTarget.GetComponentInChildren<Renderer>();

            hitRenderer.material.SetFloat("_Thickness", 0f);
            //TEMP
        }


    }

    public void InteractableCheck()
    {
        //button.interactable = skillCost <= relevantBar.currentValue;
        
        button.interactable = skillCost <= psionProfile.GetValueByName(nulColour);
        
    }
    
}
