using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader_Game : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {
        if (data.hasPlayedGameBefore)
        {
            SceneManager.LoadScene("MainMenu");
        } else
        {
            SceneManager.LoadScene("OminousEmployerIntro");
        }
    }

    public void SaveData(ref GameData data)
    {
    }
}
