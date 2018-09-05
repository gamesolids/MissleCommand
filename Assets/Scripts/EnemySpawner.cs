using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public float spawnTimer = 10f;
	public float enemySpeed = 2f;
	public float enemyHP = 5f;
	[Range(-4f, 0f)]
	public float minHeightRange = -2f;
	[Range( 0f, 4f)]
	public float maxHeightRange = 2f;

	public bool isRunning = false;

	private bool eFlag = true;
	private float timer;

	private SceneManager sm;

	void Start(){
		sm = FindObjectOfType<SceneManager> ();
	}

	// Update is called once per frame
	void Update () {

		if (isRunning) {
			//listen spawn flag
			if (eFlag) {
				Spawn ();
				eFlag = false;
			}

			//update timer
			timer = timer + Time.deltaTime;

			//check spawn timer, set flag
			if (timer > spawnTimer) {
				timer = 0;
				eFlag = true;
			}
		}
	}

	public void Spawn()
	{
		// Create the Bullet from the Bullet Prefab

		float randomHeight = Random.Range(minHeightRange, maxHeightRange );
		Vector3 randomPos = new Vector3 (transform.position.x, transform.position.y+randomHeight, transform.position.z);

		var enemy = (GameObject)Instantiate (
			enemyPrefab,
			randomPos,
			Quaternion.identity
		);
		enemy.GetComponent<Enemy>().hp = enemyHP;
		enemy.GetComponent<Enemy>().sm = sm;
		// Add velocity to the bullet
		//enemy.GetComponent<Rigidbody>().velocity = transform.right * enemySpeed;
		enemy.GetComponent<Rigidbody>().velocity = transform.right * sm.levelList[sm.currentLevel].shipSpeed;

		//enemy is de-spawned by a de-spawner object in scene

	}

	public void Run(){
		isRunning = true;
	}

	public void Stop(){
		isRunning = false;
	}
}
