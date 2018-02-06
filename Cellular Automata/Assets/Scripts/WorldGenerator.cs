using UnityEngine;
using System.Collections;
using System;

public class WorldGenerator : MonoBehaviour
{

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

    public GameObject Test_1;
    //public GameObject


    //Cave Size
    public int width;
    public int height;

    //Random Seeds
    public string seed;
    public bool useRandomSeed;

    //Amount of the cave is open
    [Range(0, 100)]
    public int randomFillPercent;

    int[,] map;

    //This runs when the program starts
    void Start()
    {
        GenerateMap();
    }

    //This runs every frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            GenerateMap();

        if (Input.GetKeyDown(KeyCode.X))
        {
            //for (int i = 0; i < 5; i++)
            SmoothMap();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateWorld();
            Debug.Log("Mouse 3 was clicked");
        }
    }

    //Holds all the code to generate the cave
    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        //for (int i = 0; i < 10; i++)
        //SmoothMap();
    }

    //Sets the amount of the cave is open
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

    

    //Selects which cells will be on and off
    void SmoothMap()
    {
        //GroundController.instance.DestoryObjects();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                // 9 Options
                // 8 Neighbours = Ground
                // 7 Neighbours = Ground_L , Ground_R_T ,  Ground_L_T ,  Ground_R_T ,  Ground_T
                // 6 Neighbours = 
                // 5 Neighbours = 
                // 4 Neighbours = 
                // 3 Neighbours = 
                // 2 Neighbours = 
                // 1 Neighbours = 
                // 0 Neighbours = Remove its self (Mabye change leter)

                // Some option to add enemys

                

                if (neighbourWallTiles >= 5)
                    map[x, y] = 1;
                else
                    map[x, y] = 0;
            }
        }

        //CreateWorld();
    }

    //Finds the amount of walls 
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

    //Draws the cave and gives color
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
                        map[x, y] = 2;

                    }

                    if(map[x,y] == 2)
                    {
                        Instantiate(Test_1, pos, Quaternion.identity);
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

    void ResetWorld()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);

                    Instantiate(ground_T, pos, Quaternion.identity);
                }
            }
        }
    }

}