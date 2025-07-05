// MazeVictoryMenu.cs
using UnityEngine;

public static class MazeVictoryMenu
{
    public static void Draw(ProceduralMaze maze)
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