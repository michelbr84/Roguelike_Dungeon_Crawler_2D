using System.Collections.Generic;
using UnityEngine;

public static class MazeTeleportUtils
{
    public static Vector2Int FindSafeTeleportPosition(ProceduralMaze maze)
    {
        List<Vector2Int> safeSpots = new List<Vector2Int>();
        for (int x = 0; x < maze.width; x++)
        {
            for (int y = 0; y < maze.height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (maze.maze[x, y] == 0 && pos != maze.exitPos && !maze.enemies.Contains(pos))
                    safeSpots.Add(pos);
            }
        }
        if (safeSpots.Count == 0) return maze.playerPos;
        return safeSpots[Random.Range(0, safeSpots.Count)];
    }
} 