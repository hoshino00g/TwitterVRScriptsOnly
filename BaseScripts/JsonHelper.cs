using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class JsonHelper : MonoBehaviour {



	public static string ArrayToObject (string arrayString) {//arrayStringというstring型の引数を定義

		if (arrayString.StartsWith ("[")) {//arraystringという文字列インスタンスの先頭が"["と一致する場合にif文がtrue

			arrayString = "{\"items\":" + arrayString + "}";

			return arrayString;

		} else {

			return arrayString;//何もない時はそのままでメソッドに返す

		}

	}

}