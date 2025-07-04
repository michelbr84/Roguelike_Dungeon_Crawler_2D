using UnityEngine;

public static class MazeInitialization
{
    public static void InitializeGame(ProceduralMaze maze)
    {
        // Carregar configurações
        MazeSaveSystem.LoadSettings();
        
        // Tentar carregar save existente
        if (!MazeSaveSystem.LoadGame(maze))
        {
            // Carregar recorde salvo (fallback)
            maze.scoreRecord = MazeSaveSystem.LoadRecord();
        }
        
        // Inicializar sistema de achievements
        MazeAchievements.Initialize();
        
        // Inicializar controles touch
        MazeTouchControls.InitializeTouchControls();
        
        // Inicializar sistema de missões
        MazeMissions.InitializeMissions();
        MazeMissions.GenerateMissions(maze.currentLevel);
        
        // Inicializar menu de configurações
        MazeSettingsMenu.Initialize();
        
        // Inicializar sistema de dificuldade
        MazeDifficultySystem.Initialize();
        MazeDifficultySystem.ApplyDifficultySettings(maze);
        
        // Inicializar sistema de estatísticas
        MazeStatistics.Initialize();
        MazeStatistics.OnGameStarted();
        
        // Resetar HUD
        MazeHUD.ResetAll();
        
        // Configurações iniciais
        maze.score = 0;
        maze.currentLevel = 1;
        maze.lives = maze.startingLives;
        maze.ammo = maze.startingAmmo;
        maze.shieldActive = false;
        maze.shieldTimer = 0f;
        
        // Configurar dificuldade e gerar maze
        MazeDifficultyUtils.AdjustDifficulty(maze, true);
        MazeGenerationUtils.RegenerateMazeEnsuringPath(maze);
        
        // Spawn de elementos
        MazeEnemyUtils.SpawnEnemies(maze);
        MazePowerUpUtils.SpawnPowerUps(maze);
        
        // Estado inicial
        ProceduralMaze.gameState = GameState.Menu;
    }
} 