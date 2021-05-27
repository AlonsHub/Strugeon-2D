using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject bars;
    public Button settings;
    public GameObject settingsPanel;

    private void Start()
    {
        bars.SetActive(true);
        settings.gameObject.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SettingsButton()
    {
        bars.SetActive(false);
        settings.gameObject.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackButton()
    {
        bars.SetActive(true);
        settings.gameObject.SetActive(true);
        settingsPanel.SetActive(false);
    }
}