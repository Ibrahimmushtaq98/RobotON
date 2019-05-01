//**************************************************//
// Class Name: beacon
// Class Description: Instantiable object for the Robot ON! game. This controls the beacons
//                    and corresponds with the Beacon Activator tool.
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 

public class beacon : Tools {
	public int actcounter = 0;
	public bool revOnce = false;
	public List<int> flowOrder;

	public AudioSource audioCorrect;
	public AudioSource audioRev;
	public Sprite activebeacon;
	public Sprite inactivebeacon;
	public Sprite progressbeacon;
	int flashCounter = 0;



	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
		// All beacons complete
		if (GlobalState.level.Tasks[0] == GlobalState.level.CompletedTasks[0] && !revOnce) {
			revOnce = true;
			GetComponent<SpriteRenderer>().sprite = activebeacon;
			audioRev.Play();
		}
		else if (GlobalState.level.Tasks[0] == GlobalState.level.CompletedTasks[0] && actcounter > 0)
		{
			flashCounter++;
			if (flashCounter > 50) {
				GetComponent<SpriteRenderer>().sprite = progressbeacon;
			}
			if (flashCounter > 100) {
				GetComponent<SpriteRenderer>().sprite = inactivebeacon;
				flashCounter = 0;
			}
		}
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.name == stringLib.PROJECTILE_BUG) {
			Destroy(collidingObj.gameObject);
			if (GetComponent<SpriteRenderer>().sprite == activebeacon || flowOrder.Count == 0) {
				selectedTool.outputtext.GetComponent<Text>().text = "Beacons must be activated in the right\n order. Sometimes they are activated\n more than once, sometimes not at all.\n You will need to start over.";
				ResetAllBeacons();
			}
			else if (actcounter > flowOrder.Count - 1) {
				selectedTool.outputtext.GetComponent<Text>().text = "You have activated this \nbeacon enough times, but the \nsequence is now broken. \nYou will have to start over.";
				ResetAllBeacons();
			}
			else if (GlobalState.level.CompletedTasks[0] != flowOrder[actcounter]) {
				selectedTool.outputtext.GetComponent<Text>().text = "You will need to start the \nsequence again. Read the code carefully for \nclues.";
				ResetAllBeacons();
			}
			else {
				// Correct Selection
				audioCorrect.Play();
				GlobalState.level.CompletedTasks[0]++;
				// Award 1 extra use of the tool.
				selectedTool.bonusTools[stateLib.TOOL_CATCHER_OR_ACTIVATOR]++;
				actcounter++;
				GetComponent<SpriteRenderer>().sprite = progressbeacon;
			}
			
		}
	}

    //.................................>8.......................................
    void ResetAllBeacons() {
	    foreach(GameObject beacon in lg.manager.robotONbeacons) {
		    beacon.GetComponent<beacon>().actcounter = 0;
		    beacon.GetComponent<SpriteRenderer>().sprite = inactivebeacon;
		    beacon.GetComponent<beacon>().flashCounter = 0;
            GlobalState.level.CompletedTasks[0] = 0; 
	    }
    }
}
