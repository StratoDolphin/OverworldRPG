using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Assets.com.stratodolphin.overworldrpg.Characters;
using AggregatGames.AI.Pathfinding;
using System;
using System.Threading;

/// <summary>
/// Extension of FeistyGameCharacter that adds actual AI to the
/// controller. Instances of this class will pull the tools
/// provided in the base classes together and make the character
/// actually choose to do stuff.
/// </summary>
public class EnemyAI : FeistyGameCharacter {

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

    #region Pathfinding
    /// <summary>
    /// The thread that the pathfinding process is run on. This
    /// thread is a background process that is meant to run
    /// continuously whether there is a target or not.
    /// </summary>
    protected Thread _pathfindingThread;

    /// <summary>
    /// Determines weather or not the AI wants to run the
    /// pathfinding routine. This is only to be set to false
    /// if the thread is to be killed altogether. There is no
    /// way to restart the pathfinding thread.
    /// </summary>
    protected volatile bool _pathfindingThreadControl;

	/// <summary>
	/// The pathfinder. Provides methods that enable pathfinding
	/// for this ai.
	/// </summary>
	protected volatile Pathfinder _pathfinder;

	protected volatile PathKnot[] _knots;

	/// <summary>
	/// The index in the list of PathKnots for the current knot.
	/// </summary>
	protected volatile int _knotIndex = -1;
	#endregion

	#region Inventory

	#endregion

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
		this.setDesireStopFiringWeapon ();
	}

    /// <summary>
    /// Make the AI determine what to do with the Games main player.
    /// If he can see him, then this AI should move towards the
    /// player. If he is next to the player, he should stop approaching
    /// him and start swinging
    /// at him (I haven't implemented archery yet).
    /// </summary>
    protected virtual void think() {
		this.resetDesires ();
		if (this._leftHandInventory.hasItemType(Storable.TYPE_RANGE))
        {
            this.thinkAsArcher();
        } else
        {
            this.thinkAsMelee();
        }
	}

    /// <summary>
    /// <para>
    /// This AI will determine what to do assuming it wants to take
    /// archer actions. This assumes that there is a bow in this
    /// characters left hand inventory.
    /// </para>
    /// </summary>
    protected virtual void thinkAsArcher()
    {
        this.setTargetEnemy(Game.MainPlayer.gameObject);

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
    }

    /// <summary>
    /// <para>
    /// This AI will determine what to do assuming it wants to take
    /// melee actions. This assumes that there is a sword in this
    /// characters left hand inventory.
    /// </para>
    /// </summary>
    protected virtual void thinkAsMelee()
    {
        this.setTargetEnemy(Game.MainPlayer.gameObject);

		if (this.canSeeObject (this._targetEnemy))
			this.approachTarget ();

        if (this.isNextToObject(this._targetEnemy))
        {
            this.setDesireToSwing(this._targetEnemy);
            this.setDesireToMove(false);
        }
        else
            this.setDesireStopSwinging();
    }

	#region Pathfinding
    /// <summary>
    /// Starts the background thread that runs the pathfinding. If the thread
    /// is not null, it is assumed that a thread is already running. So a
    /// warning is printed to Debug and nothing is done. The existing thread
    /// is left alone.
    /// </summary>
    protected void startPathfindingRoutine()
    {
        if (this._pathfindingThread != null)
        {
            Debug.LogWarning("A pathfinder is already running. Cannot start another.");
            return;
        }

        this._pathfindingThreadControl = true;
        this._pathfindingThread = new Thread(pathfindingWorker);
        this._pathfindingThread.Start();
    }

    /// <summary>
    /// The process that is run by the pathfinding thread. This runs the
    /// method that finds the path to the target until the control variable
    /// for the thread is false. This will be run in the background and
    /// will look for the path continuously.
    /// </summary>
    protected void pathfindingWorker()
    {
        while (this._pathfindingThreadControl)
        {
            this.findPathToTarget(this._targetEnemy.transform.position);
        }
    }

    /// <summary>
    /// <para>
    /// Determines whether or not there is a clear path in front leading
    /// straight to the target. It is determined that there is a clear
    /// path only if there is a straight line that can be drawn from this
    /// character to the vector at target that is not interupted by any
    /// collider.
    /// </para>
    /// <para>
    /// Returns true if a clear, straight path can be seen. false otherwise.
    /// </para>
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    protected bool hasClearPathToTarget(Vector3 target)
    {
        bool seesObsticle = Physics.Raycast(this.gameObject.transform.position, target, this._viewRange);
        return !seesObsticle;
    }

    /// <summary>
    /// Aproaches the target.
    /// <para>
    /// If a straight path is available to the target that does not contain
    /// obsticles (<see cref="hasClearPathToTarget(Vector3)"/> returns true),
    /// then this method does not use the knots or A* algorithm via
    /// <see cref="_pathfinder"/>. Instead, it just moves straight towards
    /// the target blindly without pathfinding.
    /// </para>
    /// <para>
    /// Otherwise, if an obsticle blocks his way, this character will use the
    /// <see cref="_pathfinder"/> to approach the target.
    /// </para>
    /// </summary>
    /// <param name="target"></param>
	protected void approachTarget()
    {
        Vector3 target = this._targetEnemy.transform.position;
        bool approachWithoutKnots = false;
        if (this.hasClearPathToTarget(target))
        {
			approachWithoutKnots = true;
        }
        else
        {
            // Find path at first to the target. This is only called when
            // the target is first set or no pathfinding has been done.
            Debug.Log("Recalculating.");
			try {
				if (this._knotIndex >= 0 && this._knotIndex < this._knots.Length - 1) {
					this.setDesireToApproach (this._knots [this._knotIndex + 1].position, true);
					this._knotIndex++;
				} else {
					approachWithoutKnots = true;
				}
			} catch (IndexOutOfRangeException e) {
				approachWithoutKnots = true;
			}
        }

		if (approachWithoutKnots) this.setDesireToApproach(target, true);
	}

	/// <summary>
	/// Sets the path to target that this AI wants to approach. After
	/// calling this method, the next spot to go can be found in the
	/// list <see cref="_knots"/> at index <see cref="_knotIndex"/>.
	/// </summary>
	/// <param name="target">Target.</param>
	protected void findPathToTarget(Vector3 target) {
		Debug.Log ("finding path from: " + this.gameObject.transform.position.ToString () + " to: " + target.ToString ());
        this._pathfinder.findPath (this.gameObject.transform.position, target, this.foundPath);
	}

	/// <summary>
	/// <para>
	/// Method that is used to set the path to the finish for this AI.
	/// This will take the path found by the pathfinder and set the path
	/// to the finish (<see cref="_knots"/>) to the path in the pathfinder.
	/// The first index in that path is the index that this AI is at
	/// currently.
	/// </para>
	/// </summary>
	/// <param name="pathinfo">Pathinfo.</param>
	public void foundPath(Pathinfo pathinfo) {
		if (pathinfo.foundPath) {
			this._knots = this._pathfinder.getPath ();
			this._knotIndex = 0;
		} else {
			// No path found.
			this._knotIndex = -2;
		}
	}
	#endregion

	#endregion

	#region Inventory
	/// <summary>
	/// Loads the inventory.
	/// </summary>
	protected void loadInventory() {
		foreach (Transform thing in this.gameObject.GetComponentsInChildren<Transform>()) {
			Debug.Log (thing.ToString ());
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
		if (storableScript.Type == Storable.TYPE_RANGE) {
			this._leftHandInventory.add (storableScript);
		} else if (storableScript.Type == Storable.TYPE_MELEE) {
			if (this._leftHandInventory.hasItemType(Storable.TYPE_RANGE)) {
				// If the player has a bow, just put the melee weapon
				// in the regular inventory for now.
				this._inventory.add (storableScript);
			} else {
				this._rightHandInventory.add (storableScript);
			}
		} else {
			this._inventory.add (storableScript);
		}
	}
	#endregion

    #region Frame Updates
    // Use this for initialization
    protected override void Start () {
        base.Start();
		this.loadInventory ();

		Debug.Log ("left hand: " + this._leftHandInventory.ToString ());
		Debug.Log ("right hand: " + this._rightHandInventory.ToString ());
		Debug.Log ("inventory: " + this._inventory.ToString ());

		this._pathfinder = this.gameObject.GetComponent<Pathfinder> ();
        this.startPathfindingRoutine();
	}
	
	// Update is called once per frame
	protected override void Update () {
		this.think ();
		this.evaluateRulesForSightOfTarget (this._targetEnemy);

		base.Update ();
	}
	#endregion
}
