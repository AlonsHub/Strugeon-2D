using UnityEngine;
using UnityEngine.UI; 

public class TimeChanger : MonoBehaviour
{
    public float[] timeScales = new float[3];
    public Sprite[] buttonImgs = new Sprite[3];
    [SerializeField] int currentTimeScaleIndex;

    [SerializeField] Button timeToggleButton;
    
    public void ChangeTimeScale()
    {
        currentTimeScaleIndex++;

        if (currentTimeScaleIndex >= timeScales.Length)
        {
            currentTimeScaleIndex = 0;
        }

        Time.timeScale = timeScales[currentTimeScaleIndex];

        timeToggleButton.image.sprite = buttonImgs[currentTimeScaleIndex];
    }
}
