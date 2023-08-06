using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpellButton : Hoverable
{
    public BasicSpellData basicSpellData;

    public NoolColour nulColour;
    public float skillCost; //the cost for using this skill once //TBF make ScriptableObjects which hold skills
    
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

    public Sprite GetEffectIcon { get => effectIcon; private set => effectIcon = value;}
    //Experimental
    protected virtual System.Type GetStatusEffectType()
    {
        return null; //This is only relevant for spells which leave status effects
    }

#if UNITY_EDITOR
    [ContextMenu("CopyDataToBSD")]
    public void CopyDataToBSD()
    {
        basicSpellData.spellName = name;
        basicSpellData.longDescription = GetMyData;
        basicSpellData.noolCost = skillCost;
        basicSpellData.noolColour = nulColour;
        basicSpellData.icon = effectIcon;
    }

#endif

    //Experimental!

    NoolProfile psionNoolProfile => PlayerDataMaster.Instance.currentPlayerData.noolProfile;
    //protected float modifier => psionProfile.GetMaxValueByName(nulColour);
    protected float modifier => psionNoolProfile.nools[(int)nulColour].capacity;

    private void Awake()
    {
        button = GetComponent<Button>();

        DisableIfNotCastableWhatsoever();

        InteractableCheck();
    }

    //Temp! this will not be relevant once spells are loaded from somewhere other than the prefab in the arena
    private void DisableIfNotCastableWhatsoever()
    {
        if(skillCost > psionNoolProfile.nools[(int)nulColour].capacity)
        {
            //Disalbe all visual elemets, assuming they will readjust on their own
            gameObject.SetActive(false);
        }
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

        psionNoolProfile.ModifyCurrentValue(nulColour, skillCost * -1f); //Skill cost is a postive value to reduce from current.

        if(vfxPrefab)
        {
            GameObject go = Instantiate(vfxPrefab, pawnTgt.transform);
            Renderer rend = go.GetComponent<Renderer>();
            Color col = Color.white;
            switch (nulColour)
            {
                case NoolColour.Orange:
                    break;
                case NoolColour.Yellow:
                    col = SturgeonColours.Instance.noolYellow;
                    break;
                case NoolColour.Green:
                    break;
                case NoolColour.Blue:
                    col = SturgeonColours.Instance.noolBlue;
                    break;
                case NoolColour.Red:
                    col = SturgeonColours.Instance.noolRed;
                    break;
                case NoolColour.Purple:
                    col = SturgeonColours.Instance.noolPurple;
                    break;
                case NoolColour.Black:
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

    public virtual void InteractableCheck()
    {
        //button.interactable = skillCost <= relevantBar.currentValue;

        //button.interactable = skillCost <= psionProfile.GetValueByName(nulColour);
        SetButtonInteractability(skillCost <= psionNoolProfile.nools[(int)nulColour].currentValue);
        
    }

    protected void SetButtonInteractability(bool isInteractable)
    {
        button.interactable = isInteractable;
    }
    
}
