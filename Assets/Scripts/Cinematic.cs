
using System;
using System.Transactions;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Cinematic : MonoBehaviour
{
    LevelFactory factory;
    // This is the text that is displayed at the start of the level (during the "loading screen") prior to playing the level.
    public string introtext = "Level Start Placeholder!";
    // This text basically says "Press Enter to Continue" and is displayed at the bottom of the "Loading Screen" prior to playing the level.
    public string continuetext = "Continue Text Placeholder";
    // This is the text that is displayed at the end of the level (in the "Victory Screen") after playing the level.
    public string endtext = "Winner!\nLevel End Placeholder!";
    public GameObject prompt1, prompt2;
    public GameObject[] stars = new GameObject[5];

    private bool cinerun = false;
    private float delaytime = 0f;
    private float delay = 0.1f;

    int score; 
    bool shownCharacter = false; 


    //.................................>8.......................................
    // Use this for initialization
    void Start()
    {
        continuetext = stringLib.CONTINUE_TEXT;
        UpdateText();
        GameObject.Find("Fade").GetComponent<Fade>().onFadeIn();
        score = 5; 
        if (GlobalState.toolUse != null){
            for (int i = 0; i < GlobalState.level.Tasks.Length; i++){
                score -= GlobalState.toolUse[i] - GlobalState.level.Tasks[i]; 
            } 
            if (score <= 0) score = 1; 
        }
        GlobalState.TotalEnergy += 4*score; 
        if (!GlobalState.IsDark)
        {
            GameObject.Find("BackgroundCanvas").transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/circuit_board_light");
            transform.Find("PressEnter").GetComponent<Text>().color = Color.black; 
            transform.Find("Title").GetComponent<Text>().color = Color.black; 
        }
        foreach (GameObject star in stars){
            star.GetComponent<Image>().enabled = false; 
            star.GetComponent<Animator>().enabled = false; 
        }

        //Debug.Log(SceneManager.sceneCount);
    }
    IEnumerator ShowCharacter(){
        GameObject player = transform.Find(GlobalState.Character).gameObject; 
        player.GetComponent<Animator>().SetTrigger("isRunning"); 
        Image image = player.GetComponent<Image>(); 
        while(image.color.a < 1){
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.05f); 
            yield return null; 
        }
    }
    IEnumerator AnimateStars(){

        foreach (GameObject star in stars){
            star.GetComponent<Image>().enabled = true; 
            star.GetComponent<Animator>().enabled = true; 
            yield return new WaitForSecondsRealtime(0.2f); 
        }
        for (int i = 0; i < score; i++){
            stars[i].GetComponent<Animator>().SetBool("isComplete", true); 
            yield return new WaitForSecondsRealtime(0.1f); 
        }
        if (score == 0){
            float[] speeds = new float[]{0.3f, 0.5f, 1f, 0.5f, 0.3f}; 
            while(stars[0].transform.position.y > -13){
                for (int i = 0; i < stars.Length; i++){
                    stars[i].transform.position = new Vector3(stars[i].transform.position.x, stars[i].transform.position.y - speeds[i], stars[i].transform.position.z); 
                }
                yield return new WaitForSeconds(0.15f); 
            }
        }
    }
    IEnumerator FadeFailStars(){
        while(stars[score].GetComponent<Image>().color.a > 0){
            for (int i = score; i < stars.Length; i++){
                Image image = stars[i].GetComponent<Image>(); 
                image.color = new Color(image.color.r, image.color.b, image.color.g, image.color.a - 0.05f); 
            }
            yield return null; 
        }
    }
    IEnumerator FadeInResults(){
        CanvasGroup canvas = transform.Find("void main").gameObject.GetComponent<CanvasGroup>(); 
        while(canvas.alpha < 1){
            canvas.alpha += 0.05f; 
            yield return null; 
        }
    }
    IEnumerator PushResults(){
        StartCoroutine(FadeInResults()); 
        if (score < stars.Length) StartCoroutine(FadeFailStars()); 
        float[] speeds = new float[]{1.3f,1.1f, 0.9f, 0.7f, 0.5f}; 
        float[] xPositions = new float[]{-300,-200,-100,0,100};
        float[] xdifs = new float[score]; 
        for (int i = 0; i < xdifs.Length; i++){
            xdifs[i] = xPositions[i] - stars[i].GetComponent<RectTransform>().localPosition.x; 
        }
        float frames = 20f; 
        float scaleDif = -0.5f; 
        int framecount = 0; 
        while(stars[score-1].GetComponent<RectTransform>().localPosition.y > -130f){
            for (int i = 0; i < score; i++){
                RectTransform transform = stars[i].GetComponent<RectTransform>(); 
                if (transform.localScale.x > 0.5f){
                    transform.localScale = new Vector3(transform.localScale.x + scaleDif/frames, transform.localScale.y + scaleDif/frames, transform.localScale.z); 
                }
                if (Math.Abs(transform.localPosition.x - xPositions[i]) > 1){
                    transform.localPosition = new Vector3(transform.localPosition.x + xdifs[i]/frames, transform.localPosition.y, transform.localPosition.z); 
                }
                if (transform.localPosition.y > -130f && framecount > 10){
                    float ydif = -130f - transform.localPosition.y; 
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (ydif*speeds[i])/frames, transform.localPosition.z); 
                }
            }
            framecount++; 
            yield return null; 
        }
    }
    IEnumerator LoadGame()
    {
        GameObject.Find("Fade").GetComponent<Fade>().onFadeOut();
        yield return new WaitForSecondsRealtime(1f);
        string filepath; 
        if (GlobalState.GameMode == "on"){
            string file = "onleveldata/" + GlobalState.level.FileName.Remove(GlobalState.level.FileName.IndexOf('.')) + ".txt"; 
            filepath = Path.Combine(Application.streamingAssetsPath, file); 
        }
        else{
            string file =  GlobalState.level.FileName.Remove(GlobalState.level.FileName.IndexOf('.')) + ".txt"; 
            Debug.Log(file); 
            filepath = Path.Combine(Application.streamingAssetsPath, file); 
            
        }
        if (File.Exists(filepath)){
            SceneManager.LoadScene("Transition"); 
        }
        else SceneManager.LoadScene("newgame");

    }
    public void ToggleLight()
    {
        prompt1.GetComponent<Text>().color = Color.black;
        prompt2.GetComponent<Text>().color = Color.black;
    }
    public void ToggleDark()
    {
        prompt1.GetComponent<Text>().color = Color.white;
        prompt2.GetComponent<Text>().color = Color.white;
    }
    private void UpdateText()
    {
        if (GlobalState.level == null)
        {
            UpdateLevel();
        }
        introtext = GlobalState.level.IntroText;
        endtext = GlobalState.level.ExitText;
    }
    private void UpdateLevel()
    {
        //string filepath = Application.streamingAssetsPath +"/"+ GlobalState.GameMode + "leveldata/" + GlobalState.CurrentONLevel;
        //filepath = Path.Combine(filepath,  GlobalState.CurrentONLevel);

        string filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");
        if (GlobalState.Language == "python") filepath = Path.Combine(filepath, "python"); 
        filepath = Path.Combine(filepath, GlobalState.CurrentONLevel);
        Debug.Log(filepath); 
        factory = new LevelFactory(filepath);
        GlobalState.level = factory.GetLevel();
    }
    private void UpdateLevel(string file)
    {
        string[] temp = file.Split('\\');
        for (int i = 0; i < temp.Length; i++)
        {
            Debug.Log(temp[i]);
        }
        GlobalState.CurrentONLevel = temp[temp.Length - 1];
        factory = new LevelFactory(file);
        GlobalState.level = factory.GetLevel();
    }
    //.................................>8.......................................
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
            SceneManager.LoadScene("MainMenu");
        }
        if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_START)
        {
            if (!cinerun)
            {
                cinerun = true;
                if (GlobalState.GameMode != stringLib.GAME_MODE_ON)
                {
                }
                else
                {

                }
            }

            if(!shownCharacter){
                shownCharacter = true; 
                StartCoroutine(ShowCharacter()); 
            }
            
            prompt1.GetComponent<Text>().text = introtext;
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time)
            {
                GlobalState.GameState = stateLib.GAMESTATE_IN_GAME;
                cinerun = false;
                StartCoroutine(LoadGame());
            }
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_WIN)
        {
            if (!cinerun)
            {
                cinerun = true;
            }
            StartCoroutine(AnimateStars()); 
            prompt1.GetComponent<Text>().text = endtext;

            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time)
            {
                // RobotON 2, don't always want tutorials to run comics.
                // Read in the levels.txt and grab the top one.
                if (GlobalState.CurrentONLevel.StartsWith("tut") && GlobalState.GameMode == stringLib.GAME_MODE_BUG)
                {
                    // GlobalState.GameState = stateLib.GAMESTATE_STAGE_COMIC;
                }
                else
                {
                    GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                }
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                UpdateLevel(GlobalState.level.NextLevel);
                UpdateText();
                StartCoroutine(PushResults()); 
                //GameObject.Find("Main Camera").GetComponent<GameController>().SetLevel(GlobalState.level.NextLevel);
                cinerun = false;

            }
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_LEVEL_LOSE)
        {
            score = 0; 
            StartCoroutine(AnimateStars()); 
            if (!cinerun)
            {
                cinerun = true;
            }
            prompt1.GetComponent<Text>().text = stringLib.LOSE_TEXT;
            prompt2.GetComponent<Text>().text = stringLib.RETRY_TEXT;
            if (Input.GetKeyDown(KeyCode.Escape) && delaytime < Time.time)
            {
                prompt2.GetComponent<Text>().text = stringLib.CONTINUE_TEXT;

                cinerun = false;
                GlobalState.GameState = stateLib.GAMESTATE_MENU;
            }
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && delaytime < Time.time)
            {
                prompt2.GetComponent<Text>().text = stringLib.CONTINUE_TEXT;

                cinerun = false;
                // One is called Bugleveldata and another OnLevel data.
                // Levels.txt, coding in menu.cs
                
              string filepath = Path.Combine(Application.streamingAssetsPath, GlobalState.GameMode + "leveldata");  filepath = Path.Combine(filepath, GlobalState.CurrentONLevel);
                UpdateLevel(filepath);
                GlobalState.GameState = stateLib.GAMESTATE_LEVEL_START;
                //Debug.Log("LoadingScreen");
            }
        }
        else
        {
            delaytime = Time.time + delay;
        }
    }

    //.................................>8.......................................
}
