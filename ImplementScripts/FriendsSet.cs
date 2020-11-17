using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twitter;

public class FriendsSet : MonoBehaviour {

	public GameObject friend;
	GameObject[] prefabs;
	string[] retweetnum;
	string[] favoritenum;
	string[] timenum;
	string[] textnum;
	string[] imgurl;
	float roadWidth;
	float roadHeiht;
	public int ongroundcharactercount;
	int numbox;
	Vector3 friendposi;



	// Use this for initialization
	void Start () {
		//rfps = new GameObject[] { rfp, rfp1, rfp2, rfp3 };
		prefabs = new GameObject[10];
		retweetnum = new string[10];
	    favoritenum = new string[10];
		timenum = new string[10];
		textnum = new string[10];
		imgurl  = new string[10];
		Dictionary<string, string> parameters1 = new Dictionary<string, string> ();
		parameters1 ["count"] = 10.ToString ();
		StartCoroutine (TwitterRequest.Client.Get ("friends/list", parameters1, this.FriendListTake));
		
	}
	
	// Update is called once per frame
	void FriendListTake(bool success, string response){

			if (success) {
				TwitterJson.FriendsListResponse Response = JsonUtility.FromJson<TwitterJson.FriendsListResponse> (response);
			int frendnum = Response.users.Length - 1;
			print (frendnum);
			for (int fn = 0; fn <= frendnum; fn++) {
				//int whilenum = 0;
				//while(ongroundcharactercount < frendnum){
				//ProduceOnRoad(-1000f,-475f,-1.336614f,- 593.53939f,28f,fn);
				//ProduceOnRoad (-1296f, -830.1f, 0.599914144f, 968.38869441f, 15f, fn);
				//ProduceOnRoad (-500f,-357f,-2.77622377f,-1318.111888f, 15f, fn);
				//ProduceOnRoad (-750f,-258f,0.53861788f,925.963414f, 15f, fn);
				friendposi = new Vector3(Random.Range(-9612f,9937f),Random.Range(21f,20717f),Random.Range(-5259,9226));
				prefabs[fn] = Instantiate (friend, friendposi, Quaternion.identity);
				//numbox = ongroundcharactercount;
				//float x = Random.Range (-1000f, -475f);
				//float z = -1.336614f * x - 593.53939f;
				//friendposi = new Vector3 (x, 0, z + Random.Range(-28f,28f));
				//prefabs[fn] = Instantiate (friend, friendposi, Quaternion.identity);
					//if (ongroundcharactercount == numbox) {
						//Destroy (prefabs [fn]);

					//}
				//}
				prefabs[fn].name = Response.users [fn].name;
				long id = Response.users [fn].id;
				FriendTweetInfo (id,fn);
				print (fn + "AW");
				print (prefabs[fn].name + "AW");
			}

			}else{
				Debug.Log (response);
			}
		}
		

	void FriendTweetInfo (long id, int fn) {
		Dictionary<string, string> parameters1 = new Dictionary<string, string> ();
		parameters1 ["id"] = id.ToString ();
		parameters1 ["count"] = 10.ToString ();
		StartCoroutine (PersonRequest.Client.Get (fn, "statuses/user_timeline", parameters1, this.FriendInfoTake));
	}

	void FriendInfoTake (bool success, string response, int fn){
		if (success) {
			TwitterJson.StatusesUserTimelineResponse Response = JsonUtility.FromJson<TwitterJson.StatusesUserTimelineResponse> (response);
			print (prefabs[fn].name);
			print (Response.items [0].user.name);
			for( int jn = 0; jn <= 9; jn++){
				retweetnum [jn] = Response.items [jn].retweet_count.ToString();
				favoritenum [jn] = Response.items [jn].favorite_count.ToString();
				timenum [jn] = Response.items [jn].created_at;
				textnum [jn] = Response.items [jn].text;
				imgurl [jn] = Response.items [jn].text;
				PersonTweetsDraw.PersonTextDraw (prefabs[fn],retweetnum [jn], favoritenum [jn], timenum [jn], textnum [jn]);
				PersonTweetsDraw.PersonImgDraw (prefabs [fn], imgurl [jn]);
			}


		}else{
			Debug.Log (response);
		}
	}

	void ProduceOnRoad(float Xmin, float Xmax, float slope, float segment, float vari, int fn ){
		float x = Random.Range (Xmin, Xmax);
		float z = slope * x  + segment;
		friendposi = new Vector3 (x, 0, z + Random.Range(-vari,vari));
		prefabs[fn] = Instantiate (friend, friendposi, Quaternion.identity);
	}

		
	}
	
