using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Twitter;

public class ProduceTweets : MonoBehaviour {
	public GameObject tweetprefab;
	public GameObject panel;
	public GameObject retweetnum;
	public GameObject favoritenum;
	public GameObject timetext;
	public GameObject tweettext;
	public GameObject iconimage;
	GameObject[] tweetprefabs;
	//GameObject[] panels;
	//GameObject[] retweetnums;
	//GameObject[] favoritenums;
	//GameObject[] times;
	//GameObject[] tweettexts;
	///GameObject[] iconimages;
	//public GameObject tweetprefabcrowd;
	// Use this for initialization
	void Start () {
		tweetprefabs = new GameObject[200];
		//panels = new GameObject[300];
		//retweetnums = new GameObject[300];
		//favoritenums = new GameObject[300];
		//times = new GameObject[300];
		//tweettexts = new GameObject[300];
		//iconimages = new GameObject[300];
		Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters["count"] = 200.ToString();
		StartCoroutine(TwitterRequest.Client.Get("statuses/home_timeline", parameters, this.Callback));




		//for (int i = 0; i <= 99; i++) {
		//Vector3 posi = new Vector3 (Random.Range (-9612f, 9937f), Random.Range (21f, 20717f), Random.Range (-5259f, 9226f));
		//Instantiate (tweetprefabcrowd, posi, Quaternion.identity);
		//GameObject Tcolor = tweetprefabcrowd.transform.Find ("Panel").gameObject;
		//Tcolor.GetComponent<Image> ().color = new Color (Random.Range (0f, 1.0f), Random.Range (0f, 1.0f), Random.Range (0f, 1.0f), 0.6f);
		//}
	}

	// Update is called once per frame
	void Callback (bool success, string response)
	//string型のresponseにはTwitterRequestのrequest.downloadHandler.textが入る
	{
		if (success) {
			TwitterJson.StatusesHomeTimelineResponse Response = JsonUtility.FromJson<TwitterJson.StatusesHomeTimelineResponse> (response);
			for (int i = 0; i <= 190; i++) {
				print (i);
				Vector3 posi = new Vector3 (Random.Range (-9612f, 9937f), Random.Range (21f, 20717f), Random.Range (-5259f, 9226f));
				tweetprefabs [i] = Instantiate (tweetprefab, posi, Quaternion.identity);
				GameObject Tcolor = panel;
				Tcolor.GetComponent<Image> ().color = new Color (Random.Range (0f, 1.0f), Random.Range (0f, 1.0f), Random.Range (0f, 1.0f), 0.6f);
				//アイコン画像の表示
				string own = Response.items [i].user.profile_image_url;
				StartCoroutine (IconImageRepresent (own,i));
				//リツイート数の表示
				string retweet = Response.items [i].retweet_count.ToString ();
				GameObject j = retweetnum;
				Text retweetT = j.GetComponent<Text> ();//"TimeLineEarth/TimeLineTweets/OneOtherTweet" + i + "/ReTweet/
				retweetT.text = retweet;
				//ふぁぼ数の表示
				string favorite = Response.items [i].favorite_count.ToString ();
				GameObject k = favoritenum;
				Text favoriteT = k.GetComponent<Text> ();
				favoriteT.text = favorite;
				//ツイート時刻の表示
				string time = Response.items [i].created_at;
				GameObject t = timetext;
				Text timeT = t.GetComponent<Text> ();
				timeT.text = time;
				//以下はツイートテキストの表示
				//Debug.Log (Response.items [i].text);
				GameObject l = tweettext;
				Text tweetLabel = l.GetComponent<Text> ();
				if (Response.items [i].text.Length > 32) {
					GameObject m = tweettext;
					Text changefont1 = m.GetComponent<Text> ();
					changefont1.fontSize = 7;
					tweetLabel.text = Response.items [i].text;
					if (Response.items [i].text.Length > 66) {
						GameObject n = tweettext;
						Text changefont2 = n.GetComponent<Text> ();
						changefont2.fontSize = 5;
						tweetLabel.text = Response.items [i].text;
					} else {
						tweetLabel.text = Response.items [i].text;
					}
				} else {
					tweetLabel.text = Response.items [i].text;
				}
			}
		}
		else 
		{
			Debug.Log(response);
		}


	}
	IEnumerator IconImageRepresent (string own, int i)
	{
		print (own);
		print ("WA");
		WWW www = new WWW(own);
		yield return www;
		GameObject o = tweetprefabs[i].transform.Find ("IconImage").gameObject;
		RawImage rawImage = o.GetComponent<RawImage>();
		print (rawImage);
		rawImage.texture = www.textureNonReadable;
		print (www.textureNonReadable);
		//textureNonReadable・・・ダウンロードしたデータからピクセルデータの読み込みができないTexture2Dを生成し返す
		rawImage.SetNativeSize ();
		print ("FF#");


	}
	void Update(){
		
	}
}
