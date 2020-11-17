using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Twitter;


public class TrendGet : MonoBehaviour {

	TextAsset csv;
	StringReader reader;
	public List<string[]> csvDatas;
	TwitterJson.TrendsPlaceResponse Fresponse;
	int country;
	public GameObject trends;
	public GameObject countrynamescreen;
	public Animator anim;



	// Use this for initialization
	void Start(){
		anim = trends.GetComponent<Animator> ();
		csv = Resources.Load ("WOEIDs") as TextAsset;
		reader = new StringReader (csv.text);
		csvDatas = new List<string[]> ();
		while (reader.Peek () > -1) {
			string line = reader.ReadLine ();
			csvDatas.Add (line.Split (','));
		}
	}
	void Update(){

		if (Input.GetKey (KeyCode.J) && Input.GetKey (KeyCode.A)) {
			print (0);
			country = 0;
			GetTrend (0);
		} else if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.I)) {
			print (1);
			country = 1;
			GetTrend (1);
		} else if (Input.GetKey (KeyCode.M) && Input.GetKey (KeyCode.A)) {
			print (2);
			country = 2;
			GetTrend (2);
		} else if (Input.GetKey (KeyCode.K) && Input.GetKey (KeyCode.A)) {
			print (3);
			country = 3;
			GetTrend (3);
		} else if (Input.GetKey (KeyCode.O) && Input.GetKey (KeyCode.C)) {
			print (4);
			country = 4;
			GetTrend (4);
		} else if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.F)) {
			print (5);
			country = 5;
			GetTrend (5);
		} else if (Input.GetKey (KeyCode.E) && Input.GetKey (KeyCode.U)) {
			print (6);
			country = 6;
			GetTrend (6);
		} else if (Input.GetKey (KeyCode.W)) {
			anim.SetBool ("IsTrend", false);
		}
			
	}
			

	void GetTrend (int country) {
		Dictionary<string, string> parameters1 = new Dictionary<string, string> ();
		parameters1 ["id"] = csvDatas[country] [1].ToString ();
		StartCoroutine (TwitterRequest.Client.Get ("trends/place", parameters1, this.Callback));
		// 913988430501003264
		
	}
	
	// Update is called once per frame
	void Callback (bool success, string response) {
		if (success) {
			TwitterJson.TrendsPlaceResponse Response = JsonUtility.FromJson<TwitterJson.TrendsPlaceResponse> (response);
			print (response.ToString ());
			print(Response.items[0].trends[0].name);
			print(Response.items[0].trends.Length);
			Fresponse = Response;
			StartCoroutine (this.PrintTrend (country, Fresponse));
			//trendname = Response.items [0].trends [0].name;
			print (Fresponse);


		} else {
			Debug.Log (response);
		}

	}
	IEnumerator PrintTrend(int country, TwitterJson.TrendsPlaceResponse Fresponse ){

		yield return new WaitForSeconds (0f);
		print (Fresponse.items [0].trends [0].name);
		print (csvDatas [country][0]);
		print (csvDatas [country][1]);
		string countrynum = csvDatas [country] [0];
		GameObject q =  countrynamescreen.transform.Find ("TweetText").gameObject;
		Text countryT = q.GetComponent<Text> ();
		countryT.text = countrynum;
		for (int tc = 1; tc <= 27; tc++) {
			string trendt = Fresponse.items [0].trends [tc - 1].name;
			GameObject r = trends.transform.Find ("Trend" + tc + "/TweetText").gameObject;
			Text trendsT = r.GetComponent<Text> ();//"TimeLineEarth/TimeLineTweets/OneOtherTweet" + i + "/ReTweet/
			trendsT.text = trendt;
			Text changefont2 = r.GetComponent<Text> ();
			changefont2.fontSize = 14;
			anim.SetBool ("IsTrend", true);
	}

		
}
}
