using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    GameObject disabler; //a gameobject that, if Active, prevents the effects of this button
    [SerializeField]
    string sceneName;
    public void Load()
    {
        if (disabler && disabler.activeSelf)
        {
            Debug.LogError("cant exit when " + disabler.name + " the (disabler) is Active!");
            return;
        }

        if(sceneName !=null)
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
