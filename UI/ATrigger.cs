using System;

// Tool for triggering events on certain signal and/or conditions.
// Examples:
//		ASignal(BATTLE_END)			>>>		ATrigger(BATTLE_END, ShowGameOver())
//		ASignal(NUM_ENEMIES, 1)		>>>		AConditionalTrigger(AConditionLessThan(NUM_ENEMIES, 3), ShowMessage("Less than 3 left!"))

public abstract class ITrigger
{
	public readonly Action action;
	public ITrigger(Action _action) { action = _action; }
	public abstract bool Check(ISignal sig);
	public bool CheckAndTrig(ASignalCode sig)
	{
		if (!Check(sig)) return false;
		action.Invoke();
		return true;
	}
}

public class ATrigger : ITrigger
{
	// define Check() yourself!
	public delegate bool CustomCheck(ISignal sig);
	private CustomCheck checker;
	public ATrigger(Action _action, CustomCheck _checker) : base(_action)
	{
		checker = _checker;
	}
	public override bool Check(ISignal sig)
	{
		return checker(sig);
	}
}

public class ACodeTrigger : ITrigger
{
	public readonly int code;

	public ACodeTrigger(Action _action, int _code) : base (_action)
	{
		code = _code;
	}
	public override bool Check(ISignal sig)
	{
		if (sig.GetType() != typeof(ASignalCode)) return false;
		return ((ASignalCode)sig).code == code;
	}
}

public class AValueTrigger : ITrigger
{
	public readonly int key, Value;
	public AValueTrigger(Action _action, int _key, int _Value) : base(_action)
	{
		key = _key;
		Value = _Value;
	}
	public override bool Check(ISignal sig)
	{
		if (sig.GetType() != typeof(ASignalValue)) return false;
		return ((ASignalValue)sig).key == key && ((ASignalValue)sig).Value == Value;
	}
}