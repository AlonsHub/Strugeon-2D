using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTavernScene : MonoBehaviour
{
    public static Transform tavernButtonTransform;

    private void OnEnable()
    {
        tavernButtonTransform = transform;
    }
    private void OnDisable()
    {
        tavernButtonTransform = null;
    }

    public void LoadTavern()
    {
        SceneManager.LoadScene("TavernScene");
    }
}
