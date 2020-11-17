using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Twitter;
using System.Text;


public class ProduceTweetWithImg : MonoBehaviour {

	public GameObject tweetwithimgprefab;
	GameObject[] tweetwithimgprefabs;
	//public GameObject tweetprefabcrowd;
	// Use this for initialization
	void Start () {
		tweetwithimgprefabs = new GameObject[100];
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
			for (int i = 0; i <= 99; i++) {
				string mediainfo = Response.items [i].text;
				Encoding mediabyte = Encoding.GetEncoding ("Shift_JIS");
				int num = mediabyte.GetByteCount (mediainfo);
				if (0 <= mediainfo.IndexOf ("https://t.co/") && 0 > mediainfo.IndexOf ("RT") && Response.items [i].quoted_status_id_str == null && num < 230 && CountChar(mediainfo,':') == 1   && 0 > mediainfo.IndexOf ("@") ) {
				//if (Response.items [i].entities != null ) {	
					print (Response.items [i].text);
					print (Response.items [i].user.name);
					print (response);
					print (i);
					string Imgurlcomp = Response.items [i].entities.media [0].media_url;
					StartCoroutine (TweetImageRepresent (Imgurlcomp, i));
					//print (i);
					Vector3 posi = new Vector3 (Random.Range (-9612f, 9937f), Random.Range (21f, 20717f), Random.Range (-5259f, 9226f));
					tweetwithimgprefabs [i] = Instantiate (tweetwithimgprefab, posi, Quaternion.identity);
					GameObject Tcolor = tweetwithimgprefab.transform.Find ("Panel").gameObject;
					Tcolor.GetComponent<Image> ().color = new Color (Random.Range (0f, 1.0f), Random.Range (0f, 1.0f), Random.Range (0f, 1.0f), 0.6f);
					//アイコン画像の表示
					string own = Response.items [i].user.profile_image_url;
					StartCoroutine (IconImageRepresent (own, i));
					//リツイート数の表示
					string retweet = Response.items [i].retweet_count.ToString ();
					GameObject j = tweetwithimgprefab.transform.Find ("ReTweet/ReTweetNum").gameObject;
					Text retweetT = j.GetComponent<Text> ();//"TimeLineEarth/TimeLineTweets/OneOtherTweet" + i + "/ReTweet/
					retweetT.text = retweet;
					//ふぁぼ数の表示
					string favorite = Response.items [i].favorite_count.ToString ();
					GameObject k = tweetwithimgprefab.transform.Find ("Favorite/FavoriteNum").gameObject;
					Text favoriteT = k.GetComponent<Text> ();
					favoriteT.text = favorite;
					//ツイート時刻の表示
					string time = Response.items [i].created_at;
					GameObject t = tweetwithimgprefab.transform.Find ("Time").gameObject;
					Text timeT = t.GetComponent<Text> ();
					timeT.text = time;
					//以下はツイートテキストの表示
					//Debug.Log (Response.items [i].text);
					GameObject l = tweetwithimgprefab.transform.Find ("TweetText").gameObject;
					Text tweetLabel = l.GetComponent<Text> ();
					if (Response.items [i].text.Length > 32) {
						GameObject m = tweetwithimgprefab.transform.Find ("TweetText").gameObject;
						Text changefont1 = m.GetComponent<Text> ();
						changefont1.fontSize = 7;
						tweetLabel.text = Response.items [i].text;
						if (Response.items [i].text.Length > 66) {
							GameObject n = tweetwithimgprefab.transform.Find ("TweetText").gameObject;
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
		}
		else
		{
			Debug.Log(response);
		}


	}
	IEnumerator IconImageRepresent (string own,int i)
	{
		print ("WA");
		WWW www = new WWW(own);
		yield return www;
		GameObject o = tweetwithimgprefabs[i].transform.Find ("IconImage").gameObject;
		RawImage rawImage = o.GetComponent<RawImage>();
		rawImage.texture = www.textureNonReadable;
		//textureNonReadable・・・ダウンロードしたデータからピクセルデータの読み込みができないTexture2Dを生成し返す
		rawImage.SetNativeSize ();
		print ("FF#");

	}

	IEnumerator TweetImageRepresent (string own, int i)
	{
		WWW www = new WWW(own);
		yield return www;
		GameObject o = tweetwithimgprefabs [i].transform.Find ("TweetImg").gameObject;
		RawImage rawImage = o.GetComponent<RawImage>();
		rawImage.texture = www.textureNonReadable;
		//textureNonReadable・・・ダウンロードしたデータからピクセルデータの読み込みができないTexture2Dを生成し返す
		//rawImage.SetNativeSize ();

	}

	public static int CountChar(string s, char c)
	{
		return s.Length - s.Replace (c.ToString (), "").Length;
	}
}