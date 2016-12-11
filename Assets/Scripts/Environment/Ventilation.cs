using UnityEngine;

public class Ventilation {

    public int startX;
    public int startY;
    public int length;
    public int height = 1;
    public int verticalTunnelWidth = 1;

    public VentilationMine[] ventilationMines;

    private int tunnelID = 0;

    public int endX
    {
        get
        {
            return startX + length;
        }        
    }
    public int endY
    {
        get
        {
            return startY + height;
        }
    }

	// Use this for initialization
	public void InitVentilation (Room[] levelUpper, Room[] levelDowner, LiftMine mine) {
        Room lastUpperRoom = levelUpper[levelUpper.Length - 1];
        Room lastDownerRoom = levelDowner[levelDowner.Length - 1];
        Room firstUpperRoom = levelUpper[0];
        Room firstDownerRoom = levelDowner[0];

        int maxWestLevelRoomPos = (lastUpperRoom.xPos > lastDownerRoom.xPos) 
            ? lastUpperRoom.xPos + Mathf.RoundToInt(lastUpperRoom.width/2) 
            : lastDownerRoom.xPos + Mathf.RoundToInt(lastDownerRoom.width / 2);
        
        length = Mathf.Abs(mine.startX - maxWestLevelRoomPos);

        startX = mine.startX + 1;
        startY = firstUpperRoom.yPos - Random.Range(5, 10);

        ventilationMines = new VentilationMine[levelUpper.Length + levelDowner.Length];

        InitVerticalTunnels(levelUpper);
        InitVerticalTunnels(levelDowner);
    }

    private void InitVerticalTunnels(Room[] level)
    {
        Room current;
        for (int r = 0; r < level.Length; r++)
        {
            current = level[r];
            int verticalPosx = Random.Range(current.xPos, current.xPos + current.width - 1);
            if (r == level.Length - 1) verticalPosx = current.xPos + Mathf.RoundToInt(current.width / 2);

            int verticalPosy, verticalheight;
            VentilationMine.Direction direction;
            if (startY > current.yPos)
            {
                verticalPosy = startY - 1;
                verticalheight = verticalPosy - current.yPos - current.height + 1;
                direction = VentilationMine.Direction.South;
            } else
            {
                verticalPosy = startY + 1;
                verticalheight = current.yPos - verticalPosy;
                direction = VentilationMine.Direction.North;
            }

            
            ventilationMines[tunnelID] = new VentilationMine();
            ventilationMines[tunnelID].InitMine(verticalPosx, verticalPosy, verticalheight, direction);
            tunnelID++;
        }
    }
	
}
