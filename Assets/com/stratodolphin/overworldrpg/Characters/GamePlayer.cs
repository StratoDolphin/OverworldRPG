﻿using UnityEngine;
using System.Collections;
using com.stratodolphin.overworldrpg.Characters;

public class GamePlayer : FeistyGameCharacter
{
	#region Private Variables
	#endregion

	#region Public Attributes
	/// <summary>
	/// Determines whether or not this player is on the map,
	/// working and alive.
	/// </summary>
	public bool IsAlive;
	#endregion

	// Use this for initialization
	void Start ()
	{
		GameInfo.setMainPlayer (this);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

