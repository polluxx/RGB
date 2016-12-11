using UnityEngine;

public class LiftMine {

    public int startX;
    public int startY;
    public int length;
    public int width;

    public int endX
    {
        get
        {
            return width - 1 + startX;
        }
    }

    public int endY
    {
        get
        {
            return length - 1 + startY;
        }
    }

	// Use this for initialization
	public void InitMine (int mineWidth, int cols, int rows) {
        startY = Mathf.RoundToInt(rows / 2f - 1 / 2f);
        startX = Mathf.RoundToInt(cols / 2f - width / 2f);
        length = rows;
        width = mineWidth;
	}
	
}
