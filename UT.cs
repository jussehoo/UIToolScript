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

	// touch/pointer utilities (one finger control)

	public static Vector2? PointerPosition()
	{
		if (!IsTouching()) return null;
#if UNITY_EDITOR
		return Input.mousePosition;
#else
		return Input.GetTouch(0).position;
#endif
	}

	public static bool IsTouching()
	{
#if UNITY_EDITOR
		return Input.GetMouseButton(0);
#else
		return Input.touchCount > 0;
#endif
	}
	public static bool TouchStarted()
	{
		// true on first frame when start touching
#if UNITY_EDITOR
		return Input.GetMouseButtonDown(0);
#else
		return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
	}
	public static bool TouchEnded()
	{
		// true on first frame when stop touching
#if UNITY_EDITOR
		return Input.GetMouseButtonUp(0);
#else
		return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended;
#endif
	}

	public static float VerticalFOV(float hFOVInDeg, float aspectRatio)
	{
		// TODO: description
		float hFOVInRads = hFOVInDeg * Mathf.Deg2Rad;
		float vFOVInRads = 2 * Mathf.Atan(Mathf.Tan(hFOVInRads / 2) / aspectRatio);
		float vFOV = vFOVInRads * Mathf.Rad2Deg;
		return vFOV;
	}
}
