using com.stratodolphin.overworldrpg.Characters.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace com.stratodolphin.overworldrpg.Characters.UI
{
	public class PauseUIViewModel : MonoBehaviour {

		public GameObject PauseSelectionButtonPrefab;
		private int count = 1;

		/// <summary>
		/// Creates the background, title, and buttons that represent bonfires that the main
		/// character can spawn at.
		/// </summary>
		protected void createButtons()
		{
			GameObject uiTitle = createButtonSelection (this.PauseSelectionButtonPrefab, "Pause");
			GameObject inventory = createButtonSelection (this.PauseSelectionButtonPrefab, "Inventory");
			inventory.GetComponent<Button>().onClick.AddListener(() => { onClickPauseGUI("Inventory"); });
			GameObject stats = createButtonSelection (this.PauseSelectionButtonPrefab, "Stats");
			stats.GetComponent<Button>().onClick.AddListener(() => { onClickPauseGUI("Stats"); });
			GameObject quit = createButtonSelection (this.PauseSelectionButtonPrefab, "Quit");
			quit.GetComponent<Button>().onClick.AddListener(() => { onClickPauseGUI("Quit"); });
		}

		/// <summary>
		/// Creates a title button for the every selection depending on the name
		/// </summary>
		/// <param name="buttonPrefab"></param>
		/// <param name="background"></param>
		/// <returns></returns>
		protected GameObject createButtonSelection(GameObject buttonPrefab, string name) {
			Vector2 buttonSize;
			Vector2 positioning;

			if (name.Equals ("Background")) {
				buttonSize = new Vector2 (6, 4);
				positioning = new Vector2(500, 350);
			} else if (name.Equals ("Pause")) {
				buttonSize = new Vector2 (3, 3);
				positioning = new Vector2(550, 400);
			} else {
				buttonSize = new Vector2 (2, 2);
				positioning = new Vector2(550, 350 - (count * 80));
				count++;
			}


			GameObject button = Object.Instantiate(buttonPrefab);
			button.GetComponentInChildren<Text>().text = name;
			RectTransform rectTransform = button.GetComponent<RectTransform>();
			rectTransform.SetParent(this.gameObject.transform);
			button.transform.position = positioning;
			button.transform.localScale = buttonSize;
			return button;
		}


		#region Public Methods
		/// <summary>
		/// Show GUI depending on which button is pressed
		/// </summary>
		/// <param name="button"></param>
		public void onClickPauseGUI(string name)
		{
			//Depending on the button name, open up its corresponding GUI
			if (name.Equals ("Inventory")) {
				//get showInventory() from GamePlayer
				GameLogic.InvUI.initialize();
			} else if (name.Equals ("Stats")) {
				//have to make a stats GUI and show it
				GameLogic.StatsUI.initialize();
			} else if (name.Equals ("Quit")) {
				//NOT Working
				SceneManager.LoadScene("TitleScreen");
			}
			GameLogic.PauseUI.hide();
			return;
		}

		/// <summary>
		/// Initializes this Canvas by creating the pause selection buttons.
		/// </summary>
		public void initialize()
		{
			this.createButtons();
			this.hide ();
		}

		public void show()
		{
			this.gameObject.SetActive(true);
		}

		public void hide()
		{
			this.gameObject.SetActive(false);
		}
		#endregion

		#region Frames
		// Use this for initialization
		void Start () {
			
		}
		#endregion
	}
}
