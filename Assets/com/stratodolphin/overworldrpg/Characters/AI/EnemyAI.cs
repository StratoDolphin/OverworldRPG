using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Assets.com.stratodolphin.overworldrpg.Characters;

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

    /// <summary>
    /// Make the AI determine what to do with the Games main player.
    /// If he can see him, then this AI should move towards the
    /// player. If he is next to the player, he should stop approaching
    /// him and start swinging
    /// at him (I haven't implemented archery yet).
    /// </summary>
    protected virtual void think() {
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
            this.setDesireToMove(false);
            this.setDesireStopFiringWeapon();
        }
        else if (this.isWithinArcheryRange(this._targetEnemy))
        {
            this.setDesireToFireWeapon(this._targetEnemy);
            this.setDesireToMove(false);
            this.setDesireStopSwinging();
        }
        else if (this.canSeeObject(this._targetEnemy))
        {
            this.setDesireToApproach(this._targetEnemy.transform.position, true);
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

        if (this.canSeeObject(this._targetEnemy)) this.setDesireToApproach(this._targetEnemy.transform.position, true);

        if (this.isNextToObject(this._targetEnemy))
        {
            this.setDesireToSwing(this._targetEnemy);
            this.setDesireToMove(false);
        }
        else
            this.setDesireStopSwinging();
    }
    #endregion

    #region Frame Updates
    // Use this for initialization
    protected override void Start () {
        base.Start();
        Storable sword = this.transform.Find("sword_prefab").GetComponent<Storable>();
        this._leftHandInventory.add(sword);
        Debug.Log(this._leftHandInventory.all());
	}
	
	// Update is called once per frame
	protected override void Update () {
		this.think ();
		this.evaluateRulesForSightOfTarget (this._targetEnemy);

		base.Update ();
	}
	#endregion
}
