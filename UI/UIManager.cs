using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public GameObject MenuPrefab;

	MList<AMenu> menus = new MList<AMenu>();
	MList<AMenu> popups = new MList<AMenu>();

	// Use this for initialization
	void Start () {
		
	}

	public AMenu CreateMenu(Dir anchor)
	{
		AMenu m = Instantiate(MenuPrefab).GetComponent<AMenu>();
		m.main.anchor = anchor;
		m.Initialize(this);
		if (menus.Size() > 0) m.SetLayerOrder(menus.Last().GetLayerOrder() + 1);
		else m.SetLayerOrder(1);
		menus.AddLast(m);
		return m;
	}

	public AMenu CreatePopup(Dir anchor)
	{
		AMenu m = CreateMenu(anchor);
		popups.AddFirst(m);
		popups.AssertValid();

		// inactivate others
		var it = popups.Iterator();
		it.Next();
		while (it.Next())
		{
			it.Value.gameObject.SetActive(false);
		}
		return m;
	}

	public void SetClosed(AMenu m)
	{
		// check if it's a pop-up
		if (popups.Size() > 0 && popups.First() == m)
		{
			Destroy(popups.First());
			popups.RemoveFirst();
			if (popups.Size() > 0)
			{
				popups.First().gameObject.SetActive(true);
			}
		}

		var it = menus.Iterator(); // remove menu from the list
		while (it.Next()) {
			if (it.Value == m) {
				Destroy(it.Value);
				menus.Remove(it);
				return;
		}   }
	}

	public void CloseAll()
	{
		var it = menus.Iterator(); // remove menu from the list
		while (it.Next())
		{
			it.Value.SetClosed();
			Destroy(it.Value.gameObject);
			it.Remove();
		}
		it = popups.Iterator(); // remove menu from the list
		while (it.Next())
		{
			it.Value.SetClosed();
			Destroy(it.Value.gameObject);
			it.Remove();
		}
	}
}
