using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//enum ButtonSprite_State { Regular, Hover, Clicked, Disabled};
enum ButtonStateColours { Regular, Hover, Pressed, Disabled}

public class SpriteButton : MonoBehaviour
{
    bool _interactive;

    [SerializeField]
    float _stayPressedTime;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    ButtonSprite_State _currentState;
    Sprite _currentSprite;


    #region Sprites for states
    [SerializeField]
    Sprite regularSprite;
    [SerializeField]
    Sprite hoverSprite;
    [SerializeField]
    Sprite clickedSprite;
    [SerializeField]
    Sprite disabledSprite;

    [SerializeField]
    Sprite[] stateSprites;
    #endregion

    [SerializeField]
    Color[] stateColours; // currently setup as "fall-back mode" - does colour if the relevant sprite is missing


    bool _isMouseIn = false;

    #region OnStuffEvents
    public UnityEvent OnClick;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    #endregion

    public void OnValidate()
    {
        if (stateSprites != null && stateSprites.Length != 0)
            return;
        stateSprites = new Sprite[4];
        stateSprites[0] = regularSprite;
        stateSprites[1] = hoverSprite;
        stateSprites[2] = clickedSprite;
        stateSprites[3] = disabledSprite;
        //Get name sprite for sites - Resource<Load>() or Prefabber-like solution
    }

    public void SetMe(UnityEngine.UI.Button button, Sprite reg)
    {
        hoverSprite = button.spriteState.highlightedSprite;
        clickedSprite = button.spriteState.pressedSprite;
        regularSprite = reg;
        //Get name sprite for sites - Resource<Load>() or Prefabber-like solution
    }
    //[ContextMenu("Set to reg")]
    //public void OnValidate()
    //{
    //    SetSpriteToState(ButtonSprite_State.Regular);
    //}
    private void Awake()
    {
        SetInteractive(true);
    }

    private void OnMouseUp()
    {
        if (!_interactive)
            return;

        SetSpriteToState(ButtonSprite_State.Clicked);
        StartCoroutine(BackToRegular());

        OnClick.Invoke();
    }

    private void OnMouseEnter()
    {
        if (!_interactive)
            return;

        _isMouseIn = true;
        SetSpriteToState(ButtonSprite_State.Hover);
        OnEnter.Invoke();
    }

    private void OnMouseExit()
    {
        if (!_interactive)
            return;
        _isMouseIn = false;
        SetSpriteToState(ButtonSprite_State.Regular);
        OnExit.Invoke();
    }
 
    void SetSpriteToState(ButtonSprite_State state)
    {
        Sprite newSprite = null;
        Color? newCol = null;
        if(stateSprites[(int)_currentState] == null)
        {
            newCol = Color.white; //reset previous tinting
        }
        
        if(stateSprites[(int)state] == null)
        {
            //fall-back to colour 
            newCol = stateColours[(int)state];
        }
        else
        {
            newSprite = stateSprites[(int)state];
        }


        if (newSprite != null) //ASSUMING WE ALWAYS HAVE COLOURS AND WE NEVER DO BOTH COLOUR CHANGE AND SPRITE CHANGE!
            spriteRenderer.sprite = newSprite;
        else
            spriteRenderer.color = newCol.Value;
    }
    IEnumerator BackToRegular()
    {
        yield return new WaitForSeconds(_stayPressedTime);
        if(_isMouseIn)
        SetSpriteToState(ButtonSprite_State.Hover);
        else
        SetSpriteToState(ButtonSprite_State.Regular);

    }
    public void SetInteractive(bool isInteractive)
    {
        _interactive = isInteractive;
        SetSpriteToState(_interactive ? ButtonSprite_State.Regular : ButtonSprite_State.Disabled);
    }
}
