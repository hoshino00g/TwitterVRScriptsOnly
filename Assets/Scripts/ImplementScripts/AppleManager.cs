using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleManager : MonoBehaviour {
	public Vector3 acceleration;
	public Vector3 afterfall;
	public GameObject mytweets;
	Animator appleanim;
	Animator profileanim;
	public GameObject profile;
	Rigidbody rigidbody;
	bool firstdrop;
	Animator tweetanim;
	// Use this for initialization
	void Start () {
		Physics.gravity = new Vector3(0, -9.81f, 0);
		rigidbody = GetComponent<Rigidbody> ();
		firstdrop = true;
		tweetanim = mytweets.GetComponent<Animator>();
		profileanim = profile.GetComponent<Animator> ();
		appleanim = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
		acceleration = new Vector3(0, -20.0f, 0);
		rigidbody.AddForce(acceleration, ForceMode.Acceleration);
	}

	void OnTriggerEnter (Collider other){
		if (other.name == "FloorCollider" && firstdrop == true){
			print (other.name);
			print (gameObject.transform.position);
			afterfall = new Vector3 (0, 0, 20.0f);
			tweetanim.SetBool ("IsMy", true);
			rigidbody.AddForce (afterfall, ForceMode.Impulse);
			firstdrop = false;

		}
		if(other.name == "PT"){
			appleanim.SetBool ("IsApple", true);
			profileanim.SetBool ("IsProfile", true);
			print ("S");
}
}
}
