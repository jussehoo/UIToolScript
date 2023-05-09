using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
	public Button EntryTmp;
	public Transform EntryParent;
	public GameObject EntryDetailsParent;
	public TMPro.TextMeshProUGUI EntryDetailsText;
	public ScrollRect EntryScrollRect;
	public DebugLog log;

	private void Awake()
	{
		EntryTmp.gameObject.SetActive(false);
		EntryDetailsParent.SetActive(false);
	}
	public void CloseDetails()
	{
		EntryDetailsParent.SetActive(false);
	}
	public void Close()
	{
		log.ClearEntries();
		gameObject.SetActive(false);
	}
	public void CopyTextToClipboard()
	{
		GUIUtility.systemCopyBuffer = EntryDetailsText.text;
	}
	public void ToggleLog()
	{
		log.gameObject.SetActive(!log.gameObject.activeSelf);
	}
}
