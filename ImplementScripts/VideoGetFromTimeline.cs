using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twitter;
using UnityEngine.UI;
using System;
using System.Text;

public class VideoGetFromTimeline : MonoBehaviour {

	public GameObject videoscreen;
	public MovieTexture mt;
	// Use this for initialization
	void Start () {
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters["count"] = 32.ToString();
			StartCoroutine(TwitterRequest.Client.Get("statuses/home_timeline", parameters, this.Callback));

		}
		

	
	// Update is called once per frame
	void Callback(bool success, string response)//string型のresponseにはTwitterRequestのrequest.downloadHandler.textが入る
	{
		if (success) {
			for (int i = 0; i <= 30; i++) {
				TwitterJson.StatusesUserTimelineResponse Response = JsonUtility.FromJson<TwitterJson.StatusesUserTimelineResponse> (response);
				string mediainfo = Response.items [i].text;
				print (mediainfo);
				Encoding mediabyte = Encoding.GetEncoding ("Shift_JIS");
				int num = mediabyte.GetByteCount (mediainfo);
				print (num);
				if (0 <= mediainfo.IndexOf ("https://t.co/") && 0 > mediainfo.IndexOf ("RT") && Response.items [i].quoted_status_id_str == null && num < 230) {
					if (Response.items [i].extended_entities.media[0].type == "video") {
						if (Response.items [i].extended_entities.media [0].video_info.variants [0].url != null) {
							string videourl = Response.items [i].extended_entities.media [0].video_info.variants [0].url;
							print (videourl);
							StartCoroutine(VideoStream(videourl,videoscreen));
						}

					}
				}
			}
		}else{
			Debug.Log (response);
	}
}
	IEnumerator VideoStream (string videourl, GameObject videoscreen){
		print ("START");
		WWW www = new WWW(videourl);
		yield return www;
		mt = www.GetMovieTexture();
		videoscreen.GetComponent<Renderer> ().material.mainTexture = mt;
		print ("A");
		//AudioSource audio = videoscreen.AddComponent<AudioSource> ();
		//audio.clip = mt.audioClip;
		mt.Play ();
		//saudio.Play ();
		
	}


	public void TweetAllDraw(int i, GameObject twtobj,TwitterJson.StatusesUserTimelineResponse Response, string parentname ){
		//アイコン画像の表示
		string own = Response.items [i].user.profile_image_url;
		StartCoroutine (IconImageRepresent (own, i,twtobj,parentname));
		//リツイート数の表示
		string retweet = Response.items [i].retweet_count.ToString ();
		GameObject j = twtobj.transform.Find (parentname + i + "/ReTweet/ReTweetNum").gameObject;
		Text retweetT = j.GetComponent<Text> ();//"TimeLineEarth/TimeLineTweets/OneOtherTweet" + i + "/ReTweet/
		retweetT.text = retweet;
		//ふぁぼ数の表示
		string favorite = Response.items [i].favorite_count.ToString ();
		GameObject k = twtobj.transform.Find (parentname + i + "/Favorite/FavoriteNum").gameObject;
		Text favoriteT = k.GetComponent<Text> ();
		favoriteT.text = favorite;
		//ツイート時刻の表示
		string time = Response.items [i].created_at;
		GameObject t = twtobj.transform.Find (parentname + i + "/Time").gameObject;
		Text timeT = t.GetComponent<Text> ();
		timeT.text = time;
		//以下はツイートテキストの表示
		//Debug.Log (Response.items [i].text);
		GameObject l = twtobj.transform.Find (parentname + i + "/TweetText").gameObject;
		Text tweetLabel = l.GetComponent<Text> ();
		if (Response.items [i].text.Length > 32) {
			GameObject m = twtobj.transform.Find (parentname + i + "/TweetText").gameObject;
			Text changefont1 = m.GetComponent<Text> ();
			changefont1.fontSize = 7;
			tweetLabel.text = Response.items [i].text;
			if (Response.items [i].text.Length > 66) {
				GameObject n = twtobj.transform.Find (parentname + i + "/TweetText").gameObject;
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

	public IEnumerator IconImageRepresent (string own, int i,GameObject twtobj, string parentname  )
	{

		WWW www = new WWW(own);
		yield return www;
		GameObject o = twtobj.transform.Find (parentname + i + "/IconImage").gameObject;
		RawImage rawImage = o.GetComponent<RawImage>();
		rawImage.texture = www.textureNonReadable;
		//textureNonReadable・・・ダウンロードしたデータからピクセルデータの読み込みができないTexture2Dを生成し返す
		rawImage.SetNativeSize ();

	}
}

	

