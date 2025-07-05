// MazeMenuInput.cs
using UnityEngine;

public static class MazeMenuInput
{
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
} 