using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.stratodolphin.overworldrpg.Characters.Spawning;
using com.stratodolphin.overworldrpg.Characters.UI;

namespace com.stratodolphin.overworldrpg.Characters
{
	public class GameLogic : MonoBehaviour {

		#region Spawning
        /// <summary>
        /// Getter for the UI canvas that allows user to spawn at a
        /// chosen campfire.
        /// </summary>
        /// <remarks>
        /// I cache it so that, if it is disactive when it's hidden, it
        /// can still be retreived.
        /// </remarks>
        private static SpawnUIViewModel _spawnUI;
        public static SpawnUIViewModel SpawnUI
        {
            get {
                if (_spawnUI != null) { return _spawnUI; }
                SpawnUIViewModel ui = GameObject.Find("SpawnUICanvas").GetComponent<SpawnUIViewModel>();
                _spawnUI = ui;
                return _spawnUI;
            }
        }

        /// <summary>
        /// Shows or hides the Spawn UI Canvas.
        /// </summary>
        /// <param name="visibility"></param>
        public static void toggleSpawnUIVisibility()
        {
            if (SpawnUI.gameObject.activeSelf)
            {
                SpawnUI.hide();
            } else
            {
                SpawnUI.show();
            }
        }
		#endregion

		#region Pausing
		/// <summary>
		/// Getter for the UI canvas that allows user to pause at any
		/// time.
		/// </summary>
		/// <remarks>
		/// I cache it so that, if it is disactive when it's hidden, it
		/// can still be retreived.
		/// </remarks>
		private static PauseUIViewModel _pauseUI;
		public static PauseUIViewModel PauseUI
		{
			get {
				if (_pauseUI != null) { return _pauseUI; }
				PauseUIViewModel ui = GameObject.Find("PauseUICanvas").GetComponent<PauseUIViewModel>();
				_pauseUI = ui;
				return _pauseUI;
			}
		}

		/// <summary>
		/// Shows or hides the Pause UI Canvas.
		/// </summary>
		/// <param name="visibility"></param>
		public static void togglePauseUIVisibility() {
			if (PauseUI.gameObject.activeSelf)
			{
				PauseUI.hide();
			} else
			{
				PauseUI.show();
			}
		}
		#endregion

		#region Inventory
		/// <summary>
		/// Getter for the UI canvas that allows user to go
		/// to their Inventory
		/// </summary>
		/// <remarks>
		/// I cache it so that, if it is disactive when it's hidden, it
		/// can still be retreived.
		/// </remarks>
		private static InvUIViewModel _invUI;
		public static InvUIViewModel InvUI
		{
			get {
				if (_invUI != null) { return _invUI; }
				InvUIViewModel ui = GameObject.Find("InvUICanvas").GetComponent<InvUIViewModel>();
				_invUI = ui;
				return _invUI;
			}
		}

		/// <summary>
		/// Shows or hides the Inventory UI Canvas.
		/// </summary>
		/// <param name="visibility"></param>
		public static void toggleInvUIVisibility() {
			if (InvUI.gameObject.activeSelf)
			{
				InvUI.hide();
			} else
			{
				InvUI.show();
			}
		}
		#endregion

		#region Stats
		/// <summary>
		/// Getter for the UI canvas that allows user to go
		/// to their Inventory
		/// </summary>
		/// <remarks>
		/// I cache it so that, if it is disactive when it's hidden, it
		/// can still be retreived.
		/// </remarks>
		private static StatsUIViewModel _statsUI;
		public static StatsUIViewModel StatsUI
		{
			get {
				if (_statsUI != null) { return _statsUI; }
				StatsUIViewModel ui = GameObject.Find("StatsUICanvas").GetComponent<StatsUIViewModel>();
				_statsUI = ui;
				return _statsUI;
			}
		}

		/// <summary>
		/// Shows or hides the Inventory UI Canvas.
		/// </summary>
		/// <param name="visibility"></param>
		public static void toggleStatsUIVisibility() {
			if (StatsUI.gameObject.activeSelf)
			{
				StatsUI.hide();
			} else
			{
				StatsUI.show();
			}
		}

		public void refresh() {
			_spawnUI = null;
			_statsUI = null;
			_pauseUI = null;
			_invUI = null;
			SpawnUI.initialize();
			PauseUI.initialize ();
			InvUI.initialize ();
		}
		#endregion
		// Use this for initialization
		void Start () {
			GameInfo.initialize ();
            SpawnUI.initialize();
			PauseUI.initialize ();
			InvUI.initialize ();
		}
		
		// Update is called once per frame
		void Update () { }
	}
}