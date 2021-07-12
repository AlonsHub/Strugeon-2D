using UnityEngine;
using UnityEngine.UI; 

public class TimeChanger : MonoBehaviour
{
    public Sprite[] speedButtonImgs;
    public Sprite[] pauseButtonImgs;
    [SerializeField]
    Image speedButton;
    [SerializeField]
    Image pauseButton;

    [SerializeField] int currentTimeScaleIndex;

    [SerializeField] Button timeToggleButton;

    public float fastSpeed;

    public bool isFast = false;
    public bool isPause = false;


    
    public void ToggleTimeSpeed()
    {
        isFast = !isFast;

        if (!isPause)
        {
            if (isFast)
            {
                Time.timeScale = fastSpeed;
                speedButton.sprite = speedButtonImgs[1];
            }
            else
            {
                Time.timeScale = 1;
                speedButton.sprite = speedButtonImgs[0];

            }
        }
        
    }
    public void ToggleTimePause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;
            pauseButton.sprite = pauseButtonImgs[1];

        }
        else
        {
            Time.timeScale = 1;
            pauseButton.sprite = pauseButtonImgs[0];

        }
    }

    //public void ChangeTimeScale()
    //{
    //    currentTimeScaleIndex++;

    //    if (currentTimeScaleIndex >= timeScales.Length)
    //    {
    //        currentTimeScaleIndex = 0;
    //    }

    //    Time.timeScale = timeScales[currentTimeScaleIndex];

    //    timeToggleButton.image.sprite = buttonImgs[currentTimeScaleIndex];
    //}
}
