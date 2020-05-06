using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloaterCtrl : MonoBehaviour {
	private Transform anchor;
	private TextMeshProUGUI tmp;
	const float LIFE_TIME = 2f;
	float lifeTime, offset = 0;
	public bool sticky = false;
	public Camera cam;
	ASinPulse pulse;
	// Use this for initialization
	void Start ()
	{
		tmp = GetComponentInChildren<TextMeshProUGUI>();
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
			if (anchor != null)
			{
				offset += Time.deltaTime * 1f;
				transform.position = cam.WorldToScreenPoint(anchor.position + (2  + offset) * Vector3.up);
			}
			else
			{
				transform.localPosition = new Vector2(
					transform.localPosition.x,
					transform.localPosition.y + (Time.deltaTime * 30f));
			}
			
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

	internal void Initialize(string text, Color? c, Transform _anchor, bool _sticky = false)
	{
		anchor = _anchor;
		tmp = GetComponentInChildren<TextMeshProUGUI>();
		tmp.text = text;
		if (c != null) tmp.color = c.Value;
		lifeTime = LIFE_TIME;
		sticky = _sticky;
	}
}
