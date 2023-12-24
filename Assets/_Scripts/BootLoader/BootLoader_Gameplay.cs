using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader_Gameplay : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("Bootloader_Gameplay", LoadSceneMode.Additive);
    }
}
