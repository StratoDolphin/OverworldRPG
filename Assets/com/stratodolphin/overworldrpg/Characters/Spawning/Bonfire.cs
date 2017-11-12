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


		public void spawn(GamePlayer player) {
			Vector3 position = this.gameObject.transform.position;
			position.x -= 5;
			Quaternion rotation = Quaternion.LookRotation (this.gameObject.transform.position);

			Instantiate (player.gameObject, position, rotation);
			player.IsAlive = true;
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