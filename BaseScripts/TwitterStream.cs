using System.Collections;

using System.Collections.Generic;

using System.Text;

using UnityEngine;

using UnityEngine.Networking;



namespace Twitter

{
    public class TwitterStream : MonoBehaviour
    {






        public delegate void TwitterStreamCallback(string response, StreamMessageType messageType);
        //デリゲート型関数・・・型を指定してその他の関数に引数として入れることができる（関数の中に関数を入れるイメージ）
        //第二引数・・・enum型なので列挙されている要素のどれかがmessageTypeに入る

        public class Stream

        {



            private string REQUEST_URL;

            private UnityWebRequest request;



            public Stream(StreamType type)
            //StreamType は列挙型なので要素のどれかがtypeに入る
            {

                string[] endpoints = { "statuses/filter", "statuses/sample", "user", "site" };
                //string型の配列


                if (type == StreamType.PublicFilter || type == StreamType.PublicSample)
                //StreamTypeのプロパティがPublicFilterかPublicSampleであった場合
                {

                    REQUEST_URL = "https://stream.twitter.com/1.1/" + endpoints[(int)type] + ".json";

                }
                else if (type == StreamType.User)

                {

                    REQUEST_URL = "https://userstream.twitter.com/1.1/user.json";

                }
                else if (type == StreamType.Site)

                {

                    REQUEST_URL = "https://sitestream.twitter.com/1.1/site.json";

                }//プロパティごとに条件分岐してURLを作成




            }



            public IEnumerator On(Dictionary<string, string> APIParams, TwitterStreamCallback callback)
            //Onメソッドにキーと値の形のAPIParamsと
            //TwitterStreamCallback型（届いているかどうかを条件分岐して反応を出力する型）のcallbackを指定
            {

                SortedDictionary<string, string> parameters = Helper.ConvertToSortedDictionary(APIParams);
                //APIParamsにキーと値を加えたものがparameters


                WWWForm form = new WWWForm();
                //webサーバーに情報を送るクラスのformというインスタンスを生成
                foreach (KeyValuePair<string, string> parameter in APIParams)

                {

                    form.AddField(parameter.Key, parameter.Value);
                    //formというインスタンスにparameterのキーと値のプロパティを追加
                }



                request = UnityWebRequest.Post(REQUEST_URL, form);
                //uriにform(キーと値を追加したウェブサーバーに情報を送るインスタンス）を送る
                request.SetRequestHeader("ContentType", "application/x-www-form-urlencoded");
                //requestインスタンスにHTTｐへッダを設定（名前、値）(URL化している）
                request.SetRequestHeader("Authorization", Oauth.GenerateHeaderWithAccessToken(parameters, "POST", REQUEST_URL));
                //HTTPヘッダーに"Authorization"を置く
                //リクエストヘッダ・・ブラウザからサーバーへの要望をなどを含む
                //HTTPヘッダーはHTTPリクエストの子的要素、フィールド目と実際の値をサーバーに送る
                //サーバ側で展開しているサービスを一般に公開することなく、特定のアプリやユーザからのアクセスのみ受け付けるようにする
                //Unityから特定にのTwitterというアプリに送っている
                //これを実現するためBasic認証をするため、AuthorizationキーでBasic ユーザー名:パスワード（Base64でエンコードした状態）という値で取得
                //"POST"メソッドではurl上に見えない形で情報をくっつけている。
                request.downloadHandler = new StreamingDownloadHandler(callback);
                // HTTP レスポンスボディデータ(HTTPリクエストメッセージボディの情報）を取得し、
                //StreamingDownloadHandler(DownloadHandlerScriptを継承しているスクリプト）の新たなインスタンスを生成
                //DownloadHandler・・・リモートサーバーから受信した HTTP レスポンスボディデータ(HTTPリクエストメッセージボディの情報）のプロセスと管理
                //スクリプトによってプログラムが動く
                //DownloadHandlerScriptはインスタンスを生成出来ないクラスのこと（継承させてメソッドを上書きして使うことが前提）

                yield return request.Send();
                //リモートサーバー（Twitter の特定のアカウント）にrequestインスタンスを送っている。

            }



            public void Off()

            {

                Debug.Log("Connection Aborted");

                request.Abort();
                //UnityWebRequestで送るのをやめる
            }





        }



        #region DownloadHandler



        public class StreamingDownloadHandler : DownloadHandlerScript

        {


            //プロパティを設定
            TwitterStreamCallback callback;

            StreamMessageType messageType;



            public StreamingDownloadHandler(TwitterStreamCallback callback)
            //インスタンス生成と同時に呼ばれるメソッドのこと
            {

                this.callback = callback;
                //StreamingDownloadHandlerのインスタンスのcallbackプロパティにTwitterStreamCallback型の変数（bool型、string型の引数を含む関数を呼び出す）を入れる。
            }



            protected override bool ReceiveData(byte[] data, int dataLength)
            //protected ・・・継承関係にあるクラス内だけで使えるという意味
            //DownloadHandlerScriptにあるprotected bool ReceiveDataを上書き
            //第一引数(バイト型配列）・・・リモートサーバ（おそらくこの場合Twitter）から受信したデータを記憶している
            //第二引数・・・新しくなったdataのバイト数
            {

                if (data == null || data.Length < 1)
                //受けとったデータがなかったか、１つ以下だった場合
                {

                    Debug.Log("LoggingDownloadHandler :: ReceiveData - received a null/empty buffer");

                    return false;

                }

                string response = Encoding.ASCII.GetString(data);
                //ASCIIは文字を半角英数字と記号を含むものに符号化したもの
                //responseインスタンスではASCIIをバイト型に変換されている
                //バイト型配列に変換されているdataを文字列にデコードして変換（URLの状態に戻す）
                response = response.Replace("\"event\":", "\"event_name\":");
                //置き換え
                messageType = StreamMessageType.None;
                //メッセージがなかった場合、このクラスのmessageTypeプロパティをNoneにする
                //初期値としての設定？（この後で設定するから）
                CheckMessageType(response);
                //下記のレスポンス(url)を行う
                //MessageTypeを識別してやってる挙動を確認


                try

                {

                    callback(JsonHelper.ArrayToObject(response), messageType);
                    //delegate内に登録するメソッド呼び出し用のスクリプトに例外がでたとき
                    return true;

                }
                catch (System.Exception e)//データが使えるかどうか、messagetypeと合っているか否か
                                          //実働部分void OnStreamメソッドが実行できなかった場合
                {

                    Debug.Log("ReceiveData Error : " + e.ToString());

                    return true;

                }



            }



            private void CheckMessageType(string data)

            {

                try

                {

                    TwitterJson.Tweet tweet = JsonUtility.FromJson<TwitterJson.Tweet>(data);
                    //JsonUtilityクラス・・・JSONデータを操作するためのUtility関数
                    //JSONデータ・・・テキストベースのデータ ex "name":"星野"
                    //JavaScriptのオブジェクト表記方法
                    //FromJsonメソッド・・・そのオブジェクト表記からオブジェクトを作成
                    //<Tweet>の型の指定・・・任意の型の指定
                    if (tweet.text != null && tweet.id_str != null)

                    {//あったらそのメッセージタイプをメソッドに返す


                        messageType = StreamMessageType.Tweet;

                        return;

                    }



                    TwitterJson.StreamEvent streamEvent = JsonUtility.FromJson<TwitterJson.StreamEvent>(data);

                    if (streamEvent.event_name != null)

                    {

                        messageType = StreamMessageType.StreamEvent;

                        return;

                    }



                    TwitterJson.FriendsList friendsList = JsonUtility.FromJson<TwitterJson.FriendsList>(data);

                    if (friendsList.friends != null)

                    {

                        messageType = StreamMessageType.FriendsList;

                        return;

                    }



                    TwitterJson. DirectMessage directMessage = JsonUtility.FromJson<TwitterJson.DirectMessage>(data);

                    if (directMessage.recipient_screen_name != null)

                    {

                        messageType = StreamMessageType.DirectMessage;

                        return;

                    }



                    TwitterJson. StatusDeletionNotice statusDeletionNotice = JsonUtility.FromJson<TwitterJson.StatusDeletionNotice>(data);

                    if (statusDeletionNotice.delete != null)

                    {

                        messageType = StreamMessageType.StatusDeletionNotice;

                        return;

                    }



                    TwitterJson.LocationDeletionNotice locationDeletionNotice = JsonUtility.FromJson<TwitterJson.LocationDeletionNotice>(data);

                    if (locationDeletionNotice.scrub_geo != null)

                    {

                        messageType = StreamMessageType.LocationDeletionNotice;

                        return;

                    }



                    TwitterJson. LimitNotice limitNotice = JsonUtility.FromJson<TwitterJson.LimitNotice>(data);

                    if (limitNotice.limit != null)

                    {

                        messageType = StreamMessageType.LimitNotice;

                        return;

                    }



                    TwitterJson.WithheldContentNotice withheldContentNotice = JsonUtility.FromJson<TwitterJson.WithheldContentNotice>(data);

                    if (withheldContentNotice.status_withheld != null || withheldContentNotice.user_withheld != null)

                    {

                        messageType = StreamMessageType.WithheldContentNotice;

                        return;

                    }



                    TwitterJson. DisconnectMessage disconnectMessage = JsonUtility.FromJson<TwitterJson.DisconnectMessage>(data);

                    if (disconnectMessage.disconnect != null)

                    {

                        messageType = StreamMessageType.DisconnectMessage;

                        return;

                    }



                    TwitterJson. StallWarning stallWarning = JsonUtility.FromJson<TwitterJson.StallWarning>(data);

                    if (stallWarning.warning != null)

                    {

                        messageType = StreamMessageType.StallWarning;

                        return;

                    }



                    messageType = StreamMessageType.None;

                    return;



                }
                catch (System.Exception e)

                {

                    Debug.Log("CheckMessageType Error : " + e.ToString());

                    messageType = StreamMessageType.None;

                    return;

                }

            }

        }



        #endregion





        #region Parameters for statuses/filter

        public class FilterTrack

        {

            private List<string> tracks;



            public FilterTrack(string track)
            //コンストラクタ
            {

                tracks = new List<string>();

                tracks.Add(track);
                //リストにtrackを追加
            }

            public FilterTrack(List<string> tracks)

            {

                this.tracks = tracks;

            }

            public void AddTrack(string track)

            {

                tracks.Add(track);

            }

            public void AddTracks(List<string> tracks)

            {

                foreach (string track in tracks)

                {

                    this.tracks.Add(track);

                }

            }

            public string GetKey()

            {

                return "track";
                //"track"を追加
            }

            public string GetValue()

            {

                StringBuilder sb = new StringBuilder();

                foreach (string track in tracks)

                {

                    sb.Append(track + ",");
                    //取り出して文字列を追加
                }

                sb.Length -= 1;

                return sb.ToString();
                //可変性の文字列の要素を一個消して、sbをstring型に変換
            }

        }





        public class FilterLocations

        {

            private List<Coordinate> locations;



            public FilterLocations()

            {

                locations = new List<Coordinate>();

                locations.Add(new Coordinate(-180f, -90f));

                locations.Add(new Coordinate(180f, 90f));

            }

            public FilterLocations(Coordinate southwest, Coordinate northeast)

            {

                locations = new List<Coordinate>();
                //latとlngで指定
                locations.Add(southwest);

                locations.Add(northeast);

            }

            public void AddLocation(Coordinate southwest, Coordinate northeast)

            {

                locations.Add(southwest);

                locations.Add(northeast);

            }

            public string GetKey()

            {

                return "locations";

            }

            public string GetValue()

            {

                StringBuilder sb = new StringBuilder();

                foreach (Coordinate location in locations)

                {

                    sb.Append(location.lng.ToString("F1") + "," + location.lat.ToString("F1") + ",");

                }

                sb.Length -= 1;

                return sb.ToString();

            }

        }



        public class Coordinate

        {

            public float lng { get; set; }
            //これは、
            //private string __name;
            //public string lng
            //{
            //get { return this.__name; }
            //set { this.__name = value; }
            //}
            //プライベートにすることによってプロパティを内部保持できる

            public float lat { get; set; }



            public Coordinate(float lng, float lat)

            {

                this.lng = lng;

                this.lat = lat;

            }

        }



        public class FilterFollow

        {

            private List<string> screen_names;

            private List<long> ids;



            public FilterFollow(List<string> screen_names)

            {

                this.screen_names = screen_names;

            }

            public FilterFollow(List<long> ids)

            {

                this.ids = ids;

            }

            public FilterFollow(long id)

            {

                ids = new List<long>();

                ids.Add(id);

            }

            public void AddId(long id)

            {

                ids.Add(id);

            }

            public void AddIds(List<long> ids)

            {

                foreach (long id in ids)

                {

                    this.ids.Add(id);

                }

            }

            public string GetKey()

            {

                return "follow";

            }

            public string GetValue()

            {

                StringBuilder sb = new StringBuilder();

                if (ids.Count > 0)

                {

                    foreach (long id in ids)

                    {

                        sb.Append(id.ToString() + ",");

                    }

                }
                else

                {

                    foreach (string screen_name in screen_names)

                    {

                        sb.Append(screen_name + ",");

                    }

                }

                sb.Length -= 1;

                return sb.ToString();

            }

        }

        #endregion
    }

}
