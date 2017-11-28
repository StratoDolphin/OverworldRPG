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
		}
	}
}

