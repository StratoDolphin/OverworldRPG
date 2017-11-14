using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui : MonoBehaviour {
	bool toggleMenu = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M)) {
			print("M key was pressed");
			toggleMenu = !toggleMenu;
		}
	}
	public Texture BoxTexture;      // Drag a Texture onto this item in the Inspector

	void OnGUI()
	{
		int menuWidth = Screen.width / 5 * 4;
		int menuHeight = Screen.height / 5 * 3;
		int menuX = 0 + Screen.width / 10;
		int menuY = 0 + Screen.height / 5;

		int middleOfMenu = Screen.width / 2;

		int textBoxWidth = 200;
		int textBoxHeight = 50;

		GUIStyle titleStyle = new GUIStyle ();
		titleStyle.fontSize = 30;
		//need to figure out how to change font color

		if (toggleMenu) {
			//draw background
			GUI.Box (new Rect (menuX, menuY, menuWidth, menuHeight), "");
			//draw title label
			GUI.Label (new Rect (middleOfMenu - 80, menuY + 25, textBoxWidth, textBoxHeight), "Warp to a bonfire: ");
			//draw bonfire list
			for (int x = 1; x <= 5; x++) {
				GUI.Label (new Rect (menuX + 10, menuY + 30 + (20 * x), textBoxWidth, textBoxHeight), "Bonfire " + x);
			}
		}
	}
}