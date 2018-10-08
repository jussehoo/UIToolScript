using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloaterCtrl : MonoBehaviour {
	private Text tmp;
	const float LIFE_TIME = 2f;
	float lifeTime;
	public bool sticky = false;
	ASinPulse pulse;
	// Use this for initialization
	void Start ()
	{
		tmp = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (pulse != null)
		{
			float p = pulse.Get(Time.deltaTime);
			tmp.transform.localScale = new Vector3(p,p,p);
		}

		if (lifeTime > 0f)
		{
			transform.localPosition = new Vector2(
				transform.localPosition.x,
				transform.localPosition.y + (Time.deltaTime * 30f)
			);
			lifeTime -= Time.deltaTime;
		}
		else if (!sticky)
		{
			Destroy(gameObject);
		}
		else if (pulse == null)
		{
			pulse = new ASinPulse(2f, .01f, 1f);
		}
	}

	internal void Initialize(string text, Color? c, bool _sticky = false)
	{
		tmp = GetComponentInChildren<Text>();
		tmp.text = text;
		if (c != null) tmp.color = c.Value;
		lifeTime = LIFE_TIME;
		sticky = _sticky;
	}
}
