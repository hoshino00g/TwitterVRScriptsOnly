using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class WanderingMemo : MonoBehaviour {



	//float stageWidth;
	//float stageHeight;
	private float speed = 100f;
	private float rotationSmooth = 1f;

	private Vector3 targetPosition;


	private float changeTargetSqrDistance = 40f;

	private void Start()
	{
		targetPosition = GetRandomPositionOnLevel();
		print ("A");
		//stageWidth = stage.GetComponent<Renderer> ().bounds.size.x;
		//stageHeight = stage.GetComponent<Renderer> ().bounds.size.z;


	}

	private void Update()
	{
		// 目標地点との距離が小さければ、次のランダムな目標地点を設定する
		float sqrDistanceToTarget = Vector3.SqrMagnitude(transform.position - targetPosition);
		if (sqrDistanceToTarget < changeTargetSqrDistance)
		{
			targetPosition = GetRandomPositionOnLevel();
		}

		// 目標地点の方向を向く
		Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);

		// 前方に進む
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
		print ("ASSA");
	}

	public static Vector3 GetRandomPositionOnLevel()

	{
		System.Random a = new System.Random();
		System.Random b = new System.Random();
		System.Random c = new System.Random();

		int aj = a.Next (-9612, 9937);
		int bj = b.Next (21, 20717);
		int cj = c.Next (-5259, 9226);
		return new Vector3(aj,bj,cj);
	}


}
