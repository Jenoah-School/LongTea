using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsToText : MonoBehaviour
{
    private enum PlayerPrefsType { playerPrefsString, playerPrefsInt, playerPrefsFloat };

    [SerializeField] private TMPro.TextMeshProUGUI textField;
    [SerializeField] private string playerPrefsValue = "";
    [SerializeField] private PlayerPrefsType playerPrefsType = PlayerPrefsType.playerPrefsString;
    // Start is called before the first frame update
    void Start()
    {
        ReloadText();
    }


    public void ReloadText()
    {
        switch (playerPrefsType)
        {
            case PlayerPrefsType.playerPrefsString:
                textField.text = PlayerPrefs.GetString(playerPrefsValue, "");
                break;
            case PlayerPrefsType.playerPrefsInt:
                textField.text = PlayerPrefs.GetInt(playerPrefsValue, 000000).ToString();
                break;
            case PlayerPrefsType.playerPrefsFloat:
                textField.text = PlayerPrefs.GetFloat(playerPrefsValue, 000000).ToString();
                break;
            default:
                textField.text = "";
                break;
        }
    }

    public void SetRewardText()
    {
        textField.text = PlayerPrefs.GetInt("TotalCollectedMinerals", 0).ToString("00") + " / " + PlayerPrefs.GetInt("TotalMinerals", 0).ToString("00");
    }
}
