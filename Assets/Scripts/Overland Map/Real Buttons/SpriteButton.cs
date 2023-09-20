using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//enum ButtonSprite_State { Regular, Hover, Clicked, Disabled};

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
    #endregion

    #region OnStuffEvents
    public UnityEvent OnClick;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    #endregion


    public void SetMe(UnityEngine.UI.Button button, Sprite reg)
    {
        hoverSprite = button.spriteState.highlightedSprite;
        clickedSprite = button.spriteState.pressedSprite;
        regularSprite = reg;
    }
    private void Awake()
    {
        SetInteractive(true);
    }

    private void OnMouseUp()
    {
        if (!_interactive)
            return;

        SetSpriteToState(ButtonSprite_State.Clicked);
        OnClick.Invoke();
    }

    private void OnMouseEnter()
    {
        if (!_interactive)
            return;

        SetSpriteToState(ButtonSprite_State.Hover);
        OnEnter.Invoke();
    }

    private void OnMouseExit()
    {
        if (!_interactive)
            return;
        SetSpriteToState(ButtonSprite_State.Regular);
        OnExit.Invoke();
    }
 
    void SetSpriteToState(ButtonSprite_State state)
    {
        Sprite newSprite = null;
        switch (state)
        {
            case ButtonSprite_State.Regular:
                newSprite = regularSprite;
                break;
            case ButtonSprite_State.Hover:
                newSprite = hoverSprite;
                break;
            case ButtonSprite_State.Clicked:
                newSprite = clickedSprite;
                StartCoroutine(BackToRegular());
                //Start interruptable time-out to return to regular?
                break;
            case ButtonSprite_State.Disabled:
                newSprite = disabledSprite;
                break;
            default:
                break;
        }
        spriteRenderer.sprite = newSprite;
    }
    IEnumerator BackToRegular()
    {
        yield return new WaitForSeconds(_stayPressedTime);
        SetSpriteToState(ButtonSprite_State.Regular);
    }
    public void SetInteractive(bool isInteractive)
    {
        _interactive = isInteractive;
        SetSpriteToState(_interactive ? ButtonSprite_State.Regular : ButtonSprite_State.Disabled);
    }
}
