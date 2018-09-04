using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextAnimator : MonoBehaviour {

	private float animStart;
	private float animDuration = 2.5f;

	// Use this for initialization
	void Start () {

		animStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 tmp = GetComponent<RectTransform>().anchoredPosition;
		tmp.y = GetComponent<RectTransform>().anchoredPosition.y + 0.01f;
		GetComponent<RectTransform>().anchoredPosition = tmp;
		//GetComponent<Text>().color = Color.Lerp(Color.red,

		float progress = Time.time - animStart;
		Color cm = new Color (1f, 0.1f, 0f, 1f);
		cm.a = Mathf.Lerp(1.0f, 0.0f, progress / animDuration);
		GetComponent<Text> ().color = cm;
		if (animDuration < progress) {
			Destroy (this.gameObject);
		}
	}


}
