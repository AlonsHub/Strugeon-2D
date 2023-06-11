using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscToX : MonoBehaviour
{
    public static EscToX Current;

    //Null if last
    EscToX _previous;

    [SerializeField]
    UnityEngine.UI.Button myButton;

    private void OnEnable()
    {
        if(Current !=null)
        {
            _previous = Current;
        }
        Current = this;
    }

    private void OnDisable()
    {
        if(_previous !=null)
        {
            Current = _previous;
        }
        else
        {
            Current = null;
        }
    }

    private void Update()
    {
        if (Current != this)
            return;

        if(!Current)
        {
            Debug.LogError("no current X button to use, why is this on?");
            return;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.LogError($"USED {transform.parent.name}");
            Current.myButton.onClick.Invoke();
        }
    }
}
