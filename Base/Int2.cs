
using System;

public class Int2
{
	public int x, y;
	

	public Int2() { x = 0; y = 0; }
	public Int2(int _x, int _y) { x = _x; y = _y; }
	public Int2(float _x, float _y) { x = (int)_x; y = (int)_y; }
	public Int2(double _x, double _y) { x = (int)_x; y = (int)_y; }
	public Int2(Int2 i2) { x = i2.x; y = i2.y; }

	public void set(int _x, int _y) { x = _x; y = _y; }
	public void set(Int2 i2) { x = i2.x; y = i2.y; }
	public void add(int _x, int _y) { x += _x; y += _y; }
	public void add(Int2 i2) { x += i2.x; y += i2.y; }
	public void sub(int _x, int _y) { x -= _x; y -= _y; }
	public void sub(Int2 i2) { x -= i2.x; y -= i2.y; }
	public void mul(int m) { x *= m; y *= m; }
	public void div(int m) { x /= m; y /= m; }
	public void neg() { x = -x; y = -y; }
	public bool evenX() { return x % 2 == 0; }
	public bool evenY() { return y % 2 == 0; }
	public bool equals(int _x, int _y) { return x == _x && y == _y; }

	public Int2 sum (Int2 i2) { return new Int2(x + i2.x, y + i2.y); }


	public bool equals(Int2 i2) { return x == i2.x && y == i2.y; }
	
	public static int sign(int i)
	{
		if (i > 0) return 1;
		if (i < 0) return -1;
		return 0;
	}
		
	// chess board math
	public int manhattan(){return Math.Abs(x) + Math.Abs(y);}
	public int manhattan(Int2 p){return Math.Abs(x - p.x) + Math.Abs(y - p.y);}
	public int chess(Int2 a){return chessboardDistance(this, a);}
	public int chess(int x, int y){return Math.Max(Math.Abs(this.x - x), Math.Abs(this.y - y));}
	public static int chessboardDistance(Int2 a, Int2 b){return Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));}
	public static int chessboardDistance(int ax, int ay, int bx, int by){return Math.Max(Math.Abs(ax - bx), Math.Abs(ay - by));}
	public void toChessBoardUnit(){x = sign(x);y = sign(y);}


	/* hex directions (y increases down)
	 * HEX6:
	 *
	 * 	y=0			0
	 * 	y=1		5		1
	 *	y=2		4		2
	 *	y=3			3
	 *
	 * HEX12:
	 *
	 *       [11]    0   [1]
	 *          __________
	 *     10  /     |    \  2
	 *        /      |     \
	 *   [9] /_______|______\ [3]
	 *       \       |      /
	 *     8  \      |     /  4
	 *         \_____|____/
	 *       [7]     6    [5]
	 *
	 */
	public void toHexUnit(bool evenX)
	{
		// prefers horizontal movement
		x = sign(x);

		if (x == 0)
		{
			y = sign(y);
		}
		else if (evenX)
		{
			if (y < 0) y = -1;
			else y = 0;
		}
		else
		{
			if (y <= 0) y = 0;
			else y = 1;
		}
	}
	
	public void toHexUnit(bool evenX, int dir)
	{
		switch (dir)
		{
			case 0: x = 0; y = -1; return;
			case 1: x = 1; y = evenX ? -1 : 0; return;
			case 2: x = 1; y = evenX ? 0 : 1; return;
			case 3: x = 0; y = 1; return;
			case 4: x = -1; y = evenX ? 0 : 1; return;
			case 5: x = -1; y = evenX ? -1 : 0; return;
		}
		UT.print("toHexUnit: invalid direction: " + dir);
	}

	public static Int2 hexUnit(bool evenX, int dir)
	{
		switch (dir)
		{
			case 0: return new Int2(0, -1);
			case 1: return new Int2(1, evenX ? -1 : 0);
			case 2: return new Int2(1, evenX ? 0 : 1);
			case 3: return new Int2(0, 1);
			case 4: return new Int2(-1, evenX ? 0 : 1);
			case 5: return new Int2(-1, evenX ? -1 : 0);
		}
		UT.print("toHexUnit: invalid direction: " + dir);
		return null;
	}

	public int getHexDir(bool evenX)
	{
		if (x == 0)
		{
			if (y < 0) return 0;    // up
			return 3;           // down
		}
		else if (x > 0)
		{
			if (y < 0) return 1;    // NE
			if (!evenX && y == 0) return 1;
			return 2;           // SE
		}
		else // x<0
		{
			if (y < 0) return 5;    // NW
			if (!evenX && y == 0) return 5;
			else return 4;      // SW
		}
	}

	public static int getHexDir(Int2 a, Int2 b)
	{
		Int2 tmp = new Int2(b);
		tmp.sub(a);
		return tmp.getHexDir(a.x % 2 == 0);
	}

	override public string ToString() { return "(" + x + ", " + y + ")"; }
}
