
using System;
using System.Text;

public class GridMask
{
	// general-purpose 2D int array, for masking, path-finding etc.
	// -1 is considered empty/uninitialized.

	public int [,] Mask { get; protected set; } // <0: unreached, >=0: distance from flood starting point

	public readonly int width, height;

	public delegate bool Condition(int val, int x, int y);

	public GridMask (int _w, int _h, bool clear = false)
	{
		width=_w;
		height=_h;
		Mask = new int[width,height];
		if (clear) Clear();
	}
	public int Get(Int2 p)
	{
		return Mask[p.x,p.y];
	}
	public int Get(int x, int y)
	{
		return Mask[x,y];
	}
	public bool InRange(int x, int y)
	{
		return x>=0&&x<width&&y>=0&&y<height;
	}
	public void Clear()
	{
		Fill(-1);
	}
	public void Fill(int val)
	{
		Fill(val, 0, 0, width-1, height-1);
	}
	public void FillEmpty(int val)
	{
		Condition emptyCondition = delegate (int v,int x,int y){ return v < 0; };
		Fill(val, 0, 0, width-1, height-1, emptyCondition);
	}
	public void Fill(int val, int minX, int minY, int maxX, int maxY, Condition c = null)
	{
		UT.Assert(minX <= maxX && minY <= maxY);
		
		//clamp
		if (minX < 0) minX=0;
		if (maxX >= width) maxX = width-1;
		if (minY < 0) minY=0;
		if (maxY >= height) maxY = height-1;

		for (int x=minX; x<=maxX; x++)
			for (int y=minY; y<=maxY; y++)
			{
				if (c != null && !c(val, x, y)) continue; // check if it fills the condition
				Mask[x,y]=val;
			}
	}
	public MList<Int2> GetPointListOfValue(int val)
	{
		return GetPointList(delegate (int v,int x,int y){ return v == val; });
	}

	public MList<Int2> GetPointList(Condition c)
	{
		// make list of coordinates which have centrain value
		MList<Int2> l = new MList<Int2>();
		for (int x=0; x<width; x++)
			for (int y=0; y<height; y++)
				if (c(Mask[x,y],x,y)) l.Add(new Int2(x,y));
		return l;
	}
	public string GetString()
	{
		StringBuilder s = new StringBuilder();
		for (int y=height-1; y>=0; y--) {
			for (int x=0; x<width; x++) s.Append("").Append(Mask[x,y]<0?"-":Mask[x,y].ToString()).Append(" ");
			s.Append("\n");
		}
		return s.ToString();
	}
	
	// 2D grid raytracer, max. distance 6
	// Define rays for one quarter, and then rotate the rays
	// 4 times, 90 deg. around the starting point to cover the
	// other quarters.

	public const int rt6Rays = 6;
	public const int rt6RayLength = 5;
	public static int[,,] rt6 = new int[,,] {
		{ {1,0}, {2,0}, {3,0}, {4,0}, {5,0} }, // - - - - -
		{ {1,0}, {2,1}, {3,1}, {4,1}, {0,0} }, // - / - -
		{ {1,1}, {2,1}, {3,2}, {0,0}, {0,0} }, // / - /
		{ {1,1}, {1,2}, {2,3}, {0,0}, {0,0} }, // / | /
		{ {1,1}, {2,2}, {0,0}, {0,0}, {0,0} }, // / /
		{ {0,1}, {1,2}, {1,3}, {1,4}, {0,0} }, // | / | |
		//{ {0,1}, {1,2}, {2,3}, {0,0}, {0,0} }, // | / /
	};

	public void Raytrace(int px, int py, int distance, Condition visible)
	{
		Mask[px,py] = 1;

		Int2 pos = new Int2(), orig = new Int2(px,py);

		for(int r = 0; r < rt6Rays; r++)
		{
			for (int rot = 0; rot < 4; rot++)
			{
				for (int a = 0; a < rt6RayLength; a++)
				{
					int dx = rt6[r,a,0];
					int dy = rt6[r,a,1];
					if (dx == 0 && dy == 0) break; // end of ray

					pos.Set(dx + px, dy + py);

					for(int foo = 0; foo < rot; foo++) pos.Rot90(px,py);

					if (RaytraceCheck(pos, orig, distance, visible)) Mask[pos.x,pos.y] = 1;
					else break;
				}
			}
		}
	}

	private bool RaytraceCheck(Int2 p, Int2 orig, int distance, Condition visible)
	{
		if (p.Manhattan(orig) > distance) return false;
		if (!InRange(p.x,p.y)) return false;
		return visible(0, p.x, p.y);
	}
}

public class FloodMask : GridMask
{
	private int maxReached;
	
	public int SurfaceSize = 0;
	public bool Diagonal = false;
	public int MaxDistance = 1;
	
	public delegate bool CanVisitDelegate(int x, int y);
	public delegate bool CanFinishDelegate(int x, int y);
	public delegate bool StopperDelegate(int x, int y);

	public CanVisitDelegate CanVisit = null;
	public CanFinishDelegate CanFinish = null; // e.g. surface is big enough
	public StopperDelegate Stopper = null; // e.g. collectable or a door

	// TODO: add buffers to edges: no inRange checks!
	// TODO: use for pathfinding
	
	public FloodMask(int _w, int _h) : base(_w, _h)
	{
	}	
	
	//public virtual bool Enough() { return false; } // return true when flooding can be stopped; false otherwise
	
	private bool done = false;


	public void Flood (Int2 p)
	{
		if (Mask == null) Mask = new int[width,height];
		Fill(-1);
		SurfaceSize=0;
		done = false;
		
		MList<Int2>nodes = new MList<Int2>();
		MList<Int2>next = new MList<Int2>();
		nodes.AddLast(new Int2(p));
		
	
		Mask[p.x,p.y]=0;

		int dist=1;
		maxReached = -1;
		
		while(nodes.Size()>0)
		{
//			SW.print("nodes: "+nodes.size());
			while (nodes.Size()>0)
			{
				// consume nodes list
				Int2 node = nodes.First();
				nodes.RemoveFirst();

				// visit neighbors
				if (node.x > 1)			FloodVisit(node.x-1, node.y, dist, next);
				if (node.x < width-2)	FloodVisit(node.x+1, node.y, dist, next);
				if (node.y > 1)			FloodVisit(node.x, node.y-1, dist, next);
				if (node.y < height-2)	FloodVisit(node.x, node.y+1, dist, next);
				
				if (Diagonal)
				{
					if (node.x > 1 && node.y > 1)				FloodVisit(node.x-1, node.y-1, dist, next);
					if (node.x > 1 && node.y < width-2)			FloodVisit(node.x-1, node.y+1, dist, next);
					if (node.x < width-2 && node.y > 1)			FloodVisit(node.x+1, node.y-1, dist, next);
					if (node.x < width-2 && node.y < width-2)	FloodVisit(node.x+1, node.y+1, dist, next);
				}
				
				if (done){
					maxReached = dist-1;
					return;
				}
			}
			// set the next list and increase distance
			nodes = next;
			next = new MList<Int2>();
			dist++;
			if (dist > MaxDistance) break;
		}
		maxReached = dist-2;

//		SW.print("max: "+max);
	}
	
	private void FloodVisit(int x, int y, int dist, MList<Int2> next)
	{
		if (CanVisit != null && !CanVisit(x,y)) return;

		if (Mask[x,y] < 0 && InRange(x,y))
		{
			Mask[x,y]=dist;
			if (Stopper == null || !Stopper(x,y)) // place where path would end?
			{
				next.Add(new Int2(x,y));
			}
			SurfaceSize++;
			
			if (CanFinish != null && CanFinish(x,y)) done = true;
		}
	}
	
	public MList<Int2> PathFinder(Int2 p)
	{
		return PathFinder(p.x, p.y);
	}
	
	public MList<Int2> PathFinder(int x, int y)
	{
		int val = Mask[x,y];
		if (val <= 0)
		{	
			UT.Print("PathFinder: not reached (" + x + ", " + y + ")");
			return null;
		}

		// find path by looking for neighbors with values 1 less
		// until zero is found

		var list = new MList<Int2>();
		list.AddFirst(new Int2(x,y));

		while (val > 0)
		{
			bool found = false;
			foreach(var p in Int2.GetNeighbors(new Int2(x,y)))
			{
				if (InRange(p.x,p.y) && Mask[p.x,p.y] == val-1)
				{
					//UT.Print("PathFinder: step " + p.ToString());
					list.AddFirst(p);
					x = p.x; y = p.y;
					found = true;
					break;
				}
			}
			if (!found)
			{	
				UT.Print("PathFinder: next step not found...");
				return null;
			}
			val--;
		}

		return list;
	}

	public int GetMax()
	{
		return maxReached;
	}
	
}
