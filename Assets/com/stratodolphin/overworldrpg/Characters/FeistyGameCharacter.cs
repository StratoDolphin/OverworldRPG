using UnityEngine;
using System.Collections;
using com.stratodolphin.overworldrpg.Characters;

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
	public bool _wantsToSwing = false;

    /*
	/// <summary>
	/// <para>
	/// Denotes which game object this character wants
	/// to start firing it's weapon at.
	/// </para>
	/// <para>
	/// If this is null, then it is assumed this character
	/// wants to either stop firing or doesn't want to
	/// fire at anything.
	/// </para>
	/// </summary>
	protected GameObject _rangeAttackTarget;*/
    #endregion

    #region Backend Actions
    /// <summary>
    /// Determines to start swinging the sword
    /// </summary>
    /// <param name="target">Target.</param>
	public void setDesireToSwing() {
		this._wantsToSwing = true;
		this._canWalk = false;
	}

	/// <summary>
	/// Determines to stop swinging the sword
	/// </summary>
	public void setDesireStopSwinging() {
		this._wantsToSwing = false;
		this._canWalk = true;
	}

	/*
    /// <summary>
    /// Determines to fire this characers weapon at the
    /// given game object. This will simply set the
    /// target that this character wants to fire at.
    /// </summary>
    /// <param name="target">Target.</param>
	public void setDesireToFireWeapon(GameObject target)
    {
        this._rangeAttackTarget = target;
    }

    /// <summary>
    /// Determines the stop firing this characters weapon.
    /// This method simply sets the target object that
    /// this character was firing at to null.
    /// </summary>
    public void setDesireStopFiringWeapon()
    {
        this._rangeAttackTarget = null;
    }*/
    #endregion

    #region Animated Actions
    /// <summary>
    /// <para>
    /// Animates the swing. This will make the character start swinging
    /// at a given object. Assume the desire is there. No logic to check
    /// that is needed in this method.
    /// </para>
    /// <para>
    /// You may not need the target parameter. If you don't, remove the
    /// parameter.
    /// </para>
    /// </summary>
    /// <param name="target">Target.</param>
    protected void animateSwing() {
		//Debug.Log ("Swinging");

		if (this is EnemyAI) {
			//Debug.Log ("Switching: " + GameInfo.PrefabEnemySwing.ToString ());
			this.switchAnimationModel (GameInfo.PrefabEnemySwing);
		}
		else if (this is GamePlayer)
			//Debug.Log ("Switching: " + GameInfo.PrefabMainPlayerSwing.ToString ());
			this.switchAnimationModel (GameInfo.PrefabMainPlayerSwing);
	}

	/*
    /// <summary>
    /// <para>
    /// Animates the firing of this characters ranged weapon. This will
    /// make the character start firing at a given object. Assume the
    /// desire is there. No logic to check that is needed in this method.
    /// </para>
    /// <para>
    /// You may not need the target parameter. If you don't, remove the
    /// parameter.
    /// </para>
    /// </summary>
    /// <param name="target">Target.</param>
    protected void animateFireRangedWeapon(GameObject target)
    {
        Debug.Log("Firing at: " + target.ToString());
    }*/
    #endregion

    #region Actions Execution
    /// <summary>
    /// <para>
    /// Executes the swing desire. If this character wants to swing, the
    /// animateSwing method will be called.
    /// </para>
    /// </summary>
    protected void executeSwingDesire() {
		if (this._wantsToSwing) {
			this._canWalk = false;
			this.animateSwing ();
			return;
		}

		if (this.getBody ().tag == "Swing")
			this.animateStand ();
		this._canWalk = true;
	}

	/*
    /// <summary>
    /// <para>
    /// Executes the ranged weapon firing desire. If this character
    /// wants to swing, the animateSwing method will be called.
    /// </para>
    /// </summary>
    protected void executeRangedAttackDesire()
    {
        if (this._rangeAttackTarget != null)
            this.animateFireRangedWeapon(this._rangeAttackTarget);
    }*/

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

