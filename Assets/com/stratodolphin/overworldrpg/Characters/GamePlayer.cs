

using UnityEngine;
using System.Collections;
using com.stratodolphin.overworldrpg.Characters.Inventory;
using com.stratodolphin.overworldrpg.Characters.Spawning;
using System.Collections.Generic;

public class GamePlayer : FeistyGameCharacter
{
	#region Private Variables
	#endregion

	#region Public Attributes
	/// <summary>
	/// Determines whether or not this player is on the map,
	/// working and alive.
	/// </summary>
	public bool IsAlive = false;

	/// <summary>
	/// The main camera that renders the game from the main players
	/// first person point of view.
	/// </summary>
	public Camera MainCamera;
    #endregion

    /// <summary>
    /// The bon fire that this character is currently close to. This
    /// will be null unless this character is within the respawn
    /// collider attached to a bonfire. This list simply stores
    /// the indexes in <see cref="GameInfo.Bonfires"/> that this
    /// bonfire exists.
    /// </summary>
    public Bonfire BonfireLocation;

	private string pressR;
	private string uiMessageString;
	private string communicator;
	//need to pass the item into the Inventory
	protected GameObject item;
	//make this a text, then list every item the player collects in the text
	public string inventoryName;

	// Use this for initialization
	void Start () {
		//have to call this so the instances of objects can be called first
		base.Start ();
	}

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        {
            this.decreaseHealth(10);
        }

        if (communicator == "pressR" && Input.GetKeyDown (KeyCode.R)) {
			//Get the tag of the GameObject and send it to the Player's inventory
			/*
			 * The Inventory GUI (not made yet) will have all of the items that the player has collected.
			 * Each Item will have a script giving its "Effect"
			 * When the user clicks on an item in the Inventory GUI, that item's "Effect" function will occur.
			 * It will affect the Player accordingly.
			 */
			//collects item and passes it into the inventory: DONE
			item.transform.parent = transform;
			foreach (Transform thing in this.gameObject.GetComponentsInChildren<Transform>()) {
				if (thing.name.StartsWith ("storable_")) {
					this.addItemToInventory (thing);
				}
				//takes off the GUI
				uiMessageString = null;
			}
		} 

		if (Input.GetKey (KeyCode.P)) {
			uiMessageString = null;
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			this._hitPoints = 0;
		}

		//have to apply the logic, then send it up the heiarchy in update
		base.Update ();
	}

    /// <summary>
    /// <para>
    /// Adds the storableTransform to this ai's inventory.
    /// </para>
    /// <para>
    /// If storableTransform is a ranged weapon, it is put into this
    /// characters left hand. If it is a melee weapon, it's put into
    /// the right hand. Otherwise, it's just put in this characters
    /// regular inventory.
    /// </para>
    /// </summary>
    /// <param name="storableTransform">Storable transform.</param>
    protected void addItemToInventory(Transform storableTransform)
    {
        Storable storableScript = storableTransform.GetComponent<Storable>();
        //Debug.Log ("type: " + storableScript.Type);
        if (storableScript.Type == Storable.TYPE_RANGE)
        {
            //Debug.Log ("hand: left.");
            this._leftHandInventory.add(storableScript);
        }
        else if (storableScript.Type == Storable.TYPE_MELEE)
        {
            //Debug.Log ("hand: right.");
            this._rightHandInventory.add(storableScript);
        }
        else
        {
            //Debug.Log ("regular inventory.");
            this._inventory.add(storableScript);
            //storableScript.ApplyActionsOnPickup(this);
        }
    }

	public string showInventory() {
		
		//uiMessageString = "<color=white>" + this._inventory.ToString () + "</color>";
		string mes = "<color=white> " + this._inventory.ToString () + " </color>";
		return mes;
	}

	void OnTriggerEnter(Collider otherObjective)
	{
		pressR = "Press R to collect";
		if (otherObjective.tag == "Item" && !(Input.GetKeyDown (KeyCode.R))) {
			communicator = "pressR";
			uiMessageString = "<color=white>" + pressR + "</color>";
			//collects the gameObject and name to pass it to inventory
			item = otherObjective.gameObject;
			inventoryName = otherObjective.gameObject.name;
		} 
	}
	void OnTriggerExit(Collider other)
	{
		uiMessageString = null;
		communicator = "";
	}
	void OnGUI()
	{
		if (uiMessageString != null) {
			GUI.Box (new Rect (0, 400, 1000, 200), uiMessageString);
		} 
	}
}

