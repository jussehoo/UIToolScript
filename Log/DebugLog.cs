using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class DebugLog : MonoBehaviour
{
	[SerializeField]
	private TMPro.TextMeshProUGUI screenLog;
	
	[SerializeField]
	private DebugPanel panel;
	
	public class DebugEntry
	{
		public string Condition;
		public string StackTrace;
		public LogType Type;
		public DebugEntry(string condition, string stackTrace, LogType type)
		{
			Condition = condition;
			StackTrace = stackTrace;
			Type = type;
		}
	}


	public const int MAX_ENTRIES = 100;

	public MList<DebugEntry> entries = new MList<DebugEntry>();

	private void Awake()
	{
		screenLog.text = "<<<<<<<< press 'L' to toggle debug log on/off >>>>>>>>";
	}
	public void AddEntry(string condition, string stackTrace, LogType type)
	{
		entries.AddLast(new DebugEntry(condition, stackTrace, type));
		if (entries.Size() > MAX_ENTRIES) entries.RemoveFirst();

		if (screenLog.isActiveAndEnabled) screenLog.text = GetCompact(30);
	}
	public void ClearEntries()
	{
		foreach(Transform old in panel.EntryParent)
		{
			if (old != panel.EntryTmp.transform) Destroy(old.gameObject);
		}
	}
	public void RefreshEntryButtons()
	{
		ClearEntries();

		foreach(var entry in entries)
		{
			var b = Instantiate(panel.EntryTmp.gameObject, panel.EntryParent);
			b.SetActive(true);
			var entryButton = b.GetComponent<DebugEntryButton>();
			entryButton.Title.text = CompactEntryTitle(entry, true);
			entryButton.onClick.AddListener(() => ShowEntryDetails(entry));
		}
        panel.EntryScrollRect.normalizedPosition = new Vector2(0, 0);
	}

	private void ShowEntryDetails(DebugEntry entry)
	{
		panel.EntryDetailsParent.SetActive(true);
		panel.EntryDetailsText.text = entry.Condition + "\n\n" + entry.StackTrace;
	}
	private string CompactEntryTitle(DebugEntry entry, bool button)
	{
		string tag;
		
		if (entry.Type == LogType.Error) tag = "<color=red>";
		else if (button) tag = "<color=black>";
		else tag = "<color=yellow>";
		const int MAX_LENGTH = 40;
		string cleaned;
		if (entry.Condition.Length > MAX_LENGTH) cleaned = entry.Condition.Substring(0, MAX_LENGTH) + "...";
		else cleaned = entry.Condition;
		cleaned = cleaned.Replace("\n", "");
		return tag + cleaned + "</color>";
	}
	public string GetCompact(int num = -1)
	{
		StringBuilder s = new StringBuilder();
		int n = entries.Size();
		foreach(var entry in entries)
		{
			n--;
			if (num > 0 && n >= num) continue; // print only 'num' last

			//s.AppendLine(entry.Type.ToString());
			if (entry.Type == LogType.Error)
			{
			}
			else
			{
				s.AppendLine(CompactEntryTitle(entry, false));
			}
			//s.AppendLine(entry.StackTrace);
		}
		return s.ToString();
	}
}
