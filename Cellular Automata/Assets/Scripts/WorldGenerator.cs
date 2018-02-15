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

    // Seeds
    public string seed;
    public bool useRandomSeed;

    //Random fill percent 
    [Range(0, 100)]
    public int randomFillPercent;

    // Map array 
    int[,] map;

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
        map = new int[width, height];
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
                    map[x, y] = 1;
                else
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    // Returns the value of the cell it is given
    int Rule(int x,int y, int wallcount)
    {
        int returnvalue = 0;

        if (wallcount >= 5)
            returnvalue = 1;
        else
            returnvalue = 0;

        return returnvalue;
    }
    

    // Runs the next run of the CA
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x,y] = Rule(x, y, GetSurroundingWallCount(x, y));            
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
                        wallCount += map[neighbourX, neighbourY];
                }
                else
                    wallCount++;
            }
        }
        return wallCount;
    }

    // Debug Stuff 
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

                    if (map[x, y] == 0 && y == 0 && map[x - 1, y] == 0 && map[x + 1, y] == 0)
                    {
                        //map[x, y] = 2;

                    }

                    if(map[x,y] == 2)
                    {
                        //Instantiate(Test_Object, pos, Quaternion.identity);
                    }

                    if (map[x, y] == 1)
                    {
                        if (y < height - 1 && x < width - 1 && x != 0 && map[x, y + 1] == 0 && map[x - 1, y] == 1 && map[x + 1, y] == 1)
                            Instantiate(ground_T, pos, Quaternion.identity);
                        else if (x < width - 1 && x != 0 && y != 0 && map[x, y - 1] == 0 && map[x - 1, y] == 1 && map[x + 1, y] == 1)
                            Instantiate(ground_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x != 0 && y != 0 && map[x, y + 1] == 1 && map[x - 1, y] == 0 && map[x, y - 1] == 1)
                            Instantiate(ground_L, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y - 1] == 1 && map[x + 1, y] == 1 && map[x - 1, y] == 0 && map[x - 1, y + 1] == 0 && map[x, y + 1] == 0)
                            Instantiate(ground_L_A, pos, Quaternion.identity);
                        else if (x != 0 && y != 0 && map[x, y - 1] == 1 && map[x - 1, y] == 1 && map[x - 1, y - 1] == 0)
                            Instantiate(ground_L_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y + 1] == 1 && map[x + 1, y] == 1 && map[x - 1, y - 1] == 0 && map[x, y - 1] == 0 && map[x - 1, y] == 0)
                            Instantiate(ground_L_A_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x != 0 && map[x, y + 1] == 1 && map[x - 1, y] == 1 && map[x - 1, y + 1] == 0)
                            Instantiate(ground_L_T, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && y != 0 && map[x, y + 1] == 1 && map[x + 1, y] == 0 && map[x, y - 1] == 1)
                            Instantiate(ground_R, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y + 1] == 0 && map[x + 1, y + 1] == 0 && map[x + 1, y] == 0 && map[x - 1, y] == 1 && map[x, y - 1] == 1)
                            Instantiate(ground_R_A, pos, Quaternion.identity);
                        else if (x < width - 1 && y != 0 && map[x, y - 1] == 1 && map[x + 1, y] == 1 && map[x + 1, y - 1] == 0)
                            Instantiate(ground_R_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y + 1] == 1 && map[x - 1, y] == 1 && map[x + 1, y - 1] == 0 && map[x, y - 1] == 0 && map[x + 1, y] == 0)
                            Instantiate(ground_R_A_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && map[x, y + 1] == 1 && map[x + 1, y] == 1 && map[x + 1, y + 1] == 0)
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
        }
    }

}