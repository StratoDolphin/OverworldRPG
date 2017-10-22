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
	protected void Update () {
		Debug.Log ("==========================================================");
		if (this.canSeeObject (Game.MainPlayer.gameObject)) {
			this.setDesireToApproach (Game.MainPlayer.gameObject.transform.position, true);
		} else {
			this.setDesireToMove (false);
		}

		if (this.isNextToObject (Game.MainPlayer.gameObject))
			this.setDesireToSwing (Game.MainPlayer.gameObject);
		else
			this.setDesireStopSwinging ();

		base.Update ();
	}
}
