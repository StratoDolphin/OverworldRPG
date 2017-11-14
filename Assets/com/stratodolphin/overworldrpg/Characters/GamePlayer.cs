

using UnityEngine;
using System.Collections;
using com.stratodolphin.overworldrpg.Characters;
using Assets.com.stratodolphin.overworldrpg.Characters;
using Assets.com.stratodolphin.overworldrpg.Characters.Spawning;

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
	#endregion

	/// <summary>
	/// The bon fire that this character is currently close to. This
	/// will be null unless this character is within the respawn
	/// collider attached to a bonfire.
	/// </summary>
	public Bonfire BonfireLocation;

	private string pressR;
	private string temp;
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
					this.addToInventory (thing);
					Destroy (item);
					//Supposed to destroy the item's collider
					Destroy (item.GetComponent<Collider>());
					//takes off the GUI
					temp = null;
				}
			}
		} 

		if (Input.GetKey (KeyCode.I)) {
			showInventory ();
		} 

		if (Input.GetKey (KeyCode.P)) {
			temp = null;
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
	protected void addToInventory(Transform storableTransform) {
		Storable storableScript = storableTransform.GetComponent<Storable> ();
		if (storableScript.Type == Storable.TYPE_RANGE) {
			this._leftHandInventory.add (storableScript);
		} else if (storableScript.Type == Storable.TYPE_MELEE) {
			if (this._leftHandInventory.hasItemType(Storable.TYPE_RANGE)) {
				// If the player has a bow, just put the melee weapon
				// in the regular inventory for now.
				this._inventory.add (storableScript);
			} else {
				this._rightHandInventory.add (storableScript);
			}
		} else {
			this._inventory.add (storableScript);
		}
	}

	public void showInventory() {
		temp = "<color=white>" + this._inventory.ToString () + "</color>";
	}

	void OnTriggerEnter(Collider otherObjective)
	{
		pressR = "Press R to collect";
		if (otherObjective.tag == "Item" && !(Input.GetKeyDown (KeyCode.R))) {
			communicator = "pressR";
			temp = "<color=white>" + pressR + "</color>";
			//collects the gameObject and name to pass it to inventory
			item = otherObjective.gameObject;
			inventoryName = otherObjective.gameObject.name;
		} 
	}
	void OnTriggerExit(Collider other)
	{
		temp = null;
		communicator = "";
	}
	void OnGUI()
	{
		if (temp != null) {
			GUI.Box (new Rect (0, 400, 1000, 200), temp);
		} 
	}
}

