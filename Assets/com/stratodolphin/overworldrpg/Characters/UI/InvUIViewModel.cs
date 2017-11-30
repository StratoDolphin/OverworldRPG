using com.stratodolphin.overworldrpg.Characters.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace com.stratodolphin.overworldrpg.Characters.UI
{
	public class InvUIViewModel : MonoBehaviour {

		public GameObject InvSelectionButtonPrefab;
		private int count = 1;

		public bool invPressed = false;
		#region Protected Attributes
		/// <summary>
		/// Height of the buttons that can be pressed to select the
		/// bonfire that the player wants to spawn at.
		/// </summary>
		protected int _buttonHeight = 25;

		/// <summary>
		/// Width of the buttons that can be pressed to select the
		/// bonfire that the player wants to spawn at.
		/// </summary>
		protected int _buttonWidth = 100;


		#endregion

		#region Protected Methods
		/// <summary>
		/// Creates the background, title, and buttons that represent bonfires that the main
		/// character can spawn at.
		/// </summary>
		protected void createButtons()
		{
			GameObject inventory = createButtonSelection (this.InvSelectionButtonPrefab, "Inventory");
			inventory.GetComponent<Button> ().onClick.AddListener (() => {
				onClickInvGUI ("Inventory");
			});
			GameObject back = createButtonSelection (this.InvSelectionButtonPrefab, "Back");
			back.GetComponent<Button> ().onClick.AddListener (() => {
				onClickInvGUI ("Back");
			});

			if (GameInfo.MainPlayer != null) {
				//get the toString of the GamePlayer, loop through it, and create buttons for them
				string data = GameInfo.MainPlayer.showInventory ();
				string[] inv = data.Split (' ');
				for (int i = 1; i < inv.Length - 1; i++) {
					GameObject item = createButtonSelection (this.InvSelectionButtonPrefab, inv [i]);
					item.GetComponent<Button> ().onClick.AddListener (() => {
						onClickInvGUI (inv [i - 1]);
					});

				}
			}
		}

		/// <summary>
		/// <para>
		/// Returns the position of the button to be placed.
		/// </para>
		/// <para>
		/// The buttons will be placed in columns. When one column
		/// is full of buttons, the next button will be placed on
		/// the next column and the y value will be reset to the
		/// top of the menu.
		/// </para>
		/// <para>
		/// This will return a 2D vector that is the starting point
		/// of the buttons using the bottom left corner of the buttons
		/// and the 2D vector of the starting point of the top right
		/// corner of the buttons.
		/// </para>
		/// </summary>
		/// <param name="menuX"></param>
		/// <param name="menuY"></param>
		/// <param name="buttonIndex"></param>
		/// <returns>
		/// The 2D vector of the starting point of the
		/// bottom left corner of the button and the 
		/// 2D vector of the starting point of the
		/// top right corner of the button
		/// </returns>
		protected Vector2[] getButtonStartPosition()
		{
			// Initial variables
			int buttonXStart = -105;
			int buttonYStart = -50;
			int xPadding = 10;
			int yPadding = 15;
			int buttonsPerColumn = 4;

			// Calculate the Y position
			int posY = buttonYStart;
			int rowIndex = count % buttonsPerColumn;
			int rowSpacing = yPadding + _buttonHeight;
			//this should be "-=" so that the list goes down -anthony
			posY -= rowIndex * rowSpacing;

			// Calculate the X position
			int posX = buttonXStart;
			int columnIndex = (int)(count / buttonsPerColumn);
			int columnSpacing = xPadding + _buttonWidth;
			posX += columnIndex * columnSpacing;

			Vector2 min = new Vector2(posX, posY);
			Vector2 max = new Vector2(posX + _buttonWidth, posY + _buttonHeight);
			count++;
			return new Vector2[] { min, max };
		}

		/// <summary>
		/// Creates a button for title and inventory
		/// </summary>
		/// <param name="buttonPrefab"></param>
		/// <param name="background"></param>
		/// <returns></returns>
		protected GameObject createButtonSelection(GameObject buttonPrefab, string name) {
			Vector2 buttonSize;
			Vector2[] positioning;
			GameObject button;

			if (name.Equals ("Inventory")) {
				buttonSize = new Vector2 (3, 2);
				Vector2 pos = new Vector2(500, 350);
				button = Object.Instantiate(buttonPrefab);
				button.GetComponentInChildren<Text>().text = name;
				RectTransform rectTransform = button.GetComponent<RectTransform>();
				rectTransform.SetParent(this.gameObject.transform);
				button.transform.position = pos;
				button.transform.localScale = buttonSize;
			} else {
				positioning = getButtonStartPosition();
				button = Object.Instantiate(buttonPrefab);
				button.GetComponentInChildren<Text>().text = name;
				RectTransform rectTransform = button.GetComponent<RectTransform>();
				rectTransform.SetParent(this.gameObject.transform);
				rectTransform.offsetMin = positioning[0];
				rectTransform.offsetMax = positioning[1];

			}
			return button;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Show Effects depending on what the name is
		/// </summary>
		/// <param name="button"></param>
		public void onClickInvGUI(string name)
		{
			//Depending on the button name, call a certain effect
			if (name.Equals ("Back")) {
				GameLogic.InvUI.hide ();
				GameLogic.PauseUI.show ();
			} else {
				//call an effect
				GameInfo.MainPlayer._inventory.getItemEffect(name);
			}
			GameLogic.InvUI.hide();
			return;
		}

		/// <summary>
		/// Initializes this Canvas by creating the Inventory selection buttons.
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

		// Use this for initialization
		void Start () {
			
		}
	}
}
