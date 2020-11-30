﻿using UnityEngine;
using System.Collections;

public class MovingOneself : MonoBehaviour {

	public bool CanMove = true;
	public bool CanMoveForward = true;
	public bool CanMoveBack = true;
	public bool CanMoveLeft = true;
	public bool CanMoveRight = true;
	public bool CanMoveUp = true;
	public bool CanMoveDown = true;
	public bool CanRotateYaw = true;
	public bool CanRotatePitch = true;
	public bool CanRotateRoll = true;

	public float MovementSpeed = 100f;
	public float RotationSpeed = 100f;

	private bool canTranslate;
	private bool canRotate;

	void Start() {
		canTranslate = CanRotateYaw || CanRotatePitch || CanRotateRoll;
		//canTranslateの値をデフォルトでtrue
		canRotate = CanMoveForward || CanMoveBack || CanMoveRight || CanMoveLeft || CanMoveUp || CanMoveDown;
	    //canRotateの値をデフォルトでtrueに
	}


	void Update() {

	}

	void FixedUpdate() {
		//FixedUpdate...Update関数で物理挙動を行うのに対し、FixedUpdateはその物理挙動の前に行われる。
		//今回の場合だとUpdatePosition内の関数が呼ばれる前にデフォルトでCanMoveを設定している。
		//1フレームで呼ばれる回数決まっている
		if( CanMove ) {
			UpdatePosition();
		}
	}

	void UpdatePosition() {
		if( canRotate ) {
			//canRotateがtrueの時
			Quaternion AddRot = Quaternion.identity;
			float yaw = 0;
			float pitch = 0;
			float roll = 0;
			if( CanRotateYaw ) {
				yaw = Input.GetAxis( "Yaw" ) * ( Time.fixedDeltaTime * RotationSpeed );
				//yawにfloat型の変数を変えす
				//deltatime...1/フレーム数の時間で計算（ゲームの重さによって変わる）、、、fixeddeltatimeは固定されている
			    //InputManagerから設定する
			}
			if( CanRotatePitch ){
				pitch = Input.GetAxis( "Pitch" ) * ( Time.fixedDeltaTime * RotationSpeed );
			}
			if( CanRotateRoll ){
				roll = Input.GetAxis( "Roll" ) * ( Time.fixedDeltaTime * RotationSpeed );
			}
			AddRot.eulerAngles = new Vector3( -pitch, yaw, -roll );
			//x, y, z方向の角度を設定
			GetComponent<Rigidbody>().rotation *= AddRot;
			// 演算子 x　*=  y ... x = x * yとなる
		}

		// Translation
		if( canTranslate ){
			// Check key input
			int[] input = new int[ 6 ]; // Forward, Back, Left, Right, Up, Down
			if( CanMoveForward && Input.GetKey( KeyCode.U ) ) {
				input[ 0 ] = 1;
			} else if( CanMoveBack && Input.GetKey( KeyCode.J ) ) {
				input[ 1 ] = 1;
			}
			if( CanMoveLeft && Input.GetKey( KeyCode.H ) ) {
				input[ 2 ] = 1;
			} else if( CanMoveRight && Input.GetKey( KeyCode.K ) ) {
				input[ 3 ] = 1;
			}
			if( CanMoveUp && Input.GetKey( KeyCode.Y ) ) {
				input[ 4 ] = 1;
			} else if( CanMoveDown && Input.GetKey( KeyCode.I ) ) {
				input[ 5 ] = 1;
			}
			int numInput = 0;
			for( int i = 0; i < 6; i++ ){
				print (input [i]);
				numInput += input[ i ];
			}
			//96行目の計算式のために0を設定しておく
			// Add velocity to the gameobject
			float curSpeed = numInput > 0 ? MovementSpeed : 0;
			// float curSpeedにnumInput > 0 が trueの時、Movementを返す、falseの場合は0
			Vector3 AddPos = input[ 0 ] * Vector3.forward + input[ 2 ] * Vector3.left + input[ 4 ] * Vector3.up
				+ input[ 1 ] * Vector3.back + input[ 3 ] * Vector3.right + input[ 5 ] * Vector3.down;
			
			AddPos = GetComponent<Rigidbody>().rotation * AddPos;
			print (GetComponent<Rigidbody>().rotation);
			print (AddPos);
			GetComponent<Rigidbody>().velocity = AddPos * ( Time.fixedDeltaTime * curSpeed );
			//加速度を加える
		}
	}

}