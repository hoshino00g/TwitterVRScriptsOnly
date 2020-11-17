using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twitter;
using UnityEngine.UI;

public class SetMyStatus : MonoBehaviour {

	public GameObject mytimeline;

	// Use this for initialization
	void Start () {
		Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters["count"] = 30.ToString();
		StartCoroutine(TwitterRequest.Client.Get("statuses/user_timeline", parameters, this.Callback));

	}
	void Callback(bool success, string response)//string型のresponseにはTwitterRequestのrequest.downloadHandler.textが入る
	{
		if (success) {
			TwitterJson.StatusesUserTimelineResponse Response = JsonUtility.FromJson<TwitterJson.StatusesUserTimelineResponse> (response);
			//print (Response.items [0].entities.media [0].media_url);
			print(Response.items [0].user.id);
			Myparameterdo (Response);
			for (int i=0; i <= 19; i++){
				//アイコン画像の表示
				string own = Response.items[i].user.profile_image_url;
				StartCoroutine (IconImageRepresent (own,i));
				//リツイート数の表示
				string retweet = Response.items[i].retweet_count.ToString();
				GameObject j = mytimeline.transform.Find("MyTweets/MyTweet" + i + "/ReTweet/ReTweetNum").gameObject;
				Text retweetT = j.GetComponent<Text> ();//"TimeLineEarth/TimeLineTweets/OneOtherTweet" + i + "/ReTweet/
				retweetT.text = retweet;
				//ふぁぼ数の表示
				string favorite = Response.items[i].favorite_count.ToString();
				GameObject k = mytimeline.transform.Find ("MyTweets/MyTweet" + i + "/Favorite/FavoriteNum").gameObject;
				Text favoriteT = k.GetComponent<Text> ();
				favoriteT.text = favorite;
				//ツイート時刻の表示
				string time = Response.items[i].created_at;
				GameObject t = mytimeline.transform.Find ("MyTweets/MyTweet" + i + "/Time").gameObject;
				Text timeT = t.GetComponent<Text> ();
				timeT.text = time;
				//以下はツイートテキストの表示
				Debug.Log (Response.items [i].text);
				GameObject l = mytimeline.transform.Find ("MyTweets/MyTweet" + i + "/TweetText").gameObject;
				Text tweetLabel = l.GetComponent<Text> ();
				if (Response.items [i].text.Length > 32) {
					GameObject m = mytimeline.transform.Find ("MyTweets/MyTweet" + i + "/TweetText").gameObject;
					Text changefont1 = m.GetComponent<Text> ();
					changefont1.fontSize = 7;
					tweetLabel.text = Response.items [i].text;
					if (Response.items [i].text.Length > 66) {
						GameObject n = mytimeline.transform.Find ("MyTweets/MyTweet" + i + "/TweetText").gameObject;
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
		GameObject o = mytimeline.transform.Find ("MyTweets/MyTweet" + i + "/IconImage").gameObject;
		RawImage rawImage = o.GetComponent<RawImage>();
		rawImage.texture = www.textureNonReadable;
		GameObject o1 = mytimeline.transform.Find ("MyProfile/IconImage").gameObject;
		RawImage rawImage1 = o1.GetComponent<RawImage>();
		rawImage1.texture = www.textureNonReadable;
		//textureNonReadable・・・ダウンロードしたデータからピクセルデータの読み込みができないTexture2Dを生成し返す
		rawImage.SetNativeSize ();

	}
	void Myparameterdo (TwitterJson.StatusesUserTimelineResponse Response)
	{   string name = Response.items[0].user.name;
		GameObject m = mytimeline.transform.Find ("MyProfile/Name").gameObject;
		Text n = m.GetComponent<Text> ();
		n.text = name;

		string screen = Response.items[0].user.screen_name;
		GameObject sn = mytimeline.transform.Find ("MyProfile/ScreenName").gameObject;
		Text snt = sn.GetComponent<Text> ();
		snt.text = "@" + screen;

		string follow = Response.items[0].user.followers_count.ToString();
		GameObject fn = mytimeline.transform.Find ("MyProfile/FollowerNum").gameObject;
		Text fnt = fn.GetComponent<Text> ();
		fnt.text = follow + "フォロワー" ;

		string friend = Response.items[0].user.friends_count.ToString();
		GameObject fdn = mytimeline.transform.Find ("MyProfile/FriendNum").gameObject;
		Text fdnt = fdn.GetComponent<Text> ();
		fdnt.text = friend + "フォロー" ;


	}
}
