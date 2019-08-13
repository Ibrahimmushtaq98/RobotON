﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Variables and Data that needs to be maintained between Scenes and large numbers of classes. 
/// </summary>
public static class GlobalState 
{
    public static bool RestrictGameMode = false; 
    public static bool DebugMode = false; 
    public static bool LeaderBoardMode = false;
    public static bool LoggingMode = false;
    public static List<string> passed; 
    public static string Character {get;set;}
    public static bool IsPlaying { get; set; }
    public static bool IsResume {get;set;}
    public static bool isPassed {get; set;}
    public static bool HideToolTips = false; 
    public static string CurrentONLevel { get; set; }
    public static int CurrentLevelPoints {get;set;}
    public static int CurrentLevelEnergy {get;set;}
    public static int RunningScore  = 0; 
    public static string Language ="c++"; 
    public static string CurrentBUGLevel { get; set; }
    public static string GameMode { get; set; }
    public static int GameState { get; set; }
    public static stringLib StringLib { get; set; }
    public static bool IsDark = true; 
    public static int[] toolUse; 
    public static int TextSize = 1; 
    public static bool soundon;
    public static string FilePath = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) ? @"\" : @"/";
    public static Level level; 
    public static int timeBonus = 0; 
    public static long sessionID {get; set;}
    public static int positionalID{get;set;}
    public static string currentLevelID{get; set;}
    public static string jsonStates{get; set;}
    public static string[] correctLine {get; set;}
    public static string[] obstacleLine {get; set;}
    public static string jsonOStates {get; set;}
    public static string bugLine{get; set;}
    public static CharacterStats Stats{get;set;}

    public static bool foundBug;
    public static int totalPoints {get; set;}
    public static int currentLevelStar {get; set;}
    public static int currentLevelTimeBonus {get; set;}

    public static float totalPointsCurrent {get; set;}
    public static string previousFilename {get; set;}
    public static string username {get; set;}
    public static string URL_MOVIE {get; set;}
    public static string URL_MOVIE_MENU {get; set;}
    public static string URL_MOVIE_BUG {get; set;}
    public static string URL_MOVIE_ON {get; set;}
}