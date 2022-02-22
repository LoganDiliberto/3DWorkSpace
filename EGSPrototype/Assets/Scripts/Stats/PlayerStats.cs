using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles the players stats and adds/removes modifiers when equipping items. */

public class PlayerStats : CharacterStats {

	// Use this for initialization
	void Start () {
	}

	public override void Die()
	{
		base.Die();
		PlayerManager.instance.KillPlayer();
	}
}