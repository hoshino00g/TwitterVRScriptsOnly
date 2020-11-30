using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonTweetsDraw : MonoBehaviour {

	// Use this for initialization
	public static void PersonTextDraw (GameObject prefabs,string retweet,string favorite, string time, string text) {
	    
		for (int fc = 0; fc <= 8; fc++) {
			GameObject r = prefabs.transform.Find ("FriendTweets/FriendTweet" + fc + "/ReTweet/ReTweetNum").gameObject;
		     Text retweetT = r.GetComponent<Text> ();//"TimeLineEarth/TimeLineTweets/OneOtherTweet" + i + "/ReTweet/
		     retweetT.text = retweet;
		     //ふぁぼ数の表示
			GameObject k = prefabs.transform.Find ("FriendTweets/FriendTweet" + fc + "/Favorite/FavoriteNum").gameObject;
		     Text favoriteT = k.GetComponent<Text> ();
		     favoriteT.text = favorite;
		     //ツイート時刻の表示
		     GameObject t = prefabs.transform.Find ("FriendTweets/FriendTweet" + fc + "/Time").gameObject;
		     Text timeT = t.GetComponent<Text> ();
		     timeT.text = time;
		     //以下はツイートテキストの表示
		     Debug.Log (text);
		     GameObject l = prefabs.transform.Find ("FriendTweets/FriendTweet" + fc + "/TweetText").gameObject;
		     Text tweetLabel = l.GetComponent<Text> ();
		     if (text.Length > 32) {
			     GameObject m = prefabs.transform.Find ("FriendTweets/FriendTweet" + fc + "/TweetText").gameObject;
			     Text changefont1 = m.GetComponent<Text> ();
			     changefont1.fontSize = 7;
			     tweetLabel.text = text;
			     if (text.Length > 66) {
				 GameObject n = prefabs.transform.Find ("FriendTweets/FriendTweet" + fc + "/TweetText").gameObject;
				 Text changefont2 = n.GetComponent<Text> ();
				 changefont2.fontSize = 5;
				 tweetLabel.text = text;
			         } else {
				       tweetLabel.text = text;
			           }
		          } else {
			        tweetLabel.text = text;
		            }
	          }
	}

	
	// Update is called once per frame
	public static void PersonImgDraw (GameObject prefabs, string imgurl) {
		
	}
}
