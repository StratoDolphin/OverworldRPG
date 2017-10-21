using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Player : FeistyGameCharacter
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

