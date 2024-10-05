using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : BaseHealth
{
    public static PlayerStats instance;

	protected override void Awake()
	{
		base.Awake();
		instance = this;
	}
}
