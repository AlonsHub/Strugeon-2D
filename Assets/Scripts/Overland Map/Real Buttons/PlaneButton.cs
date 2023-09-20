using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum ButtonSprite_State { Regular, Hover, Clicked, Disabled };

public class PlaneButton : MonoBehaviour
{

    bool _interactive;

    [SerializeField]
    float _stayPressedTime;

    [SerializeField]
    MeshRenderer meshRenderer;

    ButtonSprite_State _currentState;
    Texture _currentSprite;
    #region Sprites for states
    [SerializeField]
    Texture regularSprite;
    [SerializeField]
    Texture hoverSprite;
    [SerializeField]
    Texture clickedSprite;
    [SerializeField]
    Texture disabledSprite;
    #endregion

    #region OnStuffEvents
    public UnityEvent OnClick;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    #endregion


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
        
        switch (state)
        {
            case ButtonSprite_State.Regular:
                _currentSprite = regularSprite;
                break;
            case ButtonSprite_State.Hover:
                _currentSprite = hoverSprite;
                break;
            case ButtonSprite_State.Clicked:
                _currentSprite = clickedSprite;
                StartCoroutine(BackToRegular());
                //Start interruptable time-out to return to regular?
                break;
            case ButtonSprite_State.Disabled:
                _currentSprite = disabledSprite;
                break;
            default:
                break;
        }
        meshRenderer.material.SetTexture("_BaseMap",_currentSprite);
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
