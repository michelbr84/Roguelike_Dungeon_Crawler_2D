// MazeMenu.cs
using UnityEngine;

public static class MazeMenu
{
    private static readonly string controlsText =
        "Controles:\n" +
        "Mover: Setas\n" +
        "Atirar: Espaço\n" +
        "Sair: Esc";

    public static void DrawMenu(int record, int lives)
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

    public static void DrawGameOver(int score, int record, int lives)
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 52;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleStyle.normal.textColor = Color.red;

        GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 32;
        infoStyle.normal.textColor = Color.yellow;
        infoStyle.alignment = TextAnchor.UpperCenter;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 28;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        int w = 540, h = 340;
        Rect main = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        GUI.Box(main, "");

        GUILayout.BeginArea(main);

        GUILayout.Space(16);
        GUILayout.Label("GAME OVER", titleStyle);
        GUILayout.Space(12);

        GUILayout.Label($"Score: {score}", infoStyle);
        GUILayout.Label($"Recorde: {record}", infoStyle);
        GUILayout.Label($"Vidas: {lives}", infoStyle); // sempre mostra 0 vidas

        GUILayout.Space(16);

        if (GUILayout.Button("Jogar Novamente", buttonStyle, GUILayout.Height(48)))
        {
            ProceduralMaze.RestartGameFromGameOver();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Sair", buttonStyle, GUILayout.Height(36)))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        GUILayout.EndArea();
    }

    // HandleMenuInput e HandleGameOverInput já devem existir, pode manter igual ao anterior.
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
        if (Event.current != null && Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.Space)
            {
                ProceduralMaze.RestartGameFromGameOver();
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
}
