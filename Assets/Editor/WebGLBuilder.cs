using System.IO;
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
        buildPlayer.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayer);
        BuildSummary summary = report.summary;

        if(summary.result == BuildResult.Succeeded){
            File.WriteAllText(@"C:\Users\Ibrahim Mushtaq\Desktop\RobotON\stdout1.log", "Build Succed: " + summary.totalSize + " bytes\n");

        }else if(summary.result == BuildResult.Failed){
            File.WriteAllText(@"C:\Users\Ibrahim Mushtaq\Desktop\RobotON\stdout1.log", "Build Succed: " + summary.totalSize + " bytes\n");
        }
    }
}
