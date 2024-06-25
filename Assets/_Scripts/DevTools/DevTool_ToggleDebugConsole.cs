using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DevTool_ToggleDebugConsole : MonoBehaviour
{

    string myLog = "BEGIN LOG: ";
    //string filename = "";
    bool doShow = false;
    int kChars = 700;


    void OnEnable() 
    { 
        Application.logMessageReceived += Log; 
    }

    void OnDisable() 
    { 
        Application.logMessageReceived -= Log; 
    }

    public void ToggleDebugConsole()
    {
        doShow = !doShow;
    }

    private void Log(string logString, string stackTrace, LogType type)
    {
        // for onscreen...
        myLog = myLog + " " + logString;
        if (myLog.Length > kChars) { myLog = myLog.Substring(myLog.Length - kChars); }

        //// for the file ...
        //if (filename == "")
        //{
        //    string d = System.Environment.GetFolderPath(
        //    System.Environment.SpecialFolder.Desktop) + "/YOUR_LOGS";
        //    System.IO.Directory.CreateDirectory(d);
        //    string r = Random.Range(1000, 9999).ToString();
        //    filename = d + "/log-" + r + ".txt";
        //}
        //try
        //{
        //    System.IO.File.AppendAllText(filename, logString + "");
        //}
        //catch { }
    }

    void OnGUI()
    {
        if (!doShow) { return; }

        Vector3 pos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        GUI.matrix = Matrix4x4.TRS(pos, Quaternion.identity,
           new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
        GUI.TextArea(new Rect(10, 10, Screen.width / 2.8f, Screen.height / 2.8f), myLog);
    }
}
