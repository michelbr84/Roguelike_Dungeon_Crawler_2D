using UnityEngine;

public static class MazeInputHandler
{
    public static void HandleGameInput(ProceduralMaze maze)
    {
        // Controles do jogador
        MazePlayerUtils.HandlePlayerInput(maze);

        // TELEPORT: ativação (teclado + touch)
        bool teleportPressed = Input.GetKeyDown(maze.teleportKey) || MazeTouchControls.IsTeleportButtonPressed();
        if (maze.teleportAvailable && teleportPressed)
        {
            Vector2Int safePos = MazeTeleportUtils.FindSafeTeleportPosition(maze);
            if (safePos != maze.playerPos)
            {
                float cellSize = Mathf.Min(Screen.width / (float)maze.width, Screen.height / (float)maze.height);
                
                // Criar efeito de teleport na posição de origem
                MazeVisualEffects.CreateTeleportEffect(maze.playerPos, cellSize);
                
                maze.playerPos = safePos;
                maze.teleportAvailable = false;
                
                // Criar efeito de teleport na posição de destino
                MazeVisualEffects.CreateTeleportEffect(maze.playerPos, cellSize);
                
                MazeHUD.ShowStatusMessage("Teleportado!");
                MazeAchievements.OnTeleportUsed();
                MazeStatistics.OnTeleportUsed();
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.teleportSound);
            }
        }

        // Menu de configurações ou sair do jogo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MazeSettingsMenu.IsVisible())
            {
                MazeSettingsMenu.ToggleMenu();
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }

    public static void HandleMenuInput()
    {
        MazeMenu.HandleMenuInput();
    }

    public static void HandleGameOverInput()
    {
        MazeMenu.HandleGameOverInput();
    }
} 