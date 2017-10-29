using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public abstract class Game
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

		public static void setMainPlayer(GamePlayer player) {
			if (_mainPlayer != null) {
				Debug.LogError ("MainPlayer already set!");
				return;
			}

			Debug.Log ("Setting Game Player to: " + player.GetInstanceID().ToString());
			_mainPlayer = player;
		}

		/// <summary>
		/// Initialize the global variables in this class for the game.
		/// </summary>
		public static void initialize() {

		}
	}
}

