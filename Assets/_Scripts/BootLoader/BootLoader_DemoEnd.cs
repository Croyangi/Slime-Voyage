using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader_DemoEnd : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("Bootloader_Gameplay", LoadSceneMode.Additive);
        SceneManager.LoadScene("Bootloader_Global", LoadSceneMode.Additive);
    }
}
