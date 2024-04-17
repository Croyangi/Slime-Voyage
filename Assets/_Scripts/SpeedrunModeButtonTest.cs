using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedrunModeButtonTest : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TextMeshProUGUI tm_speedrunModeState;
    [SerializeField] private bool isSpeedrunModeOn;

    public void OnToggleSpeedrunButtonClicked()
    {
        isSpeedrunModeOn = !isSpeedrunModeOn;
        ChangeSpeedrunModeText();
    }

    public void OnToggleResetDataClicked()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadScene("MainMenu");
    }

    private void ChangeSpeedrunModeText()
    {
        if (isSpeedrunModeOn == true)
        {
            tm_speedrunModeState.text = "On";
        }
        else
        {
            tm_speedrunModeState.text = "Off";
        }
    }

    public void LoadData(GameData data)
    {
        isSpeedrunModeOn = data.isSpeedrunModeOn;
        ChangeSpeedrunModeText();
    }

    public void SaveData(ref GameData data)
    {
        data.isSpeedrunModeOn = isSpeedrunModeOn;
    }
}
