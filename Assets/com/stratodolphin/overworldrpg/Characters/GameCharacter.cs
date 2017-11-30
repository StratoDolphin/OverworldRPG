using UnityEngine;
using com.stratodolphin.overworldrpg.Characters;
using com.stratodolphin.overworldrpg.Characters.Inventory;
using System;

/// <summary>
/// Represents the contoller for basic operations on a character
/// and it's game object.
/// </summary>
public abstract class GameCharacter : MonoBehaviour
{

	#region Protected Variables

	#region Movement
	/// <summary>
	/// The ratation damping for this character. Higher numbers will
	/// make this character turn quicker.
	/// </summary>
	protected int _ratationDamping = 2;

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
	protected float _viewRange = 15.0f;

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
	protected float _nextToThreshold = 1.2f;

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

	/// <summary>
	/// <para>
	/// Determines whether or not this character wants to start moving
	/// towards the vector stored in _desiredMovementDestination. If
	/// this is true, this character will start moving. Otherwise, it
	/// will stop moving.
	/// </para>
	/// <para>
	/// This variable can be used to stop movement of this character.
	/// </para>
	/// </summary>
	protected bool _isDesiredToMove;

	/// <summary>
	/// <para>
	/// The desire of this character to face a game object at the given
	/// vector.
	/// </para>
	/// </summary>
	protected Vector3 _desiredTargetToFace;

	/// <summary>
	/// <para>
	/// Determines whether or not this character wants to start turning
	/// towards the vector stored in _desiredTargetToFace. If this is
	/// true, this character will start turning. Otherwise, it will stop
	/// turning.
	/// </para>
	/// <para>
	/// This variable can be used to stop turning of this character.
	/// </para>
	/// </summary>
	protected bool _isDesiredToFace;
	#endregion

	/// <summary>
	/// Amount of hit points that this character has left before he
	/// is dead.
	/// </summary>
	protected float _hitPoints;

    /// <summary>
    /// The inventory of this game character. This contains all
    /// of the <see cref="Storable"/>s taht this character can
    /// hold and carry.
    /// </summary>
    public Inventory _inventory;

    /// <summary>
    /// The inventory for this characters left hand. This will
    /// contain all thing that the character is holding in it's
    /// left hand.
    /// </summary>
    public Inventory _leftHandInventory;

    /// <summary>
    /// The inventory for this characters right hand. This will
    /// contain all thing that the character is holding in it's
    /// right hand.
    /// </summary>
    public Inventory _rightHandInventory;

	/// <summary>
	/// Determines if this frame will allow the model to be switched.
	/// This is because, when you delete an object, it is not deleted
	/// until the next frame, so if you replace an object multiple times,
	/// it will create multiple models.
	/// </summary>
	protected bool canSwitchModels = true;

	/// <summary>
	/// Determines whether or not the player can walk.
	/// </summary>
	protected bool _canWalk = true;
    #endregion

	#region Public Attributes
	/// <summary>
	/// The initial amount of hitpoints that this character has.
	/// </summary>
	public float MaxHitPoints = 100;

	/// <summary>
	/// The health bar that is displayed for this character.
	/// </summary>
	public HealthBar HealthBar;

    /// <summary>
    /// Public Accessor for <see cref="_hitPoints"/>. The setter
    /// calls <see cref="increaseHealth(float)"/>.
    /// </summary>
    public float Health
    {
        get { return this.MaxHitPoints; }
        set { this.increaseHealth(value); }
    }

    /// <summary>
    /// Determines whether or not this character is the main player.
    /// </summary>
	public bool IsMainPlayer = false;

	/// <summary>
	/// The movement speed of this character. Higher numbers
	/// will result in higher speeds.
	/// </summary>
	public float MovementSpeed = 3.5f;
	#endregion

    #region Relationship To Objects
    /// <summary>
    /// Returns the distance from this character's game object to the game
    /// object refered to in thing using the two objects' transform object.
    /// </summary>
    /// <returns>The distance from thing.</returns>
    protected Vector3 getDistanceFromObject(GameObject thing) {
        return this.getDistanceFromObject(thing.transform.position);
	}

    /// <summary>
    /// Returns the distance from this character's game object to the
    /// vector refered to in target.
    /// </summary>
    /// <returns>The distance from the target</returns>
    protected Vector3 getDistanceFromObject(Vector3 target)
    {
        return this.gameObject.transform.position - target;
    }

    /// <summary>
    /// <para>
    /// Returns the direction to the given game object using quaternions.
    /// </para>
    /// </summary>
    /// <param name="thing"></param>
    /// <returns></returns>
    protected Quaternion getDirectionToObject(Vector3 target)
    {
        Quaternion direction = Quaternion.LookRotation(this.getDistanceFromObject(target));
        direction.x = 0;
        direction.z = 0;
        return direction;
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
	public bool isNextToObject(GameObject thing) {
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
		//Debug.Log ((vectorToThing.magnitude <= this._nextToThreshold).ToString());
		return (vectorToThing.magnitude <= this._nextToThreshold);
	}
	#endregion

	#region Backend
	/// <summary>
	/// Determines whether or not the character is swinging currently.
	/// </summary>
	/// <returns><c>true</c>, if swinging was ised, <c>false</c> otherwise.</returns>
	public bool isSwinging() {
		return (this is FeistyGameCharacter && ((FeistyGameCharacter)this)._wantsToSwing);
	}

	/// <summary>
	/// Initializes the hitpoints of this character by setting <see cref="_hitpoints"/>
	/// to the value of startingHealth. This should only be used when initializing
	/// the character.
	/// </summary>
	/// <param name="startingHealth">Starting health.</param>
	protected void initializeHealth(int startingHealth) {
		this._hitPoints = startingHealth;
	}

	/// <summary>
	/// <para>
	/// Initializes the hitpoints of this character by setting <see cref="_hitpoints"/>
	/// to the value of the default <see cref="_maxHitPoints"/>. This should only be
	/// used when initializing the character.
	/// </para>
	/// <para>
	/// This also initializes the health bar.
	/// </para>
	/// </summary>
	/// <param name="startingHealth">Starting health.</param>
	protected void initializeHealth() {
		this._hitPoints = this.MaxHitPoints;
		try {
			this.HealthBar = this.transform.Find("Camera").Find ("playerHealth").GetComponent<HealthBar> ();
		}
		catch (NullReferenceException e) {
			this.HealthBar = this.transform.Find("Body").Find ("playerHealth").GetComponent<HealthBar> ();
		}
	}
	#endregion

	#region Backend Actions

	#region Movement
	public void setPosition(Vector3 position, Quaternion rotation) {
		transform.position = position;
		transform.rotation = rotation;
	}

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
	/// Sets the desire to move for this character. This method
	/// simply sets the <see cref="_isDesiredToMove"/> attribute
	/// for this character to the value of desireToMove.
	/// </summary>
	/// <param name="desireToMove">If set to <c>true</c> desire to move.</param>
	public void setDesireToMove(bool desireToMove) {
		this._isDesiredToMove = desireToMove;
	}

	/// <summary>
	/// <para>
	/// Sets the vector towards which this character will face.
	/// </para>
	/// </summary>
	/// <param name="target">Target.</param>
	public void setDesiredTargetToFace(Vector3 target) {
		this._desiredTargetToFace = target;
	}

	/// <summary>
	/// Sets the desire to turn for this character. This method
	/// simply sets the <see cref="_isDesiredToFace"/> attribute
	/// for this character to the value of desireToFace.
	/// </summary>
	/// <param name="desireToFace">If set to <c>true</c> desire to face.</param>
	public void setDesireToFace(bool desireToFace) {
		this._isDesiredToFace = desireToFace;
	}

	/// <summary>
	/// <para>
	/// Sets the desire to approach a given object at the given destination.
	/// This method sets the target to approach, sets the desire to face that
	/// target, and sets the desire to start moving to true.
	/// </para>
	/// <para>
	/// When the update method is called, the result of this will be that
	/// this character faces the destination (if isDesiredToFace is true)
	/// and starts moving towards that destination.
	/// </para>
	/// </summary>
	/// <param name="destination">Destination.</param>
	/// <param name="isDesiredToFace">If set to <c>true</c> is desired to face.</param>
	public void setDesireToApproach(Vector3 destination, bool isDesiredToFace) {
		this.setDesiredMovementDestination (destination);
		if (isDesiredToFace) {
		this.setDesiredTargetToFace (destination);
			this.setDesireToFace (isDesiredToFace);
		}
		this.setDesireToMove (true);
	}
    #endregion

    #endregion

    #region Life Methods
    /// <summary>
    /// <para>
    /// Animates the state of this character being dead. This simply
    /// removes this main players game object and replaces it with
    /// the dead main player prefab laying on the ground.
    /// </para>
    /// <para>
    /// This method returns the Game object that represents the dead
    /// Main Player prefab.
    /// </para>
    /// </summary>
    protected GameObject animateDie()
    {
        Vector3 deadPosition = this.gameObject.transform.position;

        Destroy(this.gameObject);
		if (this is EnemyAI)
			return Instantiate(GameInfo.PrefabEnemyDead, deadPosition, GameInfo.PrefabEnemyDead.transform.rotation);
		else if (this is GamePlayer)
			return Instantiate(GameInfo.PrefabMainPlayerDead, deadPosition, GameInfo.PrefabMainPlayerDead.transform.rotation);
		else
			return Instantiate(GameInfo.PrefabMainPlayerDead, deadPosition, GameInfo.PrefabMainPlayerDead.transform.rotation);
    }

    /// <summary>
    /// Checks to see if this character has no more hitpoints left.
    /// If so, this will animate the death and set the
    /// <see cref="GameInfo.MainPlayer"/> to null.
    /// </summary>
    protected void checkForDeath()
    {
        if (this.IsDead)
        {
            this.animateDie();
			if (this is GamePlayer && ((GamePlayer) this).IsMainPlayer)
				GameInfo.setMainPlayer(null);
            if (this.IsMainPlayer) { GameLogic.SpawnUI.show(); }
        }
    }

    /// <summary>
    /// Determines whether or not this game character is dead based
    /// on the hitpoints it has left.
    /// </summary>
    protected bool IsDead { get { return this._hitPoints <= 0; } }

	/// <summary>
	/// Decreases the health and refreshes the health bar.
	/// </summary>
	/// <param name="healthValue">Health value.</param>
	public void decreaseHealth (float healthValue) {
		this._hitPoints -= healthValue;
		if (this._hitPoints < 0) {
			this._hitPoints = 0;
		}
        refreshHealthBarDisplay ();
	}

	/// <summary>
	/// Increases the health and refreshes the health bar.
	/// </summary>
	/// <param name="healthValue">Health value.</param>
	public void increaseHealth(float healthValue) {
		this._hitPoints += healthValue;
		if (this._hitPoints > 100) {
			this._hitPoints = 100;
		}
        refreshHealthBarDisplay ();
	}

	/// <summary>
	///  Refreshes the health bar on the screen to match the current health.
	/// </summary>
	/// <param name="npcHealth">Npc health.</param>
	protected void refreshHealthBarDisplay() {
		//calculates health gained; if cur = 80 / 100 then 0.8f
		float healthPercent = this._hitPoints / this.MaxHitPoints;
		//npcHealth has to be a value between 0 and 1; max health has scale of 1
		HealthBar.setHealthPercent(healthPercent);
	}
    #endregion

    #region Animated Actions
	protected GameObject getBody() {
		return this.transform.Find ("Body").gameObject;
	}

	/// <summary>
	/// <para>
	/// Switchs the animation model in this character. There are a couple things that need
	/// to be done to preserve the gameobject while doing this. The main one is that the
	/// health bar needs to be preserved if it is a child of the animated model. This is
	/// because deleting the animated model will delete its children and therefore, the
	/// health bar.
	/// </para>
	/// <para>
	/// This method will not switch the body if it is already the current body.
	/// </para>
	/// </summary>
	/// <param name="prefab">Prefab.</param>
	protected void switchAnimationModel(GameObject prefab) {
		if (this.getBody ().tag == prefab.tag) {
//			if (this is GamePlayer)
//				Debug.Log ("TAG: " + prefab.tag);
			return;
		}
		if (!this.canSwitchModels) {
//			if (this is GamePlayer)
//				Debug.Log ("no move TAG: " + prefab.tag);
			return;
		}

		//if (this is GamePlayer) Debug.Log ("Switching to: " + prefab.tag);
		// ==== Health Bar Preservation (Preserve the environment! We're liberals!
		// Save health bar
		GameObject healthBar = this.GetComponentInChildren<HealthBar>().gameObject;

		// Determine if the health bar even needs to be preserved or if
		// it will be safe as is.
		bool needToPreserveHealthBar = healthBar.transform.parent.name == "Body";

		// Temporarily move it out of the body.
		if (needToPreserveHealthBar) {
			healthBar.transform.SetParent (this.gameObject.transform);
		}

		// ==== Actually switching bodies. (Dang you can do that these days?)
		GameObject body = this.getBody();
		// We want the new body to have the same position and rotation as
		// the original.
		Vector3 bodyPosition = body.transform.position;
		Quaternion bodyRotation = body.transform.rotation; // Rotate that body! hmm!

		//Debug.Log ("Destroying: " + this.getBody ().ToString ());
		Destroy (body);
		//Debug.Log ("Destroyed: " + this.getBody ().ToString ());

		GameObject newBody = Instantiate (prefab, bodyPosition, bodyRotation, this.transform);
		newBody.name = "Body";

		if (needToPreserveHealthBar) {
			healthBar.transform.SetParent (newBody.transform);
		}

		this.canSwitchModels = false;
	}

    /// <summary>
	/// <para>
    /// animates the movement of this character to a given point.
    /// This method does not contain the logic that looks at the
    /// variables that determine whether or not this character
    /// even wants to move. This method always does the animation.
    /// If you want to factor in those variables, put this method
    /// call inside the if statements.
	/// </para>
	/// <para>
	/// If this characters _canMove attribute is false, it won't move.
	/// </para>
    /// </summary>
    /// <param name="destination">Destination.</param>
    protected void animateMove(Vector3 destination) {
		if (!this._canWalk)
			return;

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
		Vector3 flatVector = new Vector3 (destination.x, this.gameObject.transform.position.y, destination.z);
		transform.position = Vector3.MoveTowards (transform.position, flatVector, MovementSpeed * Time.deltaTime);
		//Debug.Log("Moving to " + destination.ToString());
		if (this is EnemyAI)
			this.switchAnimationModel (GameInfo.PrefabEnemyWalk);
		else if (this is GamePlayer)
			this.switchAnimationModel (GameInfo.PrefabMainPlayerWalk);
	}

	/// <summary>
	/// animates the turnign of this character to a given direction.
	/// This method does not contain the logic that looks at the
	/// variables that determine whether or not this character
	/// even wants to move. This method always does the animation.
	/// If you want to factor in those variables, put this method
	/// call inside the if statements.
	/// </summary>
	/// <param name="direction">Direction.</param>
	protected void animateTurn(Vector3 direction) {
		// TODO: animate legs moving to look like this
		// character is stepping.

		Vector3 lookPos = direction - this.gameObject.transform.position;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
        rotation.x = this.gameObject.transform.rotation.x;
        rotation.z = this.gameObject.transform.rotation.z;
        transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, rotation, Time.deltaTime * this._ratationDamping);
		//Debug.Log("Turning to " + direction.ToString());
		if (this._canWalk) {
			if (this is EnemyAI)
				this.switchAnimationModel (GameInfo.PrefabEnemyWalk);
			else if (this is GamePlayer)
				this.switchAnimationModel (GameInfo.PrefabMainPlayerWalk);
		}
	}

	/// <summary>
	/// Animates the character to just stand there. This will simply
	/// switch the animated body out for the one that is doing nothing.
	/// </summary>
	protected void animateStand() {
		bool isSwinging = false;
		if (this.isSwinging())
			return;
		if (this is EnemyAI)
			this.switchAnimationModel (GameInfo.PrefabEnemyBase);
		if (this is GamePlayer)
			this.switchAnimationModel (GameInfo.PrefabMainPlayerBase);
	}

	/// <summary>
	/// Animates the approaching of this object to an object at the
	/// the vector in destination. This method will make this
	/// character turn to face the destination and move towards it.
	/// </summary>
	/// <param name="destination">Destination.</param>
	protected void animateApproach(Vector3 destination) {
		this.animateTurn (destination);
		this.animateMove (destination);
	}
	#endregion

	#region Action Executions
	/// <summary>
	/// <para>
	/// Executes the approach desire. If this character has determined
	/// that it wants to approach an object, then it will call the
	/// <see cref="animateApproach"/> method, making it carry out that
	/// desire.
	/// </para>
	/// </summary>
	protected void executeApproachDesire() {
		//Debug.Log("desire to move: " + this._isDesiredToMove.ToString());
		if (this._isDesiredToMove)
			this.animateApproach (this._desiredMovementDestination);
		else if (!this.isSwinging())
			this.animateStand ();
	}

	/// <summary>
	/// <para>
	/// Executes the face desire. This will make the character face
	/// the vector stored in <see cref="_desiredTargetToFace"/>.
	/// desire.
	/// </para>
	/// </summary>
	protected void executeFaceingDesire() {
		if (this._isDesiredToFace) this.animateTurn (this._desiredTargetToFace);
	}

	/// <summary>
	/// <para>
    /// Executes the desires of this character. If the character
	/// wants to approach an object, it will call the method that
	/// executes the approach desire. Otherwise, it will simply
	/// face the vector stored in <see cref="_desiredTargetToFace"/>.
    /// </para>
    /// <para>
    /// If this character is dead, nothing is done.
    /// </para>
	/// </summary>
	protected virtual void executeDesires() {
        if (this.IsDead) { return; }

		//if (this is GamePlayer) Debug.Log ("Desire to move: " + this._isDesiredToMove.ToString());
		if (!this._isDesiredToMove)
			this.animateStand ();
		else
			this.executeApproachDesire ();
	}
	#endregion

	#region Frames
    protected virtual void Start()
    {
		this.initializeHealth ();
        this._inventory = new Inventory(5);
        this._leftHandInventory = new Inventory(1, true);
        this._rightHandInventory = new Inventory(1, true);
    }

	protected virtual void Update()
    {
		//Debug.Log ("New Frame");
		// Refresh this every frame.
		this.canSwitchModels = true;
        this.checkForDeath ();
		this.executeDesires ();
	}
	#endregion
}

