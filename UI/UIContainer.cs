using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIContainer : UIObject
{
	public string ContainerName;

	public UIAlign anchor = UIAlign.TOP_LEFT;
	private MList<UIObject> list = new MList<UIObject>();
	private Rect bounds;
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
		base.SetScreenPosition(p);
		foreach (UIObject o in list)
		{
			o.SetScreenPosition(p + o.GetRelativeWithOffset());
		}
	}

	public void Add(UIObject newObj, UIDir dir)
	{
		Vector2 rel = Vector2.zero;

		if (list.Size() > 0)
		{
			UIObject last = list.Last();
			switch (dir)
			{
				case UIDir.W:
					rel = last.GetRelative() + new Vector2(last.GetWidth(), 0);
					break;
				case UIDir.S:
					rel = new Vector2(origin.x, last.GetRelative().y - last.GetHeight());
					break;
				default:
					UT.assert(false, "button: invalid direction");
					break;
			}
		}

		UT.print("REL: " + rel);

		newObj.SetRelative(rel);
		list.AddLast(newObj);

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
			bounds = it.Value().GetRelativeRect(); // start with the first object
			UT.print("UpdateBounds: rect " + it.Value().GetRelativeRect() + ", bounds " + bounds);
			while (it.Next())
			{
				bounds = AMenu.Union(bounds, it.Value().GetRelativeRect());
				UT.print("UpdateBounds: rect " + it.Value().GetRelativeRect() + ", bounds " + bounds);
			}
		}
	}
}