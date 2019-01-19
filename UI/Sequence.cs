using System;
using UnityEngine;
using UnityEngine.UI;

public class Sequence : MonoBehaviour
{
	// define animation sequence

	// TODO: ending condition (Action, time ending by delfault)
	// TODO: simultaneous clips (IClip [little]sibling as a member)

	private abstract class IClip
	{
		protected Sequence seq;
		protected float timeLeft;
		protected readonly float totalTime;
		public IClip(Sequence seq, float time) { this.seq = seq; totalTime = timeLeft = time; }
		abstract public void Init();
		abstract public bool Step();
		protected bool UpdateTimer()
		{
			timeLeft -= Time.deltaTime;
			return timeLeft > 0f; // return true if there's time left
		}
	}
	private class RotateTowards : IClip
	{
		Vector3 targetDir;
		float speed;
		public RotateTowards(Sequence seq, Vector3 targetDir, float time) : base (seq, time)
		{
			this.targetDir = targetDir;
		}
		public override void Init()
		{
			speed = (Mathf.Deg2Rad * Vector3.Angle(seq.transform.forward, targetDir)) / timeLeft;
		}
		public override bool Step()
		{
			if (UpdateTimer())
			{
				float step = speed * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards(seq.transform.forward, targetDir, step, 0.0f);
				seq.transform.rotation = Quaternion.LookRotation(newDir);
				return true;
			}
			seq.transform.rotation = Quaternion.LookRotation(targetDir);
			return false;
		}
	}
	private class MoveTo : IClip
	{
		Vector3 targetPosition, velocity;
		public MoveTo(Sequence seq, Vector3 targetPosition, float time) : base(seq, time)
		{
			this.targetPosition = targetPosition;
		}
		public override void Init()
		{
			// calculate move vector relative to time
			velocity = targetPosition - seq.transform.position;
			velocity /= timeLeft;
		}
		public override bool Step()
		{
			if (UpdateTimer())
			{
				// update position
				seq.transform.position += velocity * Time.deltaTime;
				UT.print("MoveTo: " + seq.transform.position);
				return true;
			}
			seq.transform.position = targetPosition;
			return false;
		}
	}
	private class LocalScaling : IClip
	{
		float start, end, velocity, currentScale, delay;
		public LocalScaling(Sequence seq, float _from, float _to, float time, float _delay = 0f) : base(seq, time)
		{
			start = _from;
			end = _to;
			delay = _delay;
		}
		public override void Init()
		{
			velocity = (end - start) / timeLeft;
			currentScale = start;
			seq.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
		}
		public override bool Step()
		{
			if (delay > 0f)
			{
				delay -= Time.deltaTime;
				return true;
			}
			if (UpdateTimer())
			{
				// update position
				currentScale += Time.deltaTime * velocity;
				seq.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
				return true;
			}
			seq.transform.localScale = new Vector3(end, end, end);
			return false;
		}
	}

	private class ImageColorLerp : IClip
	{
		Image image;
		Color from, to;
		public ImageColorLerp(Sequence seq, Image _image, Color _from, Color _to, float time) : base(seq, time)
		{
			image = _image;
			from = _from;
			to = _to;
		}
		public override void Init()
		{
			image.color = from;
		}

		public override bool Step()
		{
			if (UpdateTimer())
			{
				image.color = Color.Lerp(from, to, (totalTime - timeLeft) / totalTime);
				return true;
			}
			image.color = to;
			return false;
		}
	}

	MList<IClip> list = new MList<IClip>();
	private IClip current;
	private bool destroyWhenFinished = true;


	public void AddRotateTowards(Vector3 targetRotation, float time)
	{
		list.AddLast(new RotateTowards(this, targetRotation, time));
	}
	public void AddMoveTo(Vector3 targetPosition, float time)
	{
		list.AddLast(new MoveTo(this, targetPosition, time));
	}
	public void AddLocalScaling(float from, float to, float time, float delay = 0f)
	{
		list.AddLast(new LocalScaling(this, from, to, time, delay));
	}
	public void AddImageColorLerp(Image image, Color from, Color to, float time)
	{
		list.AddLast(new ImageColorLerp(this, image, from, to, time));
	}

	private void Start()
	{
		Step();
	}

	void Update()
	{
		Step();
		if (destroyWhenFinished && list.Size() == 0) Destroy(this);
	}

	private void Step()
	{
		float time = Time.deltaTime;
		if (current == null)
		{
			if (list.Size() == 0) return;
			current = list.First();
			current.Init();
		}
		if (!current.Step())
		{
			list.RemoveFirst();
			current = null;
		}
	}
}