using UnityEngine;

public class TimeChanger : MonoBehaviour
{
    public float timeScale;

    public void ChangeTimeScale()
    {
        Time.timeScale = timeScale;
    }
}
