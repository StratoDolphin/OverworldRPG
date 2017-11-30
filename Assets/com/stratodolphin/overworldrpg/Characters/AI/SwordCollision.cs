using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour {

	GameObject theMainPlayer;

	// Use this for initialization
	void Start () {
		theMainPlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {


		
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("Sword hit!");
	}
}
