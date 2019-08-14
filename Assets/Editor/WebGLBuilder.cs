using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class WebGLBuilder
{
    static void build() {
        File.AppendAllText(@"/home/ibrahim/Desktop/RobotON/stdout1.log", "Build Started!, time: " + System.DateTime.Now.ToString());


        BuildPlayerOptions buildPlayer = new BuildPlayerOptions();
        buildPlayer.scenes = new [] {"Assets/TitleScene.unity",
                            "Assets/TitleMenu.unity",
                            "Assets/StartScene.unity",
                            "Assets/Leaderboard.unity", 
                            "Assets/MainMenu.unity",
                            "Assets/newgame.unity",
                            "Assets/Cinematic.unity",
                            "Assets/CharacterSelect.unity",
                            "Assets/IntroScene.unity",
                            "Assets/Transition.unity",
                            "Assets/Credits.unity",
                            "Assets/TutorialDemo.unity",
                            "Assets/Progression.unity"};

        buildPlayer.locationPathName = "/home/ibrahim/Desktop/WebGLBuilds/RoboON";
        buildPlayer.target = BuildTarget.WebGL;
        Application.targetFrameRate = 30;

        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.defaultWebScreenHeight = 720;
        PlayerSettings.defaultWebScreenWidth = 1280;
        PlayerSettings.runInBackground = true;
        AspectRatio aspectRatio = AspectRatio.Aspect16by9;
        PlayerSettings.SetAspectRatio(aspectRatio, true);
        PlayerSettings.productName = "RoboGames";
        PlayerSettings.companyName = "UOIT";

        WebGLExceptionSupport web = WebGLExceptionSupport.FullWithoutStacktrace;
        PlayerSettings.WebGL.exceptionSupport = web;
        Application.runInBackground = true;
       // buildPlayer.options = (BuildOptions)web | (BuildOptions)compressionFormat;
        buildPlayer.options = (BuildOptions)web;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayer);
        BuildSummary summary = report.summary;

        if(summary.result == BuildResult.Succeeded){
            File.AppendAllText(@"/home/ibrahim/Desktop/RobotON/stdout1.log", "Build Succed: " + summary.totalSize + " bytes, Date :" + System.DateTime.Now.ToString()+"\n");

        }else if(summary.result == BuildResult.Failed){
            File.AppendAllText(@"/home/ibrahim/Desktop/RobotON/stdout1.log", "Build Failed: " + summary.totalSize + " bytes, Date :" + System.DateTime.Now.ToString()+"\n");
        }
    }
}
