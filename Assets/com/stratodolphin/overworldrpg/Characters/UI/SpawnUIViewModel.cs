using com.stratodolphin.overworldrpg.Characters.Spawning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.stratodolphin.overworldrpg.Characters.UI
{
    public class SpawnUIViewModel : MonoBehaviour
    {
        #region Public Attributes
        /// <summary>
        /// Prefab that will be used as a template to create a spawn
        /// selection button.
        /// </summary>
        public GameObject SpawnSelectionButtonPrefab;
        #endregion

        #region Protected Attributes
        /// <summary>
        /// List of buttons that are rendered on the screen to represent
        /// spawn selection buttons.
        /// </summary>
        protected List<SpawnSelectionButton> _selectionButtons = new List<SpawnSelectionButton>();

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
        protected Vector2[] getButtonStartPosition(int buttonIndex)
        {
            // Initial variables
            int buttonXStart = -105;
            int buttonYStart = -50;
            int xPadding = 10;
            int yPadding = 15;
            int buttonsPerColumn = 4;

            // Calculate the Y position
            int posY = buttonYStart;
            int rowIndex = buttonIndex % buttonsPerColumn;
            int rowSpacing = yPadding + _buttonHeight;
			//this should be "-=" so that the list goes down -anthony
            posY -= rowIndex * rowSpacing;

            // Calculate the X position
            int posX = buttonXStart;
            int columnIndex = (int)(buttonIndex / buttonsPerColumn);
            int columnSpacing = xPadding + _buttonWidth;
            posX += columnIndex * columnSpacing;

            Vector2 min = new Vector2(posX, posY);
            Vector2 max = new Vector2(posX + _buttonWidth, posY + _buttonHeight);
            return new Vector2[] { min, max };
        }

        /// <summary>
        /// Creates the background, title, and buttons that represent bonfires that the main
        /// character can spawn at.
        /// </summary>
        protected void createButtons()
        {
			//anthony's note: let's not draw the background of this menu using a button, lol. black looks better imo
			//GameObject background = createBonfireSelection (this.SpawnSelectionButtonPrefab, true);
			GameObject titleButton = createBonfireSelection (this.SpawnSelectionButtonPrefab, false);
            for (int i = 0; i < GameInfo.Bonfires.Count; i ++)
            {
                if (!GameInfo.Bonfires[i].IsActivated) { continue; }
                GameObject createdButton = CreateButton(this.SpawnSelectionButtonPrefab, i);
                createdButton.GetComponent<Button>().onClick.AddListener(() => { Click_SpawnOrRespawn(createdButton.GetComponent<SpawnSelectionButton>()); });
            }
        }

		/// <summary>
		/// Creates a title button with directions the user can follow...
		/// Also creates a background that makes it look cool.
		/// </summary>
		/// <param name="buttonPrefab"></param>
		/// <param name="background"></param>
		/// <returns></returns>
		/// <anthony's summary that is actually helpful >:( >
		/// Method that accepts a button prefab and draws a single button using that prefab.
		/// It draws it in one of 2 different sizes depending on if background is true or false.
		/// </anthony's summary that is actually helpful >:( >
		protected GameObject createBonfireSelection(GameObject buttonPrefab, bool background) {
			Vector2 buttonSize;
			Vector2 positioning = new Vector2(550, 400);
			if (background == true) {
				buttonSize = new Vector2 (400, 50);
			} else {
				buttonSize = new Vector2 (2, 2);
			}
			GameObject button = Object.Instantiate(buttonPrefab);
			button.GetComponentInChildren<Text>().text = "Choose a Bonfire";
			RectTransform rectTransform = button.GetComponent<RectTransform>();
			rectTransform.SetParent(this.gameObject.transform);
			button.transform.position = positioning;
			button.transform.localScale = buttonSize;
			return button;
		}

        /// <summary>
        /// Creates a single bonfire selection button that the main
        /// player can select to spawn at.
        /// </summary>
        /// <param name="buttonPrefab"></param>
        /// <param name="buttonIndex"></param>
        /// <returns></returns>
        protected GameObject CreateButton(GameObject buttonPrefab, int buttonIndex)
        {
            Vector2[] positioning = getButtonStartPosition(buttonIndex);
            GameObject button = Object.Instantiate(buttonPrefab);
            button.GetComponentInChildren<Text>().text = "Bonfire " + buttonIndex;
            button.GetComponent<SpawnSelectionButton>().BonfireIndex = buttonIndex;
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.SetParent(this.gameObject.transform);
            rectTransform.offsetMin = positioning[0];
            rectTransform.offsetMax = positioning[1];
            return button;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Spawns or respawns the main character at the bonfire that this
        /// action method is attached to.
        /// </summary>
        /// <param name="button"></param>
        public void Click_SpawnOrRespawn(SpawnSelectionButton button)
        {
            bool toRespawn = false;
            if (GameInfo.MainPlayer != null && GameInfo.MainPlayer.IsAlive)
            {
                toRespawn = true;
            }

            Bonfire fire = GameInfo.Bonfires[button.BonfireIndex];
            if (toRespawn)
            {
                if (GameInfo.MainPlayer.BonfireLocation != null)
                {
                    fire.respawn(GameInfo.MainPlayer);
                }
            }
            else
            {
                fire.spawn(GameLogic.createMainPlayerMeta());
            }
            GameLogic.SpawnUI.hide();
            return;
        }

        /// <summary>
        /// Initializes this Canvas by creating the bonfire selection buttons.
        /// </summary>
        public void initialize()
        {
            this.createButtons();
        }
        
        /// <summary>
        /// Refresh this Canvas by creating the bonfire selection buttons.
        /// This will make it so that newly activated bonfires are added
        /// to the ui selection panel.
        /// </summary>
        public void refresh()
        {
            this.createButtons();
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
    }
}