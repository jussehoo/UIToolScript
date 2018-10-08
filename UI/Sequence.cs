using UnityEngine;

public class Sequence
{
	// define animation sequence

	// TODO: ending condition (Action, time ending by delfault)
	// TODO: simultaneous clips (IClip [little]sibling as a member)

	public enum Type
	{
		ROTATE_TOWARDS,
		MOVE_TO
	}
	private abstract class IClip
	{
		protected Sequence seq;
		protected float time;
		public IClip(Sequence seq, float time) { this.seq = seq; this.time = time; }
		abstract public void Init();
		abstract public bool Update();
		protected bool UpdateTimer()
		{
			time -= Time.deltaTime;
			return time > 0f;
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
			speed = (Mathf.Deg2Rad * Vector3.Angle(seq.transform.forward, targetDir)) / time;
		}
		public override bool Update()
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
			velocity /= time;
		}
		public override bool Update()
		{
			if (UpdateTimer()) {
				// update position
				seq.transform.position += velocity * Time.deltaTime;
				UT.print("MoveTo: " + seq.transform.position);
				return true;
			}
			seq.transform.position = targetPosition;
			return false;
		}
	}

	MList<IClip> list = new MList<IClip>();
	public Transform transform;
	private IClip current;

	public void AddRotateTowards(Vector3 targetRotation, float time)
	{
		list.AddLast(new RotateTowards(this, targetRotation, time));
	}

	public void AddMoveTo(Vector3 targetPosition, float time)
	{
		list.AddLast(new MoveTo(this, targetPosition, time));
	}

	public Sequence(Transform tr)
	{
		transform = tr;
	}

	public bool Update()
	{
		float time = Time.deltaTime;
		if (current == null)
		{
			current = list.First();
			current.Init();
		}
		if (!current.Update())
		{
			list.RemoveFirst();
			current = null;
		}
		return list.Size() > 0;
	}
}