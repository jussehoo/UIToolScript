using System;
using UnityEngine;

public class AButton : MonoBehaviour
{
	Action action;
	AMenu menu;
	bool closeOnClick;

	public void Initialize(Action _action, AMenu _menu, bool _closeOnClick)
	{
		action = _action;
		menu = _menu;
		closeOnClick = _closeOnClick;
	}

	public void Clicked()
	{
		if (action != null) action.Invoke();
		else UT.Print("AButton: no action");

		if (closeOnClick) menu.Close();
	}
}
