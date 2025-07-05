using UnityEngine;

public static class MazeCellRenderer
{
    public static void DrawBackground(ProceduralMaze mazeObj)
    {
        if (mazeObj.backgroundTexture)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mazeObj.backgroundTexture, ScaleMode.StretchToFill);
        }
        else
        {
            Color oldBG = GUI.color;
            GUI.color = mazeObj.backgroundColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = oldBG;
        }
    }

    public static void DrawGridAndWalls(ProceduralMaze mazeObj, float cellSize)
    {
        for (int x = 0; x < mazeObj.width; x++)
        for (int y = 0; y < mazeObj.height; y++)
        {
            Rect cellRect = new Rect(x * cellSize, y * cellSize, cellSize, cellSize);
            if (mazeObj.maze[x, y] == 1)
            {
                if (mazeObj.wallTexture)
                {
                    GUI.DrawTexture(cellRect, mazeObj.wallTexture, ScaleMode.ScaleToFit);
                }
                else
                {
                    Color oldColor = GUI.color;
                    GUI.color = mazeObj.wallColor;
                    GUI.DrawTexture(cellRect, Texture2D.whiteTexture);
                    GUI.color = oldColor;
                }
            }
        }
    }

    public static void DrawExit(ProceduralMaze mazeObj, float cellSize)
    {
        int x = mazeObj.exitPos.x;
        int y = mazeObj.exitPos.y;
        Rect cellRect = new Rect(x * cellSize, y * cellSize, cellSize, cellSize);
        if (mazeObj.exitTexture)
        {
            GUI.DrawTexture(cellRect, mazeObj.exitTexture, ScaleMode.ScaleToFit);
        }
        else
        {
            Color oldColor = GUI.color;
            GUI.color = mazeObj.exitColor;
            GUI.DrawTexture(cellRect, Texture2D.whiteTexture);
            GUI.color = oldColor;
        }
    }
} 