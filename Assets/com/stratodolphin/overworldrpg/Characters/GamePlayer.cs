﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class GamePlayer : FeistyGameCharacter
{
	// Use this for initialization
	void Start ()
	{
		Game.setMainPlayer (this);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
