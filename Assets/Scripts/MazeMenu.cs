// MazeMenu.cs - Orquestrador principal dos menus
using UnityEngine;

public static class MazeMenu
{
    private static bool showRanking = false;
    private static bool showStats = false;

    // Método principal que orquestra qual menu mostrar
    public static void DrawMenu(int record, int lives)
    {
        if (showRanking)
        {
            MazeRankingMenu.Draw();
            return;
        }
        
        if (showStats)
        {
            MazeStatsMenu.Draw();
            return;
        }
        
        MazeMainMenu.Draw(record, lives);
    }

    // Métodos para compatibilidade com ProceduralMaze
    public static void DrawGameOver(int score, int record, int lives) => MazeGameOverMenu.Draw(score, record, lives);
    public static void RenderMenu(ProceduralMaze maze) => DrawMenu(maze.scoreRecord, maze.lives);
    public static void RenderPauseMenu(ProceduralMaze maze) => MazePauseMenu.Draw(maze);
    public static void RenderGameOverMenu(ProceduralMaze maze) => DrawGameOver(maze.score, maze.scoreRecord, maze.lives);
    public static void RenderVictoryMenu(ProceduralMaze maze) => MazeVictoryMenu.Draw(maze);

    // Input e navegação delegados para MazeMenuInput
    public static void HandleMenuInput() => MazeMenuInput.HandleMenuInput();
    public static void HandleGameOverInput() => MazeMenuInput.HandleGameOverInput();

    // Métodos públicos para alternar menus
    public static void ShowRanking() => showRanking = true;
    public static void HideRanking() => showRanking = false;
    public static void ShowStats() => showStats = true;
    public static void HideStats() => showStats = false;
}
