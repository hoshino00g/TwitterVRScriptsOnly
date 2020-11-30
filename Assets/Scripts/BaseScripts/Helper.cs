using System;
using System.Collections.Generic;
using System.Text;//このスクリプトだとStringBuilderクラス（可変型の文字列を表すもの）
using System.Text.RegularExpressions;//Rexクラス（変更不可の正規表現を表す）
using System.Collections;
using UnityEngine;
	
namespace Twitter //クラスをまとめるためのフォルダ的要素（unityにはSystemなどの名前空間が元々入ってる
{ 

	public class Helper : MonoBehaviour

	{



		public static SortedDictionary<string, string> ConvertToSortedDictionary(Dictionary<string, string> APIParams)
		//SortedDictionary・・・第一引数にキー、第二引数に値、それらのコレクション
		//ConvertToSortedDictionaryとparametersをSortedDictionary型に一致させる

		{

			SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
			//parametersという変数によってSoretedDictionary というインスタンスが作られる
			foreach (KeyValuePair<string, string> APIParam in APIParams)
				//APIParamsという配列の中にKeyValuePairによってstring型のキーと値を持つ数値を取り出している。
			{

				parameters.Add(APIParam.Key, APIParam.Value);
				//KeyValuePairによって取り出したキーと値（APIParam）をparameters(SortedDictionary型）に加えている
			}

			return parameters;
			//ConvertToSortedDictionaryに加えた状態で値を返している
		}



		public static string GenerateRequestparams(SortedDictionary<string, string> parameters)
		//SortedDictionary型のparametersという引数をとるstring型のメソッドGenerateRequestparams
		{

			StringBuilder requestParams = new StringBuilder();
			//可変性の文字列を定義➡関数内で文字列を変えたいときに使う
			foreach (KeyValuePair<string, string> param in parameters)
				//KeyValuePairでstring型のキーと値を引数parametersからとっていく
			{

				requestParams.Append(Helper.UrlEncode(param.Key) + "=" + Helper.UrlEncode(param.Value) + "&");
				//Appendでparamのkey（後半は値）の部分をHelperクラスのUrlEncodeメソッドで行ったあげる
			}

			requestParams.Length -= 1; // Remove "&" at the last of string
			//"&"の部分を各keyと値に加えたあげる
			return requestParams.ToString();
			//ToStringでstring型に変換
		}
		public static string UrlEncode(string value)
		//URLで使えない表現を別の表現に置き換えてURLに対応させている。
		{

			if (string.IsNullOrEmpty(value))
				//引数valueがその文字列の中にあるか否かをIsNullOrEmptyメソッドによって判断
				//何もない場合true
			{

				return string.Empty;
				//空の文字列を返している➡この後の動作のために空の場合関数の処理を終了させている。
			}



			value = Uri.EscapeDataString(value);
			//Uri・・・URI(URL➡ウェブの場所とアクセスを指定、とURN➡ウェブの名前、の総称）をいじれるクラス、valueをuriとして扱う
			//EscapeDataStringメソッド・・・エスケープ表現（¥とかを使う表現）にvalueを変換

			value = Regex.Replace(value, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
			//Regex・・変更不可な正規表現（円記号を使うやつ）にReplaceメソッドでvalueを"(%[0-9a-f][0-9a-f])"で検索し、それを第三引数に置き換えている。
			//Matchクラス（一回の正規表現検索に一致した結果を表す）Valueで文字を取得し、ToUpperで大文字にして変換
			//Valueはstring型、ToUpperは大文字にするメソッド
			//c => c・・・ デリゲート型、Matchクラスを使うことはReplaceメソッドの中で推論できるのでうえで定義しなくてよい
			value = value

				.Replace("(", "%28")
				//Repalceメソッドで"("を"%28"に置き換えている）以下も同じ
				.Replace(")", "%29")

				.Replace("$", "%24")

				.Replace("!", "%21")

				.Replace("*", "%2A")

				.Replace("'", "%27");

			value = value.Replace("%7E", "~");

			return value;



		}

	}



}
	

