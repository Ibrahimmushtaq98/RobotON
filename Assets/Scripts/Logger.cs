using System;
//**************************************************//
// Class Name: Logger
// Class Description: Class which stores log data on the filesystem. Anonymous collection of this data
//                    is handled in a different class.
// Methods:
// 		void Start()
//		void Update()
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class Logger
{

    string id;
    bool failed;

    int timeStart, timeEnd, totalTime;
    DateTime time;
	string[] linesUsed = new string[stateLib.NUMBER_OF_TOOLS]; 
	bool hasWritten = false; 

    public string jsonObj = "";

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "*");

        yield return request.SendWebRequest();

        Debug.Log("Status Code: " + request.responseCode);
    }
    public Logger()
    {
        GlobalState.toolUse = new int[stateLib.NUMBER_OF_TOOLS];
        timeStart = DateTime.Now.Second;
        failed = false;
		for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
			linesUsed[i] = ""; 
    }
    public void onGameEnd()
    {
		if(hasWritten){
			return; 
		}
        timeEnd = DateTime.Now.Second;
        totalTime = timeEnd - timeStart;
        if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_LOSE)
        {
            failed = true;
        }
		hasWritten = true; 
        WriteLog();
    }
    public void onToolUse(int index, int lineNumber)
    {
        GlobalState.toolUse[index]++;
		linesUsed[index] += lineNumber.ToString() + ' '; 
    }
    public void WriteLog()
    {
        #if UNITY_WEBGL
            jsonObj = "{ \"levels\":[{ \"name\": \"" + GlobalState.CurrentONLevel + "\" ";
            jsonObj += ", \"time\": \"" + totalTime.ToString() + "\" ";
            jsonObj += ", \"progress\": \"";   

        if(!failed){
            jsonObj += "Passed\", \"tools\":[";
        }else{
            jsonObj += "Failed\", \"tools\":[";
        }

        for(int i = 0; i < GlobalState.level.Tasks.Length; i++){
            if(GlobalState.level.Tasks[i] > 0){
                if(GlobalState.GameMode == "on"){
                    jsonObj+= "{ \"name\": \"" + GlobalState.StringLib.namesON[i] + "\",";
                }else{
                    jsonObj+= "{ \"name\": \"" + GlobalState.StringLib.namesBug[i] + "\",";
                }

                jsonObj += "\"reqTask\": \"" + GlobalState.level.Tasks[i] + "\",";
                jsonObj += "\"compTask\": \"" + GlobalState.level.CompletedTasks[i] + "\",";
                jsonObj += "\"timeTool\": \"" + GlobalState.toolUse[i] + "\",";
                jsonObj += "\"lineUsed\": \"" + linesUsed[i] + "\"}";

                if(!(GlobalState.level.Tasks.Length - 1 == i)){
                    jsonObj += ", ";
                }
            }
        }

        jsonObj = jsonObj.Substring(0,jsonObj.Length-2);
        jsonObj +="]}]}";
        Debug.Log(jsonObj);
        //Upload(url, jsonObj);
        #endif

        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
        using (StreamWriter sw = File.AppendText(Application.dataPath.Replace("/Assets","") + "/Logging/" + id + GlobalState.CurrentONLevel.Replace(".xml", "") + ".txt"))
        {
            if (!failed) sw.WriteLine("Passed Level");
            else sw.WriteLine("Failed Level");
            sw.WriteLine(totalTime.ToString() + " Seconds");
            for (int i = 0; i < GlobalState.level.Tasks.Length; i++)
            {
                if (GlobalState.level.Tasks[i] > 0)
                {
                    if (GlobalState.GameMode == "on")
                        sw.WriteLine(GlobalState.StringLib.namesON[i] + " Tool Use: ");
                    else
                        sw.WriteLine(GlobalState.StringLib.namesBug[i] + " Tool Use: ");

                    sw.WriteLine("\tRequired Tasks: " + GlobalState.level.Tasks[i]);
                    sw.WriteLine("\tCompleted Tasks: " + GlobalState.level.CompletedTasks[i]);
                    sw.WriteLine("\tTimes Tool Used: " + GlobalState.toolUse[i]);
					sw.WriteLine("\tLines Used: " + linesUsed[i]); 
                    sw.WriteLine();
                }
            }
            sw.WriteLine("--------------------------------------------------------");
            sw.Close();
        }
        #endif
    }

}
