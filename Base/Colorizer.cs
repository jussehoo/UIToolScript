using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Colorizer : MonoBehaviour
{
	Renderer rend;

	private Color originalColor, impactColor;
	private float impactTime;
	private float impactTimeLeft;

    void Awake()
    {
		rend = GetComponent<Renderer>();
        originalColor = rend.material.GetColor("_Color");
    }
	
	public void SetImpactColor(Color c, float time)
	{
		impactColor = c;
		impactTime = impactTimeLeft = time;
	}

	protected void Update()
	{
		
		// update effect
		if (impactTimeLeft > 0f)
		{
			impactTimeLeft -= Time.deltaTime;
			Color c;
			if (impactTimeLeft <= 0f)
				c = originalColor;
			else
				c = Color.Lerp(originalColor, impactColor, impactTimeLeft / impactTime);

			rend.material.SetColor("_Color", c);
		}
	}
}
