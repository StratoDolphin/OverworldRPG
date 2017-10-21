using UnityEngine;
using System.Collections;
using AssemblyCSharp;

/// <summary>
/// Represents the contoller for basic operations on a character
/// and it's game object.
/// </summary>
public abstract class GameCharacter : MonoBehaviour
{

	#region Protected Variables
	/// <summary>
	/// <para>
	/// Determines how far away from this character a given object
	/// can be in order for this object to see and evaluate
	/// that object.
	/// </para>
	/// <para>
	/// This value only represents the magnitude of a single
	/// vector. It ignores direction because the sight is
	/// assumed to be at a 360 degree angle.
	/// </para>
	/// </summary>
	protected float _viewRange = 60.0f;

	/// <summary>
	/// <para>
	/// The distance from this characters game object that a
	/// given other object must be for this character to be
	/// considered next to that object.
	/// </para>
	/// <para>
	/// This value only represents the magnitude of a single
	/// vector. It ignores direction because the sight is
	/// assumed to be at a 360 degree angle.
	/// </para>
	/// </summary>
	protected float _nextToThreshold = 0.5f;

	/// <summary>
	/// <para>
	/// A point on the map that this character desires to move.
	/// The flow of movement for the ai should be as follows:
	/// </para>
	/// <para>
	/// 1. In the backend, set the destination that the character
	/// wants to move. This is the variable that the destination
	/// should be stored in.
	/// 2. When update is called, it should animate the object
	/// moving towards this location.
	/// </para>
	/// </summary>
	protected Vector3 _desiredMovementDestination;
	#endregion

	#region Public Attributes
	/// <summary>
	/// The game object that represents this character that this
	/// AI controlls.
	/// </summary>
	public GameObject CharacterObject;
	#endregion

	#region Relationship To Objects
	/// <summary>
	/// Returns the distance from this character's game object to the game
	/// object refered to in thing using the two objects' transform object.
	/// </summary>
	/// <returns>The distance from thing.</returns>
	protected Vector3 getDistanceFromObject(GameObject thing) {
		return this.CharacterObject.transform.position - Game.MainPlayer.transform.position;
	}

	/// <summary>
	/// <para>
	/// Determines whether or not this character can see the given thing.
	/// </para>
	/// <para>
	/// This method just looks at the magnitude of the vector that
	/// represents the distance between this character and the object. If
	/// that magnitude is less than or equal to the value of
	/// <see cref="characterAI.ViewRange"/>, then true is returned.
	/// Otherwise, false is returned.
	/// </para>
	/// <para>
	/// Note: this method simply gets the vector of the game objects
	/// position and returns the value of the canSeeObject overload
	/// that takes a vector, passing the game objects position in as
	/// the vector.
	/// </para>
	/// <returns>
	/// <c>true</c>, if see this character can see thing, <c>false</c> otherwise.
	/// </returns>
	/// <param name="thing">Thing.</param>
	protected bool canSeeObject(GameObject thing) {
		Vector3 vectorToThing = this.getDistanceFromObject (thing);
		return this.canSeeObject(vectorToThing);
	}

	/// <summary>
	/// <para>
	/// Determines whether or not this character can see something based
	/// on the magnitude of vectorToThing.
	/// </para>
	/// <para>
	/// This method just looks at the magnitude of the vector that
	/// represents the distance between this character and the object. If
	/// that magnitude is less than or equal to the value of
	/// <see cref="characterAI.ViewRange"/>, then true is returned.
	/// Otherwise, false is returned.
	/// </para>
	/// </summary>
	/// 
	/// <returns>
	/// <c>true</c>, if see this character can see the thing at
	/// <paramref name="vectorToThing"/>, <c>false</c> otherwise.
	/// </returns>
	/// <param name="thing">Thing.</param>
	protected bool canSeeObject(Vector3 vectorToThing) {
		return (vectorToThing.magnitude <= this._viewRange);
	}

	/// <summary>
	/// <para>
	/// Determines whether or not this character is next to the given
	/// thing.
	/// </para>
	/// <para>
	/// This method just looks at the magnitude of the vector that
	/// represents the distance between this character and the object. If
	/// that magnitude is less than or equal to the value of
	/// <see cref="characterAI.ViewRange"/>, then true is returned.
	/// Otherwise, false is returned.
	/// </para>
	/// <para>
	/// Note: this method simply gets the vector of the game objects
	/// position and returns the value of the isNextToObject overload
	/// that takes a vector, passing the game objects position in as
	/// the vector.
	/// </para>
	/// 
	/// </summary>
	/// <returns><c>true</c>, if next to object was ised, <c>false</c> otherwise.</returns>
	/// <param name="vectorToThing">Vector to thing.</param>
	protected bool isNextToObject(GameObject thing) {
		Vector3 vectorToThing = this.getDistanceFromObject (thing);
		return this.isNextToObject (vectorToThing);
	}

	/// <summary>
	/// <para>
	/// Determines whether or not this character is next to an object based
	/// on vectorToThing. vectorToThing is assumed to be the vector that
	/// represents the distance between this character and the object.
	/// </para>
	/// <para>
	/// This method just looks at the magnitude of the vector that
	/// represents the distance between this character and the object. If
	/// that magnitude is less than or equal to the value of
	/// <see cref="characterAI.ViewRange"/>, then true is returned.
	/// Otherwise, false is returned.
	/// </para>
	/// 
	/// </summary>
	/// <returns><c>true</c>, if next to object was ised, <c>false</c> otherwise.</returns>
	/// <param name="vectorToThing">Vector to thing.</param>
	protected bool isNextToObject(Vector3 vectorToThing) {
		return (vectorToThing.magnitude <= this._nextToThreshold);
	}
	#endregion

	#region Backend Actions
	/// <summary>
	/// <para>
	/// Sets the desired movement destination. This is the same as
	/// making the AI "decide" to move somewhere.
	/// </para>
	/// <para>
	/// This method does not actually do the moving, it just sets
	/// the determination to move.
	/// </para> 
	/// </summary>
	/// <param name="destination">Destination.</param>
	public void setDesiredMovementDestination(Vector3 destination) {
		this._desiredMovementDestination = destination;
	}

	/// <summary>
	/// Makes the AI decide to stop moving. This sets the value of
	/// <see cref="_desiredMovementDestination"/> to the vector that
	/// represents the position of this character's game object.
	/// </summary>
	public void determineToStop() {
		this.setDesiredMovementDestination (this.CharacterObject.transform.position);
	}
	#endregion

	#region Animated Actions
	/// <summary>
	/// animates the movement of this character to a given point.
	/// </summary>
	/// <param name="destination">Destination.</param>
	protected void animateMove(Vector3 destination) {
		// Consider using either Vector3.Lerp or Vector3.MoveTowards
		// to do the animations.
		// 
		// Use destination as the destination that you use in
		// Vector3.MoveTowards.
		//
		// Examples online used Time.time for the t in Lerp
		// or 0.03 for the maxDistanceDelta in MoveTowards.
		//
		// Use either methods unless you find a better way.
		//
		// Also use Vector3.LookTowards to make the enemy face
		// the player.
		Debug.Log("Moving to " + destination.ToString());
	}
	#endregion
}

