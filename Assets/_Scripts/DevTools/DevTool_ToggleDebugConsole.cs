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
        myLog = myLog + "\n" + logString;
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

        // Calculate a Rect that fits a third of the screen and positions it in the top left
        float width = Screen.width / 4f;
        float height = Screen.height / 2f;
        float xPos = 10;  // 10px padding from the left edge
        float yPos = 10;  // 10px padding from the top edge

        // Create the TextArea with the calculated dimensions and position
        GUI.TextArea(new Rect(xPos, yPos, width, height), myLog);
    }



}
