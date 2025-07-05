using UnityEngine;

public static class MazeHUD
{
    public static int currentLevel = 1;
    public static int score = 0;
    public static float lastVictoryTime = 0f;
    public static string statusMessage = "";
    public static float statusMessageTime = 0f;

    public static void DrawHUD(ProceduralMaze mazeObj)
    {
        MazeLifeHUD.Draw(mazeObj);
        MazeAmmoHUD.Draw(mazeObj);
        MazePowerUpHUD.Draw(mazeObj);
        MazeStatsHUD.Draw(mazeObj);
        MazeMetaHUD.Draw(mazeObj);
        MazeStatusHUD.Draw(mazeObj, statusMessage, statusMessageTime);
        MazeMissionHUD.Draw();
    }

    public static void ShowStatusMessage(string msg)
    {
        statusMessage = msg;
        statusMessageTime = Time.time;
    }

    public static void AddScore(int amount)
    {
        score += amount;
    }

    public static void NextLevel()
    {
        currentLevel++;
        ShowStatusMessage($"Level {currentLevel}!");
    }

    public static void ResetAll()
    {
        score = 0;
        currentLevel = 1;
        statusMessage = "";
        statusMessageTime = 0;
    }
    
    // Método para compatibilidade com ProceduralMaze
    public static void RenderHUD(ProceduralMaze maze)
    {
        DrawHUD(maze);
    }
}
