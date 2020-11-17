using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Twitter;

public class SeeMyFriendImg : MonoBehaviour {
	public GameObject friends;
	public string otherid;


	// Use this for initialization
	void Start () {
		Dictionary<string, string> parameters1 = new Dictionary<string, string> ();
		parameters1 ["count"] = 5.ToString ();


		StartCoroutine (TwitterRequest.Client.Get ("friends/list", parameters1, this.ImgRepresent));

	}
	
	// Update is called once per frame
	void ImgRepresent (bool success, string response) {
		if (success) {
			TwitterJson.FriendsListResponse Response = JsonUtility.FromJson<TwitterJson.FriendsListResponse> (response);
			//TwitterJson.StatusesUserTimelineResponse Indicate = JsonUtility.FromJson<TwitterJson.StatusesUserTimelineResponse> (response);
			string friendImg = Response.users [0].profile_image_url;
			StartCoroutine (this.FriendImageRepresent (friendImg));
			for (int fc = 0; fc <= 8; fc++) {
				StartCoroutine(this.TweetIconImageRepresent (friendImg,fc));
			}
		    //string testtweet = Response.users [0].items[1].text;
			//print (testtweet + "A");
			otherid = Response.users [0].id.ToString();
			print (otherid);
			Userparameterdo (Response);
			//Dictionary<string, string> parameters2 = new Dictionary<string, string> ();
			//parameters2 ["count"] = 5.ToString ();
			//StartCoroutine (TwitterRequest.Client.Get ("statuses/user_timeline", parameters2, this.UserTimeline));

			} else {
			Debug.Log (response);

		}
	}


	void Userparameterdo (TwitterJson.FriendsListResponse Response)
	{   string name = Response.users [0].name;
		GameObject m = friends.transform.Find ("Friend/FriendTweets/FriendProfile/Name").gameObject;
		Text n = m.GetComponent<Text> ();
		n.text = name;

		string screen = Response.users [0].screen_name;
		GameObject sn = friends.transform.Find ("Friend/FriendTweets/FriendProfile/ScreenName").gameObject;
		Text snt = sn.GetComponent<Text> ();
		snt.text = "@" + screen;

		string follow = Response.users [0].followers_count.ToString();
		GameObject fn = friends.transform.Find ("Friend/FriendTweets/FriendProfile/FollowerNum").gameObject;
		Text fnt = fn.GetComponent<Text> ();
		fnt.text = follow + "フォロワー" ;

		string friend = Response.users [0].friends_count.ToString();
		GameObject fdn = friends.transform.Find ("Friend/FriendTweets/FriendProfile/FriendNum").gameObject;
		Text fdnt = fdn.GetComponent<Text> ();
		fdnt.text = friend + "フォロー" ;


	}



		IEnumerator FriendImageRepresent (string friendImg)
		{
		for ( int i = 0; i <= 4; i++){
		WWW www = new WWW(friendImg);
		yield return www;
		GameObject f = friends.transform.Find ("Friend/Face/Canvas" + i + "/RawImage").gameObject;
		RawImage rawImage = f.GetComponent<RawImage>();
		rawImage.texture = www.textureNonReadable;
		//textureNonReadable・・・ダウンロードしたデータからピクセルデータの読み込みができないTexture2Dを生成し返す
		rawImage.SetNativeSize ();
		}
		}
	IEnumerator TweetIconImageRepresent (string friendImg, int fc)
	{

		WWW www = new WWW(friendImg);
		yield return www;
		GameObject o = friends.transform.Find ("Friend/FriendTweets/FriendTweet" + fc + "/IconImage").gameObject;
		RawImage rawImage = o.GetComponent<RawImage>();
		rawImage.texture = www.textureNonReadable;
		GameObject m = friends.transform.Find ("Friend/FriendTweets/FriendProfile/IconImage").gameObject;
		RawImage rawImage1 = m.GetComponent<RawImage>();
		rawImage1.texture = www.textureNonReadable;
		//textureNonReadable・・・ダウンロードしたデータからピクセルデータの読み込みができないTexture2Dを生成し返す
		rawImage.SetNativeSize ();

	}

}

