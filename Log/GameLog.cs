using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLog : MonoBehaviour
{
    public GameLogEntry entryTmp;
	public Transform container;

	MList<GameLogEntry> entries = new MList<GameLogEntry>();

	const int MAX_ENTRIES = 20;
	
	[SerializeField]
	private Color basicColor;
	[SerializeField]
	private Color alertColor;
	[SerializeField]
	private Color exclamationColor;

	private void Awake()
	{
		entryTmp.gameObject.SetActive(false);
	}

	public void AddEntry(string s)
	{
		var go = Instantiate(entryTmp.gameObject, container);
		go.SetActive(true);
		var e = go.GetComponent<GameLogEntry>();
		e.Setup(s, basicColor, container);
		entries.AddLast(e);

		if (entries.Size() > MAX_ENTRIES)
		{
			var r = entries.First();
			r.ShutDown();
			entries.RemoveFirst();
		}
	}
}
