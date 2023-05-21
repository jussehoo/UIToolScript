using System;
using UnityEngine;



public class MListTest
{
#if UNITY_EDITOR
	const int FILL_SIZE = 10;

	[UnityEditor.MenuItem("Tools/Util test")]
	private static void UtilTest()
	{
		Rect r1 = Rect.MinMaxRect(0, 0, 3, 5);
		Rect r2 = Rect.MinMaxRect(1, 1, 6, 2);
		Rect u = AMenu.Union(r1, r2);
		UT.Assert(u.x == 0 && u.y == 0 && u.width == 6f && u.height == 5f);
		UT.Print("Union: " + u);
	}

	[UnityEditor.MenuItem("Tools/TestCompactTime")]
	private static void TestCompactTime()
	{
		for (int t = -100; t < 500; t += 50)
		{
			UT.Print(t + " sec. -> " + Util.CompactTime(t));
		}
		for (int t = -1000; t < 50000; t += 700)
		{
			UT.Print(t + " sec. -> " + Util.CompactTime(t));
		}
	}

	[UnityEditor.MenuItem("Tools/MList test")]
	private static void TestMList()
	{
		UT.Print("-------- MListTest start --------");
		MList<int> l = new MList<int>();

		// insert first
		l.AddLast(1);
		l.AssertValid();
		print(l);
		l.AddFirst(2);
		l.AssertValid();
		print(l);
		l.AddFirst(3);
		print(l);

		// insert before
		var it = l.Iterator();
		it.AssertValid();
		it.Next();
		it.InsertBefore(-1);
		it.AssertValid();
		print(l);
		it.InsertBefore(0);
		it.AssertValid();
		l.AssertValid();
		print(l); // "-1 0 1 2 3"
		while (it.HasNext()) it.Next();
		it.InsertBefore(2);
		it.AssertValid();
		l.AssertValid();
		print(l); // "-1 0 1 2 2 3"

		l = CreateTestList(FILL_SIZE);
		print(l);
		UT.Assert(l.Size() == FILL_SIZE);

		// iterator insert and remove
		it = l.Iterator();
		while (it.Next())
		{
			it.InsertAfter(555555);
			l.AssertValid();
			if (it.Next()) it.Remove();
			l.AssertValid();
		}
		it.InsertAfter(123456);
		print(l);
		l.AssertValid();

		// remove last
		it = l.Iterator();
		while (it.HasNext()) it.Next();
		it.Remove();

		l.AssertValid();
		l.RemoveAll();
		l.AssertValid();
		UT.Assert(l.Size() == 0);

		// empty by iterator
		l = CreateTestList(2);
		it = l.Iterator();
		while (it.Next())
		{
			l.AssertValid();
			it.AssertValid();
			it.Remove();
			l.AssertValid();
			it.AssertValid();
		}

		// test remove last iterator
		l = CreateTestList(2);
		it = l.Iterator();
		it.Next();
		l.Remove(it);
		l.AssertValid();

		//loop test
		l = CreateTestList(2);
		it = l.Iterator();
		while(it.Loop()) it.Remove();
		
		// fail test
		MListFailTest(0);
		MListFailTest(1);
		MListFailTest(2);
		MListFailTest(3);
		MListFailTest(99); // default

		UT.Print("-------- MListTest end --------");
	}

	private static void MListFailTest(int test)
	{
		var l = CreateTestList(5);
		var it = l.Iterator();
		it.Next();
		it.Remove();
		it.Next();
		switch(test)
		{
		case 0: l.AddFirst(1); break;
		case 1: l.AddLast(1);break;
		case 2: l.Remove(102); break;
		case 3: {
			var it2 = l.Iterator();
			it2.Next();
			it2.Remove();
		} break;
		default: l.RemoveFirst(); break;
		}
		try {
			it.Next();
			UT.Trap("fail test: " + test); // fail if we end up here
		} catch(MConcurrentModificationException) { }
	}

	private static MList<int> CreateTestList(int size)
	{
		var l = new MList<int>();
		// fill list
		for (int i = 0; i < size; i++)
		{
			l.AddLast(100 + i);
		}
		return l;
	}

	public static void print<T>(MList<T> l)
	{
		int n = 0;
		string s = "";
		foreach (T i in l)
		{
			s += "([" + (n++) + "] " + i + " )";
		}
		UT.Print(s);
	}
#endif
}