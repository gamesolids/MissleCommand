using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
	
	[HideInInspector]
	public SceneManager sm;

	void OnCollisionEnter(Collision col){
		
		GameObject g = col.gameObject;
		if (g.tag == "Projectile") {
			GameObject target = GetObjectById (Convert.ToInt32(g.name));
			if (target) {
				if (this.name == "ShieldCapsule(Clone)") {
					
					GameObject gt = (GameObject) Instantiate (
						sm.ShieldPrefab,
						target.transform.position,
						Quaternion.identity);
					gt.transform.SetParent (target.transform);
					gt.transform.localScale = Vector3.one * 1.5f;
					
					Debug.Log ("ijnvoke shireld");
				} else {
					MissleCommand tmpg = target.GetComponent<MissleCommand> ();
					if (tmpg) {
						tmpg.CooldownTimer = tmpg.CooldownTimer/10;

						Debug.Log ("ijnvoke spped");
						StartCoroutine (MC_RC(tmpg));
					}
					Enemy tmpe = target.GetComponent<Enemy> ();
					if (tmpe) {
						tmpe.CooldownTimer = tmpe.CooldownTimer/10;
						Debug.Log ("ijnvoke spped");
						StartCoroutine (EN_RC(tmpe));
					}
				}
			}

			Destroy (this.gameObject);
		}
	}

	IEnumerator MC_RC(MissleCommand mc){
		yield return new WaitForSeconds(5);
		mc.CooldownTimer = mc.CooldownTimer * 10;
	}
	IEnumerator EN_RC(Enemy mc){
		yield return new WaitForSeconds(5);
		mc.CooldownTimer = mc.CooldownTimer * 10;
	}
	
	public GameObject GetObjectById(int id)
	{
		Dictionary<int, GameObject> m_instanceMap = new Dictionary<int, GameObject>();
		//record instance map

		m_instanceMap.Clear();
		GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos)
		{
			m_instanceMap [go.GetInstanceID()] = go;
		}

		if (m_instanceMap.ContainsKey(id))
		{
			return m_instanceMap[id];
		}
		return null;
	}
}
