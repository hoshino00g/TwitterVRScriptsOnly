using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Twitter;
using System;
using System.Text;

public class GetMyTimeLine : MonoBehaviour {
    
	public GameObject timeLineEarth;
	public GameObject hometimeline;
	TwitterJson.StatusesHomeTimelineResponse Hresponse;

	// Use this for initialization
	void Start () {
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters["count"] = 32.ToString();
        StartCoroutine(TwitterRequest.Client.Get("statuses/home_timeline", parameters, this.Callback));
		
	}
     void Callback (bool success, string response)//string型のresponseにはTwitterRequestのrequest.downloadHandler.textが入る
	{
		if (success) {
			for (int i = 0; i <= 30; i++) {
				TwitterJson.StatusesHomeTimelineResponse Response = JsonUtility.FromJson<TwitterJson.StatusesHomeTimelineResponse> (response);
				//ツイート画像の有無と表示
				//print(Response.items [i].entities);
				string mediainfo = Response.items [i].text;
				//if (Response.items [i].quoted_status_id_str == null) {
				print (mediainfo);
				Encoding mediabyte = Encoding.GetEncoding ("Shift_JIS");
				int num = mediabyte.GetByteCount (mediainfo);
				print (num);
				//} else {
				//print (mediainfo.Contains ("https://t.co/"));
				//print (mediainfo.IndexOf ("RT"));
				if (0 <= mediainfo.IndexOf ("https://t.co/") && 0 > mediainfo.IndexOf ("RT") && Response.items [i].quoted_status_id_str == null && num < 230) {
					//print (mediainfo);
					//NullReferenceException none = new NullReferenceException();
					//if (Response.items [i].entities.media[0].media_url != null){
					//次のスクリーンに行く方法➡Start関数でint型の変数を用意してif文の最後にi++する
					string Imgurlcomp = Response.items [i].entities.media [0].media_url;
					//print (mediainfo);
					//if (0 <= mediainfo.IndexOf("https://t.co/") && 0 > mediainfo.IndexOf("RT")) {
					//int findnum = mediainfo.LastIndexOf ("/");
					//string Imgurlpart = mediainfo.Substring (findnum + 1);
					//string Imgurlcomp = "https://t.co/" + Imgurlpart;
					//print (Imgurlcomp);
					Hresponse = Response;
					StartCoroutine (TweetImageRepresent (Imgurlcomp, i));
				}
				//}
				//アイコン画像の表示
				string own = Response.items [i].user.profile_image_url;
				StartCoroutine (IconImageRepresent (own, i));
				//リツイート数の表示
				string retweet = Response.items [i].retweet_count.ToString ();
				GameObject j = timeLineEarth.transform.Find ("TimeLineTweets/OneOtherTweet" + i + "/ReTweet/ReTweetNum").gameObject;
				Text retweetT = j.GetComponent<Text> ();//"TimeLineEarth/TimeLineTweets/OneOtherTweet" + i + "/ReTweet/
				retweetT.text = retweet;
				//ふぁぼ数の表示
				string favorite = Response.items [i].favorite_count.ToString ();
				GameObject k = timeLineEarth.transform.Find ("TimeLineTweets/OneOtherTweet" + i + "/Favorite/FavoriteNum").gameObject;
				Text favoriteT = k.GetComponent<Text> ();
				favoriteT.text = favorite;
				//ツイート時刻の表示
				string time = Response.items [i].created_at;
				GameObject t = timeLineEarth.transform.Find ("TimeLineTweets/OneOtherTweet" + i + "/Time").gameObject;
				Text timeT = t.GetComponent<Text> ();
				timeT.text = time;
				//以下はツイートテキストの表示
				//Debug.Log (Response.items [i].text);
				GameObject l = timeLineEarth.transform.Find ("TimeLineTweets/OneOtherTweet" + i + "/TweetText").gameObject;
				Text tweetLabel = l.GetComponent<Text> ();
				if (Response.items [i].text.Length > 32) {
					GameObject m = timeLineEarth.transform.Find ("TimeLineTweets/OneOtherTweet" + i + "/TweetText").gameObject;
					Text changefont1 = m.GetComponent<Text> ();
					changefont1.fontSize = 7;
					tweetLabel.text = Response.items [i].text;
					if (Response.items [i].text.Length > 66) {
						GameObject n = timeLineEarth.transform.Find ("TimeLineTweets/OneOtherTweet" + i + "/TweetText").gameObject;
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

		WWW www = new WWW(own);
		yield return www;
        GameObject o = timeLineEarth.transform.Find ("TimeLineTweets/OneOtherTweet" + i + "/IconImage").gameObject;
		RawImage rawImage = o.GetComponent<RawImage>();
		rawImage.texture = www.textureNonReadable;
		//textureNonReadable・・・ダウンロードしたデータからピクセルデータの読み込みができないTexture2Dを生成し返す
		rawImage.SetNativeSize ();

}
	IEnumerator ImgIconRepresent (string own, int i)
	{
		WWW www = new WWW(own);
		yield return www;
		GameObject o = hometimeline.transform.Find ("ImgTV/TweetWithImg/IconImage").gameObject;
		RawImage rawImage = o.GetComponent<RawImage>();
		rawImage.texture = www.textureNonReadable;
		//textureNonReadable・・・ダウンロードしたデータからピクセルデータの読み込みができないTexture2Dを生成し返す
		rawImage.SetNativeSize ();
	}
	IEnumerator TweetImageRepresent (string Imgurlcomp, int i)
	{

		WWW www = new WWW (Imgurlcomp);
		yield return www;
		GameObject o = hometimeline.transform.Find ("ImgTV/ScreenCanvas/TweetImg").gameObject;
		RawImage rawImage = o.GetComponent<RawImage> ();
		rawImage.texture = www.textureNonReadable;
		print (Imgurlcomp + "A");
		string own = Hresponse.items [i].user.profile_image_url;
		StartCoroutine (ImgIconRepresent (own, i));
		//リツイート数の表示
		string retweet = Hresponse.items [i].retweet_count.ToString ();
		GameObject j =  hometimeline.transform.Find ("ImgTV/TweetWithImg/ReTweet/ReTweetNum").gameObject;
		Text retweetT = j.GetComponent<Text> ();
		print (retweetT.text);
		print (retweet);//"TimeLineEarth/TimeLineTweets/OneOtherTweet" + i + "/ReTweet/
		retweetT.text = retweet;
		//ふぁぼ数の表示
		string favorite = Hresponse.items [i].favorite_count.ToString ();
		GameObject k =  hometimeline.transform.Find ("ImgTV/TweetWithImg/Favorite/FavoriteNum").gameObject;
		Text favoriteT = k.GetComponent<Text> ();
		favoriteT.text = favorite;
		//ツイート時刻の表示
		string time = Hresponse.items [i].created_at;
		GameObject t =  hometimeline.transform.Find ("ImgTV/TweetWithImg/Time").gameObject;
		Text timeT = t.GetComponent<Text> ();
		timeT.text = time;
		//以下はツイートテキストの表示
		//Debug.Log (Response.items [i].text);
		GameObject l =  hometimeline.transform.Find ("ImgTV/TweetWithImg/TweetText").gameObject;
		Text tweetLabel = l.GetComponent<Text> ();
		if (Hresponse.items [i].text.Length > 32) {
			GameObject m =  hometimeline.transform.Find ("ImgTV/TweetWithImg/TweetText").gameObject;
			Text changefont1 = m.GetComponent<Text> ();
			changefont1.fontSize = 7;
			tweetLabel.text = Hresponse.items [i].text;
			if (Hresponse.items [i].text.Length > 66) {
				GameObject n =  hometimeline.transform.Find ("ImgTV/TweetWithImg/TweetText").gameObject;
				Text changefont2 = n.GetComponent<Text> ();
				changefont2.fontSize = 5;
				tweetLabel.text = Hresponse.items [i].text;
			} else {
				tweetLabel.text = Hresponse.items [i].text;
			}
		} else {
			tweetLabel.text = Hresponse.items [i].text;
		}
	}
	}

	
	// Update is called once per frame
	//void Update () {
		
	//}
//}
