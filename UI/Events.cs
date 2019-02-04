using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// event that happens on certain time
public class TimeEvent
{
	public int time;
	public System.Action act;
}

// event that happens on certain condition,
// that is checked on every frame
public class ConditionEvent
{
	public delegate bool Condition();
	public Condition condition;
	public System.Action act;
}


public class Events
{
	public MList<TimeEvent> timedEvents = new MList<TimeEvent>();
	public MList<ConditionEvent> conditionalEvents = new MList<ConditionEvent>();
	public MList<ITrigger> triggerEvents = new MList<ITrigger>();

	public void Add(ConditionEvent e)
	{
		conditionalEvents.AddLast(e);
	}

	public void Add(TimeEvent e)
	{
		// TODO: sort
		timedEvents.AddLast(e);
	}

	public void Add(ITrigger e)
	{
		triggerEvents.AddLast(e);
	}

	public void Signal(ISignal s)
	{
		var it = triggerEvents.Iterator();
		while (it.Next())
		{
			if (it.Value().Check(s))
			{
				it.Value().action.Invoke();
				it.Remove();
			}
		}
	}

	public void Step(int currentStep)
	{
		// Iterate conditional events.
		// If condition is true, execute
		// event and remove it from the list.

		// Conditional events
		var it = conditionalEvents.Iterator();
		while (it.Next())
		{
			if (it.Value().condition())
			{
				it.Value().act.Invoke();
				it.Remove();
			}
		}

		// Timed events
		var iter = timedEvents.Iterator();
		while (iter.Next())
		{
			if (iter.Value().time == currentStep)
			{
				iter.Value().act.Invoke();
				iter.Remove();
			}
		}
	}
}
