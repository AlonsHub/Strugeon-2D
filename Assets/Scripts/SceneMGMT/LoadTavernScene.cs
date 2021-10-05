using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTavernScene : MonoBehaviour
{
    public void LoadTavern()
    {
        SceneManager.LoadScene("TavernScene");
    }
}
