using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.stratodolphin.overworldrpg.Characters;

public class EnemySpawner : MonoBehaviour
{

	#region Private Variables
	/// <summary>
	/// <para>
	/// The maximum number enemies that can be spawned at a time through this spawner.
	/// There can never be more than this number of enemies that are attached to this
	/// spawner alive on the map at a time.
	/// </para>
	/// <para>
	/// If difficulty is ever implemented, this can be incremented on higher levels.
	/// </para>
	/// </summary>
	protected int _maxNumEnemies = 4;

	/// <summary>
	/// The last time that an enemy was spawned by this spawner. This will be used
	/// to time the spawning.
	/// </summary>
	protected float _lastSpawnTime = 0.0f;

	/// <summary>
	/// The spawning frequency. This designates how often the spawner will respawn its
	/// units.
	/// </summary>
	protected float _spawningFrequency = 120.0f;

	/// <summary>
	/// The enemies that are spawned and therefor attached to this spawner. This list
	/// should never be longer than <see cref="_maxNumEnemies"/>.
	/// </summary>
	protected List<EnemyAI> _attachedEnemies = new List<EnemyAI>();
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
	#endregion

	#region Spawning
	/// <summary>
	/// Returns whether or not this spawner has all of the enemies spawned that
	/// it can have spawned at a time. If there are as many or more enemies
	/// spawned than the value of <see cref="_maxNumEnemies"/> then true is
	/// returned. Otherwise, false is returned.
	/// </summary>
	/// <returns><c>true</c>, if full was ised, <c>false</c> otherwise.</returns>
	public bool isFull() {
		return this._attachedEnemies.Count >= this._maxNumEnemies;
	}

	/// <summary>
	/// <para>
	/// Adds the enemy to attached enemies list.
	/// </para>
	/// </summary>
	/// <returns>The enemy to attached enemies.</returns>
	/// <param name="enemy">Enemy.</param>
	public void addEnemyToAttachedEnemies(EnemyAI enemy) {
		this._attachedEnemies.Add (enemy);
	}

	/// <summary>
	/// Returns the index in <see cref="_attachedEnemies"/> at which the given
	/// enemy is contained. If the enemy is not found, -1 is returned.
	/// </summary>
	/// <returns>The enemy index in attached enemies.</returns>
	/// <param name="enemy">Enemy.</param>
	public int getEnemyIndexInAttachedEnemies(EnemyAI enemy) {
		for (int i = 0; i < this._attachedEnemies.Count; i++) {
			if (this._attachedEnemies [i].GetInstanceID () == enemy.GetInstanceID ()) {
				return i;
			}
		}
		return -1;
	}

	/// <summary>
	/// Returns the <see cref="Quaternion"/> rotation that the enemy to be spawned
	/// should have upon spawning.
	/// </summary>
	/// <returns>The spawn rotation.</returns>
	protected Quaternion getSpawnRotation() {
		Quaternion rotation = Quaternion.LookRotation (this.gameObject.transform.position);
		return rotation;
	}

	/// <summary>
	/// Returns the position that the enemy to be spawned should have upon spawning.
	/// </summary>
	/// <returns>The spawn location.</returns>
	protected Vector3 getSpawnLocation() {
		Vector3 position = this.gameObject.transform.position;
		position.x -= 5.0f + (1.05f * this._attachedEnemies.Count);
		position.z -= 5.0f + (1.05f * this._attachedEnemies.Count);
		return position;
	}

	/// <summary>
	/// <para>
	/// Spawns a new enemy, using the value returned in a call to
	/// <see cref="getEnemyPrefab"/> as the template for the enemy to spawn.
	/// This will then set this spawner as the newly spawned enemy's spawner.
	/// </para>
	/// <para>
	/// If there are already too many enemies spawned by this spawner,
	/// this method will not spawn another. Nothing happens and null will
	/// be returned.
	/// </para>
	/// <para>
	/// This will return the enemy that was just spawned.
	/// </para>
	/// </summary>
	protected EnemyAI spawnEnemy() {
		if (this._attachedEnemies.Count >= this._maxNumEnemies) {
			Debug.LogError ("There are already too many enemies spawned by this spawner!");
			return null;
		}

		Vector3 position = this.getSpawnLocation ();
		Quaternion rotation = this.getSpawnRotation ();

		// Instantiate is a clone funtion, not a creation function. We want to
		// control the cloned object, not player because player is not actually
		// on the map.
		GameObject instantiatedEnemy = Instantiate (GameInfo.PrefabEnemyUnit, position, rotation);

		EnemyAI aiScript = instantiatedEnemy.GetComponent<EnemyAI> ();
		aiScript.setSpawner (this);
		return aiScript;
	}

	/// <summary>
	/// <para>
	/// Checks the state of this spawner to see if it is time to spawn
	/// another enemy. If this spawner is not full and the amount of time
	/// designated by <see cref="_spawnignFrequency"/> has passed since the
	/// last spawn, a new enemy is spawned.
	/// </para>
	/// <para>
	/// Regardless of whether or not an enemy was actually spawned, the
	/// time of last spawn is set each time the amount of time designated
	/// by <see cref="_spawnignFrequency"/> has passed.
	/// </para>
	/// </summary>
	protected void checkSpawner() {
		float currentTime = Time.time;
		if (currentTime - this._lastSpawnTime >= this._spawningFrequency) {
			if (!this.isFull()) this.spawnEnemy ();
			this._lastSpawnTime = currentTime;
		}
	}

	protected void spawnInitial() {
		for (int i = 0; i < this._maxNumEnemies; i++) {
			if (!this.isFull()) this.spawnEnemy ();
		}
	}
	#endregion

	// Use this for initialization
	void Start ()
	{
		//this.spawnInitial ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		checkSpawner ();
	}
}

