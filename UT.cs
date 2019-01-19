using System;
using UnityEngine;

public class UT
{
	// Utilities

	public static void trap(string msg = "") { assert(false, msg); }

	public static void assert(bool assertion, string msg = "")
	{
		if (!assertion)
		{
			Debug.Log("ASSERTION FAILED: " + msg);
			UnityEngine.Assertions.Assert.IsTrue(false);
		}
	}
	public static void print(string s)
	{
		Debug.Log(s);
	}
	public static void verbose(string s)
	{
		Debug.Log(s);
	}
}
