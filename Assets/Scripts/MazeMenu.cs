// MazeMenu.cs
using UnityEngine;
using System.Collections.Generic;

public static class MazeMenu
{
    private static bool showRanking = false;
    private static bool showStats = false;
    
    private static readonly string controlsText =
        "Controles:\n" +
        "Mover: Setas\n" +
        "Atirar: Espaço\n" +
        "Sair: Esc";

    public static void DrawMenu(int record, int lives)
    {
        if (showRanking)
        {
            DrawRanking();
            return;
        }
        
        if (showStats)
        {
            DrawStats();
            return;
        }
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 48;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.cyan;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 30;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 28;
        infoStyle.alignment = TextAnchor.MiddleCenter;
        infoStyle.normal.textColor = Color.yellow;

        int w = 600, h = 380;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(12);
        GUILayout.Label("Procedural Maze", titleStyle);
        GUILayout.Space(24);

        GUILayout.Label($"Recorde: {record}", infoStyle);
        GUILayout.Space(8);
        GUILayout.Label($"Vidas: {lives}", infoStyle);
        GUILayout.Space(12);

        if (GUILayout.Button("Start Game", buttonStyle, GUILayout.Height(48)))
        {
            ProceduralMaze.StartGameFromMenu();
        }

        GUILayout.Space(12);
        
        if (GUILayout.Button("Tutorial", buttonStyle, GUILayout.Height(36)))
        {
            MazeTutorial.StartTutorial();
        }
        
        GUILayout.Space(8);
        
        if (GUILayout.Button("Ranking", buttonStyle, GUILayout.Height(36)))
        {
            showRanking = true;
        }
        
        GUILayout.Space(8);
        
        if (GUILayout.Button("Estatísticas", buttonStyle, GUILayout.Height(36)))
        {
            showStats = true;
        }
        
        GUILayout.Space(8);
        
        if (GUILayout.Button("Selecionar Classe", buttonStyle, GUILayout.Height(36)))
        {
            MazeClassSelectionMenu.ShowMenu();
        }
        
        GUILayout.Space(8);
        
        if (GUILayout.Button("Crafting", buttonStyle, GUILayout.Height(36)))
        {
            // TODO: Implementar menu de crafting
            MazeHUD.ShowStatusMessage("Sistema de Crafting em desenvolvimento!");
        }
        
        GUILayout.Space(8);
        
        if (GUILayout.Button("Pets", buttonStyle, GUILayout.Height(36)))
        {
            // TODO: Implementar menu de pets
            MazeHUD.ShowStatusMessage("Sistema de Pets em desenvolvimento!");
        }

        GUILayout.Space(12);

        GUILayout.Label(controlsText, infoStyle);

#if UNITY_EDITOR
        GUILayout.Space(12);
        if (GUILayout.Button("Sair", buttonStyle, GUILayout.Height(36)))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#else
        GUILayout.Space(12);
        if (GUILayout.Button("Sair", buttonStyle, GUILayout.Height(36)))
        {
            Application.Quit();
        }
#endif

        GUILayout.EndArea();
    }
    
    public static void DrawRanking()
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 48;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.yellow;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 24;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle entryStyle = new GUIStyle(GUI.skin.label);
        entryStyle.fontSize = 20;
        entryStyle.normal.textColor = Color.white;
        entryStyle.alignment = TextAnchor.MiddleCenter;

        int w = 500, h = 500;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(12);
        GUILayout.Label("TOP 10 RANKING", titleStyle);
        GUILayout.Space(20);

        List<int> ranking = MazeSaveSystem.GetRanking();
        for (int i = 0; i < ranking.Count; i++)
        {
            GUILayout.Label($"{i + 1}. {ranking[i]}", entryStyle);
            GUILayout.Space(4);
        }
        
        if (ranking.Count == 0)
        {
            GUILayout.Label("Sem scores ainda.", entryStyle);
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("Limpar Ranking", buttonStyle, GUILayout.Height(36)))
        {
            MazeSaveSystem.ClearRanking();
        }
        
        GUILayout.Space(12);
        
        if (GUILayout.Button("Voltar", buttonStyle, GUILayout.Height(36)))
        {
            showRanking = false;
        }

        GUILayout.EndArea();
    }
    
    public static void DrawStats()
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 48;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.cyan;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 24;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 18;
        infoStyle.normal.textColor = Color.white;
        infoStyle.alignment = TextAnchor.MiddleLeft;

        int w = 600, h = 600;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(12);
        GUILayout.Label("ESTATÍSTICAS", titleStyle);
        GUILayout.Space(20);

        var stats = MazeStatistics.GetPlayerStats();
        if (stats != null)
        {
            GUILayout.Label($"Rank: {MazeStatistics.GetPlayerRank()}", infoStyle);
            GUILayout.Label($"Jogos: {stats.totalGamesPlayed}", infoStyle);
            GUILayout.Label($"Vitórias: {stats.totalWins}", infoStyle);
            GUILayout.Label($"Derrotas: {stats.totalLosses}", infoStyle);
            GUILayout.Label($"Tempo Total: {MazeStatistics.GetFormattedPlayTime()}", infoStyle);
            GUILayout.Label($"Score Máximo: {stats.highestScore}", infoStyle);
            GUILayout.Label($"Nível Máximo: {stats.highestLevel}", infoStyle);
            GUILayout.Label($"Inimigos Mortos: {stats.totalEnemiesKilled}", infoStyle);
            GUILayout.Label($"Power-ups: {stats.totalPowerUpsCollected}", infoStyle);
            GUILayout.Label($"Precisão: {MazeStatistics.GetFormattedAccuracy()}", infoStyle);
            GUILayout.Label($"Taxa de Sobrevivência: {MazeStatistics.GetFormattedSurvivalRate()}", infoStyle);
            GUILayout.Label($"Conquistas: {MazeAchievements.GetUnlockedAchievementsCount()}", infoStyle);
        }
        else
        {
            GUILayout.Label("Nenhuma estatística disponível.", infoStyle);
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("Voltar", buttonStyle, GUILayout.Height(36)))
        {
            showStats = false;
        }

        GUILayout.EndArea();
    }

    public static void DrawGameOver(int score, int record, int lives)
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 52;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.red;

        GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 32;
        infoStyle.normal.textColor = Color.yellow;
        infoStyle.alignment = TextAnchor.UpperCenter;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 28;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        int w = 540, h = 340;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(16);
        GUILayout.Label("GAME OVER", titleStyle);
        GUILayout.Space(12);

        GUILayout.Label($"Score: {score}", infoStyle);
        GUILayout.Label($"Recorde: {record}", infoStyle);
        GUILayout.Label($"Vidas: {lives}", infoStyle); // sempre mostra 0 vidas

        GUILayout.Space(16);

        if (GUILayout.Button("Jogar Novamente", buttonStyle, GUILayout.Height(48)))
        {
            ProceduralMaze.RestartGameFromGameOver();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Sair", buttonStyle, GUILayout.Height(36)))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        GUILayout.EndArea();
    }

    // HandleMenuInput e HandleGameOverInput já devem existir, pode manter igual ao anterior.
    public static void HandleMenuInput()
    {
        if (Event.current != null && Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.Space)
            {
                ProceduralMaze.StartGameFromMenu();
            }
            else if (Event.current.keyCode == KeyCode.Escape)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }

    public static void HandleGameOverInput()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            ProceduralMaze.RestartGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ProceduralMaze.ReturnToMenu();
        }
    }
    
    // Novos métodos para compatibilidade com ProceduralMaze
    public static void RenderMenu(ProceduralMaze maze)
    {
        DrawMenu(maze.scoreRecord, maze.lives);
    }
    
    public static void RenderPauseMenu(ProceduralMaze maze)
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 48;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.yellow;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 30;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        int w = 400, h = 300;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(20);
        GUILayout.Label("JOGO PAUSADO", titleStyle);
        GUILayout.Space(30);

        if (GUILayout.Button("Continuar", buttonStyle, GUILayout.Height(48)))
        {
            ProceduralMaze.gameState = GameState.Playing;
        }

        GUILayout.Space(12);
        
        if (GUILayout.Button("Configurações", buttonStyle, GUILayout.Height(36)))
        {
            ProceduralMaze.gameState = GameState.Settings;
            MazeSettingsMenu.ToggleMenu();
        }
        
        GUILayout.Space(12);
        
        if (GUILayout.Button("Voltar ao Menu", buttonStyle, GUILayout.Height(36)))
        {
            ProceduralMaze.gameState = GameState.Menu;
        }

        GUILayout.EndArea();
    }
    
    public static void RenderGameOverMenu(ProceduralMaze maze)
    {
        DrawGameOver(maze.score, maze.scoreRecord, maze.lives);
    }
    
    public static void RenderVictoryMenu(ProceduralMaze maze)
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 48;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.green;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 30;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 24;
        infoStyle.alignment = TextAnchor.MiddleCenter;
        infoStyle.normal.textColor = Color.white;

        int w = 500, h = 400;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(20);
        GUILayout.Label("VITÓRIA!", titleStyle);
        GUILayout.Space(20);
        
        GUILayout.Label($"Nível {maze.currentLevel} Completado!", infoStyle);
        GUILayout.Space(10);
        GUILayout.Label($"Score: {maze.score}", infoStyle);
        GUILayout.Space(30);

        if (GUILayout.Button("Próximo Nível", buttonStyle, GUILayout.Height(48)))
        {
            maze.NextLevel();
        }

        GUILayout.Space(12);
        
        if (GUILayout.Button("Voltar ao Menu", buttonStyle, GUILayout.Height(36)))
        {
            ProceduralMaze.gameState = GameState.Menu;
        }

        GUILayout.EndArea();
    }
}
