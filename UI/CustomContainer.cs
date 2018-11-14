using UnityEngine;
using System.Collections;

public class CustomContainer : UIContainer
{
	private Vector2 size;
	public CustomContainer(Vector2 _size)
	{
		size = _size;
	}
	override public float GetWidth()
	{
		return size.x;
	}
	override public float GetHeight()
	{
		return size.y;
	}
}
