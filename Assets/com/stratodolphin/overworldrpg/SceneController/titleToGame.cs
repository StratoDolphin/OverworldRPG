using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.stratodolphin.overworldrpg.Characters;

public class titleToGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void change() {
		SceneManager.LoadScene("MainScene");
		if (GameInfo.initialized) GameInfo.refresh (); // Only works if you're restarting.
	}
}
