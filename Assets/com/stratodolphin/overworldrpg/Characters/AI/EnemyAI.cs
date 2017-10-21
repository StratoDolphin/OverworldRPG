using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class EnemyAI : MonoBehaviour {

	#region Private Variables
	/// <summary>
	/// The game object that represents this enemy that this
	/// AI controlls.
	/// </summary>
	private GameObject _enemyObject;

	/// <summary>
	/// <para>
	/// Determines how far away from this enemy a given object
	/// can be in order for this object to see and evaluate
	/// that object.
	/// </para>
	/// <para>
	/// This value only represents the magnitude of a single
	/// vector. It ignores direction because the sight is
	/// assumed to be at a 360 degree angle.
	/// </para>
	/// </summary>
	private float _viewRange = 1000000f;

	/// <summary>
	/// <para>
	/// A point on the map that this enemy desires to move.
	/// The flow of movement for the ai should be as follows:
	/// </para>
	/// <para>
	/// 1. In the backend, set the destination that the enemy
	/// wants to move. This is the variable that the destination
	/// should be stored in.
	/// 2. When update is called, it should animate the object
	/// moving towards this location.
	/// </para>
	/// </summary>
	private Vector3 _desiredMovementDestination;
	#endregion

	#region Public Attributes
	/// <summary>
	/// Accessor for the GameObject that represents this enemy.
	/// <see cref="EnemyAI._enenmyObject"/>
	/// </summary>
	public GameObject EnemyObject { get { return _enemyObject; } }

	/// <summary>
	/// Accessor for the <see cref="EnemyAI._viewRange"/>.
	/// </summary>
	/// <value>The view range.</value>
	public float ViewRange { get { return _viewRange; } }
	#endregion

	#region Relationship To Player
	/// <summary>
	/// Returns the distance from this enemy's game object to the game
	/// object refered to in thing using the two objects' transform object.
	/// </summary>
	/// <returns>The distance from thing.</returns>
	private Vector3 getDistanceFromObject(GameObject thing) {
		return this.EnemyObject.transform.position - Game.MainPlayer.transform.position;
	}

	/// <summary>
	/// <para>
	/// Determines whether or not this enemy can see the given thing.
	/// </para>
	/// <para>
	/// This method just looks at the magnitude of the vector that
	/// represents the distance between this enemy and the object. If
	/// that magnitude is less than or equal to the value of
	/// <see cref="EnemyAI.ViewRange"/>, then true is returned.
	/// Otherwise, false is returned.
	/// </para>
	/// <para>
	/// Returns:
	/// </para>
	/// <para>
	/// A dynamic type that holds the following:
	/// - <c>true</c>, if see object was caned, <c>false</c> otherwise.
	/// - The actual vector that was used to determine this is
	/// returned.
	/// </para>
	/// </summary>
	/// 
	/// <returns>
	/// A dynamic type that holds the following:
	/// - <c>true</c>, if see object was caned, <c>false</c> otherwise.
	/// - The actual vector that was used to determine this is
	/// returned.
	/// </returns>
	/// <param name="thing">Thing.</param>
	private dynamic canSeeObject(GameObject thing) {
		Vector3 vectorToThing = this.getDistanceFromObject (thing);
		return new {
			canSee = (vectorToThing.magnitude <= this.ViewRange),
			vector = vectorToThing
		};
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
	#endregion

	#region Animated Actions
	/// <summary>
	/// animates the movement of this enemy to a given point.
	/// </summary>
	/// <param name="destination">Destination.</param>
	private void animateMove(Vector2 destination) {
		// Consider using either Vector2.Lerp or Vector2.MoveTowards
		// to do the animations.
		// 
		// Use destination as the destination that you use in
		// Vector2/3.MoveTowards.
		//
		// Examples online used Time.time for the t in Lerp
		// or 0.03 for the maxDistanceDelta in MoveTowards.
		//
		// Use either methods unless you find a better way.
		//
		// To convert between Vector3 and Vector2 use:
		// Vector2 v2 = new Vector2(v3.x, v3.z);
		// Basically, you are removing 1 of the dimensions.
		Debug.Log("Moving to " + destination.ToString());
	}
	#endregion

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
