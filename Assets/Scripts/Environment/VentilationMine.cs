using UnityEngine;

public class VentilationMine {

    public int verticalPosx;
    public int verticalPosy;
    public int verticalheight;
    public Direction direction;

    public enum Direction
    {
        North, South,
    }

    // Use this for initialization
    public void InitMine (int posX, int posY, int height, Direction dir) {
        verticalPosx = posX;
        verticalPosy = posY;
        verticalheight = height;

        direction = dir;
    }
	
}
