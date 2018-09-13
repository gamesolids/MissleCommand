using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformModifier : MonoBehaviour {

	public Vector3 position;
	public bool animatePositon = false;
	public Vector3 rotation;
	public bool animateRotation = false;
	public Vector3 scale;
	public bool animateScale = false;

	private Vector3 rotFix = Vector3.zero;

	// Use this for initialization
	void Start () {
		rotFix = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		if (animatePositon) {
			StepPosition ();
		}
		if (animateRotation) {
			StepRotation ();
		}
		if(animateScale){
			StepScale ();
		}

	}

	public void StepPosition(){
		Vector3 temp = transform.position;
		temp = temp + position;
		transform.position = temp;
	}

	public void StepRotation(){
		Vector3 temp = rotFix;
		rotFix = temp + rotation;
		for (int p =0; p<3; p++) {
			if (rotFix[p] > 360)
				rotFix[p] = 0;
			if (rotFix[p] < 0)
				rotFix[p] = 360;
		}
		transform.rotation = Quaternion.Euler(rotFix) ;
	}

	public void StepScale(){
		Vector3 temp = transform.localScale;
		temp = temp + scale;
		transform.localScale = temp;
	}
}
