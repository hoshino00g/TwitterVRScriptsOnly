using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twitter;
using UnityEngine.UI;

public class SeeMyFriendTextAgain : MonoBehaviour {

	public long friendrealid;
	public GameObject friends;

	void Start () {
		Dictionary<string, string> parameters1 = new Dictionary<string, string> ();
		parameters1 ["count"] = 5.ToString ();
		StartCoroutine (TwitterRequest.Client.Get ("friends/list", parameters1, this.Callback));
		Dictionary<string, string> parameters2 = new Dictionary<string, string> ();
		parameters2 ["count"] = 10.ToString ();
		parameters2 ["id"] = friendrealid.ToString ();
		StartCoroutine (TwitterRequest.Client.Get ("statuses/home_timeline", parameters2, this.Callback));
	}

	void Callback(bool success, string response){

		if (success) {
			TwitterJson.FriendsListResponse Response = JsonUtility.FromJson<TwitterJson.FriendsListResponse> (response);
			friendrealid = Response.users [0].id;
			FriendTextRefering (friendrealid);
		}else{
			Debug.Log (response);
	}
	}

	void FriendTextRefering(long friendrealid){
		Dictionary<string, string> parameters2 = new Dictionary<string, string> ();
		parameters2 ["count"] = 10.ToString ();
		parameters2 ["id"] = friendrealid.ToString ();
		StartCoroutine (TwitterRequest.Client.Get ("statuses/user_timeline", parameters2, this.Representing));
	}
	//ここからコピペ
	void Representing(bool success, string response)//string型のresponseにはTwitterRequestのrequest.downloadHandler.textが入る
	{
		if (success) {
			TwitterJson.StatusesUserTimelineResponse Response = JsonUtility.FromJson<TwitterJson.StatusesUserTimelineResponse> (response);
			for (int fc = 0; fc <= 8; fc++) {
				string retweet = Response.items [fc].retweet_count.ToString ();
				//print (Indicate.items [0].text);
				GameObject r = friends.transform.Find ("Friend/FriendTweets/FriendTweet" + fc + "/ReTweet/ReTweetNum").gameObject;
				Text retweetT = r.GetComponent<Text> ();//"TimeLineEarth/TimeLineTweets/OneOtherTweet" + i + "/ReTweet/
				retweetT.text = retweet;
				//ふぁぼ数の表示
				string favorite = Response.items [fc].favorite_count.ToString ();
				GameObject k = friends.transform.Find ("Friend/FriendTweets/FriendTweet" + fc + "/Favorite/FavoriteNum").gameObject;
				Text favoriteT = k.GetComponent<Text> ();
				favoriteT.text = favorite;
				//ツイート時刻の表示
				string time = Response.items [fc].created_at;
				GameObject t = friends.transform.Find ("Friend/FriendTweets/FriendTweet" + fc + "/Time").gameObject;
				Text timeT = t.GetComponent<Text> ();
				timeT.text = time;
				//以下はツイートテキストの表示
				Debug.Log (Response.items [fc].text);
				GameObject l = friends.transform.Find ("Friend/FriendTweets/FriendTweet" + fc + "/TweetText").gameObject;
				Text tweetLabel = l.GetComponent<Text> ();
				if (Response.items [fc].text.Length > 32) {
					GameObject m = friends.transform.Find ("Friend/FriendTweets/FriendTweet" + fc + "/TweetText").gameObject;
					Text changefont1 = m.GetComponent<Text> ();
					changefont1.fontSize = 7;
					tweetLabel.text = Response.items [fc].text;
					if (Response.items [fc].text.Length > 66) {
						GameObject n = friends.transform.Find ("Friend/FriendTweets/FriendTweet" + fc + "/TweetText").gameObject;
						Text changefont2 = n.GetComponent<Text> ();
						changefont2.fontSize = 5;
						tweetLabel.text = Response.items [fc].text;
					} else {
						tweetLabel.text = Response.items [fc].text;
					}
				} else {
					tweetLabel.text = Response.items [fc].text;
				}
			}
		}else{
			Debug.Log (response);
		}
	}
}
	