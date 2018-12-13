
/*
 * TODO: direction with alignment, e.g.
 * --------------
 *               |   east top
 *               |
 *               |
 *               |
 *               |  east middle
 *               |
 *               |
 *               |
 *               |  east bottom
 * -------------- 
 * 
 *       south      south-east
 *       right
 */

public enum UIDir
{
	N, NE, E, SE, S, SW, W, NW
}

public enum UIAlign
{
	// positioning inside a container
	// vertical: left, center, right
	// horizontal: top, middle, bottom

	TOP_CENTER,
	TOP_RIGHT,
	RIGHT,
	BOTTOM_RIGHT,
	BOTTOM_CENTER,
	BOTTOM_LEFT,
	LEFT,
	TOP_LEFT,
	CENTER
}
