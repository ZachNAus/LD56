using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadyBugDialogue : DialogueManager
{
	public static LadyBugDialogue instance;

	private void Awake()
	{
		instance = this;
	}
}
