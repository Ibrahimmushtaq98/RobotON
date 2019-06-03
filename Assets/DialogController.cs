﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Video; 
using UnityEngine.SceneManagement; 
using UnityEngine.Networking;
using System;
using System.IO; 
using System.Runtime.InteropServices;

public class DialogController : MonoBehaviour
{
    public GameObject girlDialog, boyDialog, botDialog; 
    List<string> lines; 
    VideoPlayer player; 
    List<string> actorOrder; 
    bool started = false; 
    int index = 0; 
    // Start is called before the first frame update

    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetData(string url);
    #endif

    IEnumerator GetVideoFile(string url){
        yield return null;
    }
    void Start()
    {
        string filepathON ="";
        string filepathBug = "";
        player = GameObject.Find("Video Player").GetComponent<VideoPlayer>(); 
        #if UNITY_WEBGL                    
            filepathON = "StreamingAssets/IntroScene.mp4";
            filepathBug = "StreamingAssets/RoboBugIntro.mp4";
            Debug.Log("OldMenu: Update() WEBGL AND WINDOW");

            if (GlobalState.GameMode == "bug"){
                player.url = stringLib.SERVER_URL + filepathBug;
                //player.clip = Resources.Load<VideoClip>(stringLib.SERVER_URL + filepathBug); 
                girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(150, 250, 0); 
                boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(-300, 250, 0); 
                botDialog.GetComponent<RectTransform>().localPosition = new Vector3(500,250,0); 
            }else{
                player.url = stringLib.SERVER_URL + filepathON;
                //player.clip = Resources.Load<VideoClip>(stringLib.SERVER_URL + filepathON);
            }
            
        #else
            if (GlobalState.GameMode == "bug"){
                player.clip = Resources.Load<VideoClip>("Video/RoboBugIntro"); 
                girlDialog.GetComponent<RectTransform>().localPosition = new Vector3(150, 250, 0); 
                boyDialog.GetComponent<RectTransform>().localPosition = new Vector3(-300, 250, 0); 
                botDialog.GetComponent<RectTransform>().localPosition = new Vector3(500,250,0); 
            }
        #endif
        ReadFile(); 
    }
    // Update is called once per frame
    void Update()
    {
        if (!player.isPlaying && Time.timeSinceLevelLoad > 1 && !started){
            StartCoroutine(ShowDialog(GetDialog(actorOrder[index]))); 
            started = true; 
        }
        else if (!player.isPlaying && Time.timeSinceLevelLoad > 1 && Input.GetKeyDown(KeyCode.Return)){
            NextDialog(); 
        }
        else if(player.isPlaying && Input.GetKeyDown(KeyCode.Return)){
            EndScene(); 
        }
    }
    void EndScene(){
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut(); 
        StartCoroutine(WaitForSwitchScene()); 
    }
    IEnumerator WaitForSwitchScene(){
        yield return new WaitForSeconds(1f); 
        SceneManager.LoadScene("CharacterSelect"); 
    }
    void NextDialog(){
        if (index +1 == lines.Count){
            EndScene(); 
            return; 
        }
        if (actorOrder[index] == actorOrder[index+1]){
            index++; 
            StartCoroutine(ShowDialog(GetDialog(actorOrder[index]))); 
        }else{
            StopAllCoroutines(); 
            StartCoroutine(HideDialog(GetDialog(actorOrder[index]))); 
            index++; 
            StartCoroutine(ShowDialog(GetDialog(actorOrder[index]))); 
        }
    }
    GameObject GetDialog(string name){
        if (name == "Boy") return boyDialog; 
        else if (name == "Girl") return girlDialog; 
        else return botDialog; 
    }

        string webdata;

    IEnumerator GetXMLFromServer(string url){
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SendWebRequest();
        System.Threading.Thread.Sleep(stringLib.DOWNLOAD_TIME);        
        if(www.isNetworkError || www.isHttpError){
            Debug.Log(www.error);
        }else{
            Debug.Log(www.downloadHandler.text);
            webdata = www.downloadHandler.text;
        }
        yield return new WaitForSeconds(0.5f);
    }
    void ReadFile(){
        string bug = ""; 
        if( GlobalState.GameMode == "bug") bug = "bug"; 
        string filepath = Path.Combine(Application.streamingAssetsPath, "onleveldata/Intro" + bug +".txt");
        actorOrder = new List<string>(); 
        lines = new List<string>(); 
        #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
            filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameObject+"leveldata/Intro.txt");
            
            using (StreamReader reader = new StreamReader(filepath)){
                while(!reader.EndOfStream){
                    string line = reader.ReadLine(); 
                    if (line.Contains("$Boy")){
                        actorOrder.Add("Boy"); 
                        line = line.Remove(0,line.IndexOf(':')); 
                    }
                    else if (line.Contains("$Girl")){
                        actorOrder.Add("Girl"); 
                    }
                    else{
                        actorOrder.Add("Robot"); 
                    }
                    line = line.Remove(0,line.IndexOf(':')+1); 
                    lines.Add(line); 
                }
            }
            Console.Writeline"DialogController: ReadFile() WINDOWS");

        #endif

        #if UNITY_WEBGL && !UNITY_EDITOR
            filepath = "StreamingAssets" + "/" + "onleveldata/Intro" + bug +".txt";
            webdata = GetData(stringLib.SERVER_URL + filepath);
            Debug.Log("DialogController: ReadFile() WEBGL");

        #elif UNITY_WEBGL && UNITY_EDITOR
            filepath = "StreamingAssets" + "/" + "onleveldata/Intro" + bug +".txt";
            StartCoroutine(GetXMLFromServer(stringLib.SERVER_URL + filepath));
            Debug.Log("DialogController: ReadFile() WEBGL AND WINDOWS");
        #endif

        #if UNITY_WEBGL
            String[] linesS = webdata.Split('\n');
            for(int i = 0; i < linesS.Length - 1; i++){
                String[] scriptLines = linesS[i].Split('\r');
                if(scriptLines[0].Contains("$Boy")){
                    actorOrder.Add("Boy");
                    scriptLines[0] = scriptLines[0].Remove(0, scriptLines[0].IndexOf(':'));
                }else if (scriptLines[0].Contains("$Girl")){
                    actorOrder.Add("Girl"); 
                }else{
                    actorOrder.Add("Robot"); 
                }
                scriptLines[0] = scriptLines[0].Remove(0,scriptLines[0].IndexOf(':')+1); 
                lines.Add(scriptLines[0]);
            }
            Debug.Log("DialogController: ReadFile() WEBGL");
        #endif
    }

    IEnumerator HideDialog(GameObject dialog){
        //Initialization
        RectTransform transformm = dialog.GetComponent<RectTransform>(); 
        Image image = dialog.GetComponent<Image>(); 
        CanvasGroup canvas = dialog.GetComponent<CanvasGroup>(); 
        
        transform.localScale = new Vector3(1,1,1); 
        //image.color = new Color(image.color.r, image.color.g, image.color.b, 1); 
        canvas.alpha = 1; 
        float frames = 10; 
        while(canvas.alpha > 0){
            canvas.alpha -= (1/frames); 
            transform.localScale = new Vector3(transform.localScale.x - (0.25f/frames), transform.localScale.y - (0.25f/frames), transform.localScale.z - (0.25f/frames)); 
            yield return null; 
        }
        dialog.transform.GetChild(0).GetComponent<Text>().text = ""; 
    }
    IEnumerator ShowDialog(GameObject dialog){
        //Initialization
        RectTransform transformm = dialog.GetComponent<RectTransform>(); 
        Image image = dialog.GetComponent<Image>(); 
        CanvasGroup canvas = dialog.GetComponent<CanvasGroup>(); 
        dialog.transform.GetChild(0).GetComponent<Text>().text = lines[index];  
        //Initialize values 
        transform.localScale = new Vector3(0,0,0); 
        canvas.alpha = 0;    //over push the animation
        float frames = 15; 

        while(transform.localScale.x < 1.2){
            transform.localScale = new Vector3(transform.localScale.x + (1/frames), transform.localScale.y + (1/frames), transform.localScale.z + (1/frames)); 
            if (canvas.alpha < 1)
                canvas.alpha+= (2/frames); 
            yield return null; 
        }

        //bring back 
        while(transform.localScale.x > 1){
            transform.localScale = new Vector3(transform.localScale.x - (1/frames), transform.localScale.y - (1/frames), transform.localScale.z - (1/frames)); 
            yield return null; 
        }

    }
}
