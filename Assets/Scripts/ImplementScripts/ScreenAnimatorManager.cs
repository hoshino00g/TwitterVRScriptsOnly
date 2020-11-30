using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAnimatorManager : MonoBehaviour {

	Animator screenanim;
	// Use this for initialization
	void Start () {
		screenanim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.Alpha1)) {
			screenanim.SetBool ("IsImg", true);
		
		}
	}
		
}
