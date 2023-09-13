using System;
using System.Drawing;
#if !SERVER
using UnityEngine;
 
public class DebugLogWriter : System.IO.TextWriter {
     public override void Write(string value) { base.Write(value);
	 Debug.Log(value); }
     public override System.Text.Encoding Encoding { get { return System.Text.Encoding.UTF8; }}
 }
#endif
public static class ArrayExtensions
{
	public static void Fill<T>(this T[] originalArray, T with)
	{
		for (int i = 0; i < originalArray.Length; i++)
		{
			originalArray[i] = with;
		}
	}
}
public class ASinPulse
{
	readonly float interval, height, offset;
	public float Delta;
	public ASinPulse(float _interval, float _height, float _offset = 0f)
	{
		interval = _interval;
		height = _height;
		offset = _offset;
	}
	public float Get(float timeDelta)
	{
		Delta += timeDelta * interval * 3.14f;
		return ((float)Math.Sin(Delta) * height) + offset;
	}
}
public static class Util
{
	static public float Clamp(float value, float minInclusive, float maxInclusive)
	{
		if (value < minInclusive) return minInclusive;
		if (value > maxInclusive) return maxInclusive;
		return value;
	}
	static public int Clamp(int value, int minInclusive, int maxInclusive)
	{
		if (value < minInclusive) return minInclusive;
		if (value > maxInclusive) return maxInclusive;
		return value;
	}
	
	public const int SEC_IN_MIN = 60;
	public const int MIN_IN_HOUR = 60;
	public const int HOUR_IN_DAY = 24;
	public const int SEC_IN_HOUR = SEC_IN_MIN * MIN_IN_HOUR;
	public const int SEC_IN_DAY = SEC_IN_HOUR * HOUR_IN_DAY;
	
	// for a project, modify in the project's code
	public static int MAX_HOURS_TO_SHOW_MINUTES = 1;
	public static int MAX_MINUTES_TO_SHOW_SECONDS = 1;
	
	static public string TimerTime(int sec)
	{
		string s;
		if (sec >= 60) s = (sec / 60).ToString() + ":";
		else s = ":";
		if (sec % 60 / 10 < 1) s += "0";
		s += (sec % 60).ToString();
		return s;
	}
	static public string CompactTime(int sec)
	{
		// time in a compact text format
		const int MAX_ITEMS = 2;
		int items = 0;
		string s = "";
		if (sec < 0)
		{
			s = "-";
			sec = -sec;
		}
		while (true)
		{
			if (items > 0)
			{
				if (sec == 0) break;
				s += " ";
			}
			if (sec <= SEC_IN_MIN)
			{
				s += sec + "s";
				break;
			}
			if (sec <= SEC_IN_HOUR)
			{
				int minutes = sec / SEC_IN_MIN;
				s += minutes + "m";
				if (minutes > MAX_MINUTES_TO_SHOW_SECONDS) break;
				sec -= minutes * SEC_IN_MIN;
			}
			else
			{
				int hours = sec / SEC_IN_HOUR;
				s += hours + "h";
				if (hours > MAX_HOURS_TO_SHOW_MINUTES) break;
				sec -= hours * SEC_IN_HOUR;
			}
			items ++;
			if (items >= MAX_ITEMS) break;
		}
		return s;
	}

	static public float OffsetToIncludeSegment(float w0, float w1, float s0, float s1, bool centralizeGreaterSegment = true)
	{
		// return minimal offset so that the window (w0 ... w1) includes the segment (s0 ... s1)
		// for example:
		//					w0-----------------w1
		//					               s0------s1
		//					                     <--| negative offset

		UT.Assert(w0 <= w1);
		UT.Assert(s0 <= s1);
		float windowWidth = w1 - w0;
		float segmentWidth = s1 - s0;

		if (segmentWidth >= windowWidth)
		{
			// segment is bigger than window, so return offset to centralize
			if (centralizeGreaterSegment) return OffsetToCentralizeSegment(w0, w1, s0, s1);
			if (s1 < w1) return w1 - s1; // segment is on left
			if (s0 > w0) return w0 - s0; // segment is on right
		}
		else
		{
			if (s1 > w1) return w1 - s1; // segment is on right
			if (s0 < w0) return w0 - s0; // segment is on left
		}
		return 0f; // segment is already inside
	}

	static public float OffsetToCentralizeSegment(float w0, float w1, float s0, float s1)
	{ 
		UT.Assert(w0 <= w1);
		UT.Assert(s0 <= s1);
		float windowWidth = w1 - w0;
		float segmentWidth = s1 - s0;
		return w0 - s0 + ((windowWidth - segmentWidth) / 2f);
	}

	static public Rect Union(Rect r, float x, float y)
	{
		// if necessary, expand rect so that it contains (x, y)
		return Rect.MinMaxRect(
			Math.Min(r.xMin, x),
			Math.Min(r.yMin, y),
			Math.Max(r.xMax, x),
			Math.Max(r.yMax, y)
		);
	}

	static public float LinearInterpolation(float x, float x0, float x1, float y0, float y1)
	{
//		|              / (x1,y1)
//		|             /_____________ return value y
//		|            /|
//		|           / |
//		|  (x0,y0) /  |
//		|             x

		UT.Assert(x >= x0 && x <= x1);
		UT.Assert(x0 <= x1);

		if ((x1 - x0) == 0)	return (y0 + y1) / 2;
		return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
	}
	static public float LinearInterpolation(float factor, float min, float max)
	{
		UT.Assert0to1(factor);
		return min + (max - min) * factor;
	}
    static public float CosineInterpolate(
		float a1,float a2,
		float mu)
	{
		float mu2;

		mu2 = (1-Mathf.Cos(mu*Mathf.PI))/2;
		return(a1*(1-mu2)+a2*mu2);
	}
    static public Vector3 CosineInterpolate(
		Vector3 v1, Vector3 v2,
		float mu)
	{
		return new Vector3(
			CosineInterpolate(v1.x, v2.x, mu),
			CosineInterpolate(v1.y, v2.y, mu),
			CosineInterpolate(v1.z, v2.z, mu)
		);
	}
	public const int RIGHT = 1, LEFT = -1, ZERO = 0;
	public static int DirectionOfPoint(Vector2 A, Vector2 B, Vector2 P)
	{
		//    A *
		//       \
		//        \   * P (left)
		//         \
		//        B *
		//
		// source: https://www.geeksforgeeks.org/direction-point-line-segment/
		// subtracting co-ordinates of point A from
		// B and P, to make A as origin
		B.x -= A.x;
		B.y -= A.y;
		P.x -= A.x;
		P.y -= A.y;
 
		
		float cp = B.x * P.y - B.y * P.x;	// Determining cross Product
		if (cp > 0) return RIGHT;			// return RIGHT if cross product is positive
		if (cp < 0) return LEFT;			// return LEFT if cross product is negative
		return ZERO;						// return ZERO if cross product is zero.
	}
#if !SERVER
	public static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
	{
		Vector2 temp = camera.WorldToViewportPoint(position);
		temp.x *= canvas.sizeDelta.x;
		temp.y *= canvas.sizeDelta.y;
		temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
		temp.y -= canvas.sizeDelta.y * canvas.pivot.y;
		return temp;
	}
	public static Rect GetWorldRect(RectTransform rectTransform)
	{
		Vector3[] corners = new Vector3[4];
		rectTransform.GetWorldCorners(corners);
		// Get the bottom left corner.
		Vector3 position = corners[0];
         
		Vector2 size = new Vector2(
			rectTransform.lossyScale.x * rectTransform.rect.size.x,
			rectTransform.lossyScale.y * rectTransform.rect.size.y);
 
		return new Rect(position, size);
	}
#endif

	// Unique int getter with a recognizable Value(), for constant ID's etc.
	private static int _unique = 333000;
	public static int Unique() { return ++_unique; }
	
	public static MList<Int2> GetBlockLine (Int2 p1, Int2 p2)
	{
		// http://tech-algorithm.com/articles/drawing-line-using-bresenham-algorithm/
		MList<Int2>list = new MList<Int2>();
		int x = p1.x;
		int y = p1.y;
		int w = p2.x - x;
		int h = p2.y - y;
		int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
		if (w<0) dx1 = -1; else if (w>0) dx1 = 1;
		if (h<0) dy1 = -1; else if (h>0) dy1 = 1;
		if (w<0) dx2 = -1; else if (w>0) dx2 = 1;
		int longest = Math.Abs(w);
		int shortest = Math.Abs(h);
		if (!(longest>shortest)) {
			longest = Math.Abs(h);
			shortest = Math.Abs(w);
			if (h<0) dy2 = -1; else if (h>0) dy2 = 1;
			dx2 = 0;            
	    }
		int numerator = longest >> 1;
		for (int i=0;i<=longest;i++) {
			list.Add(new Int2(x,y));
			numerator += shortest;
			if (!(numerator<longest)) {
				numerator -= longest;
				x += dx1;
				y += dy1;
			} else {
				x += dx2;
				y += dy2;
			}
		}
		return list;
	}
	
	public static MList<Int2> GetNeighbors(Int2 p, int max, int min)
	{
		UT.Assert(max>=1 && min>=1 && max>=min);
		// get set of neighbors in min/max range
		MList<Int2> l = new MList<Int2>();
		
		for (int y=p.y-max; y<=p.y+max; y++)
			for (int x=p.x-max; x<=p.x+max; x++)
				if (p.Chess(x,y)>=min)
					l.Add(new Int2(x,y));
		return l;
	}

	// Miscellaneous

	public static void Repeat(int repeatCount, Action action)
	{
		for (int i = 0; i < repeatCount; i++) action();
	}

	internal static string ToRoman(int number)
	{
		// source: https://stackoverflow.com/questions/7040289/converting-integers-to-roman-numerals
		if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
		if (number < 1) return string.Empty;            
		if (number >= 1000) return "M" + ToRoman(number - 1000);
		if (number >= 900) return "CM" + ToRoman(number - 900); 
		if (number >= 500) return "D" + ToRoman(number - 500);
		if (number >= 400) return "CD" + ToRoman(number - 400);
		if (number >= 100) return "C" + ToRoman(number - 100);            
		if (number >= 90) return "XC" + ToRoman(number - 90);
		if (number >= 50) return "L" + ToRoman(number - 50);
		if (number >= 40) return "XL" + ToRoman(number - 40);
		if (number >= 10) return "X" + ToRoman(number - 10);
		if (number >= 9) return "IX" + ToRoman(number - 9);
		if (number >= 5) return "V" + ToRoman(number - 5);
		if (number >= 4) return "IV" + ToRoman(number - 4);
		if (number >= 1) return "I" + ToRoman(number - 1);
		throw new Exception("Impossible state reached");
	}
	
	internal static T Get<T>(MonoBehaviour m)
	{
		var c = m.GetComponent<T>();
		UT.AssertNotNull(c);
		return c;
	}
	internal static T Get<T>(GameObject m)
	{
		var c = m.GetComponent<T>();
		UT.AssertNotNull(c);
		return c;
	}
}