using UnityEngine;
using UnityEngine.UI; 
//public enum TimeSpeed {Pause, Play, Fast};
public class TimeChanger : MonoBehaviour
{
    public static TimeChanger Instance;

    public Sprite[] speedButtonImgs;
    public Sprite[] pauseButtonImgs;
    [SerializeField]
    Image speedButton;
    [SerializeField]
    Image pauseButton;


    //[SerializeField]
    //int[] speeds;

    //TimeSpeed oldSpeed = TimeSpeed.Play; //should be enumed - 0 pause, 1 play, 2 doublespeed, 3 turbo
    //public TimeSpeed currentTimeSpeed; 

    [SerializeField] Button timeToggleButton;

    public float fastSpeed;

    public bool isFast = false;
    public bool isPause = false;

    private void Start()
    {
        if(Instance!= null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //currentTimeScaleIndex = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if (isPause)
            //    Time.timeScale = 1;
            //else
            //    Time.timeScale = 0;

            //isPause = !isPause;
            //pauseButton.sprite = pauseButtonImgs[(int)Time.timeScale];
            ToggleTimePause();
        }
    }

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
            pauseButton.sprite = pauseButtonImgs[0];

        }
        else
        {
            Time.timeScale = isFast ? fastSpeed : 1;
            pauseButton.sprite = pauseButtonImgs[1];
        }
    }
    public void ToggleTimePause(bool b)
    {
        isPause = b;
        if (isPause)
        {
            Time.timeScale = 0;
            pauseButton.sprite = pauseButtonImgs[0];

        }
        else
        {
            Time.timeScale = isFast ? fastSpeed : 1;
            pauseButton.sprite = pauseButtonImgs[1];

        }
    }

    //void _SetTimeSpeed(TimeSpeed timeSpeed) //INTERNAL method to set the time and it's buttons - used by all types of outside methods
    //{
    //    //pauseButton.sprite = pauseButtonImgs[(timeSpeed == 0) ? 0 : 1]; // BEAUTIFUL!!!

    //     //consider checking if the current and new speeds are not the same - in which case, saving-over the oldSpeed might make a "freak double-pause" become a "premanent pause"
    //    oldSpeed = currentTimeSpeed;
    //    isPause = (timeSpeed == 0);

    //    switch (timeSpeed)
    //    {
    //        case TimeSpeed.Pause:
    //            pauseButton.sprite = pauseButtonImgs[0];
    //            Time.timeScale = 0;
    //            break;

    //        case TimeSpeed.Play:
    //            pauseButton.sprite = pauseButtonImgs[1];
    //            Time.timeScale = speeds[(int)oldSpeed];
    //            break;

    //        case TimeSpeed.Fast:
    //            break;
    //        default:
    //            break;
    //    }

    //    currentTimeSpeed = timeSpeed;
    //}

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
