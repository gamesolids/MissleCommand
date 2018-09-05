using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDespawner : MonoBehaviour {

	public UnityEvent OnDespawn;

	void OnCollisionEnter(Collision bump){
		if (bump.gameObject.tag == "Enemy" || bump.gameObject.tag == "Projectile") {
			Destroy (bump.gameObject);

			OnDespawn.Invoke ();
		}

	}
}
