using System;

namespace AssemblyCSharp
{
	public abstract class Game
	{
		/// <summary>
		/// Actual value for the main player global variable. This should
		/// only be accessed by the Attribute <see cref="Game.MainPlayer"/>
		/// except by the initialize function.
		/// </summary>
		private static Player _mainPlayer;

		/// <summary>
		/// Accessor for Game._mainPlayer.
		/// </summary>
		/// <value>The main player.</value>
		public static Player MainPlayer { get { return _mainPlayer; } }

		/// <summary>
		/// Initialize the global variables in this class for the game.
		/// </summary>
		public static void initialize() {

		}
	}
}

