using UnityEngine;

public class UIToolTopCanvas : Singleton<UIToolTopCanvas>
{
	// FOR A NEW PROJECT: copy the prefab from another project and instantiate it on Awake

	public DebugPanel DebugPanel;
	public DebugLog DebugLog;
	public GameObject TopBlocker;
	public GameLog Log;

	private void Awake()
	{
		TopBlocker.SetActive(false);
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
		if (type == LogType.Error || type == LogType.Assert || type == LogType.Exception)
		{
			Log.AddEntry(type.ToString() + ": " + condition);
		}
		DebugLog.AddEntry(condition, stackTrace, type);
	}
}
