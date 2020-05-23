using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloaterCtrl : MonoBehaviour {
	private Transform anchor;
	public Vector3? worldPosition;
	private TextMeshProUGUI tmp;
	const float LIFE_TIME = 2f;
	float lifeTime, offset = 0;
	float speed = 50f;
	public bool sticky = false;
	public Camera cam;
	ASinPulse pulse;
	// Use this for initialization
	void Start ()
	{
		tmp = GetComponentInChildren<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if (pulse != null)
		{
			float p = pulse.Get(Time.deltaTime);
			tmp.transform.localScale = new Vector3(p,p,p);
		}

		if (lifeTime > 0f)
		{
			if (worldPosition != null)
			{
				if (anchor != null)
				{
					worldPosition = anchor.position;
				}
				transform.position = cam.WorldToScreenPoint(worldPosition.Value + (2 * Vector3.up));
				offset += speed * Time.deltaTime;

				transform.localPosition = new Vector2(
					transform.localPosition.x,
					transform.localPosition.y + offset);
			}
			else
			{
				transform.localPosition = new Vector2(
					transform.localPosition.x,
					transform.localPosition.y + speed * Time.deltaTime);
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
