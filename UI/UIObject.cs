using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject : MonoBehaviour
{
	/*
	 * Creating a new object template:
	 *	- Set pivot point to top left corner
	*/

	private Vector2 relative; // position, relative to container
	private Vector2 offset;
	private Vector2 size;

	protected void Awake()
	{
		var rt = GetComponent<RectTransform>();
		size.x = rt.rect.width;
		size.y = rt.rect.height;

		offset.x = rt.pivot.x * size.x;
		offset.y = (rt.pivot.y - 1f) * size.y;
	}

	virtual public float GetWidth()
	{
		return size.x;
	}
	virtual public float GetHeight()
	{
		return size.y;
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
	public Vector2 GetRelativeWithOffset()
	{
		return relative + offset;
	}
	public Rect GetRelativeRect()
	{
		Vector2 p = GetRelative();
		// GameObject's position on is in top left corner so that's why 'y - height'
		return new Rect(p.x, p.y - GetHeight(), GetWidth(), GetHeight());
	}
}