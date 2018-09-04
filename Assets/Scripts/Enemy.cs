using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public float hp = 5f;
	public GameObject Explosion;
	public GameObject Ammunition;

	public GameObject DamageText;

	public SceneManager sm;

	void Start(){
		float randomTime = UnityEngine.Random.Range(0.5f, 1.5f );
		Invoke("Fire", randomTime);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (hp <= 0f) {
			
			Explode ();
		}
	}


	void OnCollisionEnter (Collision bump){
		
		if (bump.gameObject.tag == "Projectile") {
			float massh = bump.gameObject.GetComponent<Rigidbody> ().mass;
			hp = hp - massh;
			DisplayText (massh.ToString ());
			sm.score.text = ((float)Convert.ToDouble(sm.score.text) + hp).ToString();
			Destroy (bump.gameObject);
		}

	}

	public void Explode(){

		//collect score for ship
		float tscore = sm.levelList [sm.currentLevel].bonus * 100f;
		//show points collected
		DisplayText (tscore.ToString (), 2f);
		//add points to total score
		sm.score.text = ((float)Convert.ToDouble(sm.score.text) + tscore).ToString();
		//drop ship count for round
		if(Convert.ToInt32 (sm.ships.text) > 0)
			sm.ships.text = (Convert.ToInt32 (sm.ships.text) - 1).ToString ();
		//instantiate explosion
		var splode = (GameObject)Instantiate (
			Explosion,
			transform.position,
			Quaternion.identity);
		//remove ship and clean up
		Destroy (this.gameObject);
		Destroy (splode, 2f);
	}

	public void DisplayText(string text){
		DisplayText (text, 1f, Color.red);
	}
	public void DisplayText(string text, float scale){
		DisplayText (text, scale, Color.red);
	}
	public void DisplayText(string text, float scale, Color color){

		var dmg = (GameObject)Instantiate (
			DamageText,
			transform.position,
			Quaternion.identity);
		dmg.transform.SetParent(FindObjectOfType<Canvas>().rootCanvas.transform, true);
		dmg.transform.localScale = Vector3.one*scale;
		dmg.GetComponent<Text> ().text = text;
		dmg.GetComponent<Text> ().color = color;

	}

	void Fire()
	{   
		//Grab random structure to target
		GameObject[] mcss = GameObject.FindGameObjectsWithTag("CommandCenter");
		if (mcss.Length > 0) {
			Vector3 mcsPosition = mcss [UnityEngine.Random.Range (0, mcss.Length)].transform.position;

			//aim at it
			Vector3 pRotation = new Vector3 (0, 0, Mathf.Atan2 ((mcsPosition.y - transform.position.y), (mcsPosition.x - transform.position.x)) * Mathf.Rad2Deg - 90);
			Vector3 pPos = new Vector3 (transform.position.x+0.2f, transform.position.y - 0.7f, transform.position.z);

			// Create the Bullet from the Bullet Prefab
			var bullet = (GameObject)Instantiate (
				            Ammunition,
				            pPos,
				            Quaternion.Euler (pRotation)
			            );

			// Add velocity to the bullet
			bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.up * 5;
		}
		float randomTime = UnityEngine.Random.Range(0.3f, 2.0f );
		Invoke("Fire", randomTime);

	}
}
