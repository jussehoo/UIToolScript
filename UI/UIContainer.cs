﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIContainer : UIObject
{
	public MList<UIObject> list = new MList<UIObject>();

	public Rect bounds;

	Vector2 origin = Vector2.zero;

	override public float GetWidth()
	{
		return bounds.width;
	}
	override public float GetHeight()
	{
		return bounds.height;
	}
	override public void SetScreenPosition(Vector2 p)
	{
		foreach (UIObject o in list)
		{
			o.SetScreenPosition(p + o.GetRelative());
		}
	}

	public void Add(UIObject buttonObject, Dir parentAnchor, Dir dir)
	{

		Vector2 rel = Vector2.zero;

		if (list.Size() > 0)
		{
			UIObject last = list.Last();
			switch (dir)
			{
				case Dir.RIGHT:
					rel = last.GetRelative() + new Vector2(last.GetWidth(), 0);
					break;
				case Dir.BELOW:
					rel = new Vector2(origin.x - buttonObject.GetWidth() / 2f, last.GetRelative().y - last.GetHeight());
					break;
				default:
					UT.assert(false, "button: invalid direction");
					break;
			}
		}
		else if (parentAnchor == Dir.CENTER)
		{
			rel = new Vector2(-buttonObject.GetWidth() / 2f, 0);
		}

		buttonObject.SetRelative(rel);
		list.AddLast(buttonObject);

		UpdateBounds();

		UT.print("New bounds: " + bounds);
	}

	public void UpdateBounds()
	{
		if (list.Size() == 0)
		{
			bounds = new Rect(); // no objects, no bounds
		}
		else
		{
			var it = list.Iterator();
			it.Next();
			bounds = it.Value.GetRelativeRect(); // start with the first object
			UT.print("UpdateBounds: rect " + it.Value.GetRelativeRect() + ", bounds " + bounds);
			while (it.Next())
			{
				bounds = AMenu.Union(bounds, it.Value.GetRelativeRect());
				UT.print("UpdateBounds: rect " + it.Value.GetRelativeRect() + ", bounds " + bounds);
			}
		}
	}
}