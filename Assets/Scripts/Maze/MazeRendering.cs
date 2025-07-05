using UnityEngine;

public static class MazeRendering
{
    public static void DrawMaze(ProceduralMaze mazeObj)
    {
        float cellSize = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);

        // Aplicar screen shake
        Vector2 shakeOffset = MazeShaderEffects.GetScreenShakeOffset();
        Matrix4x4 shakeMatrix = GUI.matrix;
        MazeVisualEffectRenderer.ApplyScreenShake();

        // Fundo, grid, paredes, saída
        MazeCellRenderer.DrawBackground(mazeObj);
        MazeCellRenderer.DrawGridAndWalls(mazeObj, cellSize);
        MazeCellRenderer.DrawExit(mazeObj, cellSize);

        // Power-ups
        MazePowerUpRenderer.DrawPowerUps(mazeObj, cellSize);

        // Inimigos
        MazeEnemyRenderer.DrawEnemies(mazeObj, cellSize);

        // Player
        MazePlayerRenderer.DrawPlayer(mazeObj, cellSize);

        // Tiros
        MazeBulletRenderer.DrawBullets(mazeObj, cellSize);

        // Efeitos visuais (glow, partículas, etc)
        MazeVisualEffectRenderer.RenderGlowAndParticles(cellSize);

        // Controles touch
        MazeTouchControls.RenderTouchControls();

        // HUD e menus
        MazeHUD.DrawHUD(mazeObj);
        MazeSettingsMenu.RenderMenu();

        // Reset da matriz após screen shake
        MazeVisualEffectRenderer.ResetScreenShake(shakeMatrix, shakeOffset);
    }
} 