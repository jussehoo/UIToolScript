﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class AMenu : MonoBehaviour {

	public GameObject BackBlocker;

	public const float REF_WIDTH = 768f;

	public UIContainer main;
	private MList<UIContainer> containers = new MList<UIContainer>();

	public Vector2 origin = new Vector2(0, 0);
	private bool closeOnBlockerClicked;
	private UIManager manager;

	private bool closed = false;

	void Awake ()
	{
		BackBlocker.SetActive(false);
		main = AddContainer("MAIN", Dir.TOP_LEFT);

	}

	public UIContainer AddContainer(string name, Dir anchor)
	{
		UIContainer c = gameObject.AddComponent<UIContainer>();
		containers.AddLast(c);
		c.name = name;
		c.anchor = anchor;
		return c;
	}

	public void Initialize(UIManager m)
	{
		manager = m;
	}

	void OnDestroy()
	{
		// TODO: proper closing in all cases
		//AG.assert(closed == true);
	}

	public void Close()
	{
		//AG.assert(closed == false);
		manager.SetClosed(this);
		closed = true;
		Destroy(gameObject);
	}

	public void SetClosed() { closed = true; }

	public void SetLayerOrder(int i)
	{
		GetComponent<Canvas>().sortingOrder = i;
	}

	public int GetLayerOrder()
	{
		return GetComponent<Canvas>().sortingOrder;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void BlockerClicked()
	{
		if (closeOnBlockerClicked) Close();
	}

	public void ActivateBackBlocker(bool _closeOnBlockerClicked)
	{
		BackBlocker.SetActive(true);
		closeOnBlockerClicked = _closeOnBlockerClicked;
	}

	public void ActivateFrontBlocker(Action act)
	{
		UT.trap();
	}

	public void AddObject(UIObject newObject, UIContainer container, Dir dir)
	{
		InitializeNewObject(transform, container, newObject, dir);
	}

	public void AddObject(UIObject newObject, Dir dir)
	{
		InitializeNewObject(transform, main, newObject, dir);
	}

	private static void InitializeNewObject(Transform parent, UIContainer container, UIObject obj, Dir dir)
	{
		GameObject item = obj.gameObject;
		item.SetActive(true);
		item.transform.SetParent(parent);
		item.transform.localScale = new Vector3(1, 1, 1);

		UT.print("Init. size: " + obj.GetWidth() + ", " + obj.GetHeight());

		container.Add(obj, container.anchor, dir); // add new item after previous to 'dir' direction.

		if (container.anchor == Dir.CENTER)
		{
			Vector2 offset = new Vector2(-container.GetWidth() / 2f, container.GetHeight() / 2f);
			container.SetScreenPosition(getScreenPosition(0, 0) + offset);
		}
		else if (container.anchor == Dir.TOP_LEFT)
		{
			container.SetScreenPosition(getScreenPosition(-.5f, .5f));
		}
		else if (container.anchor == Dir.BOTTOM_LEFT)
		{
			Vector2 offset = new Vector2(0f, container.GetHeight());
			container.SetScreenPosition(getScreenPosition(-.5f, -.5f) + offset);
		}
		else
		{
			UT.trap("AMenu.InitializeButton: invalid orientation.");
		}
	}


	public static Rect Union(Rect a, Rect b)
	{
		// return smallest rectangle to include 'a' and 'b'
		return Rect.MinMaxRect(
			Mathf.Min(a.xMin, b.xMin),
			Mathf.Min(a.yMin, b.yMin),
			Mathf.Max(a.xMax, b.xMax),
			Mathf.Max(a.yMax, b.yMax));
	}

	public static Vector2 getScreenPosition(float mx, float my)
	{
		return new Vector2(REF_WIDTH * mx, (Screen.height * (REF_WIDTH / Screen.width) * my));
	}
}