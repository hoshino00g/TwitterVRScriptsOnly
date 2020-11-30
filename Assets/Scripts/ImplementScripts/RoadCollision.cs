using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCollision : MonoBehaviour {

	public FriendsSet fs;

	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag == "friend" ){
			fs.ongroundcharactercount++;
		}
	}

}
