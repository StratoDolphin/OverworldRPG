using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.stratodolphin.overworldrpg.Characters;
using System;
using System.Reflection;

namespace Assets.com.stratodolphin.overworldrpg.Characters.Spawning
{
	public class Bonfire : MonoBehaviour {

		#region Private Variables
		/// <summary>
		/// The key that signifies that this is the campfire that the player wants
		/// to spawn at.
		/// </summary>
		private KeyCode _spawnKey;
		#endregion

		#region Spawning
		/// <summary>
		/// The key that signifies that this is the campfire that the player wants
		/// to spawn at.
		/// </summary>
		/// <value>The spawn key.</value>
		public KeyCode SpawnKey { get{ return this._spawnKey; } }

		/// <summary>
		/// Sets the spawn key according to its index in <see cref="Game.Bonfires"/>.
		/// The key will be the number that its index is in that list. For example,
		/// if this is the 3rd bonfire (index 2) in <see cref="Game.Bonfires"/>, the
		/// spawn key will be <see cref="KeyCode.Alpha2"/>. This method assumes that
		/// this bonfire has already been added to the list.
		/// </summary>
		protected void setSpawnKey() {
			int keyNum = GameInfo.Bonfires.Count - 1;
			String keyCodeString = "Alpha" + keyNum.ToString ();

			Type keyCodeType = typeof(KeyCode);
			FieldInfo alphakeyInfo = keyCodeType.GetField (keyCodeString);
			KeyCode code = (KeyCode) alphakeyInfo.GetValue(null);

			this._spawnKey = code;
		}

		/// <summary>
		/// Returns the <see cref="Quaternion"/> rotation that the player to be spawned
		/// should have upon spawning.
		/// </summary>
		/// <returns>The spawn rotation.</returns>
		protected Quaternion getSpawnRotation() {
			Quaternion rotation = Quaternion.LookRotation (this.gameObject.transform.position);
			return rotation;
		}

		/// <summary>
		/// Returns the position that the player to be spawned should have upon spawning.
		/// </summary>
		/// <returns>The spawn location.</returns>
		protected Vector3 getSpawnLocation() {
			Vector3 position = this.gameObject.transform.position;
			position.x -= 5;
			return position;
		}

		/// <summary>
		/// Spawns the specified player at this campfire. This method will instantiate
		/// the player on the game map, set the Main player of the game <see cref="GameInfo.MainPlayer"/>
		/// to this player and set the IsAlive attribute on player to true.
		/// </summary>
		/// <param name="player">Player.</param>
		public void spawn(GameObject player) {
			Debug.Log ("Spawning.");
			Vector3 position = this.getSpawnLocation ();
			Quaternion rotation = this.getSpawnRotation ();

			// Instantiate is a clone funtion, not a creation function. We want to
			// control the cloned object, not player because player is not actually
			// on the map.
			GameObject instantiatedPlayer = Instantiate (player, position, rotation);
			GameInfo.setMainPlayer (instantiatedPlayer.GetComponent<GamePlayer>());
			GameInfo.MainPlayer.IsAlive = true;
		}

		public void respawn(GamePlayer player) {
			Debug.Log ("Respawning. " + player.IsAlive.ToString());
			if (player.IsAlive == false) {
				return;
			} else {
				Vector3 position = this.getSpawnLocation ();
				Quaternion rotation = this.getSpawnRotation ();

				Debug.Log ("Setting position.");
				player.setPosition (position, rotation);
			}
		}
		#endregion

		// Use this for initialization
		void Start () {
			GameInfo.Bonfires.Add (this);
			this.setSpawnKey ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}