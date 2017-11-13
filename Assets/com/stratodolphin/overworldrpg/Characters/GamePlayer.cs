using UnityEngine;
using System.Collections;
using com.stratodolphin.overworldrpg.Characters;
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

	/// <summary>
	/// The bon fire that this character is currently close to. This
	/// will be null unless this character is within the respawn
	/// collider attached to a bonfire.
	/// </summary>
	public Bonfire BonfireLocation;
	#endregion

	void Start ()
	{
        base.Start();
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.K))
        {
            this._hitPoints = 0;
        }
        base.Update();
    }
}

