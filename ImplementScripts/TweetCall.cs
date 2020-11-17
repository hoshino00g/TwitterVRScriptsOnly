using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System;

public class TweetCall : MonoBehaviour {
	

		


	// Use this for initialization
	void Awake () {
	Twitter.Oauth.consumerKey = "a3o88LSLidiOqmNCi00aE5n7G";
	Twitter.Oauth.consumerSecret = "f8fnZmg6yTCI5SSudw3quNiniLH5HvZGAT6dZBGipmzuO56XPp";
	Twitter.Oauth.accessToken = "913988430501003264-0z07nNAyp7AIjmpqTLMKbnbAuEAmhvX";
    Twitter.Oauth.accessTokenSecret = "xUO39BKZMnUreLVedikPhL7Gk5twcCHt8x3w4MDZt1xKz";

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
