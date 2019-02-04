public abstract class ISignal
{
	public readonly int type;
	public const int CODE	= 100001;
	public const int VALUE	= 100002;
	protected ISignal(int _type) { type = _type; }
}

public class ASignalCode : ISignal
{
	public readonly int code;
	public ASignalCode(int _code) : base(ISignal.CODE)
	{
		code = _code;
	}
}

public class ASignalValue : ISignal
{
	public readonly int key;
	public readonly int Value;
	public ASignalValue(int _key, int _Value) : base(ISignal.VALUE)
	{
		key = _key;
		Value = _Value;
	}
}
