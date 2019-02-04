using UnityEngine;

public class ASinPulse
{
	readonly float interval, height, offset;
	float d;
	public ASinPulse(float _interval, float _height, float _offset = 0f)
	{
		interval = _interval;
		height = _height;
		offset = _offset;
	}
	public float Get(float timeDelta)
	{
		d += timeDelta * interval * 3.14f;
		return (Mathf.Sin(d) * height) + offset;
	}
}
public static class Util
{

	public static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
	{
		Vector2 temp = camera.WorldToViewportPoint(position);
		temp.x *= canvas.sizeDelta.x;
		temp.y *= canvas.sizeDelta.y;
		temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
		temp.y -= canvas.sizeDelta.y * canvas.pivot.y;
		return temp;
	}
	// Unique int getter with a recognizable Value(), for constant ID's etc.
	private static int _unique = 333000;
	public static int Unique() { return ++_unique; }
}