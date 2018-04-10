using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    // Prefabs of all the tiles
    [System.Serializable]
    public class gameobjects
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

        public GameObject marker;
    };
    public gameobjects GameObjects;

    //Test Objects
    //public GameObject Test_Object;
    //public int Testint = 0;

    [Header("Map Size")]
    public int width;
    public int height;

    public int cellSightLength;
    [Space(10)]

    [Header("Seeds")]
    public string seed;
    public bool useRandomSeed;
    [Space(10)]

    //Random fill percent 
    [Range(0, 100)]
    public int randomFillPercent;
    [Space(10)]

    [Header("Graph")]
    public bool makeGraph = false;
    public int jumpDistance = 3;

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
        {
            CreateWorld();
            CreateGraph();
        }
            
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

        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < height; y++)
        //    {
        //        GetNeighbors(x, y);

        //        map[x, height - 1][1, 1] = 0;
        //        map[x, height - 2][1, 1] = 0;

        //        if (x != 0 && x < width - 1 && y != 0 && y < height - 1)
        //        {
        //            if (map[x - 1, y + 1][1, 1] == 1 || map[x, y + 1][1, 1] == 1 || map[x + 1, y + 1][1, 1] == 1)
        //            {
        //                map[x - 1, y + 1] = 0;
        //                map[x, y + 1] = 0;
        //                map[x + 1, y + 1] = 0;
        //            }
        //        }
        //    }
        //}
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

        //for (int j = -cellSightLength; j < cellSightLength + 1; j++)
        //{
        //    for (int i = -cellSightLength; i < cellSightLength + 1; i++)
        //    {
        //        Debug.Log("[" + x + "," + y + "]" + "[" + (j + cellSightLength) + "," + (i + cellSightLength) + "]: " + map[x, y][i + cellSightLength, i + cellSightLength]);
        //    }
        //}
    }

    class Vertex
    {
        public int x;
        public int y;

        public void SetXandY(int xin, int yin)
        {
            x = xin;
            y = yin;
        }

        public override string ToString()
        {
            string str = x.ToString() + "," + y.ToString();

            return str;
        }
    }

    class Edges
    {
        public Vertex source;
        public Vertex target;

        public void SetEdge(Vertex sourc, Vertex targe)
        {
            source = sourc;
            target = targe;
        }

        public override string ToString()
        {
            String str = source.ToString() + " " + target.ToString();

            return str;
        }
    }

    class Graph
    {
        public List<Vertex> vertices = new List<Vertex>();
        public List<Edges> edges = new List<Edges>();

        public void AddVertex(Vertex v)
        {
            vertices.Add(v);
        }

        public void AddEdge(Edges e)
        {
            edges.Add(e);
        }
    }

    
    Graph g = new Graph();

    Vector2 source = new Vector2();
    Vector2 target = new Vector2();

    void CreateGraph()
    {
        if (makeGraph)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 pos = new Vector3(x, y, 0);

                    if (map[x,y][1,1] == 1)
                    {
                        
                        Instantiate(GameObjects.marker, pos, Quaternion.identity);
                        FindEdges(x, y);

                        Debug.DrawLine(source, target, Color.red, 20f);
                        //Debug.Log(source.x.ToString() + "," + source.y.ToString() + " to " + target.x.ToString() + "," + target.y.ToString());
                    }
                }
            }
        }
    }

    void FindEdges(int x,int y)
    {
        Vertex v = new Vertex();
        v.SetXandY(x, y);
        g.AddVertex(v);
        for (int j = -cellSightLength; j < cellSightLength + 1; j++)
        {
            for (int i = -cellSightLength; i < cellSightLength + 1; i++)
            {

                //if (j != 1 && i != 1)
                //{
                if (map[x,y][j + cellSightLength, i + cellSightLength] == 1)
                {
                    
                    Vertex v1 = new Vertex();
                    Edges e = new Edges();
                    e.source = v;

                    v1.SetXandY(x + j, y + i);
                    g.AddVertex(v1);
                    e.target = v1;
                    g.AddEdge(e);
                    Debug.Log(e.ToString());
                    //Create new vector2 instead of overriding 
                    source.Set(x, y);
                    target.Set(x + j, y + i);
                    Debug.DrawLine(source, target, Color.red, 20f);
                }
                //}
            }
        }
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
                    Vector3 pos = new Vector3(x, y, 0);

                    if (map[x, y][1,1] == 1)
                    {
                        if (y < height - 1 && x < width - 1 && x != 0 && map[x, y + 1][1, 1] == 0 && map[x - 1, y][1, 1] == 1 && map[x + 1, y][1, 1] == 1)
                            Instantiate(GameObjects.ground_T, pos, Quaternion.identity);
                        else if (x < width - 1 && x != 0 && y != 0 && map[x, y - 1][1, 1] == 0 && map[x - 1, y][1, 1] == 1 && map[x + 1, y][1, 1] == 1)
                            Instantiate(GameObjects.ground_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x != 0 && y != 0 && map[x, y + 1][1, 1] == 1 && map[x - 1, y][1, 1] == 0 && map[x, y - 1][1, 1] == 1)
                            Instantiate(GameObjects.ground_L, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y - 1][1, 1] == 1 && map[x + 1, y][1, 1] == 1 && map[x - 1, y][1, 1] == 0 && map[x - 1, y + 1][1, 1] == 0 && map[x, y + 1][1, 1] == 0)
                            Instantiate(GameObjects.ground_L_A, pos, Quaternion.identity);
                        else if (x != 0 && y != 0 && map[x, y - 1][1, 1] == 1 && map[x - 1, y][1, 1] == 1 && map[x - 1, y - 1][1, 1] == 0)
                            Instantiate(GameObjects.ground_L_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y + 1][1, 1] == 1 && map[x + 1, y][1, 1] == 1 && map[x - 1, y - 1][1, 1] == 0 && map[x, y - 1][1, 1] == 0 && map[x - 1, y][1, 1] == 0)
                            Instantiate(GameObjects.ground_L_A_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x != 0 && map[x, y + 1][1, 1] == 1 && map[x - 1, y][1, 1] == 1 && map[x - 1, y + 1][1, 1] == 0)
                            Instantiate(GameObjects.ground_L_T, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && y != 0 && map[x, y + 1][1, 1] == 1 && map[x + 1, y][1, 1] == 0 && map[x, y - 1][1, 1] == 1)
                            Instantiate(GameObjects.ground_R, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y + 1][1, 1] == 0 && map[x + 1, y + 1][1, 1] == 0 && map[x + 1, y][1, 1] == 0 && map[x - 1, y][1, 1] == 1 && map[x, y - 1][1, 1] == 1)
                            Instantiate(GameObjects.ground_R_A, pos, Quaternion.identity);
                        else if (x < width - 1 && y != 0 && map[x, y - 1][1, 1] == 1 && map[x + 1, y][1, 1] == 1 && map[x + 1, y - 1][1, 1] == 0)
                            Instantiate(GameObjects.ground_R_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && x != 0 && y != 0 && map[x, y + 1][1, 1] == 1 && map[x - 1, y][1, 1] == 1 && map[x + 1, y - 1][1, 1] == 0 && map[x, y - 1][1, 1] == 0 && map[x + 1, y][1, 1] == 0)
                            Instantiate(GameObjects.ground_R_A_B, pos, Quaternion.identity);
                        else if (y < height - 1 && x < width - 1 && map[x, y + 1][1, 1] == 1 && map[x + 1, y][1, 1] == 1 && map[x + 1, y + 1][1, 1] == 0)
                            Instantiate(GameObjects.ground_R_T, pos, Quaternion.identity);
                        else
                            Instantiate(GameObjects.ground, pos, Quaternion.identity);
                    }
                        
                }
            }
        }
    }
}