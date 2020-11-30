using System;

using System.Collections.Generic;

using System.Security.Cryptography;

using System.Text;

using UnityEngine;

using System.Collections;



namespace Twitter

{

	public class Oauth : MonoBehaviour


	{   

		//個人の情報にアクセスするためのアクセスキー

		#region Tokens

		public static string consumerKey { get; set; }
		//get・・・consumerKeyを読み込んでいる
		//set・・・consumerKeyに個人の固有のキーという新しい値を割り当てるためsetを行う(条件を指定する場合）
		//プロパティ｛｝でクラス内で定義される
		public static string consumerSecret { get; set; }

		public static string accessToken { get; set; }

		public static string accessTokenSecret { get; set; }

		#endregion



		#region Public Method

		public static string GenerateHeaderWithAccessToken(SortedDictionary<string, string> parameters, string requestMethod, string requestURL)
		//SortedDictionary型の変数の指定（３つ）
		{

			string signature = GenerateSignatureWithAccessToken(parameters, requestMethod, requestURL);
			//signiture によってGenerateSignatureWithAccessTokenが呼び出される

			StringBuilder requestParamsString = new StringBuilder();
			//可変のstring型の変数
			foreach (KeyValuePair<string, string> param in parameters)
				//KeyValuePairはSortedDictionary型の引数で多く用いられるっぽい
			{

				requestParamsString.Append(String.Format("{0}=\"{1}\",", Helper.UrlEncode(param.Key), Helper.UrlEncode(param.Value)));
				//Append(string)・・・（）内の文字列のコピーをrequestParamsStringというインスタンスに入れている
				//Object型はすべての方の親
				//"{0}=\"{1}\","・・・０には第二引数（url本体を呼びだす用のキー）、１には第三引数（URL本体）
				//""でURLが文字列として取得できるように（バックスラッシュは"を文字列に含めたいときに使う。
			}



			string authHeader = "OAuth realm=\"Twitter API\",";

			string requestSignature = String.Format("oauth_signature=\"{0}\"", Helper.UrlEncode(signature));
			//インスタンスsignitureをURL化したものをURLのフォーマットの中に入れる
			authHeader += requestParamsString.ToString() + requestSignature;
		
			return authHeader;

		}



		#endregion



		#region HelperMethods



		private static string GenerateSignatureWithAccessToken(SortedDictionary<string, string> parameters, string requestMethod, string requestURL)

		{

			AddDefaultOauthParams(parameters, consumerKey);

			parameters.Add("oauth_token", accessToken);

			//SortedDictionary型のparametersにstring型のキーと値をAddメソッドで追加

			StringBuilder paramString = new StringBuilder();

			foreach (KeyValuePair<string, string> param in parameters)

			{

				paramString.Append(Helper.UrlEncode(param.Key) + "=" + Helper.UrlEncode(param.Value) + "&");
				//Append(引数一つ）・・・paramStringというインスタンスにstring型のコピーを追加
			}

			paramString.Length -= 1; // Remove "&" at the last of string





			string requestHeader = Helper.UrlEncode(requestMethod) + "&" + Helper.UrlEncode(requestURL);
			//requestHeaderはUrl化されたSortedDictionary型のrequestMethodとrequestURLをつなぎ合わせたもの
			string signatureData = requestHeader + "&" + Helper.UrlEncode(paramString.ToString());
			//requestHeaderとUrl化されたparamString


			string signatureKey = Helper.UrlEncode(consumerSecret) + "&" + Helper.UrlEncode(accessTokenSecret);

			HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes(signatureKey));
			//計算されたハッシュベースメッセージ認証コードを利用してSHA１入力を行う。
			//おそらくhmacsha1というハッシュ値（規則性のない固定長の値）のインスタンスを作ってるっぽぃ
			//Encoding・・複数の文字集合を同時に扱うために規格を決めている
			//ASCIIプロパティ・・７ビットの文字をエンコーディングとして取得
			//ASCIIは文字を半角英数字と記号を含むものに符号化したもの
			//GetBytesメソッド・・派生クラスでオーバーライドされた場合、指定した文字列をバイトの形に変換
			byte[] signatureBytes = hmacsha1.ComputeHash(Encoding.ASCII.GetBytes(signatureData));
			//バイト型配列のsignatureBytes
			//signitureDataをバイト型にしたものをハッシュ値として計算
			//ここら辺の流れ
			//consumerSecretとaccessTokenSecretでキーを取得
			//consumerKeyとaccessTokenでデータ（値）を取得
			//ComputeHashでバイトをハッシュ値に計算
			//キーが入力される➡それをハッシュ値として取得➡それを使ってデータのハッシュ値のインスタンスを取得
			//signatureBytesは８ビットの整数の配列の状態
			return Convert.ToBase64String(signatureBytes);
			//signatureBytesを６４種類の印字可能な英数字のみを用いるBase64の文字列形式にする
		}
		private static void AddDefaultOauthParams(SortedDictionary<string, string> parameters, string consumerKey)

		{

			parameters.Add("oauth_consumer_key", consumerKey);

			parameters.Add("oauth_signature_method", "HMAC-SHA1");

			parameters.Add("oauth_timestamp", GenerateTimeStamp());

			parameters.Add("oauth_nonce", GenerateNonce());

			parameters.Add("oauth_version", "1.0");

		}



		private static string GenerateTimeStamp()

		{

			TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			//TimeSpanクラス・・時間間隔を表す
			//DateTimeクラス（日付や時刻を表すクラス）をUtcNow（世界協定時刻でDateTimeクラスを表すメソッド）
			//DateTimeクラス・・・年、月、日、時、分、秒、およびミリ秒を指定
			return Convert.ToInt64(ts.TotalSeconds).ToString();
			//tsを秒数化したものを符号付き 64 ビット整数（string型）に変換
		}



		private static string GenerateNonce()

		{

			return new System.Random().Next(123400, int.MaxValue).ToString("X");
			//123400からint32の最大有効値（2147483647)をランダムに選び、１６進数のstirng型に変換
		}



		#endregion



	}







}
	


