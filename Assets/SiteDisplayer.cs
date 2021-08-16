using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiteDisplayer : MonoBehaviour
{
    [SerializeField]
    Image difficultyImage;
    [SerializeField]
    GameObject dwellerPortraitPrefab;
    [SerializeField]
    Transform dwellerPortraitGroupRoot;
    [SerializeField]
    List<Image> dwellerPortraitImgs;

    LevelData levelData;

    [SerializeField]
    Sprite[] difficultySprties;

    public void SetMe(SiteButton sb)
    {
        levelData = sb.levelSO.levelData;

        for (int i = 0; i < levelData.enemies.Count; i++)
        {
            if (i >= dwellerPortraitImgs.Count)
            {
                Image img = Instantiate(dwellerPortraitPrefab, dwellerPortraitGroupRoot).GetComponent<Image>();
                if (!img)
                {
                    Debug.LogError("not UI image found on dwellerPortraitPrefab");
                }
                img.sprite = levelData.enemies[i].PortraitSprite; //consider keeping them somewhere
                dwellerPortraitImgs.Add(img);
            }
            else
            {
                dwellerPortraitImgs[i].sprite = levelData.enemies[i].PortraitSprite;
            }
        }
        if (levelData.enemies.Count < dwellerPortraitImgs.Count)
        {
            int delta = dwellerPortraitImgs.Count - levelData.enemies.Count;
            for (int i = 1; i <= delta; i++)
            {
                GameObject go = dwellerPortraitImgs[dwellerPortraitImgs.Count - 1].gameObject;
                dwellerPortraitImgs.RemoveAt(dwellerPortraitImgs.Count - 1);
                Destroy(go);
            }
        }
        difficultyImage.sprite = difficultySprties[(int)levelData.difficulty];
    }

    //public void UnSetMe()
    //{
    //    levelData = new LevelData(); //empty
    //    dwellerPortraitGroupRoot.ch
    //}
}
