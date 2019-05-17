//**************************************************//
// Class Name: Breakpoint
// Class Description: Instantiable object for the RoboBUG game. This class controls the Breakpoints and corresponds
//                    with the breakpointer tool.
// Methods:
// 		void Start()
//		void Update()
//		void OnTriggerEnter2D(Collider2D collidingObj)
// Author: Michael Miljanovic
// Date Last Modified: 6/1/2016
//**************************************************//

using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI; 

public class Breakpoint : Tools {

	public string values;
	public AudioClip[] sound = new AudioClip[2];

	Animator anim; 
	private bool activated = false;
	public override void Initialize(){
		anim = GetComponent<Animator>();
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
        if (!activated && collidingObj.name == stringLib.PROJECTILE_DEBUG) {
			if (!activated) {
				GetComponent<AudioSource>().clip = sound[0];
				GetComponent<AudioSource>().Play();
			}
			activated = true;
			anim.SetTrigger("Complete");
			this.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/yellowbreakpoint")[0];
            Destroy(collidingObj.gameObject);
        }
		else if (activated && collidingObj.name == stringLib.PROJECTILE_ACTIVATOR) {
			Debug.Log("activated");
			GetComponent<AudioSource>().clip = sound[1];
			GetComponent<AudioSource>().Play();
			output.Text.text = values;
			if (!toolgiven) {
				toolgiven = true;
				for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++) {
					if (tools[i] > 0) {
                        // Must be called from level generator, not ToolSelectorObject
						lg.floatingTextOnPlayer(GlobalState.StringLib.COLORS[i]);
					}
					selectedTool.toolCounts[i] += tools[i];
				}
			}
			Destroy(collidingObj.gameObject); 
		}
	}

	//.................................>8.......................................

}
