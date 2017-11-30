using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pressRToTalk : MonoBehaviour {

	/*
	 * When you go into the collision box of the 
	 * GameObject this is attached to, it will 
	 * say, "Press R to Talk". When the user presses
	 * 'R', whatever's in the public message in the
	 * inspector is going to show up. Use \n for
	 * newlines in the inspector. For messages where
	 * you want a new box, use the backtick `.
	 */

	private string pressR;
	public string message;
	private string temp;
	private string communicator;
	private string[] newText;
	private int count = 0;

	void Start() {
		message = message.Replace ("\\n", "\n");
		newText = message.Split ('`');
	}

	void Update() {
		if (communicator == "pressR" && Input.GetKeyDown (KeyCode.R)) {
			if (count < newText.Length) {
				temp = "<color=white>" + newText [count] + "</color>";
				count++;
			} else {
				temp = pressR;
				communicator = pressR;
				count = 0;
			}
		}
	}
	void OnTriggerEnter(Collider otherObjective)
	{
		pressR = "Press R to talk";
		if (otherObjective.tag == "Player" && !(Input.GetKeyDown (KeyCode.R)))
		{
			communicator = "pressR";
			temp = "<color=white>" + pressR + "</color>";
		}
	}
	void OnTriggerExit(Collider other)
	{
		temp = null;
	}
	void OnGUI()
	{
		if (temp != null) {
			GUI.Box (new Rect (0, 400, 1000, 200), temp);
		} 
	}
}