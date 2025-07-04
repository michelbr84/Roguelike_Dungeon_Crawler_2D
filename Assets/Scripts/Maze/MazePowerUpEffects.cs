using UnityEngine;

public static class MazePowerUpEffects
{
    public static void UpdatePowerUpTimers(ProceduralMaze maze)
    {
        // Shield
        if (maze.shieldActive)
        {
            maze.shieldTimer -= Time.deltaTime;
            if (maze.shieldTimer <= 0f)
            {
                maze.shieldActive = false;
                maze.shieldTimer = 0f;
                MazeHUD.ShowStatusMessage("Escudo acabou!");
            }
        }

        // Double Shot
        if (maze.doubleShotActive)
        {
            maze.doubleShotTimer -= Time.deltaTime;
            if (maze.doubleShotTimer <= 0f)
            {
                maze.doubleShotActive = false;
                maze.doubleShotTimer = 0f;
                MazeHUD.ShowStatusMessage("Tiro Duplo acabou!");
            }
        }

        // Triple Shot
        if (maze.tripleShotActive)
        {
            maze.tripleShotTimer -= Time.deltaTime;
            if (maze.tripleShotTimer <= 0f)
            {
                maze.tripleShotActive = false;
                maze.tripleShotTimer = 0f;
                MazeHUD.ShowStatusMessage("Tiro Triplo acabou!");
            }
        }

        // Speed Boost
        if (maze.speedBoostActive)
        {
            maze.speedBoostTimer -= Time.deltaTime;
            if (maze.speedBoostTimer <= 0f)
            {
                maze.speedBoostActive = false;
                maze.speedBoostTimer = 0f;
                MazeHUD.ShowStatusMessage("Velocidade normalizada!");
            }
        }

        // Invisibility
        if (maze.invisibilityActive)
        {
            maze.invisibilityTimer -= Time.deltaTime;
            if (maze.invisibilityTimer <= 0f)
            {
                maze.invisibilityActive = false;
                maze.invisibilityTimer = 0f;
                MazeHUD.ShowStatusMessage("Visível novamente!");
            }
        }

        // Score Booster
        if (maze.scoreBoosterActive)
        {
            maze.scoreBoosterTimer -= Time.deltaTime;
            if (maze.scoreBoosterTimer <= 0f)
            {
                maze.scoreBoosterActive = false;
                maze.scoreBoosterTimer = 0f;
                MazeHUD.ShowStatusMessage("Score normalizado!");
            }
        }
    }
} 