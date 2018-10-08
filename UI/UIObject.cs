using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject : MonoBehaviour
{
	private Vector2 relative; // position, relative container

	virtual public float GetWidth()
	{
		return GetComponent<RectTransform>().rect.width;
	}
	virtual public float GetHeight()
	{
		return GetComponent<RectTransform>().rect.height;
	}
	virtual public void SetScreenPosition(Vector2 p)
	{
		transform.localPosition = p;
	}
	public Vector2 GetScreenPosition()
	{
		return transform.localPosition;
	}
	public void SetRelative(Vector2 p)
	{
		relative = p;
	}
	public Vector2 GetRelative()
	{
		return relative;
	}
	public Rect GetRelativeRect()
	{
		Vector2 p = GetRelative();
		// GameObject's position on is in top left corner so that's why 'y - height'
		return new Rect(p.x, p.y - GetHeight(), GetWidth(), GetHeight());
	}
}