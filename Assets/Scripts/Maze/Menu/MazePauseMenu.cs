// MazePauseMenu.cs
using UnityEngine;

public static class MazePauseMenu
{
    public static void Draw(ProceduralMaze maze)
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
} 