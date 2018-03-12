using UnityEngine;
using System.Collections;
using System;

public class WorldGenerator : MonoBehaviour
{
    // Prefabs of all the tiles
    public GameObject ground_T;
    public GameObject ground_B;
    public GameObject ground_L;
    public GameObject ground_L_A;
    public GameObject ground_L_B;
    public GameObject ground_L_A_B;
    public GameObject ground_L_T;
    public GameObject ground_R;
    public GameObject ground_R_A;
    public GameObject ground_R_B;
    public GameObject ground_R_A_B;
    public GameObject ground_R_T;
    public GameObject ground;

    //Test Objects
    public GameObject Test_Object;
    public int Testint = 0;

    // Map Size
    public int width;
    public int height;

    public int cellSightLength;

    // Seeds
    public string seed;
    public bool useRandomSeed;

    //Random fill percent 
    [Range(0, 100)]
    public int randomFillPercent;

    // Map array 
    int[,][,] map;

    //This runs when the program starts
    void Start()
    {
        GenerateMap();
    }

    //This runs every frame and at the moment is only used for finding keys pressed
    void Update()
    {
        // Keys used for showing the map on screen
        if (Input.GetKeyDown(KeyCode.Z))
            GenerateMap();
        if (Input.GetKeyDown(KeyCode.X))
            SmoothMap();
        if (Input.GetKeyDown(KeyCode.C))
            CreateWorld();

        // Keys used for the Counting Input function
        if (Input.GetKeyDown(KeyCode.UpArrow))
            CountInput("Up");
        if (Input.GetKeyDown(KeyCode.DownArrow))
            CountInput("Down");
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            CountInput("Enter");
    }

    // The begining process of creating the grid
    void GenerateMap()
    {
        map = new int[width, height][,];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = new int[cellSightLength + cellSightLength + 1, cellSightLength + cellSightLength + 1];
            }
        }
        RandomFillMap();
    }

    // Randomly sets cells to 1 or 0
    void RandomFillMap()
    {
        if (useRandomSeed)
            seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 )
                    map[x, y][1,1] = 1;
                else
                    map[x, y][1,1] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    // Returns the value of the cell it is given
    int Rule(int x,int y, int wallcount)
    {
        // Original Rule: wallcount >= 5
        if (wallcount >= 5)
            return 1;
        else
            return 0;
    }
    

    // Runs the next run of the CA
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x,y][1,1] = Rule(x, y, GetSurroundingWallCount(x, y));            
            }
        }
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GetNeighbors(x,y);

                //map[x, height - 1][1,1] = 0;
                //map[x, height - 2][1,1] = 0;

                if (x != 0 && x < width - 1 && y != 0 && y < height - 1)
                {
                    if (map[x - 1, y + 1][1,1] == 1 || map[x, y + 1][1,1] == 1 || map[x + 1, y + 1][1,1] == 1)
                    {
                        //map[x - 1, y + 1] = 0;
                        //map[x, y + 1] = 0;
                        //map[x + 1, y + 1] = 0;
                    }
                }
            }
        }
    }

    // Returns the aount of walls of the given cell
    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                        wallCount += map[neighbourX, neighbourY][1,1];
                }
                else
                    wallCount++;
            }
        }
        return wallCount;
    }

    // Sets the neighbors for a cell using a the veriable cellSightLength
    void GetNeighbors(int x, int y)
    {
        for (int j = -cellSightLength; j < cellSightLength+1; j++)
        {
            for (int i = -cellSightLength; i < cellSightLength+1; i++)
            {
                
                if (i == 0 && j == 0) 
                    continue;
                else if (x == 0 && j < 0)
                    continue;
                else if (y == 0 && i < 0)
                    continue;
                else if ((x == width - 1) && (j > 0))
                    continue;
                else if ((y == height - 1) && (i > 0))
                    continue;
                else if (map[x + j, y + i][1, 1] == 1)
                    map[x, y][j + cellSightLength, i + cellSightLength] = 1;
                    
            }
        }

        // DEBUG STUFF

        //for (int j = 0; j < cellSightLength; j++)
        //{
        //    for (int i = 0; i < cellSightLength; i++)
        //    {
        //        Debug.Log("[" + x + "," + y + "]" + "[" + (j + 0) + "," + (i + 0) + "]: " + map[x, y][j, i]);
        //    }
        //}
    }

    // DEBUG STUFF

    //void OnDrawGizmos()
    //{
    //    if (map != null)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            for (int y = 0; y < height; y++)
    //            {
    //                Gizmos.color = (map[x, y] == 1) ? Color.black : Color.green;
    //                Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
    //                Gizmos.DrawCube(pos, Vector3.one);
    //                //Rect rekt = new Rect(-width / 2 + x + .5f, -height / 2 + y + .5f, 1, 1);
    //                //Gizmos.DrawGUITexture(new Rect(pos, Vector.one), myTexture); 
    //            }
    //        }
    //    }
    //}


    // Spawns in objects to create the world
    void CreateWorld()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);

                    if (map[x, y][1,1] == 1)
                    {
                        if (y < height - 1 && x < width - 1 && x != 0 && map[x, y + 1][1, 1] == 0 && map[x - 1, y][1, 1] == 1 && map[x + 1, y][1, 1] == 1)
                            Instantiate(ground_T, pos, Quaternion.identity);
                        else if (x < width - 1 && x != 0 && y != 0 && map[x, y - 1][1, 1] == 0 && map[x - 1, y][1, 1] == 1 && map[x + 1, y][1, 1] == 1)
                            Instantiate(ground_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x != 0 && y != 0 && map[x, y + 1][1, 1] == 1 && map[x - 1, y][1, 1] == 0 && map[x, y - 1][1, 1] == 1)
                            Instantiate(ground_L, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y - 1][1, 1] == 1 && map[x + 1, y][1, 1] == 1 && map[x - 1, y][1, 1] == 0 && map[x - 1, y + 1][1, 1] == 0 && map[x, y + 1][1, 1] == 0)
                            Instantiate(ground_L_A, pos, Quaternion.identity);
                        else if (x != 0 && y != 0 && map[x, y - 1][1, 1] == 1 && map[x - 1, y][1, 1] == 1 && map[x - 1, y - 1][1, 1] == 0)
                            Instantiate(ground_L_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y + 1][1, 1] == 1 && map[x + 1, y][1, 1] == 1 && map[x - 1, y - 1][1, 1] == 0 && map[x, y - 1][1, 1] == 0 && map[x - 1, y][1, 1] == 0)
                            Instantiate(ground_L_A_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x != 0 && map[x, y + 1][1, 1] == 1 && map[x - 1, y][1, 1] == 1 && map[x - 1, y + 1][1, 1] == 0)
                            Instantiate(ground_L_T, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && y != 0 && map[x, y + 1][1, 1] == 1 && map[x + 1, y][1, 1] == 0 && map[x, y - 1][1, 1] == 1)
                            Instantiate(ground_R, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y + 1][1, 1] == 0 && map[x + 1, y + 1][1, 1] == 0 && map[x + 1, y][1, 1] == 0 && map[x - 1, y][1, 1] == 1 && map[x, y - 1][1, 1] == 1)
                            Instantiate(ground_R_A, pos, Quaternion.identity);
                        else if (x < width - 1 && y != 0 && map[x, y - 1][1, 1] == 1 && map[x + 1, y][1, 1] == 1 && map[x + 1, y - 1][1, 1] == 0)
                            Instantiate(ground_R_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y + 1][1, 1] == 1 && map[x - 1, y][1, 1] == 1 && map[x + 1, y - 1][1, 1] == 0 && map[x, y - 1][1, 1] == 0 && map[x + 1, y][1, 1] == 0)
                            Instantiate(ground_R_A_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && map[x, y + 1][1, 1] == 1 && map[x + 1, y][1, 1] == 1 && map[x + 1, y + 1][1, 1] == 0)
                            Instantiate(ground_R_T, pos, Quaternion.identity);
                        else
                            Instantiate(ground, pos, Quaternion.identity);
                    }
                        
                }
            }
        }
    }

    int count = 0;
    
    void CountInput(string keydown)
    {
        if (keydown == "Up")
        {
            count++;
        }
        else if (keydown == "Down")
        {
            count--;
        }
        else if(keydown == "Enter")
        {
            Debug.Log("Input Value: " + count);
            count = 0;
        }
    }

}