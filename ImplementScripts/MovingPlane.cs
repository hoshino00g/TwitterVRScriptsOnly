using UnityEngine;

using System.Collections;



public class MovingPlane : MonoBehaviour

{



	[SerializeField, Range(0, 30)]

	float time = 1;



	[SerializeField]
	//インスペクタービューから編集可能にしている
	public Vector3	endPosition;



	//[SerializeField]

	//AnimationCurve curve;



	private float startTime;

	private Vector3 startPosition;



	void OnEnable ()
	//Start関数の前に呼ばれるメソッド
	//Awake,OnEnable,OnLevelWasLoaded,Startの順番

	{

		if (time <= 0) {
			//インスペクター上で設定したタイムの値が０の時
			transform.position = endPosition;

			enabled = false;
			//移動中はインスペクター上での編集を禁ずる
			return;

		}



		startTime = Time.timeSinceLevelLoad;
		//フレームが開始された時間をstartTimeに入れる
		//timeSinceLevelLoadプロパティ→このメソッドが呼ばれたフレームの時間
		startPosition = transform.position;

	}



	void Update ()

	{

		var diff = Time.timeSinceLevelLoad - startTime;
		//diffには経過時間が入っている
		if (diff > time) {
			//経過時間がtimeより大きくなった瞬間に目的地に着き、そのフレームの間はコントローラとユーザの対話を禁ずる
			transform.position = endPosition;

			enabled = false;

		}



		var rate = diff / time;
		//設定時間に対する時間の進行具合をrateに入れる
		//var pos = curve.Evaluate(rate);



		transform.position = Vector3.Lerp (startPosition, endPosition, rate);
		//Vector3.Lerpメソッドで開始地点と目的地点の点を取る
		//その点をtransform.positionに入れる
		//transform.position = Vector3.Lerp (startPosition, endPosition, pos);

	}



	void OnDrawGizmosSelected ()

	{

		#if UNITY_EDITOR



		if( !UnityEditor.EditorApplication.isPlaying || enabled == false ){

			startPosition = transform.position;

		}



		UnityEditor.Handles.Label(endPosition, endPosition.ToString());

		UnityEditor.Handles.Label(startPosition, startPosition.ToString());

		#endif

		Gizmos.DrawSphere (endPosition, 0.1f);

		Gizmos.DrawSphere (startPosition, 0.1f);



		Gizmos.DrawLine (startPosition, endPosition);

	}

}