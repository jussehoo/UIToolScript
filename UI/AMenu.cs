using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class AMenu : MonoBehaviour {

	public GameObject BackBlocker, FrontBlocker;

	public const float REF_WIDTH = 768f;

	public UIContainer main;
	private MList<UIContainer> containers = new MList<UIContainer>();

	public Vector2 origin = new Vector2(0, 0);
	private bool closeMenuOnBackBlockerOnClick, disableFrontBlockerOnClick;
	private Action frontBlockerAction;
	private UIManager manager;

	private bool closed = false;

	void Awake ()
	{
		BackBlocker.SetActive(false);
		FrontBlocker.SetActive(false);
		main = AddContainer("MAIN", UIAlign.TOP_LEFT);

	}

	public UIContainer AddContainer(string name, UIAlign anchor)
	{
		UIContainer c = gameObject.AddComponent<UIContainer>();
		containers.AddLast(c);
		c.ContainerName = name;
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

	public bool IsClosed() { return closed; }

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

	public void BackBlockerClicked()
	{
		if (closeMenuOnBackBlockerOnClick) Close();
	}

	public void FrontBlockerClicked()
	{
		if (disableFrontBlockerOnClick) FrontBlocker.SetActive(false);
		if (frontBlockerAction != null)
		{
			frontBlockerAction.Invoke();
			frontBlockerAction = null;
		}
	}

	public void ActivateBackBlocker(bool _closeMenuOnBackBlockerClick)
	{
		BackBlocker.SetActive(true);
		closeMenuOnBackBlockerOnClick = _closeMenuOnBackBlockerClick;
	}

	public void ActivateFrontBlocker(Action act, bool disableOnClick)
	{
		FrontBlocker.SetActive(true);
		disableFrontBlockerOnClick = disableOnClick;
		frontBlockerAction = act;
	}
	public void DisableFrontBlocker()
	{
		FrontBlocker.SetActive(false);
		frontBlockerAction = null;
	}

	public void AddObject(UIObject newObject, UIContainer container, UIDir dir)
	{
		InitializeNewObject(transform, FrontBlocker.transform.GetSiblingIndex(), container, newObject, dir);
	}

	public void AddObject(UIObject newObject, UIDir dir)
	{
		InitializeNewObject(transform, FrontBlocker.transform.GetSiblingIndex(), main, newObject, dir);
	}

	private static void InitializeNewObject(Transform parent, int siblingIndex, UIContainer container, UIObject obj, UIDir dir)
	{
		GameObject item = obj.gameObject;
		item.SetActive(true);
		item.transform.SetParent(parent);
		item.transform.SetSiblingIndex(siblingIndex);
		item.transform.localScale = new Vector3(1, 1, 1);

		UT.print("Init. size: " + obj.GetWidth() + ", " + obj.GetHeight());

		container.Add(obj, container.anchor, dir); // add new item after previous to 'dir' direction.

		if (container.anchor == UIAlign.CENTER)
		{
			Vector2 offset = new Vector2(-container.GetWidth() / 2f, container.GetHeight() / 2f);
			container.SetScreenPosition(getScreenPosition(0, 0) + offset);
		}
		else if (container.anchor == UIAlign.TOP_LEFT)
		{
			container.SetScreenPosition(getScreenPosition(-.5f, .5f));
		}
		else if (container.anchor == UIAlign.LEFT)
		{
			container.SetScreenPosition(getScreenPosition(-.5f, 0f));
		}
		else if (container.anchor == UIAlign.BOTTOM_LEFT)
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
