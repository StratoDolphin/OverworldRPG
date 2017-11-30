using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.stratodolphin.overworldrpg.Characters;
using com.stratodolphin.overworldrpg.Characters.Inventory;
using System;

/// <summary>
/// Extension of FeistyGameCharacter that adds actual AI to the
/// controller. Instances of this class will pull the tools
/// provided in the base classes together and make the character
/// actually choose to do stuff.
/// </summary>
public class EnemyAI : FeistyGameCharacter {


	//anthony's variables
	public int swingFrames = 0; //number of frames since begun swing
	public const int swingFinish = 50;

    #region Constant Variables
    /// <summary>
    /// Integer that designates that this enemy is of the type
    /// warrior. This means that he will carry a sword around
    /// and use melee attacks to attack the main player.
    /// </summary>
    public const int ENEMY_TYPE_WARRIOR = 1;

    /// <summary>
    /// Integer that designates that this enemy is of the type
    /// Archer. This means that he will carry a bow and arrow
    /// arround and use ranged attacks to attack the main player.
    /// </summary>
    public const int ENEMY_TYPE_ARCHER = 2;

    /// <summary>
    /// Integer that designates that this enemy is of the type
    /// Mage. This means that he will use spells and magic to
    /// attack the main player.
    /// </summary>
    public const int ENEMY_TYPE_MAGE = 3;
    #endregion

    #region Private Variables
	/// <summary>
	/// The spawner that this enemy is attached to and spawned from.
	/// </summary>
	protected EnemySpawner _attachedSpawner;

    /// <summary>
    /// The character that this AI will attempt to attack. This
    /// will dictate the AI's actions such as who it approaches
    /// and who it swings at (Those targets will be this target).
    /// </summary>
    protected volatile GameObject _targetEnemy;

    /// <summary>
    /// Designates how far this enemy can stand away from its
    /// target and be shooting to hit him. In simple terms,
    /// this is the range of its bow.
    /// </summary>
    protected float _archeryRange = 10f;
    #endregion

	#region Spawning
	/// <summary>
	/// Sets the spawner that this enemy is attached to and add
	/// this enemy to that spawners list of attached enemies.
	/// </summary>
	/// <param name="spawner">Spawner.</param>
	public void setSpawner(EnemySpawner spawner) {
		this._attachedSpawner = spawner;
		this._attachedSpawner.addEnemyToAttachedEnemies (this);
	}
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

    #region Relationship to Objects
    /// <summary>
    /// Determines whether or not this AI is within the range
    /// of shooting at the target according to
    /// <see cref="_archeryRange"/>. If it is within range,
    /// true is returned, otherwise, false is returned.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    protected bool isWithinArcheryRange(GameObject target)
    {
        Vector3 distanceFromTarget = this.getDistanceFromObject(target);
        return (distanceFromTarget.magnitude <= this._archeryRange);
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

	protected void resetDesires() {
		this.setDesireToFace (false);
		this.setDesireToMove (false);
		this.setDesireStopSwinging ();
	}

    /// <summary>
    /// Make the AI determine what to do with the Games main player.
    /// If he can see him, then this AI should move towards the
    /// player. If he is next to the player, he should stop approaching
    /// him and start swinging
    /// at him (I haven't implemented archery yet).
    /// </summary>
    protected virtual void think() {
		//this.resetDesires ();
		//Debug.Log ("left: " + this._leftHandInventory.ToString () + " but has range: " + this._leftHandInventory.hasItemType (Storable.TYPE_RANGE).ToString());
		this.thinkAsMelee();
		if (GameInfo.MainPlayer == null) { return; }
		//this.evaluateRulesForSightOfTarget (this._targetEnemy);
	}

	/*
    /// <summary>
    /// <para>
    /// This AI will determine what to do assuming it wants to take
    /// archer actions. This assumes that there is a bow in this
    /// characters left hand inventory.
    /// </para>
    /// </summary>
    protected virtual void thinkAsArcher()
    {
		if (GameInfo.MainPlayer == null) {
			return;
		}

		this.setTargetEnemy(GameInfo.MainPlayer.gameObject);

        if (this.isNextToObject(this._targetEnemy))
        {
            this.setDesireToSwing(this._targetEnemy);
        }
        else if (this.isWithinArcheryRange(this._targetEnemy))
        {
            this.setDesireToFireWeapon(this._targetEnemy);
        }
        else if (this.canSeeObject(this._targetEnemy))
        {
			this.approachTarget ();
        }
    }*/

    /// <summary>
    /// <para>
    /// This AI will determine what to do assuming it wants to take
    /// melee actions. This assumes that there is a sword in this
    /// characters left hand inventory.
    /// </para>
    /// </summary>
    protected virtual void thinkAsMelee()
    {
		if (this.swingFrames > 0) {//if we are currently in a swinging motion
			swingFrames += 1;
			if (swingFrames >= swingFinish) {
				swingFrames = 0;
			}
		} else {
			if (GameInfo.MainPlayer == null) {
				return;
			}

			this.setTargetEnemy(GameInfo.MainPlayer.gameObject);

			//Debug.Log(this.canSeeObject(this._targetEnemy));
			//Debug.Log(this.isNextToObject(this._targetEnemy));
			if (this.canSeeObject (this._targetEnemy))
				this.approachTarget ();

			if (this.isNextToObject(this._targetEnemy))
			{
				this.setDesireToSwing();
				this.swingFrames = 1;
				this.setDesireToMove(false);
			}
			else
				this.setDesireStopSwinging();
		}
    }

	#region Pathfinding
    /// <summary>
	/// Approaches the target. This will not use pathfinding. I am
	/// saving pathfinding for when I find time for it and when I
	/// can figure out how to make unity work with multiple threads.
	/// This method will just walk straight towards the target.
	/// </summary>
	protected void approachTarget() {
		Vector3 target = this._targetEnemy.transform.position;
		this.setDesireToApproach(target, true);
	}
	#endregion

	#endregion

	#region Inventory
	/// <summary>
	/// Loads the inventory.
	/// </summary>
	protected void loadInventory() {
		foreach (Transform thing in this.gameObject.GetComponentsInChildren<Transform>()) {
			if (thing.name.StartsWith ("storable_")) {
				this.addToInventory (thing);
			}
		}
	}

	/// <summary>
	/// <para>
	/// Adds the storableTransform to this ai's inventory.
	/// </para>
	/// <para>
	/// If storableTransform is a ranged weapon, it is put into this
	/// characters left hand. If it is a melee weapon, it's put into
	/// the right hand. Otherwise, it's just put in this characters
	/// regular inventory.
	/// </para>
	/// </summary>
	/// <param name="storableTransform">Storable transform.</param>
	protected void addToInventory(Transform storableTransform) {
		Storable storableScript = storableTransform.GetComponent<Storable> ();
		//Debug.Log ("type: " + storableScript.Type);
		if (storableScript.Type == Storable.TYPE_RANGE) {
			//Debug.Log ("hand: left.");
			this._leftHandInventory.add (storableScript);
		} else if (storableScript.Type == Storable.TYPE_MELEE) {
			//Debug.Log ("hand: right.");
			this._rightHandInventory.add (storableScript);
		} else {
			//Debug.Log ("regular inventory.");
			this._inventory.add (storableScript);
		}
	}
	#endregion

    #region Frame Updates
    // Use this for initialization
    protected override void Start () {
        base.Start();
		this.loadInventory ();
		this.MovementSpeed = 2;
	}
	
	// Update is called once per frame
	protected override void Update () {
		this.think ();

		base.Update ();
	}
	#endregion
}
