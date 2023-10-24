using UnityEngine;

public class UIToolTopCanvas : Singleton<UIToolTopCanvas>
{
	// FOR A NEW PROJECT: copy the prefab from another project and instantiate it on Awake

	public DebugPanel DebugPanel;
	public DebugLog DebugLog;
	public GameObject TopBlocker, DebugButton;
	public GameLog Log;
	public UnityEngine.Events.UnityAction TopBlockedAction { get; private set; } = null;
	public FloaterCtrl FloaterPrefab;

	public GameObject [] RemoveForRealease;

	private void Awake()
	{
		TopBlocker.SetActive(false);
		DebugPanel.gameObject.SetActive(false);
		DebugLog.gameObject.SetActive(false);

		if (!UT.DEBUG)
		{
			foreach(var x in RemoveForRealease) x.SetActive(false);
		}
	}
	
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

	private void HandleLog(string condition, string stackTrace, LogType type)
	{
		if (!UT.DEBUG) return;
		if (type == LogType.Error || type == LogType.Assert || type == LogType.Exception)
		{
			Log.AddEntry(type.ToString() + ": " + condition);
		}
		DebugLog.AddEntry(condition, stackTrace, type);
	}

	private float floaterOffset=0,floaterLastTime=0;

	public FloaterCtrl AddFloater(string text, Color? c, Transform _anchor, bool _sticky = false)
	{
		// prevent overlapping
		if (floaterLastTime < Time.time - 2f)
		{
			floaterOffset = 0;
		}
		else
		{
			floaterOffset -= FloaterPrefab.GetComponent<RectTransform>().sizeDelta.y / 2f;
		}

		floaterLastTime = Time.time;

		var go = Instantiate(FloaterPrefab.gameObject, transform);
		go.transform.position = _anchor.position;
		go.transform.localPosition = go.transform.localPosition + (floaterOffset * Vector3.up);
		var f = go.GetComponent<FloaterCtrl>();
		f.Initialize(text, c, _anchor, _sticky);
		return f;
	}
	public void TopBlockerClicked()
	{
		TopBlockedAction?.Invoke();
		TopBlockedAction = null;
	}
	public void OpenDebugPanel()
	{
		DebugPanel.gameObject.SetActive(true);
		DebugPanel.log.RefreshEntryButtons();
	}
	public void PauseButtonPressed()
	{
		MainCtrl.Instance.PauseButtonPressed();
	}
}
