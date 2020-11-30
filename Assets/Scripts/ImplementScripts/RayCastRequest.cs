using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Twitter;
using System.Text;
using Unity.Linq;


public class RayCastRequest : MonoBehaviour {

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

    void RayTest()
    {
        //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
        Ray ray = new Ray (transform.position, new Vector3 (0, -1, 0));

        //Rayが当たったオブジェクトの情報を入れる箱
        RaycastHit hit;

        //Rayの飛ばせる距離
        int distance = 10;

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　　　　↓Rayの色
        Debug.DrawLine (ray.origin, ray.direction * distance, Color.red);

        //もしRayにオブジェクトが衝突したら
        //                  ↓Ray  ↓Rayが当たったオブジェクト ↓距離
        if (Physics.Raycast(ray,out hit,distance))
        {
              //Rayが当たったオブジェクトのtagがPlayerだったら
                if (hit.collider.tag == "Player")
                Debug.Log ("RayがPlayerに当たった");
        }
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
					GameObject l = tweetwithimgprefab.transform.Find ("TweetText").gameObject;
					Text tweetLabel = l.Children;//Linq
					if (Response.items [i].text.Length > 32) {
						GameObject m = tweetwithimgprefab.transform.Find ("TweetText").gameObject;
						Text changefont1 = m.Children;///Linq
						changefont1.fontSize = 7;
						tweetLabel.text = Response.items [i].text;
						if (Response.items [i].text.Length > 66) {
							GameObject n = tweetwithimgprefab.transform.Find ("TweetText").gameObject;
							Text changefont2 = n.Children;////Linq
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