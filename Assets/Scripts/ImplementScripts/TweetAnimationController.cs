using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Twitter;

public class TweetAnimationController : MonoBehaviour {
	Animator anim;
	public GameObject otheruser;
	//public GameObject Implement;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	 
	void OnTriggerEnter (Collider other){
		if (other.name == "Me"){
			Vector3 j = new Vector3 (other.gameObject.transform.position.x, 0, other.gameObject.transform.position.z);
			otheruser.transform.LookAt (j);
			anim.SetBool ("IsOpen", true);

		}

}
}
