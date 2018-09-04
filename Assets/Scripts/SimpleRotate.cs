using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour {

	[Range(-10f,10f)]
	public float Speed = 0;
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 temp = transform.eulerAngles;
		temp.y = temp.y + Speed;
		transform.rotation = Quaternion.Euler( temp );
	}
}
