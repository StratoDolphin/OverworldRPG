using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Assets.com.stratodolphin.overworldrpg.Characters;
using AggregatGames.AI.Pathfinding;

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
    protected GameObject _targetEnemy;

    /// <summary>
    /// <para>
    /// Type of enemy that this character is. This could be things
    /// such as archer, mage, or warrior.
    /// </para>
    /// <para>
    /// The available types for this variable are the constants
    /// in this class that are pre-pended by "ENEMY_TYPE_".
    /// </para>
    /// </summary>
    protected int _enemyType;

    /// <summary>
    /// Designates how far this enemy can stand away from its
    /// target and be shooting to hit him. In simple terms,
    /// this is the range of its bow.
    /// </summary>
    protected float _archeryRange = 10f;

	#region Pathfinding
	/// <summary>
	/// The pathfinder. Provides methods that enable pathfinding
	/// for this ai.
	/// </summary>
	protected Pathfinder _pathfinder;

	protected PathKnot[] _knots;

	/// <summary>
	/// The index in the list of PathKnots for the current knot.
	/// </summary>
	protected int _knotIndex = -1;
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
		if (this._leftHandInventory.getItemsByType(Storable.TYPE_RANGE).Count == 1)
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
			this.approachTargetViaKnots (this._targetEnemy.gameObject.transform.position);
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
			this.approachTargetViaKnots (this._targetEnemy.gameObject.transform.position);

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
	protected void approachTargetViaKnots(Vector3 target) {
        if (this.hasClearPathToTarget(target))
        {
            this.setDesireToApproach(target, true);
        }
        else
        {
            // Find path at first to the target. This is only called when
            // the target is first set or no pathfinding has been done.
            Debug.Log("Recalculating.");
            this.findPathToTarget(target);
            this.setDesireToApproach(this._knots[this._knotIndex + 1].position, true);
            this._knotIndex++;
        }
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
			Debug.Log ("Found path: " + this._pathfinder.getPath().ToString());
			Debug.Log ("Found path: " + this._pathfinder.getPath().Length.ToString());
			this._knots = this._pathfinder.getPath ();
			this._knotIndex = 0;
		} else {
			// No path found.
			Debug.Log("No path found. " + pathinfo.comment);
			this._knotIndex = -2;
		}
	}
	#endregion

    #endregion

    #region Frame Updates
    // Use this for initialization
    protected override void Start () {
        base.Start();
        Storable sword = this.transform.Find("sword_prefab").GetComponent<Storable>();
        this._leftHandInventory.add(sword);
        Debug.Log(this._leftHandInventory.all());

		this._pathfinder = this.gameObject.GetComponent<Pathfinder> ();
		Debug.Log (this._pathfinder);
	}
	
	// Update is called once per frame
	protected override void Update () {
		this.think ();
		this.evaluateRulesForSightOfTarget (this._targetEnemy);

		base.Update ();
	}
	#endregion
}
