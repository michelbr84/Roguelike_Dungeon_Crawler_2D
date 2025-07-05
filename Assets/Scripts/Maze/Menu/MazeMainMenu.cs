// MazeMainMenu.cs
using UnityEngine;

public static class MazeMainMenu
{
    private static readonly string controlsText =
        "Controles:\n" +
        "Mover: Setas\n" +
        "Atirar: Espaço\n" +
        "Sair: Esc";

    public static void Draw(int record, int lives)
    {
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
            MazeMenu.ShowRanking();
        }
        
        GUILayout.Space(8);
        
        if (GUILayout.Button("Estatísticas", buttonStyle, GUILayout.Height(36)))
        {
            MazeMenu.ShowStats();
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
} 