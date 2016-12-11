using UnityEngine;


public enum Direction
{
    Up, Right, Down, Left,
}

public class Corridor {

    public int startXPos;
    public int startYPos;
    public int corridorLen;
    public Direction direction;

    public int endXPos
    {
        get
        {
            if (direction == Direction.Up || direction == Direction.Down)
                return startXPos;
            if (direction == Direction.Right)
                return startXPos + corridorLen - 1;
            return startXPos - corridorLen + 1;
        }
    }

    public int ebdYPos
    {
        get
        {
            if (direction == Direction.Left || direction == Direction.Right)
                return startYPos;
            if (direction == Direction.Up)
                return startYPos + corridorLen - 1;
            return startYPos - corridorLen + 1;
        }
    }

    public void InitCorridor(Room room, IntRandom corrLen, IntRandom roomWidth, IntRandom roomHeight, int cols, int rows, bool first)
    {
        //direction = (Direction)Random.Range(0, 4);
        direction = Direction.Right;

        Direction oppositeDirr = (Direction)((int)(room.enteringCorridor + 2) % 4);

        if(!first && direction == oppositeDirr)
        {
            int directionInt = (int)direction;
            directionInt++;
            directionInt = directionInt % 4;
            direction = (Direction)directionInt;
        }

        corridorLen = corrLen.Random;

        int maxLen = corrLen.max;

        switch(direction)
        {
            case Direction.Up:
                startXPos = Random.Range(room.xPos, room.xPos + room.width - 1);
                startYPos = room.height + room.yPos;

                maxLen = (int)(rows - startYPos - roomHeight.min);
                break;
            case Direction.Down:
                startXPos = Random.Range(room.xPos, room.xPos + room.width - 1);
                startYPos = room.yPos - 1;

                maxLen = (int)(startYPos - roomHeight.min);
                break;
            case Direction.Left:
                startXPos = room.xPos - 1;
                startYPos = Random.Range(room.yPos, room.yPos + room.height - 1);

                maxLen = (int)(startXPos - roomWidth.min);
                break;
            case Direction.Right:
                startXPos = (room.width) + room.xPos;
                //startYPos = Random.Range(room.yPos, room.yPos + room.height - 1);
                startYPos = room.yPos;
                maxLen = (int)(cols - startYPos - roomWidth.min);
                break;
        }

        corridorLen = Mathf.Clamp(corridorLen, 1, maxLen);
    }

    public void InitCorridorToMine(LiftMine mine, Room roomContact, IntRandom corrLen)
    {
        startXPos = mine.startX + mine.width;
        corridorLen = roomContact.xPos - startXPos;

        startYPos = roomContact.yPos;
        direction = Direction.Right;
    }
}
