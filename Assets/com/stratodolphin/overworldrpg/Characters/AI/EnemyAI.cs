using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

/// <summary>
/// Extension of FeistyGameCharacter that adds actual AI to the
/// controller. Instances of this class will pull the tools
/// provided in the base classes together and make the character
/// actually choose to do stuff.
/// </summary>
public class EnemyAI : FeistyGameCharacter {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Distance from player: " + this.getDistanceFromObject (Game.MainPlayer.gameObject).magnitude.ToString());
		if (this.canSeeObject (Game.MainPlayer.CharacterObject)) {
			this.setDesiredMovementDestination (Game.MainPlayer.gameObject.transform.position);
		} else {
			this.determineToStop ();
		}
		animateMove (this._desiredMovementDestination);
	}
}
