﻿using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement; 
using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// Controls the credits functionality. 
/// </summary>
public class CreditsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FileInfo fi = new FileInfo(Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata") + "/credits.txt");
        StreamReader sr = fi.OpenText();
        string text;
        string credtext = ""; 
        do
        {
            text = sr.ReadLine();
            credtext += text + "\n";
        } while (text != null);
        this.GetComponent<TextMesh>().text = credtext;
        this.GetComponent<Animator>().SetBool("Ended", true);
        if (!GlobalState.IsDark){
            GameObject.Find("BackgroundCanvas").transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/circuit_board_light"); 
            GameObject.Find("Credits").GetComponent<TextMesh>().color = Color.black; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetMouseButton(0))
        {
            GlobalState.GameState = 0;
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}
