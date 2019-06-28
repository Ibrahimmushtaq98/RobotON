using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class WebGLBuilder
{
    static void build() {

        BuildPlayerOptions buildPlayer = new BuildPlayerOptions();
        buildPlayer.scenes = new [] {"Assets/TitleScene.unity",
                            "Assets/TitleMenu.unity", 
                            "Assets/MainMenu.unity",
                            "Assets/newgame.unity",
                            "Assets/Cinematic.unity",
                            "Assets/CharacterSelect.unity",
                            "Assets/IntroScene.unity",
                            "Assets/Transition.unity",
                            "Assets/Credits.unity",
                            "Assets/TutorialDemo.unity"};

        buildPlayer.locationPathName = "C:\\Users\\Ibrahim Mushtaq\\Desktop\\WebGLBUild";
        buildPlayer.target = BuildTarget.WebGL;
        Screen.SetResolution(1280,720,false,60);
        WebGLExceptionSupport web = WebGLExceptionSupport.FullWithoutStacktrace;
        buildPlayer.options = (BuildOptions)web;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayer);
        BuildSummary summary = report.summary;

        if(summary.result == BuildResult.Succeeded){
            File.WriteAllText(@"C:\Users\Ibrahim Mushtaq\Desktop\RobotON\stdout1.log", "Build Succed: " + summary.totalSize + " bytes, Date :" + System.DateTime.Now.ToString()+"\n");

        }else if(summary.result == BuildResult.Failed){
            File.WriteAllText(@"C:\Users\Ibrahim Mushtaq\Desktop\RobotON\stdout1.log", "Build Succed: " + summary.totalSize + " bytes, Date :" + System.DateTime.Now.ToString()+"\n";
        }
    }
}
