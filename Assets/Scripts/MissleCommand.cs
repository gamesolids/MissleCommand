using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissleCommand : MonoBehaviour {

	public int hp = 30;
	public Image HPImage;
	public GameObject Explosion;

	public string InputAxis = "Fire1";

	public GameObject bulletPrefab;
	public Transform bulletSpawner;

	[Range(5f,100f)]
	public float bulletSpeed = 10f;
	public float CooldownTimer = 10f;

	private int timer = 0;
	private bool mFlag = true;

	private ParticleSystem.ColorOverLifetimeModule pscolol;


	// Update is called once per frame
	void Update () {
		
		if (hp <= 0) {// Create the Bullet from the Bullet Prefab
			var splode = (GameObject)Instantiate (
				Explosion,
				transform.position,
				Quaternion.identity);
			//Destroy (this.gameObject);
			gameObject.SetActive(false);
			Destroy (splode, 2f);
		}

		//listen for user input
		if (Input.GetAxis (InputAxis)>0 && mFlag) {
			Fire ();
			mFlag = false;
		}
		//update cooldown timer
		timer = timer+1;
		if (timer > CooldownTimer) {
			timer = 0;
			mFlag = true;
		}

		//Grab the current mouse position on the screen
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - transform.position.x,Input.mousePosition.y - transform.position.y, 0f));

		//Rotates toward the mouse
		transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x))*Mathf.Rad2Deg - 90);

	}

	bool SetParticleColor(GameObject pSys, Gradient grad){

		if (pSys.GetComponent<ParticleSystem> () != null) {
			ParticleSystem ps = pSys.GetComponent<ParticleSystem> ();
			var col = ps.colorOverLifetime;
			col.enabled = true;
			col.color = grad;
			return true;
		}
		return false;

	}

	void OnCollisionEnter (Collision bump){
		if (bump.gameObject.tag == "Projectile") {
			hp = hp - 1;
			if (HPImage) {
				RectTransform lsp = HPImage.GetComponent<Image> ().rectTransform;
				Vector3 ls = lsp.localScale;
				ls.x = ((float)hp / 30f);
				lsp.localScale = ls;
			}

			Gradient grad = new Gradient();
			grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );

			if (!SetParticleColor (bump.gameObject, grad)) {
				Destroy (bump.gameObject,4f);

			}
		}
	}

	void Fire()
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawner.position,
			bulletSpawner.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = transform.up * bulletSpeed;
		bullet.gameObject.name = this.gameObject.name;
		bullet.name = this.gameObject.GetInstanceID ().ToString();
		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}

}
