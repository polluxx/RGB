using UnityEngine;

public class Room {

    public int width;
    public int height;
    public int xPos;
    public int yPos;

    public int depth = 0;

    public Direction enteringCorridor;

    public void InitRoom(IntRandom roomWidth, IntRandom roomHeight, int cols, int rows, LiftMine mine, int level, int deepnestOfLevel)
    {
        width = roomWidth.Random;
        height = roomHeight.Random;

        depth = level * deepnestOfLevel;

        xPos = Mathf.RoundToInt(mine.startX + mine.width + 3);
        yPos = Mathf.RoundToInt(mine.endY - height + 1 - depth);
    }

    public void InitRoom(IntRandom roomWidth, IntRandom roomHeight, int cols, int rows, Corridor corridor)
    {
        enteringCorridor = corridor.direction;

        width = roomWidth.Random;
        height = roomHeight.Random;

        switch(corridor.direction)
        {
            case Direction.Up:
                height = Mathf.Clamp(height, 1, rows - corridor.ebdYPos);

                yPos = corridor.ebdYPos + 1;
                xPos = Random.Range(corridor.endXPos - width + 1, corridor.endXPos);
                xPos = Mathf.Clamp(xPos, 0, cols - width);
                break;
            case Direction.Down:
                height = Mathf.Clamp(height, 1, corridor.ebdYPos);

                yPos = corridor.ebdYPos - height;
                xPos = Random.Range(corridor.endXPos - width + 1, corridor.endXPos);
                xPos = Mathf.Clamp(xPos, 0, cols - width);
                break;
            case Direction.Left:
                width = Mathf.Clamp(width, 1, corridor.endXPos);
                xPos = corridor.endXPos - width;

                //yPos = Random.Range(corridor.ebdYPos - height + 1, corridor.ebdYPos);
                yPos = corridor.ebdYPos;
                //yPos = Mathf.Clamp(yPos, 0, rows - height);
                break;
            case Direction.Right:
                width = Mathf.Clamp(width, 1, cols - corridor.endXPos);
                xPos = corridor.endXPos + 1;

                //yPos = Random.Range(corridor.ebdYPos - height+1, corridor.ebdYPos);
                yPos = corridor.ebdYPos;
                //yPos = Mathf.Clamp(yPos, 0, rows - height);
                break;
        }
    }
}
