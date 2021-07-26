using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public enum LevelEnum {Forest, Runis, Swamp};
public class LevelRef : MonoBehaviour
{
    public static LevelRef Instance;


    public LevelSO currentLevel;
    public List<LevelSO> levelSOs;

    //public SiteButton siteToCooldown;
    public string siteName; //WORK AROUND!

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCurrentLevel(int levelIndex)
    {
        if (levelSOs[levelIndex])
            currentLevel = levelSOs[levelIndex];
        else
            Debug.LogError("now level so in " + levelIndex + " " + (LevelEnum)levelIndex);
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    switch (level)
    //    {
    //        case 1:
    //            Instantiate(currentLevel.levelData.levelPrefab);
    //            break;
    //        case 0:
    //            SiteButton[] siteButtons = FindObjectsOfType<SiteButton>();
    //            SiteButton sb = siteButtons.Where(x => x.name == siteName).Single();
    //            sb.StartCooldownCaller();
    //            break;

    //        default:
    //            break;
    //    }
    //}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 2:
                Instantiate(currentLevel.levelData.levelPrefab);
                break;

            case 1:
                SiteButton[] siteButtons = FindObjectsOfType<SiteButton>();
                SiteButton sb = siteButtons.Where(x => x.name == siteName).Single();
                sb.StartCooldownCaller();
                break;

            default:
                break;
        }
    }
}
