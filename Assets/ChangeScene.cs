using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    string sceneName;
    public void Load()
    {
        if(sceneName !=null)
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
