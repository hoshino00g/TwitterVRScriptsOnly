using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class WanderingCrowd : MonoBehaviour {



	//float stageWidth;
	//float stageHeight;
	private float speed = 10000f;
	private float rotationSmooth = 1f;

	private Vector3 targetPosition;


	private float changeTargetSqrDistance = 40f;

	private void Start()
	{
		targetPosition = GetRandomPositionOnLevel();

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

	}

	public static Vector3 GetRandomPositionOnLevel()

	{
		
		return new Vector3(Random.Range (-9612, 9937),Random.Range(21, 20717),Random.Range(-5259, 9226));
	}


}

