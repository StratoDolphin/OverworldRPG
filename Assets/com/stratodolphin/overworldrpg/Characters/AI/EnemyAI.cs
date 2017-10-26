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

	#region Private Variables
	/// <summary>
	/// The character that this AI will attempt to attack. This
	/// will dictate the AI's actions such as who it approaches
	/// and who it swings at (Those targets will be this target).
	/// </summary>
	protected GameObject _targetEnemy;
	#endregion

	#region Rules
	/// <summary>
	/// Sets the rules for sight of target. If this AI cannot see
	/// the target, then it will set the desires for movement,
	/// swinging and facing to null/false.
	/// </summary>
	/// <param name="target">Target.</param>
	protected void evaluateRulesForSightOfTarget(GameObject target) {
		if (!this.canSeeObject (target)) {
			this.setDesireToMove (false);
			this.setDesireStopSwinging ();
			this.setDesireToFace (false);
		}
	}
	#endregion

	#region Think
	/// <summary>
	/// Sets the target enemy. This target will be what the AI
	/// will swing at and approach.
	/// </summary>
	/// <param name="target">Target.</param>
	protected void setTargetEnemy(GameObject target) {
		this._targetEnemy = target;
	}

	/// <summary>
	/// Make the AI determine what to do with the Games main player.
	/// If he can see him, then this AI should move towards the
	/// player. If he is next to the player, he should start swinging
	/// at him (I haven't implemented archery yet).
	/// </summary>
	protected virtual void think() {
		this.setTargetEnemy (Game.MainPlayer.gameObject);

		if (this.canSeeObject (this._targetEnemy)) this.setDesireToApproach (this._targetEnemy.transform.position, true);

		if (this.isNextToObject (this._targetEnemy))
			this.setDesireToSwing (this._targetEnemy);
		else
			this.setDesireStopSwinging ();
	}
	#endregion

	#region Frame Updates
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	protected void Update () {
		this.think ();
		this.evaluateRulesForSightOfTarget (this._targetEnemy);

		base.Update ();
	}
	#endregion
}
