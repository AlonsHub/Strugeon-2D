using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TEMP_RevealRingRadiusSetter : MonoBehaviour
{
    [SerializeField]
    TMP_InputField[] inputFields;
    [SerializeField]
    TMP_InputField purpleCheatInputField;


    private void OnEnable()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].text = GameStats.revealRingRadiusMods[i].ToString();
        }

        purpleCheatInputField.text = PlayerDataMaster.Instance.currentPlayerData.noolProfile.nools[(int)NoolColour.Purple].capacity.ToString();
    }

    public void SetSiteRevealRadiusRing()
    {
        float o;
        if(float.TryParse(inputFields[(int)RevealRingType.Site].text, out o))
        {
            GameStats.SetRevealRingRadius(RevealRingType.Site, o);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void SetEnemyAmountRevealRadiusRing()
    {
        float o;
        if (float.TryParse(inputFields[(int)RevealRingType.EnemyAmount].text, out o))
        {
            GameStats.SetRevealRingRadius(RevealRingType.EnemyAmount, o);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        }
    }
    public void SetEnemyIDRevealRadiusRing()
    {
        float o;
        if (float.TryParse(inputFields[(int)RevealRingType.EnemyID].text, out o))
        {
            GameStats.SetRevealRingRadius(RevealRingType.EnemyID, o);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        }
    }
    public void SetEnemyLevelRevealRadiusRing()
    {
        float o;
        if (float.TryParse(inputFields[(int)RevealRingType.EnemyLevel].text, out o))
        {
            GameStats.SetRevealRingRadius(RevealRingType.EnemyLevel, o);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        }
    }
    public void SetRewardRevealRadiusRing()
    {
        float o;
        if (float.TryParse(inputFields[(int)RevealRingType.Reward].text, out o))
        {
            GameStats.SetRevealRingRadius(RevealRingType.Reward, o);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        }
    }

    public void CheatSetPurpleCapacity()
    {
        PlayerDataMaster.Instance.currentPlayerData.noolProfile.nools[(int)NoolColour.Purple].capacity = float.Parse(purpleCheatInputField.text); //TBF use a method to add to nool cap and call any nool changed 
        //PlayerDataMaster.Instance.currentPlayerData.noolProfile.OnAnyValueChanged?.Invoke();

        //GameStats.OnRevealRadiusChanged?.Invoke();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

    }
}
