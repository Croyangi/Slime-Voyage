using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader_Global : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("Bootloader_Global", LoadSceneMode.Additive);
    }
}
