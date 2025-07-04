using UnityEngine;

public static class MazeGameLoop
{
    public static void StartGameFromMenu()
    {
        var instance = Object.FindObjectOfType<ProceduralMaze>();
        if (instance == null) return;
        instance.score = 0;
        instance.currentLevel = 1;
        instance.lives = instance.startingLives;
        instance.ammo = instance.startingAmmo;
        instance.shieldActive = false;
        instance.shieldTimer = 0f;
        MazeHUD.ResetAll();
        MazeHUD.score = 0;
        MazeHUD.currentLevel = 1;
        MazeDifficultyUtils.AdjustDifficulty(instance, true);
        MazeGenerationUtils.RegenerateMazeEnsuringPath(instance);
        MazeEnemyUtils.SpawnEnemies(instance);
        MazeAdvancedEnemies.InitializeAdvancedEnemies();
        MazeAdvancedEnemies.SpawnAdvancedEnemies(instance, instance.currentLevel);
        MazePowerUpUtils.SpawnPowerUps(instance);
        
        // Atualizar missões para novo nível
        MazeMissions.ResetMissionsForNewLevel();
        MazeMissions.GenerateMissions(instance.currentLevel);
        
        // Gerar clima e eventos para o novo nível
        MazeWeatherSystem.GenerateRandomWeather();
        MazeEventSystem.GenerateRandomWeather();
        
        instance.bullets.Clear();
        ProceduralMaze.gameState = GameState.Playing;
        
        // Limpar efeitos visuais
        MazeVisualEffects.ClearAllEffects();
        
        // Ativar proteção ao spawnar
        MazePlayerUtils.ActivateSpawnProtection();
    }

    public static void RestartGameFromGameOver() => StartGameFromMenu();

    public static void NextLevel(ProceduralMaze maze)
    {
        maze.currentLevel++;
        MazeDifficultyUtils.AdjustDifficulty(maze, false);
        MazeGenerationUtils.RegenerateMazeEnsuringPath(maze);
        MazeEnemyUtils.SpawnEnemies(maze);
        MazeAdvancedEnemies.InitializeAdvancedEnemies();
        MazeAdvancedEnemies.SpawnAdvancedEnemies(maze, maze.currentLevel);
        MazePowerUpUtils.SpawnPowerUps(maze);
        maze.bullets.Clear();
        MazeHUD.NextLevel();

        maze.ammo = Mathf.Min(maze.ammo + 3, maze.maxAmmo); // bônus de munição por fase
        int scoreToAdd = 100 * maze.currentLevel;
        if (maze.scoreBoosterActive) scoreToAdd = Mathf.RoundToInt(scoreToAdd * maze.scoreBoosterMultiplier);
        MazeHUD.AddScore(scoreToAdd);
        maze.score = MazeHUD.score;

        if (maze.score > maze.scoreRecord)
        {
            maze.scoreRecord = maze.score;
            MazeSaveSystem.SaveRecord(maze.scoreRecord);
            MazeHUD.ShowStatusMessage("Novo Recorde!");
        }
        
        // Registrar achievement de nível completado
        MazeAchievements.OnLevelCompleted(maze.currentLevel);
        
        // Ativar proteção ao spawnar no próximo nível
        MazePlayerUtils.ActivateSpawnProtection();
    }

    public static void LoseLifeAndRespawn(ProceduralMaze maze)
    {
        if (maze.shieldActive)
        {
            MazeHUD.ShowStatusMessage("Escudo protegeu!");
            return;
        }
        maze.lives--;
        if (maze.lives > 0)
        {
            MazeHUD.ShowStatusMessage($"Você perdeu uma vida! ({maze.lives} restantes)");
            MazeGenerationUtils.RegenerateMazeEnsuringPath(maze);
            MazeEnemyUtils.SpawnEnemies(maze);
            MazeAdvancedEnemies.InitializeAdvancedEnemies();
            MazeAdvancedEnemies.SpawnAdvancedEnemies(maze, maze.currentLevel);
            MazePowerUpUtils.SpawnPowerUps(maze);
            maze.bullets.Clear();
            maze.shieldActive = false;
            maze.shieldTimer = 0f;
            
            // Ativar proteção ao respawnar
            MazePlayerUtils.ActivateSpawnProtection();
        }
        else
        {
            GameOver(maze);
        }
    }

    public static void GameOver(ProceduralMaze maze)
    {
        ProceduralMaze.gameState = GameState.GameOver;
        MazeHUD.ShowStatusMessage("Game Over!");
        
        // Registrar estatísticas
        MazeStatistics.OnGameLost();
        
        // Salvar score no ranking local
        if (maze.score > 0)
            MazeSaveSystem.AddScoreToRanking(maze.score);
        
        // Salvar jogo ao perder
        MazeSaveSystem.SaveGame(maze);
        
        // Salvar estado dos novos sistemas
        MazeWeatherSystem.SaveWeatherState();
        MazeEventSystem.SaveEventState();
    }

    public static void CollectPowerUp(ProceduralMaze maze, PowerUpType type)
    {
        maze.powerUpFlashTimer = maze.powerUpFlashDuration;
        
        // Calcular tamanho da célula para efeitos visuais
        float cellSize = Mathf.Min(Screen.width / (float)maze.width, Screen.height / (float)maze.height);
        
        // Criar efeito visual de coleta
        MazeVisualEffects.CreatePowerUpCollectEffect(maze.playerPos, cellSize);
        
        // Registrar achievement de power-up coletado
        MazeAchievements.OnPowerUpCollected();
        MazeStatistics.OnPowerUpCollected();

        switch (type)
        {
            case PowerUpType.Ammo:
                maze.ammo = Mathf.Min(maze.ammo + 5, maze.maxAmmo);
                MazeHUD.ShowStatusMessage("Munição +5!");
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpSound);
                break;
            case PowerUpType.Life:
                maze.lives = Mathf.Min(maze.lives + 1, 9);
                MazeHUD.ShowStatusMessage("Vida extra!");
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpSound);
                break;
            case PowerUpType.Shield:
                maze.shieldActive = true;
                maze.shieldTimer = maze.shieldDuration;
                MazeHUD.ShowStatusMessage("Escudo ativado!");
                MazeAchievements.OnShieldUsed();
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpSound);
                break;
            case PowerUpType.DoubleShot:
                maze.doubleShotActive = true;
                maze.doubleShotTimer = maze.doubleShotDuration;
                maze.tripleShotActive = false;
                MazeHUD.ShowStatusMessage("Tiro Duplo!");
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.doubleShotSound);
                break;
            case PowerUpType.TripleShot:
                maze.tripleShotActive = true;
                maze.tripleShotTimer = maze.tripleShotDuration;
                maze.doubleShotActive = false;
                MazeHUD.ShowStatusMessage("Tiro Triplo!");
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.tripleShotSound);
                break;
            case PowerUpType.SpeedBoost:
                maze.speedBoostActive = true;
                maze.speedBoostTimer = maze.speedBoostDuration;
                MazeHUD.ShowStatusMessage("Velocidade!");
                MazeAchievements.OnSpeedBoostUsed();
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.speedBoostSound);
                break;
            case PowerUpType.Invisibility:
                maze.invisibilityActive = true;
                maze.invisibilityTimer = maze.invisibilityDuration;
                MazeHUD.ShowStatusMessage("Invisível!");
                MazeAchievements.OnInvisibilityUsed();
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.invisibilitySound);
                break;
            case PowerUpType.Teleport:
                maze.teleportAvailable = true;
                MazeHUD.ShowStatusMessage("Teleport Pronto!");
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.teleportSound);
                break;
            case PowerUpType.ScoreBooster:
                maze.scoreBoosterActive = true;
                maze.scoreBoosterTimer = maze.scoreBoosterDuration;
                MazeHUD.ShowStatusMessage("Score x2!");
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.scoreBoosterSound);
                break;
        }
    }
} 