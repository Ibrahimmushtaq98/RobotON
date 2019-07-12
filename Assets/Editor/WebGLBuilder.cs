using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class WebGLBuilder
{
    static void build() {
        File.WriteAllText(@"home/ibrahim/Desktop/RobotON/stdout1.log", "Build Started!, time: " + System.DateTime.Now.ToString());


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

        buildPlayer.locationPathName = "/home/ibrahim/DesktopWebGLBuilds";
        buildPlayer.target = BuildTarget.WebGL;
        Application.targetFrameRate = 30;

        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.defaultWebScreenHeight = 720;
        PlayerSettings.defaultWebScreenWidth = 1280;
        PlayerSettings.WebGL.memorySize = 1000;
        PlayerSettings.stripEngineCode = true;
        PlayerSettings.runInBackground = true;
        AspectRatio aspectRatio = AspectRatio.Aspect16by9;
        PlayerSettings.SetAspectRatio(aspectRatio, true);

        WebGLExceptionSupport web = WebGLExceptionSupport.None;
        WebGLCompressionFormat compressionFormat = WebGLCompressionFormat.Gzip;

        PlayerSettings.WebGL.compressionFormat = compressionFormat;

        Application.runInBackground = true;
        buildPlayer.options = (BuildOptions)web | (BuildOptions)compressionFormat;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayer);
        BuildSummary summary = report.summary;

        if(summary.result == BuildResult.Succeeded){
            File.WriteAllText(@"/home/ibrahim/Desktop/RobotON/stdout1.log", "Build Succed: " + summary.totalSize + " bytes, Date :" + System.DateTime.Now.ToString()+"\n");

        }else if(summary.result == BuildResult.Failed){
            File.WriteAllText(@"/home/ibrahim/Desktop/RobotON/stdout1.log", "Build Failed: " + summary.totalSize + " bytes, Date :" + System.DateTime.Now.ToString()+"\n");
        }
    }
}
