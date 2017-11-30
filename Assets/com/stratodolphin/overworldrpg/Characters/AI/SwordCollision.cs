 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour {

	GameObject theMainPlayer;
	GamePlayer playerScript;

	// Use this for initialization
	void Start () {
		theMainPlayer = GameObject.FindGameObjectWithTag("Player");
		playerScript = theMainPlayer.GetComponent<GamePlayer> ();
	}
	
	// Update is called once per frame
	void Update () {


		
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Sword hit!");
		Debug.Log (other);
		if(other.tag == "Player")
			playerScript.decreaseHealth ((float)10.0);
		//theMainPlayer.decreaseHealth (1.0);
	}
}
