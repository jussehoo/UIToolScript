using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class AMenu : MonoBehaviour {

	public GameObject BackBlocker;

		//ButtonTmp,
		//TextTmp,
		//ImageTmp;

	public const float REF_WIDTH = 480f;

	UIContainer main;

	public Dir anchor = Dir.TOP_LEFT;
	public Vector2 origin = new Vector2(0, 0);
	private bool closeOnBlockerClicked;
	private UIManager manager;

	private bool closed = false;

	void Awake ()
	{
		BackBlocker.SetActive(false);
		//ButtonTmp.SetActive(false);
		//TextTmp.SetActive(false);
		//ImageTmp.SetActive(false);

		main = gameObject.AddComponent<UIContainer>();
	}
	
	public void Initialize(UIManager m, Dir _anchor)
	{
		manager = m;
		anchor = _anchor;
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

	public void AddObject(UIObject objectTemplate, Dir dir)
	{
		InitializeNewObject(Instantiate(objectTemplate), dir);
	}

	private void InitializeNewObject(UIObject obj, Dir dir)
	{
		GameObject item = obj.gameObject;
		item.SetActive(true);
		item.transform.SetParent(transform);
		item.transform.localScale = new Vector3(1, 1, 1);

		UIObject buttonObject = item.AddComponent<UIObject>();

		UT.print("Init. size: " + buttonObject.GetWidth() + ", " + buttonObject.GetHeight());

		main.Add(buttonObject, anchor, dir); // add new item after previous to 'dir' direction.

		if (anchor == Dir.CENTER)
		{
			Vector2 offset = new Vector2(main.bounds.width / 2f, main.bounds.height / 2f);
			main.SetScreenPosition(getScreenPosition(0, 0) + offset);
		}
		else if (anchor == Dir.TOP_LEFT)
		{
			main.SetScreenPosition(getScreenPosition(-.5f, .5f));
		}
		else if (anchor == Dir.BOTTOM_LEFT)
		{
			Vector2 offset = new Vector2(0f, main.bounds.height);
			main.SetScreenPosition(getScreenPosition(-.5f, -.5f) + offset);
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
