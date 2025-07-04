// Assets/Scripts/Maze/MazeGenerationUtils.cs
using System.Collections.Generic;
using UnityEngine;

public static class MazeGenerationUtils
{
    /// <summary>
    /// Gera um novo labirinto garantindo sempre um caminho válido do player até a saída.
    /// </summary>
    public static void RegenerateMazeEnsuringPath(ProceduralMaze mazeObj)
    {
        int maxTries = 100;
        for (int attempt = 0; attempt < maxTries; attempt++)
        {
            GenerateMaze(mazeObj);
            PlacePlayerAndExit(mazeObj);
            if (PathExists(mazeObj, mazeObj.playerPos, mazeObj.exitPos))
            {
                ClearPathBetween(mazeObj, mazeObj.playerPos, mazeObj.exitPos);
                return;
            }
        }
        Debug.LogError("Falha ao gerar maze com caminho válido após várias tentativas!");
        GenerateMaze(mazeObj);
        PlacePlayerAndExit(mazeObj);
    }

    /// <summary>
    /// Gera o labirinto randomicamente (1 = parede, 0 = caminho).
    /// </summary>
    public static void GenerateMaze(ProceduralMaze mazeObj)
    {
        mazeObj.maze = new int[mazeObj.width, mazeObj.height];
        var rng = new System.Random();
        for (int x = 0; x < mazeObj.width; x++)
        for (int y = 0; y < mazeObj.height; y++)
            mazeObj.maze[x, y] = rng.NextDouble() < mazeObj.wallProbability ? 1 : 0;
    }

    /// <summary>
    /// Define a posição inicial do jogador e da saída do labirinto.
    /// </summary>
    public static void PlacePlayerAndExit(ProceduralMaze mazeObj)
    {
        mazeObj.playerPos = new Vector2Int(0, 0);
        mazeObj.exitPos = new Vector2Int(mazeObj.width - 1, mazeObj.height - 1);
        mazeObj.maze[mazeObj.playerPos.x, mazeObj.playerPos.y] = 0;
        mazeObj.maze[mazeObj.exitPos.x, mazeObj.exitPos.y] = 0;
        mazeObj.playerDir = Vector2Int.down;
    }

    /// <summary>
    /// Checa se existe um caminho válido entre dois pontos (BFS).
    /// </summary>
    public static bool PathExists(ProceduralMaze mazeObj, Vector2Int from, Vector2Int to)
    {
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        bool[,] visited = new bool[mazeObj.width, mazeObj.height];
        q.Enqueue(from);
        visited[from.x, from.y] = true;
        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };
        while (q.Count > 0)
        {
            var p = q.Dequeue();
            if (p == to) return true;
            for (int i = 0; i < 4; i++)
            {
                int nx = p.x + dx[i], ny = p.y + dy[i];
                if (nx >= 0 && nx < mazeObj.width && ny >= 0 && ny < mazeObj.height &&
                    !visited[nx, ny] && mazeObj.maze[nx, ny] == 0)
                {
                    visited[nx, ny] = true;
                    q.Enqueue(new Vector2Int(nx, ny));
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Garante que todas as células no caminho do player até a saída estejam livres.
    /// </summary>
    public static void ClearPathBetween(ProceduralMaze mazeObj, Vector2Int from, Vector2Int to)
    {
        Dictionary<Vector2Int, Vector2Int> prev = new Dictionary<Vector2Int, Vector2Int>();
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        bool[,] visited = new bool[mazeObj.width, mazeObj.height];
        q.Enqueue(from);
        visited[from.x, from.y] = true;
        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };
        bool found = false;
        while (q.Count > 0)
        {
            var p = q.Dequeue();
            if (p == to)
            {
                found = true;
                break;
            }
            for (int i = 0; i < 4; i++)
            {
                int nx = p.x + dx[i], ny = p.y + dy[i];
                Vector2Int np = new Vector2Int(nx, ny);
                if (nx >= 0 && nx < mazeObj.width && ny >= 0 && ny < mazeObj.height &&
                    !visited[nx, ny] && mazeObj.maze[nx, ny] == 0)
                {
                    visited[nx, ny] = true;
                    prev[np] = p;
                    q.Enqueue(np);
                }
            }
        }
        if (found)
        {
            var p = to;
            while (p != from)
            {
                mazeObj.maze[p.x, p.y] = 0;
                p = prev[p];
            }
            mazeObj.maze[from.x, from.y] = 0;
        }
    }
}
