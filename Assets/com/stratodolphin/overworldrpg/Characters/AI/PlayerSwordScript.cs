using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordScript : MonoBehaviour {
	EnemyAI enemyScript;
	GameObject enemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}


	void OnTriggerEnter(Collider other) {
		Debug.Log ("Sword hit!");
		Debug.Log ("ON!!!!");
		Debug.Log ("object: " + other.ToString());
		Debug.Log ("other: " + other.tag);
		enemy = other.transform.gameObject;
		if (enemy.tag == "AI") {
			enemyScript = enemy.GetComponent<EnemyAI> ();
			enemyScript.decreaseHealth ((float)10.0);
		}
		//theMainPlayer.decreaseHealth (1.0);
	}
}
