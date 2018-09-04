using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDespawner : MonoBehaviour {

	void OnCollisionEnter(Collision bump){
		if (bump.gameObject.tag == "Enemy" || bump.gameObject.tag == "Projectile") {
			Destroy (bump.gameObject);
		}

	}
}
