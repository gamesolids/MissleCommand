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

	[HideInInspector]
	public SceneManager sm;

	public float CooldownTimer = 2f;

	void Start(){
		float randomTime = UnityEngine.Random.Range(0.5f, CooldownTimer );
		Invoke("Fire", randomTime);
	}
	
	// If no HP left, die.
	void Update () {
		
		if (hp <= 0f) {
			
			Explode ();
		}
	}

	/// <summary>
	/// Responds to collisions from projectiles.
	/// </summary>
	/// <param name="bump">Bump.</param>
	void OnCollisionEnter (Collision bump){
		//if colliding with projectile
		if (bump.gameObject.tag == "Projectile") {
			//find mass of projectile
			float massh = bump.gameObject.GetComponent<Rigidbody> ().mass;
			//update hitpoints
			hp = hp - massh;
			//diplay damage taken
			DisplayText (massh.ToString ());
			//update score
			sm.score.text = ((float)Convert.ToDouble(sm.score.text) + hp).ToString();
			//remove projectile
			Destroy (bump.gameObject);
		}

	}

	/// <summary>
	/// Explode this instance, and add to score.
	/// </summary>
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

	/// <summary>
	/// Displaies the text, at scale, in color.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="scale">Scale.</param>
	/// <param name="color">Color.</param>
	public void DisplayText(string text){
		DisplayText (text, 1f, Color.red);
	}
	public void DisplayText(string text, float scale){
		DisplayText (text, scale, Color.red);
	}
	public void DisplayText(string text, float scale, Color color){
		//instantiate text prefab
		var dmg = (GameObject)Instantiate (
			DamageText,
			transform.position,
			Quaternion.identity);
		//place text on canvas
		dmg.transform.SetParent(FindObjectOfType<Canvas>().rootCanvas.transform, true);
		//update text, scale and color
		dmg.transform.localScale = Vector3.one*scale;
		Text t = dmg.GetComponent<Text> ();
		t.text = text;
		t.color = color;

	}

	/// <summary>
	/// Fire a lazer at one of the missle commands.
	/// </summary>
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
			bullet.name = this.GetInstanceID ().ToString();
		}
		float randomTime = UnityEngine.Random.Range(0.3f, CooldownTimer);
		Invoke("Fire", randomTime);

	}
}
