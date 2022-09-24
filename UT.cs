#if UNITY_EDITOR || UNITY_STANDALONE
#define UT_MOUSE
#endif
#if !SERVER
	using UnityEngine;
#endif

public class UT
{
	public static readonly System.Random rnd = new System.Random(1234567);

	// Utilities

	public static float RandomFloat() { return (float) rnd.NextDouble(); }

	public static void Trap(string msg = "")
	{
		Assert(false, msg);
	}
	
	public static void Print(string s)
	{
#if SERVER
		System.Console.WriteLine(s);
#else
		Debug.Log(s);
#endif
	}
	public static void Error(string s)
	{
#if SERVER
		System.Console.WriteLine("ERROR: " + s); // TODO: write to error stream
#else
		Debug.LogError(s);
#endif
	}
	public static void Verbose(string s)
	{
		Print(s);
	}
	public static void Assert0to1(float f, string msg = "")
	{
		Assert(f >= 0 && f <= 1, msg);
	}
	public static void AssertNotNull(object o, string msg = "")
	{
		Assert(o != null, msg);
	}
	public static void Assert(bool assertion, string msg = "")
	{
		if (!assertion)
		{
#if SERVER
			System.Diagnostics.Debug.Assert(false,"ASSERTION FAILED: " + msg);
#else
			UnityEngine.Assertions.Assert.IsTrue(false, msg);
#endif
		}
	}

#if !SERVER
	// touch/pointer utilities (one finger control)

	public static Vector2? MousePosition()
	{
#if UT_MOUSE
		return Input.mousePosition;
#else
		return null;
#endif
	}

	public static Vector2? PointerPosition()
	{
		if (!IsTouching()) return null;
#if UT_MOUSE
		return Input.mousePosition;
#else
		return Input.GetTouch(0).position;
#endif
	}
	
	public static bool IsTouching()
	{
#if UT_MOUSE
		return Input.GetMouseButton(0);
#else
		return Input.touchCount > 0;
#endif
	}
	
	public static bool IsLeftButtonDown()
	{
#if UT_MOUSE
		return Input.GetMouseButton(0);
#else
		return false;
#endif
	}

	public static bool IsRightButtonDown()
	{
#if UT_MOUSE
		return Input.GetMouseButton(1);
#else
		return false;
#endif
	}

	public static bool TouchStarted()
	{
		// true on first frame when start touching
#if UT_MOUSE
		return Input.GetMouseButtonDown(0);
#else
		return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
	}
	public static bool TouchEnded()
	{
		// true on first frame when stop touching
#if UT_MOUSE
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
#endif // !SERVER
}
