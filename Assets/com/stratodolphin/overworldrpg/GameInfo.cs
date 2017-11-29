using System;
using UnityEngine;
using com.stratodolphin.overworldrpg.Characters.Spawning;
using System.Collections.Generic;

namespace com.stratodolphin.overworldrpg.Characters
{
	/// <summary>
	/// Keeps track of various global variables for the game. This
	/// includes players, characters, bonfires and other stuff like
	/// that. This is not a <see cref="MonoBehavior"/> script and
	/// does not have an update function. It simply stores information
	/// that should be global.
	/// </summary>
	public abstract class GameInfo
	{
		#region Prefabs
		/// <summary>
		/// The animated enemy that is swinging.
		/// </summary>
		public static GameObject PrefabEnemySwing;

		/// <summary>
		/// The animated enemy that is walking.
		/// </summary>
		public static GameObject PrefabEnemyWalk;

		/// <summary>
		/// The animated enemy that is preparing to walk.
		/// </summary>
		public static GameObject PrefabEnemyPrepWalk;

		/// <summary>
		/// The animated enemy that is not doing anything.
		/// </summary>
		public static GameObject PrefabEnemyBase;

		/// <summary>
		/// The animated enemy that is dead.
		/// </summary>
		public static GameObject PrefabEnemyDead;

		/// <summary>
		/// The prefab that contains all the information and objects
		/// connected to an enemy AI.
		/// </summary>
		public static GameObject PrefabEnemyUnit;

		/// <summary>
		/// The animated main player that is swinging.
		/// </summary>
		public static GameObject PrefabMainPlayerSwing;

		/// <summary>
		/// The animated main player that is walking.
		/// </summary>
		public static GameObject PrefabMainPlayerWalk;

		/// <summary>
		/// The animated main player that is preparing to walk.
		/// </summary>
		public static GameObject PrefabMainPlayerPrepWalk;

		/// <summary>
		/// The animated main player that is not doing anything.
		/// </summary>
		public static GameObject PrefabMainPlayerBase;

		/// <summary>
		/// The animated enemy that is dead.
		/// </summary>
		public static GameObject PrefabMainPlayerDead;

		/// <summary>
		/// The animated main player that is preparing to swing.
		/// </summary>
		public static GameObject PrefabMainPlayerPrepMelee;

		/// <summary>
		/// The prefab that contains all the information and objects
		/// connected to the main player.
		/// </summary>
		public static GameObject PrefabMainPlayerUnit;
		#endregion

		/// <summary>
		/// Actual value for the main player global variable. This should
		/// only be accessed by the Attribute <see cref="Game.MainPlayer"/>
		/// except by the initialize function.
		/// </summary>
		private static GamePlayer _mainPlayer;

		/// <summary>
		/// Accessor for Game._mainPlayer.
		/// </summary>
		/// <value>The main player.</value>
		public static GamePlayer MainPlayer { get { return _mainPlayer; } }

		/// <summary>
		/// The list of bonfires that this player can spawn from.
		/// </summary>
		private static List<Bonfire> _bonfires = new List<Bonfire> ();

		/// <summary>
		/// Public accessor for Game._bonfires.
		/// </summary>
		/// <value>The bonfires.</value>
		public static List<Bonfire> Bonfires { get{ return _bonfires; } }

		/// <summary>
		/// Loads all the prefabs that should be set as global templates
		/// to be used for object creation.
		/// </summary>
		protected static void loadPrefabs() {
			PrefabEnemyUnit = (GameObject) Resources.Load ("prefabs/Characters/Enemies/Unit");
			UnityEngine.Object[] enemyPrefabs = Resources.LoadAll ("prefabs/Characters/Enemies/AnimatedModels/");
			foreach (GameObject prefab in enemyPrefabs) {
				String prefabName = prefab.name;
				switch (prefabName) {
				case "Dead":
					PrefabEnemyDead = prefab;
					break;
				case "Still":
					PrefabEnemyBase = prefab;
					break;
				case "PrepareWalk":
					PrefabEnemyPrepWalk = prefab;
					break;
				case "Swing":
					PrefabEnemySwing = prefab;
					break;
				case "Walk":
					PrefabEnemySwing = prefab;
					break;
				}
			}

			//PrefabMainPlayerUnit = (GameObject) Resources.Load ("prefabs/MainPlayer");
			PrefabMainPlayerUnit = (GameObject) Resources.Load ("prefabs/Characters/MainPlayer/Unit");
			UnityEngine.Object[] mainPlayerPrefabs = Resources.LoadAll ("prefabs/Characters/MainPlayer/AnimatedModels/");
			foreach (GameObject prefab in mainPlayerPrefabs) {
				String prefabName = prefab.name;
				switch (prefabName) {
				case "Dead":
					PrefabMainPlayerDead = prefab;
					break;
				case "Still":
					PrefabMainPlayerBase = prefab;
					break;
				case "PrepareWalk":
					PrefabMainPlayerPrepWalk = prefab;
					break;
				case "Swing":
					PrefabMainPlayerSwing = prefab;
					break;
				case "Walk":
					PrefabMainPlayerSwing = prefab;
					break;
				case "PrepareMelee":
					PrefabMainPlayerPrepMelee = prefab;
					break;
				}
			}
		}

		/// <summary>
		/// Sets the main player of the game.
		/// </summary>
		/// <param name="player">Player.</param>
		public static void setMainPlayer(GamePlayer player) {
			if (_mainPlayer != null && player != null) {
				Debug.LogError ("MainPlayer already set!");
				return;
			}

			Debug.Log ("Setting Game Player.");
			_mainPlayer = player;
            if (player != null) { player.IsMainPlayer = true; }
		}

		/// <summary>
		/// Initialize the global variables in this class for the game.
		/// </summary>
		public static void initialize() {
            foreach (GameObject bonfire in GameObject.FindGameObjectsWithTag("Bonfire"))
            {
                Bonfire fireObject = bonfire.GetComponent<Bonfire>();
                Bonfires.Add(fireObject);
            }
            // Allow user to spawn at the very first bonfire at least.
            Bonfires[0].IsActivated = true;
			loadPrefabs ();
		}
	}
}

