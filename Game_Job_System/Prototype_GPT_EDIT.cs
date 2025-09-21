using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// ======================================
// Concept: Workload & Task System
// - Pawns trigger workloads (pathfinding, job assignment, etc.)
// - Tasks are defined by mod-friendly strings like:
//   (a5 "Stone Floor" in Range of distance 0)
//   (i5 "Metal Bar" in Range of distance 5 with count 10)
// - Engine parses these strings into compact, bitmask-based checks
// ======================================

public class Prototype_GPT_EDIT
{
    private List<Pawn> pawns;
    private List<int> pathWaitLine;        
    private List<int> taskWaitLine;        
    private List<int> calculationWaitLine; 
    private List<GameTask> taskLibrary;

    public Prototype_GPT_EDIT()
    {
        pawns = new List<Pawn>();
        taskLibrary = new List<GameTask>();
        pathWaitLine = new List<int>();
        taskWaitLine = new List<int>();
        calculationWaitLine = new List<int>();
    }

    // Called every simulation tick
    public void Update()
    {
        EnqueueWork();
    }

    // Sort pawns into engine workload queues
    private void EnqueueWork()
    {
        foreach (var pawn in pawns)
        {
            if (!pawn.gotTask)
                taskWaitLine.Add(pawn.uid);

            if (!pawn.isMoving && !pawn.workingOnTask && !pawn.gotPath)
                pathWaitLine.Add(pawn.uid);

            if (pawn.isAtTaskPosition && pawn.gotTask)
                calculationWaitLine.Add(pawn.uid);
        }
    }

    // Assign a job-like task to a pawn (engine-side decision)
    public void GiveTask(Pawn pawn)
    {
        foreach (var layer in pawn.workLayers)
        {
            foreach (var task in taskLibrary)
            {
                if (task.workLayer == layer && task.IsPossible(pawn, null))
                {
                    pawn.currentTask = task;
                    pawn.gotTask = true;
                    return;
                }
            }
        }
    }

    // ========================================================
    // GameTask: A parsed engine-friendly definition of work
    // ========================================================
    public class GameTask
    {
        public string name;
        public string description;
        public int workLayer;

        public char[] taskFilter;    // 'a', 'b', 'i', etc.
        public int[] count;          // counts required (e.g. "with count 10")
        public int[] layer;          // map layers to check
        public int[] distance;       // distance requirement
        public int[] distanceMax;    // optional max distance (like 5/10)
        public int[] objectMask;     // hashed object ID (fast lookup)

        public string requiredStat;  // optional stat requirement

        // Engine check: can pawn fulfill this?
        public bool IsPossible(Pawn pawn, Map map)
        {
            for (int i = 0; i < taskFilter.Length; i++)
            {
                char filter = taskFilter[i];

                if (filter == 'a') // "Tile nearby"
                {
                    if (distance[i] == 0 &&
                        Matches(map.GetTile(pawn.position.x, pawn.position.y, layer[i]), objectMask[i]))
                        continue;
                    else return false;
                }

                if (filter == 'i') // "Object in sight with count"
                {
                    if (count[i] > 0 && distance[i] > 0 &&
                        Matches(map.GetInSightOfTileDistanceAndCount(
                            pawn.position.x, pawn.position.y, distance[i], count[i], layer[i]), objectMask[i]))
                        continue;
                    else return false;
                }
            }
            return true;
        }

        private bool Matches(int mask, int objectMask) => (mask & objectMask) != 0;
    }

    // ========================================================
    // Parser: Convert mod-friendly strings into GameTasks
    // ========================================================
    public static class TaskParser
    {
        public static GameTask ParseTaskString(string input)
        {
            var task = new GameTask();

            // Step 1: Extract filter letter + distance (e.g. a5, i10, b5/15)
            var match = Regex.Match(input, @"([a-z])(\d+)(?:\/(\d+))?");
            if (match.Success)
            {
                task.taskFilter = new[] { match.Groups[1].Value[0] };
                task.distance = new[] { int.Parse(match.Groups[2].Value) };
                task.distanceMax = match.Groups[3].Success
                    ? new[] { int.Parse(match.Groups[3].Value) }
                    : new[] { 0 };
            }

            // Step 2: Extract object name ("Stone Floor")
            var objectMatch = Regex.Match(input, "\"([^\"]+)\"");
            if (objectMatch.Success)
            {
                task.name = objectMatch.Groups[1].Value;
                task.objectMask = new[] { objectMatch.Groups[1].Value.GetHashCode() }; // hashed ID
            }

            // Step 3: Count requirement
            var countMatch = Regex.Match(input, @"count\s+(\d+)");
            task.count = new[] { countMatch.Success ? int.Parse(countMatch.Groups[1].Value) : 0 };

            // Step 4: Stat requirement (optional)
            var statMatch = Regex.Match(input, @"stat\s+\""(.*?)\""");
            if (statMatch.Success)
                task.requiredStat = statMatch.Groups[1].Value;

            return task;
        }
    }

    // ========================================================
    // World Components
    // ========================================================
    public class Map
    {
        public int GetTile(int x, int y, int layer) => 0;
        public int GetInSightOfTileDistanceAndCount(int x, int y, float distance, int count, int layer) => 0;
    }

    public class Pawn
    {
        public int uid;
        public Vector2Int position;
        public List<int> workLayers = new List<int>();

        public GameTask currentTask;

        // State flags used by the engine
        public bool gotPath;
        public bool isMoving;
        public bool workingOnTask;
        public bool gotTask;
        public bool isAtTaskPosition;
        public bool heavyWorkLoadTask;

        public List<Stat> stats = new List<Stat>();
    }

    public class Stat
    {
        public string name;
        public string description;
    }
}

// Helper struct
public struct Vector2Int { public int x, y; }

// Conceptual Flow
// Moders write:
// 	(i5 "Metal Bar" in Range of distance 5 with count 10)
// Parser turns it into:
// 	Filter = 'i'
// 	Distance = 5
// 	Count = 10
// 	ObjectMask = Hash("Metal Bar" + Key)
