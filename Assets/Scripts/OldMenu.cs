﻿//**************************************************//
// Class Name: Menu
// Class Description: This is the class for the menu, used for both games. This class gets called first.
// Methods:
// 		void Start()
//		void Update()
//		public void saveGame(string currentlevel)
//		void m2switch(bool on)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System; 
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Analytics;

public class OldMenu : MonoBehaviour
{
    public bool gameon = false;
    public string filepath;
    public List<string> levels;
    public List<string> passed;
    public GameObject background;
    public GameObject cinematic;
    public GameObject[] buttons = new GameObject[5];
    public GameObject[] buttontext = new GameObject[5];
    public GameObject[] m2buttons = new GameObject[3];
    public GameObject[] m2buttontext = new GameObject[3];
    public GameObject[] m2arrows = new GameObject[2];
    public GameObject menu2;
    public Sprite bluebutton;
    public Sprite greenbutton;

    int optionPage = 0;
    private bool soundon = true;
    private float delaytime = 0f;
    private float delay = 0.1f;
    private int option = 0;
    private int levoption = 0;
    private string lfile;
    private StreamReader sr;
    private string windowsFilepath = @"\";
    private string unixFilepath = @"/";

    int selectedIndex = -1;
    int textOption = GlobalState.TextSize;
    string[] textsizes;
    int[] fontSizes;
    bool entered = false;
    bool isDONE = false;
    string webdata;

    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetData(string url);
    #endif


    IEnumerator GetXMLFromServer(string url) {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SendWebRequest();
        System.Threading.Thread.Sleep(stringLib.DOWNLOAD_TIME);        
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }else {
            //Debug.Log(www.downloadHandler.text);
            webdata = www.downloadHandler.text;
        }
        yield return new WaitForSeconds(0.5f);
    }

[System.Serializable]
    public class JsonObjectNewUser{
        public string name;
        public JsonObjectNewUser(string name){
            this.name = name;
        }
    }

    // IEnumerator PostToDB(string url, JsonObjectNewUser jsonData) {
    //     string tmp = JsonUtility.ToJson(jsonData);
    //     Debug.Log(tmp);
    //     using (UnityWebRequest www = UnityWebRequest.Post(url, tmp))
    //     {
    //         www.method = UnityWebRequest.kHttpVerbPOST;
    //         www.SetRequestHeader("Content-Type", "application/json");
    //         //www.SetRequestHeader("Accept", "application/json");

    //         yield return www.SendWebRequest();

    //         if (www.isNetworkError || www.isHttpError)
    //         {
    //             Debug.Log(www.error);
    //         }
    //         else
    //         {
    //             Debug.Log("Form upload complete!");
    //         }
    //     }
    // }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "*");

        yield return request.SendWebRequest();

        //Debug.Log("Status Code: " + request.responseCode);
    }



    IEnumerator LoadWEB(string url) {
        yield return StartCoroutine(GetXMLFromServer(url));
    }
    //.................................>8.......................................
    // Use this for initialization
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        if (!GlobalState.IsResume)
        {
            InitializeGlobals();
            GameObject.Find("Fade").GetComponent<Fade>().onFadeIn();
        }
        buttontext[stateLib.GAMEMENU_NEW_GAME].GetComponent<TextMesh>().text = "New Game";
        buttontext[stateLib.GAMEMENU_LOAD_GAME].GetComponent<TextMesh>().text = "Load Game";
        buttontext[stateLib.GAMEMENU_SOUND_OPTIONS].GetComponent<TextMesh>().text = "Options";
        buttontext[stateLib.GAMEMENU_EXIT_GAME].GetComponent<TextMesh>().text = "Exit Game";
        buttontext[stateLib.GAMEMENU_RESUME_GAME].GetComponent<TextMesh>().text = "Resume Game";
        buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<SpriteRenderer>().color = Color.grey;
        textsizes = new string[] { "Small", "Normal", "Large", "Large++" };
        fontSizes = new int[] { stateLib.TEXT_SIZE_SMALL, stateLib.TEXT_SIZE_NORMAL, stateLib.TEXT_SIZE_LARGE, stateLib.TEXT_SIZE_VERY_LARGE };
        m2switch(false);
        GlobalState.IsDark = !GlobalState.IsDark;
        ToggleTheme();
        GlobalState.sessionID = AnalyticsSessionInfo.sessionId;
        filepath = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) ? windowsFilepath : unixFilepath;
        string json = "{ \"name\": \"" + GlobalState.sessionID.ToString()+"\"}";
        StartCoroutine(Post(stringLib.DB_URL, json));

    }

    public void onClick(int index)
    {
        if (GlobalState.IsResume && index == 4)
            return;
        if (index < -1)
        {
            if (index == -2)
            {
                if (levoption < levels.Count - 1 && passed[levoption] == "1")
                {
                    levoption++;
                }
                m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
            }
            else if (index == -3)
            {
                levoption = (levoption == 0) ? 0 : levoption - 1;
                m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
            }
            return;
        }
        selectedIndex = index;
        buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
        option = selectedIndex;
        entered = true;
    }
    private void InitializeGlobals()
    {
        GlobalState.IsPlaying = false;
        if (GlobalState.CurrentONLevel == null)
        {
            GlobalState.CurrentONLevel = "level0.xml";
        }
        else GlobalState.IsPlaying = true;
        GlobalState.CurrentBUGLevel = "level0.xml";
        GlobalState.GameState = stateLib.GAMEMENU_NEW_GAME;
        GlobalState.level = null;
        GlobalState.Character = "Robot";
        GlobalState.StringLib = new stringLib();
    }
    private void ToggleTheme()
    {
        GlobalState.IsDark = !GlobalState.IsDark;

        if (GlobalState.IsDark)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/panel-2");
            menu2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/panel-4");
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/circuit_board_dark");
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MenuPrefabs/LogoDark");
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/panel-9");
            menu2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/panel-8");
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/circuit_board_light");
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MenuPrefabs/LogoLight");
        }

    }
    //.................................>8.......................................
    // Update is called once per frame
    void Update()
    {

        // Handle "Resume Game" button behavior. If we have a game session we can click it, otherwise grey it out. --[
        if (!GlobalState.IsResume)
        {
            buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<SpriteRenderer>().color = Color.grey;
        }
        else
        {
            buttons[stateLib.GAMEMENU_RESUME_GAME].GetComponent<SpriteRenderer>().color = Color.white;
        }
        // ]-- End of "Resume Game" button behavior.
        // If we are in the menu, handle up and down arrows --[
        if (GlobalState.GameState == stateLib.GAMESTATE_MENU)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // The previous button should be made blue (change from green to blue).
                // If we are on the first option (New Game), don't allow the up arrow to wrap-around.
                buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = (option == stateLib.GAMEMENU_NEW_GAME) ? stateLib.GAMEMENU_NEW_GAME : option - 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // The previous button should be made blue (change from green to blue).
                // The last option will be either Resume Game or Exit game. In either case, don't allow the down arrow to wrap-around.
                buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                if (GlobalState.IsResume)
                {
                    option = (option == stateLib.GAMEMENU_RESUME_GAME) ? stateLib.GAMEMENU_RESUME_GAME : option + 1;
                }
                else
                {
                    option = (option == stateLib.GAMEMENU_EXIT_GAME) ? stateLib.GAMEMENU_EXIT_GAME : option + 1;
                }
            }

            // ]-- End of Arrow controller.

            // Make the current button appear green. The previous buttons change to blue
            // because there is an Up or Down arrow event. This will fire outside of the event.
            buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;

            // When we press Return (Enter Key), take us to the sub-menus
            if ((entered || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time)
            {
                entered = false;
                switch (option)
                {
                    case stateLib.GAMEMENU_NEW_GAME:
                        // Select between RobotON or RoboBUG.
                        GlobalState.GameState = -3;
                        buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                        option = 0;
                        m2switch(true);
                        m2buttontext[0].GetComponent<TextMesh>().text = stringLib.GAME_ROBOT_ON;
                        m2buttontext[1].GetComponent<TextMesh>().text = stringLib.GAME_ROBOT_BUG;
                        break;
                    case stateLib.GAMEMENU_LOAD_GAME:
                        // Load a level from RobotON or RoboBUG.
                        GlobalState.GameState = -4;
                        buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                        option = 0;
                        m2switch(true);
                        m2buttontext[0].GetComponent<TextMesh>().text = stringLib.GAME_ROBOT_ON;
                        m2buttontext[1].GetComponent<TextMesh>().text = stringLib.GAME_ROBOT_BUG;
                        break;
                    case stateLib.GAMEMENU_SOUND_OPTIONS:
                        GlobalState.GameState = -2;
                        buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                        option = 0;
                        m2switch(true);
                        if (optionPage == 0)
                        {
                            m2buttontext[0].GetComponent<TextMesh>().text = "Sound: " + (soundon ? GlobalState.StringLib.menu_sound_on_color_tag + "ON" + stringLib.CLOSE_COLOR_TAG : GlobalState.StringLib.menu_sound_off_color_tag + "OFF" + stringLib.CLOSE_COLOR_TAG);
                            m2buttontext[1].GetComponent<TextMesh>().text = (!GlobalState.IsDark) ? "Light Mode" : "Dark Mode";
                            m2buttontext[2].GetComponent<TextMesh>().text = "Next";
                            m2buttontext[3].GetComponent<TextMesh>().text = "Back";
                        }
                        else if (optionPage == 1)
                        {
                            m2buttontext[0].GetComponent<TextMesh>().text = textsizes[textOption];
                            m2buttontext[0].GetComponent<TextMesh>().fontSize = fontSizes[textOption];
                            m2buttontext[1].GetComponent<TextMesh>().text = (GlobalState.Language == "c++") ? "C++" : "Python";
                            m2buttontext[2].GetComponent<TextMesh>().text = "Previous";
                            m2buttontext[3].GetComponent<TextMesh>().text = "Back";
                        }
                        break;
                    case stateLib.GAMEMENU_EXIT_GAME:
                        postToDatabase.Start();
                        Application.Quit();
                        break;
                    case stateLib.GAMEMENU_RESUME_GAME:
                        GlobalState.GameState = stateLib.GAMESTATE_IN_GAME;
                        buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                        GlobalState.IsResume = false;
                        SceneManager.UnloadSceneAsync("MainMenu");
                        break;
                    default:
                        break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && GlobalState.GameState < 0)
        {
            m2switch(false);
            flushButtonColor();
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_MENU_LOADGAME_SUBMENU)
        {
            if (levoption < levels.Count - 1 && passed[levoption] == "1")
            {
                m2arrows[1].GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                m2arrows[1].GetComponent<SpriteRenderer>().enabled = false;
            }
            if (levoption != 0)
            {
                m2arrows[0].GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                m2arrows[0].GetComponent<SpriteRenderer>().enabled = false;
            }
            m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 0;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (levoption < levels.Count - 1 && passed[levoption] == "1")
                {
                    levoption++;
                }
                m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                levoption = (levoption == 0) ? 0 : levoption - 1;
                m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
            }
            if ((entered || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                entered = false;
                switch (option)
                {
                    case 0:
                        GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                        GlobalState.CurrentONLevel = levels[levoption];
                        GlobalState.IsResume = false;
                        SceneManager.LoadScene("CharacterSelect");

                        buttons[4].GetComponent<SpriteRenderer>().color = Color.white;
                        m2switch(false);
                        break;
                    case 1:
                        m2switch(false);
                        flushButtonColor();
                        GlobalState.GameState = stateLib.GAMESTATE_MENU;
                        break;
                    default:
                        break;
                }
            }

        }

        //
        else if (GlobalState.GameState == stateLib.GAMESTATE_MENU_SOUNDOPTIONS)
        {
            m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m2buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = (option - 1 < 0) ? option = 3 : option - 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m2buttons[option].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = (option + 1) % 4;
            }
            if ((entered || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                entered = false;
                switch (option)
                {
                    case 0:
                        if (optionPage > 0)
                        {
                            textOption = (textOption + 1) % textsizes.Length;
                            m2buttontext[0].GetComponent<TextMesh>().text = textsizes[textOption];
                            m2buttontext[0].GetComponent<TextMesh>().fontSize = fontSizes[textOption];
                            GlobalState.TextSize = textOption;
                            break;
                        }
                        soundon = !soundon;
                        m2buttontext[0].GetComponent<TextMesh>().text = "Sound: " + ((soundon) ? GlobalState.StringLib.menu_sound_on_color_tag + "ON" + stringLib.CLOSE_COLOR_TAG : GlobalState.StringLib.menu_sound_off_color_tag + "OFF" + stringLib.CLOSE_COLOR_TAG);
                        AudioListener.volume = (soundon) ? 1 : 0;
                        break;
                    case 1:
                        if (optionPage > 0)
                        {
                            m2buttontext[1].GetComponent<TextMesh>().text = (GlobalState.Language == "c++") ? "Python" : "C++";
                            GlobalState.Language = (GlobalState.Language == "c++") ? "python" : "c++";
                            break;
                        }
                        ToggleTheme();
                        m2buttontext[1].GetComponent<TextMesh>().text = (!GlobalState.IsDark) ? "Light Mode" : "Dark Mode";
                        break;
                    case 2:
                        if (optionPage == 0) optionPage++;
                        else optionPage = 0;
                        if (optionPage == 0)
                        {
                            m2buttontext[0].GetComponent<TextMesh>().text = "Sound: " + (soundon ? GlobalState.StringLib.menu_sound_on_color_tag + "ON" + stringLib.CLOSE_COLOR_TAG : GlobalState.StringLib.menu_sound_off_color_tag + "OFF" + stringLib.CLOSE_COLOR_TAG);
                            m2buttontext[1].GetComponent<TextMesh>().text = (!GlobalState.IsDark) ? "Light Mode" : "Dark Mode";
                            m2buttontext[2].GetComponent<TextMesh>().text = "Next";
                            m2buttontext[3].GetComponent<TextMesh>().text = "Back";
                        }
                        else if (optionPage == 1)
                        {
                            m2buttontext[0].GetComponent<TextMesh>().text = textsizes[textOption];
                            m2buttontext[0].GetComponent<TextMesh>().fontSize = fontSizes[textOption];
                            m2buttontext[1].GetComponent<TextMesh>().text = (GlobalState.Language == "c++") ? "C++" : "Python";
                            m2buttontext[2].GetComponent<TextMesh>().text = "Previous";
                            m2buttontext[3].GetComponent<TextMesh>().text = "Back";
                        }
                        break;
                    case 3:
                        GlobalState.GameState = stateLib.GAMESTATE_MENU;
                        m2buttons[3].GetComponent<SpriteRenderer>().sprite = bluebutton;
                        m2switch(false);
                        option = 2;
                        break;
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (option == 2)
                {
                    textOption = (textOption + 1) % textsizes.Length;
                    m2buttontext[2].GetComponent<TextMesh>().text = textsizes[textOption];
                    GlobalState.TextSize = textOption;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (option == 2)
                {
                    textOption = (textOption - 1 < 0) ? textOption = textsizes.Length - 1 : textOption - 1;
                    m2buttontext[2].GetComponent<TextMesh>().text = textsizes[textOption];
                    GlobalState.TextSize = textOption;
                }
            }
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_MENU_NEWGAME)
        {
            m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 0;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 1;
            }
            if ((entered || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                entered = false;
                switch (option)
                {
                    case 0:
                        InitializeGlobals();
                        GlobalState.GameMode = stringLib.GAME_MODE_ON;
                        GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                        GlobalState.IsResume = false;

                        SceneManager.LoadScene("IntroScene");

                        break;
                    case 1:
                        InitializeGlobals();
                        GlobalState.GameMode = stringLib.GAME_MODE_BUG;
                        GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                        GlobalState.IsResume = false;
                        GlobalState.CurrentONLevel = "tut1.xml";
                        SceneManager.LoadScene("IntroScene");
                        break;
                }
                m2switch(false);
                gameon = true;
                buttons[4].GetComponent<SpriteRenderer>().color = Color.white;

            }
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_MENU_LOADGAME)
        {
            m2buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 0;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
                option = 1;
            }
            if ((entered || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                entered = false;
                switch (option)
                {
                    case 0:
                        GlobalState.GameMode = stringLib.GAME_MODE_ON;
                        break;
                    case 1:
                        GlobalState.GameMode = stringLib.GAME_MODE_BUG;
                        break;
                }

                levels.Clear();
                passed.Clear();
                //lfile = Application.streamingAssetsPath +"/" + GlobalState.GameMode + "leveldata" + filepath + "levels.txt";
                string filepath ="";
                #if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
                    filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");
                    filepath = Path.Combine(filepath, "levels.txt");
                    Debug.Log("OldMenu: Update() WINDOWS");

                    sr = File.OpenText(filepath);
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] data = line.Split(' ');
                        levels.Add(data[0]);
                        passed.Add(data[1]);
                    }
                    sr.Close();
                #endif

                #if UNITY_WEBGL && !UNITY_EDITOR
                    filepath = "StreamingAssets" + "/" + GlobalState.GameMode + "leveldata" + "/levels.txt";
                    Console.WriteLine(stringLib.SERVER_URL + filepath);
                    webdata = GetData(stringLib.SERVER_URL + filepath);
                #elif UNITY_WEBGL  
                    filepath = "StreamingAssets" + "/" + GlobalState.GameMode + "leveldata" + "/levels.txt";
                    StartCoroutine(GetXMLFromServer(stringLib.SERVER_URL + filepath));
                    Console.WriteLine(stringLib.SERVER_URL + filepath);
                #endif

                #if UNITY_WEBGL                    
                    filepath = webdata;
                    string[] leveldata = filepath.Split('\n');
                    for (int i = 0; i < leveldata.Length - 1; i++) {
                        string[] tmp = leveldata[i].Split(' ');
                        string[] tmpTwo = tmp[1].Split('\r');
                        levels.Add(tmp[0]);
                        passed.Add(tmpTwo[0]);
                    }
                    Debug.Log("OldMenu: Update() WEBGL AND WINDOW");
                #endif

                GlobalState.GameState = -1;
                option = 0;
                m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
                m2buttontext[0].GetComponent<TextMesh>().text = levels[levoption];
                m2buttontext[1].GetComponent<TextMesh>().text = "Back";           
            }
        }
        else
        {
            delaytime = Time.time + delay;
        }
    }


    //.................................>8.......................................
    //************************************************************************//
    // Method: private void m2switch(bool on)
    // Description: Sub-menu switch (menu 2 switch). If bool is TRUE, show the Sub-menu
    // and all the buttons for that sub-menu. If the bool is FALSE, hide the
    // sub-menu, the buttons, text, arrows, etc.
    //************************************************************************//
    private void m2switch(bool on)
    {
        if (on)
        {
            menu2.GetComponent<SpriteRenderer>().enabled = true;
            for (int i = 0; i < m2buttons.Length; i++)
            {
                if (i > 1 && GlobalState.GameState != stateLib.GAMESTATE_MENU_SOUNDOPTIONS)
                    break;
                m2buttons[i].GetComponent<SpriteRenderer>().enabled = true;
            }
            if (GlobalState.GameState == stateLib.GAMESTATE_MENU_SOUNDOPTIONS)
            {
                Transform trans = menu2.GetComponent<Transform>();
                trans.position = new Vector3(trans.position.x, -0.9f, trans.position.z);
                trans.localScale = new Vector3(trans.localScale.x, 1.2f, trans.localScale.z);
                Transform button = m2buttons[0].GetComponent<Transform>();
                button.position = new Vector3(button.position.x, 0.42f, button.position.z);
                for (int i = 0; i < m2buttons.Length; i++)
                {
                    Transform thisButton = m2buttons[i].GetComponent<Transform>();
                    thisButton.position = new Vector3(thisButton.position.x, thisButton.position.y + 0.7f, thisButton.position.z);
                }
            }
            else
            {
                Transform trans = menu2.GetComponent<Transform>();
                trans.position = new Vector3(trans.position.x, 0, trans.position.z);
                trans.localScale = new Vector3(trans.localScale.x, 0.8f, trans.localScale.z);
                Transform button = m2buttons[0].GetComponent<Transform>();
                button.position = new Vector3(button.position.x, 0.82f, button.position.z);
                button = m2buttons[1].GetComponent<Transform>();
                button.position = new Vector3(button.position.x, -0.945f, button.position.z);
            }
        }
        else
        {
            menu2.GetComponent<SpriteRenderer>().enabled = false;
            foreach (GameObject button in m2buttons)
            {
                button.GetComponent<SpriteRenderer>().enabled = false;
            }
            foreach (GameObject btext in m2buttontext)
            {
                btext.GetComponent<TextMesh>().text = "";
            }
            foreach (GameObject arrow in m2arrows)
            {
                arrow.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    //.................................>8.......................................
    //************************************************************************//
    // Method: public void flushButtonColor()
    // Description: Change all menu and sub-menu buttons to blue. Set the cursor
    // to the New Game option. Basically, reset the menu.
    //************************************************************************//
    public void flushButtonColor()
    {
        m2buttons[0].GetComponent<SpriteRenderer>().sprite = bluebutton;
        m2buttons[1].GetComponent<SpriteRenderer>().sprite = bluebutton;
        option = 0;
        buttons[option].GetComponent<SpriteRenderer>().sprite = greenbutton;
    }
}
