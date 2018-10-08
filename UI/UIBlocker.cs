using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBlocker : MonoBehaviour {

	public TextMeshProUGUI text;
	public Button button;
	private Action act;

	void Awake()
	{
		Disable();
	}

	// Use this for initialization
	public void Activate (string _text, Action _act) {
		text.text = _text;
		act = _act;
		text.gameObject.SetActive(true);
		button.gameObject.SetActive(true);
	}
	
	public void Disable ()
	{
		text.gameObject.SetActive(false);
		button.gameObject.SetActive(false);
	}

	// Update is called once per frame
	public void Tapped ()
	{
		if (act != null) act.Invoke();
	}
}
