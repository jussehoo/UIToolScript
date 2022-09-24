using System;
using UnityEngine;
using UnityEngine.UI;

public class Sequence : MonoBehaviour
{
	// define animation sequence

	// TODO: ending condition (Action, time ending by delfault)
	// TODO: simultaneous clips (IClip [little]sibling as a member)
	

	MList<IClip> list = new MList<IClip>();
	private IClip current;
	private bool destroyWhenFinished = false;

	public void AddWait(float time, Action onUpdate = null)
	{
		list.AddLast(new Waiting(this, onUpdate, time));
	}
	public void AddRotateTowards(Vector3 targetRotation, float time)
	{
		list.AddLast(new RotateTowards(this, targetRotation, time));
	}
	public void AddMoveTo(Vector3 targetPosition, float time)
	{
		list.AddLast(new MoveTo(this, targetPosition, time));
	}
	public void AddMoveTo2DArc(Vector3 targetPosition, float height, float time)
	{
		list.AddLast(new MoveTo2DArc(this, targetPosition, height, time));
	}
	public void AddMoveSmoothTo(Vector3 targetPosition, float time)
	{
		list.AddLast(new MoveSmoothTo(this, targetPosition, time));
	}
	public void AddShake(float amount, float time)
	{
		list.AddLast(new Shake(this, amount, time));
	}
	public void AddLocalScaling(float from, float to, float time, float delay = 0f)
	{
		list.AddLast(new LocalScaling(this, from, to, time, delay));
	}
	public void AddImageColorLerp(Image image, Color from, Color to, float time)
	{
		list.AddLast(new ImageColorLerp(this, image, from, to, time));
	}
	public void AddCallback(Action a)
	{
		list.AddLast(new Callback(this, a));
	}
	public delegate void CustomCallback (float t);
	public void AddCustom(CustomCallback c, float time)
	{
		list.AddLast(new CustomClip(this, c, time));
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

	// CLIPS: various animation types

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
	private class Callback : IClip
	{
		private Action action;
		public Callback(Sequence seq, Action a) : base(seq, 0) { action = a; }
		public override void Init(){}
		public override bool Step() { action.Invoke(); return UpdateTimer(); }
	}
	private class Waiting : IClip
	{
		private Action onUpdate;
		public Waiting(Sequence seq, Action onUpdate, float time) : base(seq, time)
		{
			this.onUpdate = onUpdate;
		}
		public override void Init(){}
		public override bool Step() { onUpdate?.Invoke(); return UpdateTimer(); }
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
				return true;
			}
			seq.transform.position = targetPosition;
			return false;
		}
	}
	private class MoveTo2DArc : IClip
	{
		Vector3 targetPosition, groundPosition, velocity;
		float arcHeight;
		public MoveTo2DArc(Sequence seq, Vector3 targetPosition, float arcHeight, float time) : base(seq, time)
		{
			this.targetPosition = targetPosition;
			this.arcHeight = arcHeight;
		}
		public override void Init()
		{
			// calculate move vector relative to time
			groundPosition = seq.transform.position;
			velocity = targetPosition - seq.transform.position;
			velocity /= timeLeft;
		}
		public override bool Step()
		{
			if (UpdateTimer())
			{
				// update position
				groundPosition += velocity * Time.deltaTime;

				// add arc height
				
				float delta = (totalTime - timeLeft) / totalTime;
				float sin = Mathf.Sin(delta*3.14f);
				//arcHeight = (arcHeight * sin);

				seq.transform.position = groundPosition + ((arcHeight * sin) * Vector3.up);
				return true;
			}
			seq.transform.position = targetPosition;
			return false;
		}
	}
	private class MoveSmoothTo : IClip
	{
		Vector3 origin, target;
		public MoveSmoothTo(Sequence seq, Vector3 targetPosition, float time) : base(seq, time)
		{
			origin = seq.transform.position;
			target = targetPosition;
		}
		public override void Init()
		{
		}
		public override bool Step()
		{
			if (UpdateTimer())
			{
				// update position
				seq.transform.position = Util.CosineInterpolate(origin, target, 1f - (timeLeft / totalTime));
				return true;
			}
			seq.transform.position = target;
			return false;
		}
	}
	private class Shake : IClip
	{
		Vector3 origin;
		float amount;
		public Shake(Sequence seq, float _amount, float time) : base(seq, time)
		{
			amount = _amount;
		}
		public override void Init()
		{
			// calculate move vector relative to time
			origin = seq.transform.position;
		}
		public override bool Step()
		{
			if (UpdateTimer())
			{
				// update position
				seq.transform.position = origin + new Vector3(((UnityEngine.Random.value-.5f)*2f)*amount,((UnityEngine.Random.value-.5f)*2f)*amount,0f);
				return true;
			}
			seq.transform.position = origin;
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
	private class CustomClip : IClip
	{
		private Sequence.CustomCallback callback;

		public CustomClip(Sequence seq, Sequence.CustomCallback c, float time) : base(seq, time)
		{
			this.callback = c;
		}

		public override void Init()
		{
		}

		public override bool Step()
		{
			if (UpdateTimer()) {
				callback.Invoke(timeLeft / totalTime);
				return true;
			}
			return false;
		}
	}
}
