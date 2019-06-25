using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class WebGLBuilder
{
    static void build() {

    // Place all your scenes here
    string[] scenes = {"Assets/TitleScene.unity",
                        "Assets/TitleMenu.unity", 
                        "Assets/MainMenu.unity",
                        "Assets/newgame.unity",
                        "Assets/Cinematic.unity",
                        "Assets/CharacterSelect.unity",
                        "Assets/IntroScene.unity",
                        "Assets/Transition.unity",
                        "Assets/Credits.unity",
                        "Assets/TutorialDemo.unity"};

    string pathToDeploy = "/home/ibrahimmushtaq/Desktop/WebGLBUild/";       

    BuildPipeline.BuildPlayer(scenes, pathToDeploy, BuildTarget.WebGL, BuildOptions.None);      
    }
}
