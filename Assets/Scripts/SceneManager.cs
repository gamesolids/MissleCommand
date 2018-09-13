using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SceneManager : MonoBehaviour {


	public int currentLevel = 0;
	public Text score;
	public Text ships;
	public Text time;
	public Text level;
	public EnemySpawner[] spawners;
	public GameObject[] commandPoints;

	public GameObject Explosion;
	public GameObject ShieldPrefab;

	public Button startButton;

	public PlayLevel[] levelList = new PlayLevel[10]{
		new PlayLevel(5, 1, 1),
		new PlayLevel(10, 1, 2),
		new PlayLevel(15, 2, 4),
		new PlayLevel(20, 2, 8),
		new PlayLevel(25, 3, 16),
		new PlayLevel(35, 3, 32),
		new PlayLevel(45, 4, 64),
		new PlayLevel(60, 4, 128),
		new PlayLevel(80, 5, 256),
		new PlayLevel(100, 5, 512)
	};

	private bool isPlaying = false;

	// Use this for initialization
	void Start () {

		SetupNewLevel (0);
	}
	
	// Update is called once per frame
	void Update () {

		if (isPlaying) {
			if (Convert.ToInt32 (ships.text) - 1 < 0) {
				EndLevel (currentLevel);
			}
			int c = 0;
			foreach (GameObject cp in commandPoints) {
				if (cp.activeSelf)
					c = c + 1;
			}
			if (c == 0)
				EndLevel (currentLevel);

		}
	}

	// tidy up at end of level
	/// <summary>
	/// Ends the level.
	/// </summary>
	/// <param name="lvl">Lvl.</param>
	public void EndLevel(int lvl){

		isPlaying = false;

		//store bonus points
		float bp = 0;

		//stop spawning enemies
		foreach (EnemySpawner spw in spawners) {
			spw.Stop ();
		}

		//clear ships
		Enemy[] ships = FindObjectsOfType<Enemy>();
		if (ships.Length > 0) {
			foreach (Enemy ship in ships) {
				bp = bp+(ship.hp+1);
				ship.Explode ();
			}
		}

		//collect time
		bp = bp + Convert.ToInt32(time.text);

		//collect command bases
		if (commandPoints.Length > 0) {
			int c = 3;
			foreach (GameObject cp in commandPoints) {
				c = c + 1;
				bp = bp + (c * levelList[currentLevel].bonus);
			}
		}

		//update score
		float intScore = (float)Convert.ToDouble(score.text);
		intScore = intScore+(bp*(currentLevel+1));
		score.text = intScore.ToString ();
		//
		if (currentLevel + 1 < levelList.Length) {
			StartCoroutine (StartWrap (currentLevel + 1, 5f));
		}else{
			//end game?
		}

	}


	/// <summary>
	/// Starts the wrap.
	/// </summary>
	/// <returns>The wrap.</returns>
	/// <param name="lvl">Lvl.</param>
	/// <param name="delayTime">Delay time.</param>
	IEnumerator StartWrap(int lvl, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		// Now do your thing here
		SetupNewLevel(lvl);
	}



	/// <summary>
	/// Setups the new level.
	/// </summary>
	/// <param name="lvl">Lvl.</param>
	public void SetupNewLevel(int lvl){

		//set level
		currentLevel = lvl;

		//reset command bases
		foreach (GameObject cp in commandPoints) {
			cp.SetActive (true);
			cp.GetComponent<MissleCommand> ().hp = 30;
		}

		//configure spawners
		foreach (EnemySpawner spw in spawners) {
			spw.spawnTimer = 10f/(levelList[currentLevel].shipSpeed);
		}
		//reset timer

		//set level text

		//set ships text
		ships.text = (levelList[currentLevel].shipCount).ToString ();

		//
		startButton.gameObject.SetActive (true);

	}

	/// <summary>
	/// Run this instance.
	/// </summary>
	public void Run(){
		isPlaying = true;

		foreach (EnemySpawner spw in spawners) {
			spw.Run ();
		}

	}

	/// <summary>
	/// Pause this instance.
	/// </summary>
	public void Pause(){

		isPlaying = false;

	}

}

public struct PlayLevel{

	public int shipCount;
	public float shipSpeed;
	public float bonus;

	public PlayLevel(int sc, float ss, float b){
		this.shipCount = sc;
		this.shipSpeed = ss;
		this.bonus = b;

	}
}
