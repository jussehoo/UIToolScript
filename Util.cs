using System;
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
	float d;
	public ASinPulse(float _interval, float _height, float _offset = 0f)
	{
		interval = _interval;
		height = _height;
		offset = _offset;
	}
	public float Get(float timeDelta)
	{
		d += timeDelta * interval * 3.14f;
		return ((float)Math.Sin(d) * height) + offset;
	}
}
public static class Util
{
	static public float clamp(float value, float min, float max)
	{
		if (value < min) return min;
		if (value > max) return max;
		return value;
	}
	
	public const int SEC_IN_MIN = 60;
	public const int MIN_IN_HOUR = 60;
	public const int HOUR_IN_DAY = 24;
	public const int SEC_IN_HOUR = SEC_IN_MIN * MIN_IN_HOUR;
	public const int SEC_IN_DAY = SEC_IN_HOUR * HOUR_IN_DAY;

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
				sec -= minutes * SEC_IN_MIN;
			}
			else
			{
				int hours = sec / SEC_IN_HOUR;
				s += hours + "h";
				sec -= hours * SEC_IN_HOUR;
			}
			items ++;
			if (items >= MAX_ITEMS) break;
		}
		return s;
	}

	static public float linearInterpolation(float x, float x0, float x1, float y0, float y1)
	{
//		|              / (x1,y1)
//		|             /_____________ return value y
//		|            /|
//		|           / |
//		|  (x0,y0) /  |
//		|             x

		UT.assert(x >= x0 && x <= x1);
		UT.assert(x0 <= x1);

		if ((x1 - x0) == 0)	return (y0 + y1) / 2;
		return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
	}
	static public float linearInterpolation(float factor, float min, float max)
	{
		UT.assert0to1(factor);
		return min + (max - min) * factor;
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
#endif

	// Unique int getter with a recognizable Value(), for constant ID's etc.
	private static int _unique = 333000;
	public static int Unique() { return ++_unique; }
	
	public static MList<Int2> getBlockLine (Int2 p1, Int2 p2)
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
	
	public static MList<Int2> getNeighbors(Int2 p, int max, int min)
	{
		UT.assert(max>=1 && min>=1 && max>=min);
		// get set of neighbors in min/max range
		MList<Int2> l = new MList<Int2>();
		
		for (int y=p.y-max; y<=p.y+max; y++)
			for (int x=p.x-max; x<=p.x+max; x++)
				if (p.chess(x,y)>=min)
					l.Add(new Int2(x,y));
		return l;
	}
}