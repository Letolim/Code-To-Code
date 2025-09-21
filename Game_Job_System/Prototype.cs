using System;
using System.Threading;

public class Prototype
{
    List<Pawn> pawns;


    public List<int> pathWaitLine;
    public List<int> taskWaitLine;

    public List<int> calculationWaitLine;

    public class Map()
    {
        string name;
        string descrtiption;

        public int[,][] bitMaskMaps;
        public float[,] heuristic;//Update on terrainLayerChange


        public int GetTile(int x, int y, int layer)
        {
            return [x, y][i];
        }

        public int GetInRangeOfTile(int x, int y, int layer)
        {
            return [x, y][i];
        }

        public Vector2Int GetInRangeOfTileWithDistance(int x, int y, float distance, int layer)
        {

            return null;
        }

        public int GetInSightOfTileDistanceAndCount(int x, int y, float distance, int count, int layer)
        {

            return -1;
        }

        public Vector2Int GetInSightOfTileCount(int x, int y, int distance, int layer)
        {
            return null;
        }
    }

    public Prototype()
    {
        pawns = new List<Pawn>();
        pathWaitLine = new List<int>();
        calculationWaitLine = new List<int>();
        taskWaitLine = new List<int>();
    }



    public void Update()//6 =  15%
    {
        //Draw
        TaskManager();
    }


    Thread taskManagerThread = taskManagerThreads;              //6 =  15% of CPU cores
    Thread[] pathFindingThreads = taskManagerThreads[2];        //6 = 30% of CPU cores
    Thread[] calculationThreads = calculationThreads[2];        //6 = 30% of CPU cores



    public void TaskManager()
    {
        for (int i = 0; i < pawns.Count; i++)
        {

            if (!pawns[i].gotTask)
                taskWaitLine.Add(pawns.[i].uid);

            if (!pawns[i].isMoving)
                continue;

            if (!pawns[i].workingOnTask && !pawns[i].gotPath)
                pathWaitLine.Add(pawns.[i].uid);

            if (pawns[i].isAtTaskPosition && pawns[i].gotTask)
                calculationWaitLine.Add(pawns.[i].uid);

        }
    }

    List<Task> taskList = new List<Task>(); // sorted

    public void GiveTask(Pawn pawn)
    {
        for (int i = 0; i < pawn.workLayer.Count; i++)
            for (int n = 0; n < taskList.Count; n++)
                if (taskList[n].workLayer == pawn.workLayer[i] && taskList[n].IsPossible(pawn, null))
                {
                    pawn.task = taskList[n];
                    return;
                }
    }


    public class Task()
    {
        //public List<string> xml;
        //<string>
        //<li>a5 "Stone Floor" in Range of distance 0;</li> 
        //</string>

        //a5 "" in Range of distance 5;       |   1    |    3    |   27   |
        //b5 "" in Range of distance 5/10;    |   1    |    3    |   29   |

        //c5 "" in Sight;  |   1    |    3   |
        //d5 "" in Sight;  |   1    |    3   |

        //e5 "" in Range of distance 5 with stat "";       |   1    |    3    |   27   |   39    |
        //f5 "" in Range of distance 5/10 with stat "";    |   1    |    3    |   29   |   41    |

        //g5 "" in Range of distance 5 with stats "";       |   1    |    3    |   27   |   40    |
        //h5 "" in Range of distance 5/10 with stats "";    |   1    |    3    |   29   |   42    |

        //i5 "" in Range of distance 5 with count 10;  |   1    |    3    |   27   |   41    |
        //f5 "" in Range of distance 5/10 with count 10;  


        //Simple Synstax for modding and intern workflow for example 
        //(a5 "Stone Floor" in Range of distance 0) would mean IsPossible if (Stone Floor in range 5).
        //(i5 "Metal Bar" in Range of distance 5 with count 10) would mean IsPossible if (Metal Bar in range 5 with count 10).
        //For compatiblity everything gets hashed/stored as bitmask no need work with strings after loading the game data, just use mod name + autor + what ever they name the game objects to genrate unique ID's.


        Animator animator;

        string name;
        string descrtiption;

        public int workLayer;

        public char[] taskFilter;
        public int[] count;
        public int[] layer;
        public int[] distance;
        public int[] distanceMax;
        public int[] objectMask;


        public bool GetBitmask(int mask, int objectMask)//if match
        {


        }



        public bool IsPossible(Pawn pawn, Map map)
        {
            for (int i = 0; i < taskFilter.Count; i++)
            {
                if (taskFilter[i] == 'a')//a would be deriverated from a xml listing like "a5 "Stone Floor" in Range of distance 0;"
                {
                    if (distance[i] == 0 && GetBitmask(map.GetTile(pawn.position.x, pawn.position.y, layer[i]), objectMask);
                    {
                            continue;
                    }else return false;
                    
                    if (distance[i] != 0 && GetBitmask(map.GetInRangeOfTileWithDistance(pawn.position.x, pawn.position.y, distance[i], layer[i]), objectMask);
                    {
                        continue;
                    }else return false;
                }

                if (taskFilter[i] == 'i')//i could be from i5 "Metal Bar" in Range of distance 5 with count 10
                {
                    if (count[i] != 0 && distance[i] != 0 && GetBitmask(map.GetInSightOfTileDistanceAndCount(pawn.position.x, pawn.position.y, distance[i], count[i], layer[i]), objectMask);
                    {
                        continue;
                    }else return false;
                }
            }

            return true;
        }


        public void Load()
        {
            //xml to count; layer distance distanceMax objectMask
        }

    }

    public class Stat()
    {
        string name;
        string descrtiption;
    }


    public class Pawn()
    {
        public List<Stat> stats = new List<Stat>();

        public Vector2Int position;

        public Task currentTask;

        public int uid;

        public bool gotPath;
        public bool isMoving;

        public bool workingOnTask;
        public bool gotTask;
        public bool isAtTaskPosition;

        public bool heavyWorkLoadTask;

        public List<int> workLayer;

        public void Update()
        {

        }
    }