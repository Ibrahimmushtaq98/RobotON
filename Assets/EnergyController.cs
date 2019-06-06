﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnergyController : MonoBehaviour
{
    GameObject energyBar; 
    RectTransform energyBarTrans; 
    float initialEnergy;
    public float currentEnergy;
    Text indicator;
    float[] throwEnergy = new float[stateLib.NUMBER_OF_TOOLS];
    SelectedTool tools;
    bool initial = true;
    bool hidden = false; 
    float initialScale; 
    float initialY; 
    float positionCompensation = 220f; 

    public float[] percentPerUse(){
        float[] percent = new float[throwEnergy.Length]; 
        for (int i = 0; i < percent.Length; i++){
            if (GlobalState.GameMode == "bug" && i == stateLib.TOOL_CATCHER_OR_CONTROL_FLOW){
                percent[i] = 99f;                 
            }
            else percent[i] = (throwEnergy[i]/initialEnergy)*100f; 
        }
        return percent; 
    }
    // Start is called before the first frame update
    void Start()
    {
        initialEnergy = GlobalState.TotalEnergy;
        currentEnergy = initialEnergy;
        indicator = transform.GetChild(0).GetComponent<Text>();
        tools = GameObject.Find("Sidebar").transform.Find("Sidebar Tool").GetComponent<SelectedTool>();
        energyBar = transform.GetChild(1).gameObject; 
        energyBarTrans = energyBar.GetComponent<RectTransform>(); 
        initialY = energyBarTrans.position.y; 
        initialScale = energyBar.GetComponent<RectTransform>().localScale.x; 
    }
    public void onThrow(int projectileCode)
    {
        
        currentEnergy -= throwEnergy[projectileCode];
        if (GlobalState.GameMode == "bug" && projectileCode == stateLib.TOOL_CATCHER_OR_CONTROL_FLOW)
        {
            currentEnergy = 0;
        }
        if (currentEnergy > 0 ){
            indicator.text = ((int)(currentEnergy*100f / initialEnergy)).ToString() + '%';
            energyBar.GetComponent<RectTransform>().localScale = new Vector3(initialScale*((currentEnergy / initialEnergy)), 1, 1);
            energyBarTrans.position = new Vector3(energyBarTrans.position.x, initialY + positionCompensation*((initialEnergy-currentEnergy) / initialEnergy), 0 ); 
        } 
        else {
            indicator.text = "0%"; 
            energyBar.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1); 
        }
    }
    public void onFail(int projectileCode){
        currentEnergy-= throwEnergy[projectileCode];
        if (currentEnergy> 0)
            indicator.text = (currentEnergy / initialEnergy).ToString() + '%'; 
        else indicator.text = "0%"; 
        energyBar.GetComponent<RectTransform>().localScale = new Vector3(initialScale*(currentEnergy / initialEnergy), 1, 1); 
        energyBarTrans.position = new Vector3(energyBarTrans.position.x, initialY + positionCompensation*((initialEnergy -currentEnergy) / initialEnergy), 0 ); 
    }
    // Update is called once per frame
    void Update()
    {
        if (initial)
        {
            int totalCounts = 0; 
            for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++){
                if (tools.toolCounts[i] > 0){
                    totalCounts++; 
                }
            } 
            //Debug.Log(totalCounts); 
            for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
            {
                throwEnergy[i] = (100f/((float)totalCounts));
                if (tools.toolCounts[i] > 0){
                    if (tools.toolCounts[i] < 999)
                        throwEnergy[i] /= (float)tools.toolCounts[i]; 
                    else throwEnergy[i] /= (GlobalState.level.Tasks[i] + 5);
                }
                else if (i < 5){
                    throwEnergy[i] /= (GlobalState.level.Tasks[i] + 10);
                }
                
            }
            initial = false;
        }
        if (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME && !hidden){
            energyBar.GetComponent<Image>().enabled = false; 
            energyBar.transform.GetChild(0).GetComponent<Image>().enabled = false; 
            indicator.text = ""; 
            hidden = true; 
        }
        else if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME && hidden){
            energyBar.GetComponent<Image>().enabled = true; 
            energyBar.transform.GetChild(0).GetComponent<Image>().enabled = false; 
            if (currentEnergy > 0)
                indicator.text = ((int)(currentEnergy*100f / initialEnergy)).ToString() + '%'; 
            else indicator.text = "0%"; 
            hidden = false; 
        }
    }
}
