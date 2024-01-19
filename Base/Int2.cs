
using System;

public class Int2
{
	public int x, y;
	

	public Int2() { x = 0; y = 0; }
	public Int2(int _x, int _y) { x = _x; y = _y; }
	public Int2(float _x, float _y) { x = (int)_x; y = (int)_y; }
	public Int2(double _x, double _y) { x = (int)_x; y = (int)_y; }
	public Int2(Int2 i2) { x = i2.x; y = i2.y; }
	public Int2 Copy() { return new Int2(this); }
	public void Set(int _x, int _y) { x = _x; y = _y; }

	public void Set(Int2 i2) { x = i2.x; y = i2.y; }
	public void Add(int _x, int _y) { x += _x; y += _y; }
	public void Add(Int2 i2) { x += i2.x; y += i2.y; }
	public void Sub(int _x, int _y) { x -= _x; y -= _y; }
	public void Sub(Int2 i2) { x -= i2.x; y -= i2.y; }
	public void Mul(int m) { x *= m; y *= m; }
	public void Div(int m) { x /= m; y /= m; }
	public void Neg() { x = -x; y = -y; }
	public bool EvenX() { return x % 2 == 0; }
	public bool EvenY() { return y % 2 == 0; }
	public bool Equals(int _x, int _y) { return x == _x && y == _y; }
	public bool Equals(Int2 i2) { return x == i2.x && y == i2.y; }

	public Int2 Sum (Int2 i2) { return new Int2(x + i2.x, y + i2.y); }

	public static int Sign(int i)
	{
		if (i > 0) return 1;
		if (i < 0) return -1;
		return 0;
	}
	public void Clamp(int minX, int minY, int maxX, int maxY)
	{
		UT.Assert(minX <= maxX && minY <= maxY);
		// clamp inclusively
		if (x < minX) x = minX;
		if (y < minY) y = minY;
		if (x > maxX) x = maxX;
		if (y > maxY) y = maxY;
	}
	public bool Contains(Int2 p)
	{
		return ContainsExcl(p.x, p.y);
	}
	public bool ContainsExcl(int _x, int _y)
	{
		return _x >= 0 && _y >= 0 && _x < x && _y < y;
	}

	// chess board math
	public int Manhattan(){return Math.Abs(x) + Math.Abs(y);}
	public int Manhattan(Int2 p){return Manhattan(p.x, p.y);}
	public int Manhattan(int x, int y){return Math.Abs(this.x - x) + Math.Abs(this.y - y);}
	public int Chess(Int2 a){return ChessboardDistance(this, a);}
	public int Chess(int x, int y){return Math.Max(Math.Abs(this.x - x), Math.Abs(this.y - y));}
	public static int ChessboardDistance(Int2 a, Int2 b){return Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));}
	public static int ChessboardDistance(int ax, int ay, int bx, int by){return Math.Max(Math.Abs(ax - bx), Math.Abs(ay - by));}
	public void ToChessBoardUnit() {x = Sign(x);y = Sign(y);}

	// SQUARE GRID
	
	public void Rot90(int ox, int oy)
	{
		// rotate 90 CW around (ox,oy)
		Sub(ox,oy);
		int tmp = x;
		x = y;
		y = -tmp;
		Add(ox,oy);
	}
	
	public static int CmpInterval (int i, int start, int end)
	{
		// compare i with interval, return -1 (less), 0 (equal), or 1 (greater)
		UT.Assert(start <= end);
		if (i<start) return -1;
		if (i>end) return 1;
		return 0;
	}

	public static int NeighborDir4(Int2 a, Int2 b)
	{
		// return Dir8 if a neighbor, or -1 otherwise
		if (a.Equals(b.x + 1, b.y)) return E;
		if (a.Equals(b.x - 1, b.y)) return W;
		if (a.Equals(b.x, b.y + 1)) return N;
		if (a.Equals(b.x, b.y - 1)) return S;
		return -1;
	}
	
	public const int N=0, NE=1, E=2, SE=3, S=4, SW=5, W=6, NW=7;
	//
	//      0
	//    7   1
	//  6  -1   2
	//    5   3
	//      4

	public static int Dir8(int x, int y)
	{
		if (y<0) {
			if (x<0) return 7;
			if (x==0) return 0;
			return 1;
		}
		if (y>0) {
			if (x<0) return 5;
			if (x==0) return 4;
			return 3;
		}
		if (x<0) return 6;
		if (x>0) return 2;
		return -1;
	}
	public static int [] Dir8array(int x, int y)
	{
		// return all valid directions, starting with best options, or null if (0,0)
		// TODO: 'rnd' parameter to get other orders
		// TODO: unit test
		// TODO: impl. dir16 & array for return values (arrays)?
		if (y<0)
		{
			if (x==0) return  new int [] {0,7,1};
			if (x<0)
			{
				if (x<y) return new int [] {6,7,0};
				if (y<x) return new int [] {0,7,6};
				return new int [] {7,0,6};
			}
			if (-x<y) return new int [] {1,0,2};
			if (y<-x) return new int [] {2,1,0};
			return new int [] {1,0,2};
		}
		if (y>0) {
			if (x==0) return new int [] {4,5,3};
			if (x<0)
			{
				if (-x>y) return new int [] {6,5,4};
				if (y>-x) return new int [] {4,5,6};
				return new int [] {5,6,4};
			}
			if (x>y) return new int [] {2,3,4};
			if (y>x) return new int [] {4,3,2};
			return new int [] {3,2,4};
		}
		if (x<0) return new int [] {6,7,5};
		if (x>0) return new int [] {2,3,1};

		return null;
	}
	public int Dir8() {return Dir8(x, y);}
	public int Dir8(Int2 i) {return Dir8(i.x-x,i.y-y);}
	public int [] Dir8array() {return Dir8array(x, y);}
	public int [] Dir8array(Int2 i) {return Dir8array(i.x-x,i.y-y);}
	
	public static int[][] dir8delta = new int[][] {new int[]{0,-1},new int[]{1,-1},new int[]{1,0},new int[]{1,1},new int[]{0,1},new int[]{-1,1},new int[]{-1,0},new int[]{-1,-1}};
	public void Move(int d)
	{
		x += dir8delta[d][0];
		y += dir8delta[d][1];		
	}
	public static int[] dir8opposite = {4,5,6,7,0,1,2,3};
	public static int OppositeDir8(int dir)
	{
		return dir8opposite[dir];
	}
	public static int Turn8 (int d, bool cw)
	{
		if (cw) {
			d++;
			if (d>7) d=0;
		} else {
			d--;
			if (d<0) d=7;
		}
		return d;		
	}
	public static int RectToPointDir8(Int2 p, Int2 rect, Int2 size)
	{
		// TODO: test
		return Dir8(CmpInterval(p.x, rect.x, rect.x+size.x-1), CmpInterval(p.y, rect.y, rect.y+size.y-1));
	}
	
	public static Int2 ClosestPointOnRect (Int2 p, Int2 tl, Int2 size)
	{
		switch (RectToPointDir8(p,tl,size))
		{
		case 0: return new Int2(p.x, tl.y);					// N
		case 1: return new Int2(tl.x+size.x-1, tl.y);		// NE
		case 2: return new Int2(tl.x+size.x-1, p.y);		// E
		case 3: return new Int2(tl.x+size.x-1, tl.y+size.y-1);// SE
		case 4: return new Int2(p.x, tl.y+size.y-1);		// S
		case 5: return new Int2(tl.x, tl.y+size.y-1);		// SW
		case 6: return new Int2(tl.x, p.y);					// W
		case 7: return new Int2(tl);						// NW
		default: return new Int2(p); // inside
		}
	}
	
	
	public static MList<Int2> GetBlockLine (Int2 p1, Int2 p2)
	{
		// TODO: optimoi: anna array (ei Int2?) täytettäväksi

		// http://tech-algorithm.com/articles/drawing-line-using-bresenham-algorithm/
		var list = new MList<Int2>();
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
	public static MList<Int2> Get4Neighbors(Int2 p)
	{
		var list = new MList<Int2>();
		list.Add(new Int2(p.x + 1, p.y));
		list.Add(new Int2(p.x - 1, p.y));
		list.Add(new Int2(p.x, p.y + 1));
		list.Add(new Int2(p.x, p.y - 1));
		return list;
	}
	public static bool AreNeighbors4(Int2 a, Int2 b)
	{
		foreach(var n in Get4Neighbors(a)) if (n.Equals(b)) return true;
		return false;
	}


	public static MList<Int2> GetNeighbors(Int2 p)
	{
		return GetNeighbors(p,1,1);
	}

	public static MList<Int2> GetNeighbors(Int2 p, int max, int min, bool manhattan = false)
	{
		UT.Assert(max>=1 && min>=1 && max>=min);
		// get set of neighbors in min/max range
		var l = new MList<Int2>();
		
		for (int y=p.y-max; y<=p.y+max; y++)
			for (int x=p.x-max; x<=p.x+max; x++)
			{
				if (manhattan)
				{
					int dist = p.Manhattan(x,y);
					if (dist >= min && dist <= max)
						l.Add(new Int2(x,y));
				}
				else if (p.Chess(x,y) >= min)
					l.Add(new Int2(x,y));
			}
		return l;
	}

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
	public void ToHexUnit(bool evenX)
	{
		// prefers horizontal movement
		x = Sign(x);

		if (x == 0)
		{
			y = Sign(y);
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
	
	public void ToHexUnit(bool evenX, int dir)
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
		UT.Print("toHexUnit: invalid direction: " + dir);
	}

	public static Int2 HexUnit(bool evenX, int dir)
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
		UT.Print("toHexUnit: invalid direction: " + dir);
		return null;
	}

	public int GetHexDir(bool evenX)
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

	public static int GetHexDir(Int2 a, Int2 b)
	{
		Int2 tmp = new Int2(b);
		tmp.Sub(a);
		return tmp.GetHexDir(a.x % 2 == 0);
	}

	override public string ToString() { return "(" + x + ", " + y + ")"; }
}
