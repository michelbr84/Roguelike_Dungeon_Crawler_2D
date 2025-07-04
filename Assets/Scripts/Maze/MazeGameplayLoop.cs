using UnityEngine;

public static class MazeGameplayLoop
{
    public static void UpdateGameplay(ProceduralMaze maze)
    {
        // Movimento de bullets
        MazePlayerUtils.MoveBullets(maze);
        
        // Movimento de inimigos normais
        MazeEnemyUtils.MoveEnemies(maze);
        
        // Movimento de inimigos avançados
        MazeAdvancedEnemies.UpdateAdvancedEnemies(maze);
        
        // Verificação de colisões
        MazePlayerUtils.HandleCollisions(maze);
        MazePowerUpUtils.HandlePowerUpCollisions(maze);

        // Atualização de timers de power-ups
        MazePowerUpEffects.UpdatePowerUpTimers(maze);

        // Timer de animação de coleta de power-up
        if (maze.powerUpFlashTimer > 0f)
            maze.powerUpFlashTimer -= Time.deltaTime;
            
        // Atualizar sistema de missões
        MazeMissions.UpdateMissions(maze);
        
        // Atualizar efeitos visuais
        MazeVisualEffects.UpdateEffects();
        MazeShaderEffects.UpdateEffects();
        MazeShaderEffects.UpdateParticleTrails();
    }
} 