using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogEntry : MonoBehaviour
{
    public TMPro.TextMeshProUGUI EntryText;
	private Sequence seq;
	private Transform container;
	private float lifeTime = 0f;
	private bool shuttingDown = false;
	public const float DEFAULT_LIFE_TIME = 5f;

	private void Awake()
	{
		seq = gameObject.AddComponent<Sequence>();
	}

	public void Setup(string text, Color color, Transform container)
	{
		EntryText.text = text;
		EntryText.color = color;
		this.container = container;
		lifeTime = DEFAULT_LIFE_TIME;
	}
	void Update()
	{
		if (lifeTime > 0)
		{
			lifeTime -= Time.deltaTime;
			if (lifeTime <= 0) ShutDown();
		}
	}
	public void ShutDown()
	{
		if (shuttingDown) return;
		shuttingDown = true;
		var r = transform.GetComponent<RectTransform>();
		var originalDelta = r.sizeDelta;
		var containerRect = container.GetComponent<RectTransform>();
		seq.AddLocalScaling(1,0.001f, 0.2f);
		seq.AddCustom((float t) => {
			r.sizeDelta = new Vector2(originalDelta.x, t * originalDelta.y);
			LayoutRebuilder.ForceRebuildLayoutImmediate(containerRect);
		}, .2f);
		seq.AddCallback(() => Destroy(gameObject));
	}
}
