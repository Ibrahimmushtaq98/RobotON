//**************************************************//
// Class Name: projectile2
// Class Description: Script tied to wrench projectiles
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

public class projectile2 : MonoBehaviour {

	//.................................>8.......................................
	// Use this for initialization
	void Start() {
	}

	//.................................>8.......................................
	// Update is called once per frame
	void Update() {
	}

	//.................................>8.......................................
	void OnTriggerEnter2D(Collider2D collidingObj) {
		if (collidingObj.GetType() == typeof(EdgeCollider2D)) {
			string sMessage = this.name + stringLib.LOG_TOOL_WASTED;
			Logger.printLogFile(sMessage, this.transform.position);
			Destroy(gameObject);
		}
	}

	//.................................>8.......................................
}
