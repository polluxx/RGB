using UnityEngine;
using System.Collections;

public class LevelBuilder : MonoBehaviour {

    public GameObject scene;

    public GameObject floor;

    public GameObject sceneBlock;

    // level objects
    public GameObject voidScene;
    public GameObject tunnel;

    //level construct elements
    public GameObject[] wall;

    private enum TilesTypes {
        Void, Floor, Level, Ventiliation, VentiliationMine, Corridor, LiftMine,
    }

    // elements numbers
    public int levelMaxX = 100;
    public int levelMaxY = 100;

    public IntRandom numberOfRooms = new IntRandom(4, 20);
    public IntRandom roomHeight = new IntRandom(1, 5);
    public IntRandom roomWidth = new IntRandom(2, 10);
    public IntRandom corridorLength = new IntRandom(4, 15);
    public int numberOfLevels = 3;
    public int minesNum = 1;
    public int mineWidth = 3;
    public int deepnestOfLevel = 10;

    // contructor elements
    private GameObject levelHolder;
    private LiftMine[] liftMines;
    private Ventilation[] ventilationLevels;
    private Room[][] rooms;
    private Corridor[][] corridors;

    // board tiles
    private TilesTypes[][] tiles;

	// Use this for initialization
	public void StartLevel () {
        levelHolder = Instantiate(new GameObject("LevelHolder")) as GameObject;

        // floor 
        Instantiate(floor, new Vector3(0.5f, -22f, 1.33f), Quaternion.identity);

        SetupVoid();

        SetupMines();
        SetTilesForMines();

        ventilationLevels = new Ventilation[numberOfLevels-1];

        rooms = new Room[numberOfLevels][];
        corridors = new Corridor[numberOfLevels][];
        for (int level=0; level < numberOfLevels; level++)
        {
            SetupRoomsAndCorridors(level);
            SetTilesForRooms(level);
            SetTilesForCorridors(level);
        }
        SetTilesForVentilation();
        InstantiateTiles();
    }

    public Transform InstantiateBlock(float x, float y ,float z)
    {
        Instantiate(sceneBlock, new Vector3(x, y, z), Quaternion.identity);

        return sceneBlock.transform;
    }

    private void SetupVoid()
    {
        tiles = new TilesTypes[levelMaxX][];
        for (int i=0;i<levelMaxX;i++)
        {
            tiles[i] = new TilesTypes[levelMaxY];
        }
    }

    private void SetupMines()
    {
        liftMines = new LiftMine[minesNum];
        liftMines[0] = new LiftMine();
        liftMines[0].InitMine(mineWidth, levelMaxX, Mathf.RoundToInt(levelMaxY/2));
    }

    private void SetupRoomsAndCorridors(int level)
    {
        rooms[level] = new Room[numberOfRooms.Random];

        corridors[level] = new Corridor[rooms[level].Length - 1 + liftMines.Length];

        rooms[level][0] = new Room();
        corridors[level][0] = new Corridor();

        rooms[level][0].InitRoom(roomWidth, roomHeight, levelMaxX, levelMaxY, liftMines[0], level, deepnestOfLevel);

        corridors[level][0].InitCorridor(rooms[level][0], corridorLength, roomWidth, roomHeight, levelMaxX, levelMaxY, true);

        for(int r=1;r<rooms[level].Length;r++)
        {
            rooms[level][r] = new Room();
            rooms[level][r].InitRoom(roomWidth, roomHeight, levelMaxX, levelMaxY, corridors[level][r - 1]);

            if (r < corridors[level].Length)
            {
                corridors[level][r] = new Corridor();
                corridors[level][r].InitCorridor(rooms[level][r], corridorLength, roomWidth, roomHeight, levelMaxX, levelMaxY, false);
            }
        }

        // here we setup ventilation between levels
        if (level > 0) SetupVentilation(level - 1, rooms[level - 1], rooms[level], liftMines[0]);

        for (int m = 0; m < liftMines.Length; m++)
        {
            int corridorsInited = corridors[level].Length-1;
            corridors[level][corridorsInited] = new Corridor();
            corridors[level][corridorsInited].InitCorridorToMine(liftMines[m], rooms[level][0], corridorLength);
        }

    }

    private void SetupVentilation(int level, Room[] upper, Room[] downer, LiftMine liftmine)
    {
        ventilationLevels[level] = new Ventilation();
        ventilationLevels[level].InitVentilation(upper, downer, liftmine);
    }

    private void SetTilesForMines()
    {
        for(int i = 0; i < liftMines.Length; i++)
        {
            LiftMine mine = liftMines[i];
            for(int w = 0; w < mine.width; w++)
            {
                int xPos = mine.startX + w;
                for(int h = 0; h < mine.length; h++)
                {
                    int yPos = mine.startY + h;
                    tiles[xPos][yPos] = TilesTypes.LiftMine;
                }
            }
        }
    }

    private void SetTilesForVentilation()
    {
        for(int v=0; v < ventilationLevels.Length; v++)
        {
            Ventilation tunnel = ventilationLevels[v];

            for (int w = 0; w < tunnel.length; w++)
            {
                int xPos = tunnel.startX + w;
                
                for (int h=0;h<tunnel.height;h++)
                {
                    int yPos = tunnel.startY + h;
                    tiles[xPos][yPos] = TilesTypes.Ventiliation;
                }
            }

            // mines
            for(int vm=0;vm< tunnel.ventilationMines.Length;vm++)
            {
                VentilationMine mine = tunnel.ventilationMines[vm];
                for(int mh=0;mh<mine.verticalheight;mh++)
                {
                    int mYPos;
                    if(mine.direction == VentilationMine.Direction.North)
                    {
                        mYPos = mine.verticalPosy + mh;
                    } else
                    {
                        mYPos = mine.verticalPosy - mh;
                    }
                    
                    tiles[mine.verticalPosx][mYPos] = TilesTypes.VentiliationMine;
                }
            }
        }
    }

    private void SetTilesForRooms(int level)
    {
        for(int i=0;i<rooms[level].Length;i++)
        {
            Room current = rooms[level][i];

            if (current == null) continue;
            for(int w = 0; w < current.width; w++)
            {
                int xPos = current.xPos + w;
                for(int h = 0; h < current.height; h++)
                {
                    int yPos = current.yPos + h;
                    tiles[xPos][yPos] = TilesTypes.Floor;
                }
            }
        }
    }

    private void SetTilesForCorridors(int level)
    {
        for(int i = 0; i < corridors[level].Length; i++)
        {
            Corridor current = corridors[level][i];
            if (current == null) continue;

            for(int l = 0; l < current.corridorLen; l++)
            {
                int startX = current.startXPos;
                int startY = current.startYPos;
                switch(current.direction)
                {
                    case Direction.Up:
                        startY += l;
                        break;
                    case Direction.Down:
                        startY -= l;
                        break;
                    case Direction.Left:
                        startX -= l;
                        break;
                    case Direction.Right:
                        startX += l;
                        break;
                }
                tiles[startX][startY] = TilesTypes.Corridor;
            }
        }
    }

    private void InstantiateTiles()
    {
        bool wasInstantiatedPlayer = false;
        for(int i=0;i<tiles.Length;i++)
        {
            for(int j=0;j<tiles[i].Length;j++)
            {
                switch(tiles[i][j])
                {
                    case TilesTypes.Floor:
                        GameObject roomItem = InstantiateFromPosition(voidScene, i, j);
                        SetupWalls(i,j, roomItem);

                        if(!wasInstantiatedPlayer)
                        {
                            GameObject Zet = GameObject.Find("Zet");
                            Zet.transform.position = new Vector3(roomItem.transform.position.x, roomItem.transform.position.y);
                            wasInstantiatedPlayer = true;
                        }

                        break;
                    case TilesTypes.Level:
                        InstantiateFromPosition(tunnel, i, j);
                        break;
                    case TilesTypes.Corridor:
                        InstantiateFromPosition(tunnel, i, j);
                        //SetupWalls(i, j, tunnel);
                        break;
                    case TilesTypes.LiftMine:
                        GameObject mine = InstantiateFromPosition(voidScene, i, j);

                        SetupWalls(i, j, mine);
                        break;
                    case TilesTypes.Ventiliation:
                        GameObject ventilate = InstantiateFromPosition(voidScene, i, j);

                        if(tiles[i][j-1] != TilesTypes.VentiliationMine) AttachToObject(wall[Random.Range(0, wall.Length-1)], ventilate, 0f);
                        if (tiles[i][j + 1] != TilesTypes.VentiliationMine) AttachToObject(wall[Random.Range(0, wall.Length-1)], ventilate, 180f);
                        
                        break;
                    case TilesTypes.VentiliationMine:
                        GameObject instance = InstantiateFromPosition(voidScene, i, j);

                        AttachToObject(wall[Random.Range(0, wall.Length)], instance, 0f);
                        AttachToObject(wall[Random.Range(0, wall.Length)], instance, 180f);
                        instance.transform.Rotate(new Vector3(0f, 0f, 1f), 90f);
                        break;
                }
            }
        }
    }

    private GameObject InstantiateFromPosition(GameObject prefab, float xPos, float ypos)
    {
        Vector3 position = new Vector3(xPos * 8, ypos * 8, 0f);
        GameObject instance = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        instance.transform.parent = levelHolder.transform;
        return instance;
    }

    private void AttachToObject(GameObject prefab, GameObject parent, float rotate)
    {
        GameObject wallAttach = Instantiate(prefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        wallAttach.transform.parent = parent.transform;

        BoxCollider2D collider = wallAttach.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(8f, 2f);
        collider.offset = new Vector2(0f, -4.5f);
        wallAttach.transform.localPosition = new Vector3(0f, 0f, 0f);
        if(rotate != 0f)
        {
            wallAttach.transform.Rotate(new Vector3(0f, 0f, 1f), rotate);
        }
        
    }

    private void SetupWalls(int xPos, int yPos, GameObject parent)
    {
        if(tiles[xPos + 1][yPos] == TilesTypes.Void) AttachToObject(wall[Random.Range(0, wall.Length)], parent, 90f);

        if (tiles[xPos - 1][yPos] == TilesTypes.Void) AttachToObject(wall[Random.Range(0, wall.Length)], parent, -90f);

        if (tiles[xPos][yPos + 1] == TilesTypes.Void) AttachToObject(wall[Random.Range(0, wall.Length)], parent, 180f);

        if (tiles[xPos][yPos - 1] == TilesTypes.Void) AttachToObject(wall[Random.Range(0, wall.Length)], parent, 0f);
    }
	
}
