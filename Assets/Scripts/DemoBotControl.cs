﻿using System.IO;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DemoBotControl : MonoBehaviour
{
    public List<Action> callstack;
    private hero2Controller controller;
    private Output output;
    int indexOfAction = 0;
    int currentIndex = -1;
    bool entered = false;
    float timeDelay;
    float enterDelay;
    bool autoEnabled = true; 
    bool nextAction = false; 

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<hero2Controller>();
        callstack = new List<Action>();
        timeDelay = 5f;
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).GetComponent<Output>();
        StartCoroutine(AutoPlay()); 
    }
    IEnumerator AutoPlay()
    {
        yield return new WaitForSecondsRealtime(timeDelay); 
        while (autoEnabled)
        {
            if (nextAction){
                nextAction = false; 
                entered = true; 
            }
            yield return new WaitForSecondsRealtime(timeDelay);
        }
    }
    void UpdateDelay(Action action)
    {
        if (action.Category == ActionType.Dialog) timeDelay = 0f;
        else if (action.Category == ActionType.SwitchTool) timeDelay = 0.5f;
        else if (action.Category == ActionType.Throw) timeDelay = 2f;
        Debug.Log(timeDelay); 
    }

    // Update is called once per frame
    void Update()
    {
        if (enterDelay < 0)
        {
            if (callstack.Count > 0 && indexOfAction < callstack.Count && currentIndex != indexOfAction && output.text.GetComponent<Text>().text == "" && controller.reachedPosition)
            {
                
                currentIndex = indexOfAction;
                if (callstack[currentIndex].Category == ActionType.Dialog)
                {
                    controller.reachedPosition = false;
                    StartCoroutine(controller.MoveToPosition(controller.RoundPosition(callstack[currentIndex].Position)));
                }
                else if (callstack[currentIndex].Category == ActionType.Throw)
                {
                    controller.ThrowTool();
                    nextAction = true; 
                }
                else if (callstack[currentIndex].Category == ActionType.SwitchTool)
                {
                    controller.selectedTool.GetComponent<SelectedTool>().NextTool();
                    nextAction = true; 
                }
                UpdateDelay(callstack[currentIndex]);
                
            }
            if (controller.reachedPosition && indexOfAction < callstack.Count && callstack[indexOfAction].Category == ActionType.Dialog)
            {
                output.text.GetComponent<Text>().text = callstack[currentIndex].text;
            }

            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)|| entered) && controller.reachedPosition)
            {
                entered = false;
                indexOfAction++;
                enterDelay = 1f;
                output.text.GetComponent<Text>().text = "";
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)){
                //autoEnabled = false;
            }

        }
        enterDelay -= Time.deltaTime;
    }
}
