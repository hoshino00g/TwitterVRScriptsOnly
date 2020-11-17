using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Networking;



namespace Twitter

{
	public class PersonRequest : MonoBehaviour
	{

		public delegate void PersonCallback(bool success, string response, int fn);
		//TwitterCallBack➡このスクリプトではこのクラスデリゲート型にしてを引数として扱っている
		//関数兼引数の役割

		public class Client

		{

			public static int fnt;


			public static IEnumerator Get(int fn, string APIPath, Dictionary<string, string> APIParams, PersonCallback callback)
			//非ジェネリックコレクション(複数の配列の中に何が来るかわからない状態）に対する単純な反復作業(型を指定）
			//第三引数TC型のcallbackは呼び出し部分のvoid Callbackをdelegate void(最初は何も入ってない）に入れる➡sendrequestに入る➡呼び出し部分のtrueorfalseが返される。
			//この瞬間上記のdelegate型の関数にvoid Callbackが入る

			{
				
				fnt = fn;
				print (fnt);

				string REQUEST_URL = "https://api.twitter.com/1.1/" + APIPath + ".json";

				SortedDictionary<string, string> parameters = Helper.ConvertToSortedDictionary(APIParams);
				//APIParamsをキーと値に加えた状態のインスタンスがparameters

				string requestURL = REQUEST_URL + "?" + Helper.GenerateRequestparams(parameters);
				//parametersをUrl化したのがrequestURL
				UnityWebRequest request = UnityWebRequest.Get(requestURL);
				//HTTPサーバーにrequestURLを使うことでアクセス
				request.SetRequestHeader("ContentType", "application/x-www-form-urlencoded");
				//requestインスタンスにHTTｐへッダを設定（名前、値）

				yield return SendRequest(fnt,request, parameters, "GET", REQUEST_URL, callback);
				//yieldで指定・・・反復子として指定して IEnumerator型の参照を省略
			}
				





			private static IEnumerator SendRequest(int fnt,UnityWebRequest request, SortedDictionary<string, string> parameters, string method, string requestURL, PersonCallback callback)

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

					callback(false, JsonHelper.ArrayToObject(request.error),fnt);
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
						callback(true, JsonHelper.ArrayToObject(request.downloadHandler.text),fnt);
						//downloadHandler.textでHTTPリクエストメッセージボディ（HTTPリクエストのメモ的存在、POSTメソッドによく使われる。
						//POST・・・今回だと他人に見られたくないパスワード等を情報として送る場合、バイナリデータを送信する場合なため使われる。）
					}

					else

					{

						Debug.Log(request.responseCode);

						callback(false, JsonHelper.ArrayToObject(request.downloadHandler.text),fnt);
						//delegate型の関数を通してvoid Callbackに上記の変数を入れる
						//Responsecodeプロパティでステータスコードを取得
						//trueの処理のためにfalseに戻しておく
						print("a");
					}

				}

			}



		



		}
	}
}
