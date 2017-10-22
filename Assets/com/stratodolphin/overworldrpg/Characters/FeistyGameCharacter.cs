using UnityEngine;
using System.Collections;

/// <summary>
/// Extension of GameCharacter. This class simply includes the
/// actions of attacking and doing damage.
/// </summary>
public abstract class FeistyGameCharacter : GameCharacter
{

	#region Private Variables
	/// <summary>
	/// <para>
	/// Denotes which game object this character wants
	/// to start swinging it's weapon at.
	/// </para>
	/// <para>
	/// If this is null, then it is assumed this character
	/// wants to either stop swinging or doesn't want to
	/// swing at anything.
	/// </para>
	/// </summary>
	protected GameObject _swingAttackTarget;
	#endregion

	#region Backend Actions
	/// <summary>
	/// Determines to swing at the given game object. This
	/// will simply set the target that this character
	/// wants to swing at.
	/// </summary>
	/// <param name="target">Target.</param>
	protected void setDesireToSwing(GameObject target) {
		this._swingAttackTarget = target;
	}

	/// <summary>
	/// Determines the stop swinging. This method simply
	/// sets the target object that this character was
	/// swinging at to null.
	/// </summary>
	protected void setDesireStopSwinging() {
		this._swingAttackTarget = null;
	}
	#endregion

	#region Animated Actions
	protected void animateSwing(GameObject target) {
		Debug.Log ("Swinging at: " + target.ToString ());
	}
	#endregion

	#region Actions Execution
	protected void executeSwingDesire() {
		if (this._swingAttackTarget != null)
			this.animateSwing (this._swingAttackTarget);
	}

	/// <summary>
	/// Executes the desires of this character. This overrides the
	/// executeDesires of the base class (GameCharacter) but does
	/// call the base classes version of this method before running
	/// it's own logic.
	/// </summary>
	protected override void executeDesires() {
		base.executeDesires ();
		this.executeSwingDesire ();
	}
	#endregion
}

