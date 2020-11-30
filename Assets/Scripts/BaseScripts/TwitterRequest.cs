using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Networking;



namespace Twitter

{
    public class TwitterRequest : MonoBehaviour
    {

        public delegate void TwitterCallback(bool success, string response);
        //TwitterCallBack➡このスクリプトではこのクラスデリゲート型にしてを引数として扱っている
        //関数兼引数の役割

        public class Client

        {



            #region API Methods



            public static IEnumerator Get(string APIPath, Dictionary<string, string> APIParams, TwitterCallback callback)
            //非ジェネリックコレクション(複数の配列の中に何が来るかわからない状態）に対する単純な反復作業(型を指定）
            //第三引数TC型のcallbackは呼び出し部分のvoid Callbackをdelegate void(最初は何も入ってない）に入れる➡sendrequestに入る➡呼び出し部分のtrueorfalseが返される。
            //この瞬間上記のdelegate型の関数にvoid Callbackが入る
            
            {

                string REQUEST_URL = "https://api.twitter.com/1.1/" + APIPath + ".json";

                SortedDictionary<string, string> parameters = Helper.ConvertToSortedDictionary(APIParams);
                //APIParamsをキーと値に加えた状態のインスタンスがparameters

                string requestURL = REQUEST_URL + "?" + Helper.GenerateRequestparams(parameters);
                //parametersをUrl化したのがrequestURL
                UnityWebRequest request = UnityWebRequest.Get(requestURL);
                //HTTPサーバーにrequestURLを使うことでアクセス
                request.SetRequestHeader("ContentType", "application/x-www-form-urlencoded");
                //requestインスタンスにHTTｐへッダを設定（名前、値）

                yield return SendRequest(request, parameters, "GET", REQUEST_URL, callback);
                //yieldで指定・・・反復子として指定して IEnumerator型の参照を省略
            }



            public static IEnumerator Post(string APIPath, Dictionary<string, string> APIParams, TwitterCallback callback)

            {

                List<string> endpointForFormdata = new List<string>

            {//リストを作られた

				"media/upload",

                "account/update_profile_image",

                "account/update_profile_banner",

                "account/update_profile_background_image"

            };



                string REQUEST_URL = "";
                //
                if (APIPath.Contains("media/"))

                {

                    REQUEST_URL = "https://upload.twitter.com/1.1/" + APIPath + ".json";
                    //APIPathに"media/"が含まれていればREQUEST_URLのインスタンスに上の文字列を加える
                }
                else

                {

                    REQUEST_URL = "https://api.twitter.com/1.1/" + APIPath + ".json";

                }



                WWWForm form = new WWWForm();
                //WWWクラスを使用してwebサーバーにアクセスするインスタンス
                SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
                //キーと値のインスタンス


                if (endpointForFormdata.IndexOf(APIPath) != -1)
                //endpointForFormdataにIndexOfメソッド（string型のAPIPathがあれば０以上の整数で返す）
                //何かある場合はtrueを返す
                {

                    // multipart/form-data



                    foreach (KeyValuePair<string, string> parameter in APIParams)
                    //GetのDictionary型の第二引数APIParamsから引きだす
                    {

                        if (parameter.Key.Contains("media"))
                        //parameterのキーの部分に"media"があるか否か
                        {

                            form.AddBinaryData("media", Convert.FromBase64String(parameter.Value), "", "");
                            //AddBinaryData・・・（フィールド名（キーとしてのもの）,バイト配列のコンテンツ,ファイル名、mimeType(ウェブ上では拡張子を持たないため）を指定）
                            //AddBinaryDataではバイナリデータ（テキスト型ではないものでコンピュータしか読めない）をwebサーバーにポストする

                            //FromBase64String・・・Base64（64種類の英数字）を8 ビット符号なし整数配列（二進数８桁の数）に変換する

                        }


                        else if (parameter.Key == "image")

                        {

                            form.AddBinaryData("image", Convert.FromBase64String(parameter.Value), "", "");

                        }

                        else if (parameter.Key == "banner")

                        {

                            form.AddBinaryData("banner", Convert.FromBase64String(parameter.Value), "", "");

                        }

                        else

                        {

                            form.AddField(parameter.Key, parameter.Value);
                            //文字列のキーと値をフォームのフィールドに追加
                        }

                    }



                    UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL, form);

                    yield return SendRequest(request, parameters, "POST", REQUEST_URL, callback);

                }

                else

                {

                    // application/x-www-form-urlencoded



                    parameters = Helper.ConvertToSortedDictionary(APIParams);

                    foreach (KeyValuePair<string, string> parameter in APIParams)

                    {

                        form.AddField(parameter.Key, parameter.Value);

                    }



                    UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL, form);
                    //Unityからhttpサーバーに第一引数にURI（REQUEST_UR）を指定して、form(if文で指定された投稿する内容）を指定
                    request.SetRequestHeader("ContentType", "application/x-www-form-urlencoded");
                    //requestインスタンスにHTTｐへッダを設定（名前、値）(URL化している）
                    yield return SendRequest(request, parameters, "POST", REQUEST_URL, callback);



                }

            }

            #endregion



            #region RequestHelperMethods



            private static IEnumerator SendRequest(UnityWebRequest request, SortedDictionary<string, string> parameters, string method, string requestURL, TwitterCallback callback)

            {

                request.SetRequestHeader("Authorization", Oauth.GenerateHeaderWithAccessToken(parameters, method, requestURL));
                //HTTPヘッダーに"Authorization"を置く
                //リクエストヘッダ・・ブラウザからサーバーへの要望をなどを含む
                //HTTPヘッダーはHTTPリクエストの子的要素、フィールド目と実際の値をサーバーに送る
                //サーバ側で展開しているサービスを一般に公開することなく、特定のアプリやユーザからのアクセスのみ受け付けるようにする
                //Unityから特定にのTwitterというアプリに送っている
                //これを実現するためBasic認証をするため、AuthorizationキーでBasic ユーザー名:パスワード（Base64でエンコードした状態）という値で取得
                yield return request.Send();
                //リモートサーバー（Twitter の特定のアカウント）にrequestインスタンスを送っている。
                //yield return ・・・Sendが終わるまで次の行（エラーの場合の実行）は行われない。（このメソッドが呼ばれたときにほかの処理を待つ）


                if (request.isError)
                //requestインスタンスを送った時にエラーになった場合trueになる
                {

                    callback(false, JsonHelper.ArrayToObject(request.error));
					print ("A");
                    //システム エラーを説明する人間が判読できる文字列を表示
                    //falseにすることによって既定値にする。（デフォルトの状態から下記の処理でtrueにしたいためfalseにする）
                }

                else

                {

                    if (request.responseCode == 200 || request.responseCode == 201)
                    //httpステータスコードが200(リクエストは成功し、レスポンスとともに要求に応じた情報が返される)
                    //または、httpステータスコードが201（リクエストは完了し、新たに作成されたリソースのURIが返される）
                    {
                        callback(true, JsonHelper.ArrayToObject(request.downloadHandler.text));
                        //downloadHandler.textでHTTPリクエストメッセージボディ（HTTPリクエストのメモ的存在、POSTメソッドによく使われる。
                        //POST・・・今回だと他人に見られたくないパスワード等を情報として送る場合、バイナリデータを送信する場合なため使われる。）
                    }

                    else

                    {

                        Debug.Log(request.responseCode);

                        callback(false, JsonHelper.ArrayToObject(request.downloadHandler.text));
                        //delegate型の関数を通してvoid Callbackに上記の変数を入れる
                        //Responsecodeプロパティでステータスコードを取得
                        //trueの処理のためにfalseに戻しておく
						print("a");
                    }

                }

            }



            #endregion



        }
    }



}