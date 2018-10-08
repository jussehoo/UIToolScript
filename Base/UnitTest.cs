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
		UT.assert(u.x == 0 && u.y == 0 && u.width == 6f && u.height == 5f);
		UT.print("Union: " + u);
	}

	[UnityEditor.MenuItem("Tools/MList test")]
	private static void RunTest()
	{
		UT.print("-------- MListTest start --------");
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

		l = new MList<int>();
		// fill list
		for (int i = 0; i < FILL_SIZE; i++)
		{
			l.AddLast(100 + i);
		}
		print(l);
		UT.assert(l.Size() == FILL_SIZE);

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
		UT.assert(l.Size() == 0);

		// empty by iterator
		l = new MList<int>();
		for (int i = 0; i < 2; i++)
		{
			l.AddLast(10 + i);
		}
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
		l = new MList<int>();
		for (int i = 0; i < 2; i++)
		{
			l.AddLast(10 + i);
		}
		it = l.Iterator();
		it.Next();
		l.Remove(it);
		l.AssertValid();

		UT.print("-------- MListTest end --------");
	}

	public static void print<T>(MList<T> l)
	{
		int n = 0;
		string s = "";
		foreach (T i in l)
		{
			s += "([" + (n++) + "] " + i + " )";
		}
		UT.print(s);
	}
#endif
}