using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GetVersion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp_version;

    private void Awake()
    {
        tmp_version.text = "v" + Application.version;
    }
}
